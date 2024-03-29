var Util = require('../../util');

var TimeUtil = require('@antv/scale/lib/time-util');

var getColDefs = require('./get-col-defs');

module.exports = function (chart) {
  chart.on('beforeinitgeoms', function () {
    chart.set('limitInPlot', true);
    var data = chart.get('data');
    var colDefs = getColDefs(chart);
    if (!colDefs) return data;
    var geoms = chart.get('geoms');
    var isSpecialGeom = false;
    Util.each(geoms, function (geom) {
      if (['area', 'line', 'path'].indexOf(geom.get('type')) !== -1) {
        isSpecialGeom = true;
        return false;
      }
    });
    var fields = [];
    Util.each(colDefs, function (def, key) {
      if (!isSpecialGeom && def && (def.values || def.min || def.max)) {
        fields.push(key);
      }
    });

    if (fields.length === 0) {
      return data;
    }

    var geomData = [];
    Util.each(data, function (obj) {
      var flag = true;
      Util.each(fields, function (field) {
        var value = obj[field];

        if (value) {
          var colDef = colDefs[field];

          if (colDef.type === 'timeCat') {
            var values = colDef.values;

            if (Util.isNumber(values[0])) {
              value = TimeUtil.toTimeStamp(value);
            }
          }

          if (colDef.values && colDef.values.indexOf(value) === -1 || colDef.min && value < colDef.min || colDef.max && value > colDef.max) {
            flag = false;
          }
        }
      });

      if (flag) {
        geomData.push(obj);
      }
    });
    chart.set('filteredData', geomData);
  });
};