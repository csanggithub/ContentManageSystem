using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContentManageSystem.Common;
using ContentManageSystem.Entity.Models;
using ContentManageSystem.Services;
using ContentManageSystem.Web.Areas.Admin.Models;
using ContentManageSystem.Web.Models;

namespace ContentManageSystem.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [AdminAuthorize]
    public class UserController : Controller
    {
        private UserServices userManager = new UserServices();

        /// <summary>
        /// 默认页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 分页列表【json】
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="username">用户名</param>
        /// <param name="name">名称</param>
        /// <param name="sex">性别</param>
        /// <param name="email">Email</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="order">排序</param>
        /// <returns>Json</returns>
        public ActionResult PageListJson(int? roleID, string username, string name, int? sex, string email, int? pageNumber, int? pageSize, int? order)
        {
            Paging<User> _pagingUser = new Paging<User>();
            if (pageNumber != null && pageNumber > 0) _pagingUser.PageIndex = (int)pageNumber;
            if (pageSize != null && pageSize > 0) _pagingUser.PageSize = (int)pageSize;
            var _paging = userManager.FindPageList(_pagingUser, roleID, username, name, sex, email, null);
            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            //角色列表
            var _roles = new RoleServices().FindList();
            List<SelectListItem> _listItems = new List<SelectListItem>(_roles.Count());
            foreach (var _role in _roles)
            {
                _listItems.Add(new SelectListItem() { Text = _role.Name, Value = _role.RoleID.ToString() });
            }
            ViewBag.Roles = _listItems;
            //角色列表结束
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddUserViewModel userViewModel)
        {
            if (userManager.HasUsername(userViewModel.Username)) ModelState.AddModelError("Username", "用户名已存在");
            if (userManager.HasEmail(userViewModel.Email)) ModelState.AddModelError("Email", "Email已存在");
            if (ModelState.IsValid)
            {
                User _user = new User();
                _user.RoleID = userViewModel.RoleID;
                _user.Username = userViewModel.Username;
                _user.Name = userViewModel.Name;
                _user.Sex = userViewModel.Sex;
                _user.Password = Security.SHA256(userViewModel.Password);
                _user.Email = userViewModel.Email;
                _user.RegTime = System.DateTime.Now;
                var _response = userManager.Add(_user);
                if (_response.Code == 1) return View("Prompt", new Prompt()
                {
                    Title = "添加用户成功",
                    Message = "您已成功添加了用户【" + _response.Data.Username + "（" + _response.Data.Name + "）】",
                    Buttons = new List<string> {"<a href=\"" + Url.Action("Index", "User") + "\" class=\"btn btn-default\">用户管理</a>",
                 "<a href=\"" + Url.Action("Details", "User",new { id= _response.Data.UserID }) + "\" class=\"btn btn-default\">查看用户</a>",
                 "<a href=\"" + Url.Action("Add", "User") + "\" class=\"btn btn-default\">继续添加</a>"}
                });
                else ModelState.AddModelError("", _response.Message);
            }
            //角色列表
            var _roles = new RoleServices().FindList();
            List<SelectListItem> _listItems = new List<SelectListItem>(_roles.Count());
            foreach (var _role in _roles)
            {
                _listItems.Add(new SelectListItem() { Text = _role.Name, Value = _role.RoleID.ToString() });
            }
            ViewBag.Roles = _listItems;
            //角色列表结束

            return View(userViewModel);
        }

        /// <summary>
        /// 用户名是否可用
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns></returns> 
        [HttpPost]
        public JsonResult CanUsername(string UserName)
        {
            return Json(!userManager.HasUsername(UserName));
        }

        /// <summary>
        /// Email是否存可用
        /// </summary>
        /// <param name="Email">Email</param>
        /// <returns></returns> 
        [HttpPost]
        public JsonResult CanEmail(string Email)
        {
            return Json(!userManager.HasEmail(Email));
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="id">用户主键</param>
        /// <returns>分部视图</returns>
        public ActionResult Modify(int id)
        {
            //角色列表
            var _roles = new RoleServices().FindList();
            List<SelectListItem> _listItems = new List<SelectListItem>(_roles.Count());
            foreach (var _role in _roles)
            {
                _listItems.Add(new SelectListItem() { Text = _role.Name, Value = _role.RoleID.ToString() });
            }
            ViewBag.Roles = _listItems;
            //角色列表结束
            return PartialView(userManager.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modify(int id, FormCollection form)
        {
            Response _resp = new Response();
            var _user = userManager.Find(id);
            if (TryUpdateModel(_user, new string[] { "RoleID", "Name", "Sex", "Email" }))
            {
                if (_user == null)
                {
                    _resp.Code = 0;
                    _resp.Message = "用户不存在，可能已被删除，请刷新后重试";
                }
                else
                {
                    if (_user.Password != form["Password"].ToString()) _user.Password = Security.SHA256(form["Password"].ToString());
                    _resp = userManager.Update(_user);
                }
            }
            else
            {
                _resp.Code = 0;
                _resp.Message = General.GetModelErrorString(ModelState);
            }
            return Json(_resp);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            return Json(userManager.Delete(id));
        }
    }
}