using ContentManageSystem.Services;
using ContentManageSystem.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContentManageSystem.Entity.Models;

namespace ContentManageSystem.Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        private AdminServices adminManager = new AdminServices();
        // GET: Admin/Admin
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                //string _passowrd = Security.SHA256(loginViewModel.Password);
                //var _response = adminManager.Verify(loginViewModel.Accounts, _passowrd);
                //if (_response.Code == 1)
                //{
                //    var _admin = adminManager.Find(loginViewModel.Accounts);
                //    Session.Add("AdminID", _admin.AdministratorID);
                //    Session.Add("Accounts", _admin.Accounts);
                //    _admin.LoginTime = DateTime.Now;
                //    _admin.LoginIP = Request.UserHostAddress;
                //    adminManager.Update(_admin);
                //    return RedirectToAction("Index", "Home");
                //}
                //else if (_response.Code == 2) ModelState.AddModelError("Accounts", _response.Message);
                //else if (_response.Code == 3) ModelState.AddModelError("Password", _response.Message);
                //else ModelState.AddModelError("", _response.Message);

                if (loginViewModel.Password == "111111")
                {
                    var _admin = new ContentManageSystem.Entity.Models.Admin();
                    _admin.Accounts = "admin";
                    _admin.AdministratorID = 1;
                    Session.Add("AdminID", _admin.AdministratorID);
                    Session.Add("Accounts", _admin.Accounts);
                    return RedirectToAction("Index", "Home");
                }

            }
            return View(loginViewModel);
        }
    }
}