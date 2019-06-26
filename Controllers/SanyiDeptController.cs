using Microsoft.AspNetCore.Mvc;

namespace syDeptStatus.Controllers
{
    [Route("sy/[action]")]
    public class SanyiDeptController : Controller
    {        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SanYi()
        {
            return View();
        }

        public IActionResult QualityData()
        {
            return View();
        }
    }
}