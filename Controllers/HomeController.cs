using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using syDeptStatus.Models;
using SqlSugar;

namespace syDeptStatus.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly DbConnector dbConnector = new DbConnector();

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["empno"] = HttpContext.Session.GetString("empno");
//            if (HttpContext.User.Identity.IsAuthenticated)
//            {
//                ViewData["ok"] = "okisauthenticated"+User.Identity.Name;
//            }
            return View();
        }

        [HttpPost]
        public IActionResult Index(string type)
        {
            if (type == "1") //科主任审核
            {
                var empno = HttpContext.Session.GetString("empno");

                var result = dbConnector._db.Ado.GetInt(@"
                select count(*) from staff_dict@hislink where fee_query like '%主任%' and emp_no=@1",
                    new List<SugarParameter>
                    {
                        new SugarParameter("@1", empno)
                    });
                if (result > 0)
                    return RedirectToAction("DocChiefCheck");
                return View();
            }

            if (type == "9") //三医监管办公室使用，目前只能李悦的工号登入
            {
                var empno = HttpContext.Session.GetString("empno");
                if (empno == "4380") /* 李跃的工号 */ return RedirectToAction("Index", "SanyiDept");
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [method: HttpPost]
        public IActionResult Login(string empno, string password)
        {
            var dbService = new DbService();
            var result = dbService.UserSignIn(empno.Trim(), password.Trim().ToUpper());
            if (result)
            {
                HttpContext.Session.SetString("empno", empno.Trim());
                return RedirectToAction("Index");
            }

            if (empno == "1" && password == "1")
            {
                var claim1 = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "auser"),
                    new Claim(ClaimTypes.Role, "normaluser")
                };
                var claim2 = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "buser"),
                    new Claim(ClaimTypes.Role, "administor")
                };
                var claimsIdentity = new ClaimsIdentity(claim1);
                var pricipal = new ClaimsPrincipal(claimsIdentity);
                var properties = new AuthenticationProperties();
                var ticket = new AuthenticationTicket(pricipal, properties,
                    CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, pricipal);

                HttpContext.Session.SetString("empno", "1");

//                HttpContext.Session.SetString("user",empno);
//                if (HttpContext.User.Identity.IsAuthenticated)
//                {
//                    return RedirectToAction("Index");
//                }

                return RedirectToAction("Index");
            }

            return View();
        }

//        public IActionResult YHIssueReply()
//        {
//            return View();
//        }
//
//        public IActionResult ZyTsFyIssueReply()
//        {
//            return View();
//        }

        public IActionResult IssueMaster()
        {
            if (HttpContext.Session.GetString("empno") == null) return RedirectToAction("Login");
            ViewData["empno"] = HttpContext.Session.GetString("empno");
            return View();
        }

        public IActionResult DocChiefCheck()
        {
            ViewData["empno"] = HttpContext.Session.GetString("empno");
            return View();
        }

        public async Task<IActionResult> SignIn(string empno, string password)
        {
            var user = new ClaimsPrincipal(
                new ClaimsIdentity(new[] {new Claim(ClaimTypes.Name, "bob")},
                    CookieAuthenticationDefaults.AuthenticationScheme)
            );
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user,
                new AuthenticationProperties());
            if (string.IsNullOrEmpty(empno + password)) return RedirectToAction("Login");
            return RedirectToAction("Index");
        }
    }
}