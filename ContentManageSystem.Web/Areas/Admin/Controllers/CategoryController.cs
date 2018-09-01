using ContentManageSystem.Common;
using ContentManageSystem.Entity.Models.Category;
using ContentManageSystem.Services;
using ContentManageSystem.Services.Category;
using ContentManageSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContentManageSystem.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 栏目控制器
    /// </summary>
    [AdminAuthorize]
    public class CategoryController : Controller
    {
        private CategoryServices categoryManager;
        public CategoryController()
        {
            categoryManager = new CategoryServices();
        }

        /// <summary>
        /// 添加栏目
        /// </summary>
        /// <param name="id">父栏目ID</param>
        /// <returns></returns>
        public ActionResult Add(int? id)
        {
            Category _category = new Category() { ParentID = 0 };
            if (id != null && id > 0)
            {
                var _parent = categoryManager.Find((int)id);
                if (_parent != null && _parent.Type == CategoryType.General) _category.ParentID = (int)id;
            }
            //_category.General = new CategoryGeneral() { View="Index",ContentView = "Index" };
            //_category.Page = new CategoryPage() { View="index" };
            //_category.Link = new CategoryLink() { Url = "http://" };
            //var _parentCategoryList = ComboTreeList();
            //_parentCategoryList.Insert(0,new SelectListItem() { Text="无", Value="0" });
            //ViewBag.ParentCategoryList = _parentCategoryList;
            return View(_category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add()
        {
            Category _category = new Category();
            TryUpdateModel(_category, new string[] { "Type", "ParentID", "Name", "Description", "Order", "Target" });
            if (ModelState.IsValid)
            {
                //检查父栏目
                if (_category.ParentID > 0)
                {
                    var _parentCategory = categoryManager.Find(_category.ParentID);
                    if (_parentCategory == null) ModelState.AddModelError("ParentID", "父栏目不存在，请刷新后重新添加");
                    else if (_parentCategory.Type != CategoryType.General) ModelState.AddModelError("ParentID", "父栏目不允许添加子栏目");
                    else
                    {
                        _category.ParentPath = _parentCategory.ParentPath + "," + _parentCategory.CategoryID;
                        _category.Depth = _parentCategory.Depth + 1;
                    }
                }
                else
                {
                    _category.ParentPath = "0";
                    _category.Depth = 0;
                }
                //栏目基本信息保存
                Response _response = new Response() { Code = 0, Message = "初始失败信息" };
                //根据栏目类型进行处理
                switch (_category.Type)
                {
                    case CategoryType.General:
                        var _general = new CategoryGeneral();
                        TryUpdateModel(_general);
                        _response = categoryManager.Add(_category, _general);
                        break;
                    case CategoryType.Page:
                        var _page = new CategoryPage();
                        TryUpdateModel(_page);
                        _response = categoryManager.Add(_category, _page);
                        break;
                    case CategoryType.Link:
                        var _link = new CategoryLink();
                        TryUpdateModel(_link);
                        _response = categoryManager.Add(_category, _link);
                        break;
                }
                if (_response.Code == 1) return View("Prompt", new Prompt() { Title = "添加栏目成功", Message = "添加栏目【" + _category.Name + "】成功" });
                else return View("Prompt", new Prompt() { Title = "添加失败", Message = "添加栏目【" + _category.Name + "】时发生系统错误，未能保存到数据库，请重试" });
            }
            //var _parentCategoryList = ComboTreeList();
            //_parentCategoryList.Insert(0, new SelectListItem() { Text = "无", Value = "0" });
            //ViewBag.ParentCategoryList = _parentCategoryList;
            return View(_category);
        }

        /// <summary>
        /// 常规栏目附件信息
        /// </summary>
        /// <returns></returns>
        public ActionResult AddGeneral()
        {
            var _general = new CategoryGeneral() { ContentView = "Index", View = "Index" };
            List<SelectListItem> _contentTypeItems = new List<SelectListItem>();
            ContentTypeServices _contentTypeManager = new ContentTypeServices();
            var _contentTypes = _contentTypeManager.FindList();
            foreach (var contentType in _contentTypes)
            {
                _contentTypeItems.Add(new SelectListItem() { Value = contentType.ContentTypeID.ToString(), Text = contentType.Name });
            }
            ViewBag.ContentTypeItems = _contentTypeItems;
            return PartialView(_general);
        }

        /// <summary>
        /// 添加单页栏目
        /// </summary>
        /// <returns></returns>
        public ActionResult AddPage()
        {
            var _page = new CategoryPage() { View = "Index" };
            return PartialView(_page);

        }
        /// <summary>
        /// 添加外部链接
        /// </summary>
        /// <returns></returns>
        public ActionResult AddLink()
        {
            var _link = new CategoryLink() { Url = "http://" };
            return View(_link);
        }


        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// 子栏目列表【暂时只返回Json格式】
        /// </summary>
        /// <param name="id">栏目ID.0-表示根栏目</param>
        /// <returns></returns>
        public ActionResult Children(int id)
        {
            return Json(categoryManager.FindChildren(id));

        }

        /// <summary>
        /// 栏目树【ztree简单数据】
        /// </summary>
        /// <param name="type">栏目类型 可为空</param>
        /// <returns></returns>
        public ActionResult TreeList(CategoryType? type)
        {
            var _categories = categoryManager.FindList(0, type, null);
            //_categories.Add(new Category() { CategoryID = 1, Name = "测试", Order = 0, ParentID = 0, ParentPath = "0,1", Type = CategoryType.General });
            return Json(_categories);
        }

        /// <summary>
        /// 组合树
        /// </summary>
        /// <returns></returns>
        public ActionResult DropdownTreeList()
        {
            List<TreeNode> _nodes = new List<TreeNode>();
            //栏目并进行排序使最深的节点排在最前
            var _categories = categoryManager.FindList(0, CategoryType.General, new OrderParam[] { new OrderParam() { Method = OrderMethod.DESC, PropertyName = "ParentPath" }, new OrderParam() { Method = OrderMethod.ASC, PropertyName = "Order" } });
            TreeNode _node;
            //遍历常规栏目
            foreach (var _category in _categories)
            {
                _node = new TreeNode() { parentid = _category.ParentID, value = _category.CategoryID, id = "node_" + _category.CategoryID, label = _category.Name, html = Url.Action("Detials", "Category", new { @id = _category.CategoryID }) };
                if (_nodes.Exists(n => n.parentid == _category.CategoryID))
                {
                    var _children = _nodes.Where(n => n.parentid == _category.CategoryID).ToList();
                    _nodes.RemoveAll(n => n.parentid == _category.CategoryID);
                    _node.items = _children;
                    _node.expanded = false;
                    _nodes.Add(_node);
                }
                else _nodes.Add(_node);
            }
            _nodes.Insert(0, new TreeNode() { id = "node_0", value = 0, label = "无" });
            return Json(_nodes);
        }

        /// <summary>
        /// 树形节点数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Tree(bool showIcon = false)
        {
            List<TreeNode> _nodes = new List<TreeNode>();
            //栏目并进行排序使最深的节点排在最前
            var _categories = categoryManager.FindList(0, null, new OrderParam[] { new OrderParam() { Method = OrderMethod.DESC, PropertyName = "ParentPath" }, new OrderParam() { Method = OrderMethod.ASC, PropertyName = "Order" } });
            TreeNode _node;
            //遍历常规栏目
            foreach (var _category in _categories)
            {
                _node = new TreeNode() { parentid = _category.ParentID, value = _category.CategoryID, id = "node_" + _category.CategoryID, label = _category.Name, html = Url.Action("Detials", "Category", new { @id = _category.CategoryID }) };
                if (showIcon)
                {
                    switch (_category.Type)
                    {
                        case CategoryType.General:
                            _node.icon = Url.Content("~/Content/Images/folder.png");
                            break;
                        case CategoryType.Page:
                            _node.icon = Url.Content("~/Content/Images/page.png");
                            break;
                        case CategoryType.Link:
                            _node.icon = Url.Content("~/Content/Images/link.png");
                            break;
                    }
                }
                if (_nodes.Exists(n => n.parentid == _category.CategoryID))
                {
                    var _children = _nodes.Where(n => n.parentid == _category.CategoryID).ToList();
                    _nodes.RemoveAll(n => n.parentid == _category.CategoryID);
                    _node.items = _children;
                    _node.expanded = false;
                    _nodes.Add(_node);
                }
                else _nodes.Add(_node);
            }

            return Json(_nodes);
        }

        public ActionResult zTree(bool showIcon = false)
        {
            List<TreeNode> _nodes = new List<TreeNode>();
            //栏目并进行排序使最深的节点排在最前
            var _categories = categoryManager.FindList(0, null, new OrderParam[] { new OrderParam() { Method = OrderMethod.DESC, PropertyName = "ParentPath" }, new OrderParam() { Method = OrderMethod.ASC, PropertyName = "Order" } });
            TreeNode _node;
            //遍历常规栏目
            foreach (var _category in _categories)
            {
                _node = new TreeNode() { parentid = _category.ParentID, value = _category.CategoryID, id = "node_" + _category.CategoryID, label = _category.Name, html = Url.Action("Detials", "Category", new { @id = _category.CategoryID }) };
                if (showIcon)
                {
                    switch (_category.Type)
                    {
                        case CategoryType.General:
                            _node.icon = Url.Content("~/Content/Images/folder.png");
                            break;
                        case CategoryType.Page:
                            _node.icon = Url.Content("~/Content/Images/page.png");
                            break;
                        case CategoryType.Link:
                            _node.icon = Url.Content("~/Content/Images/link.png");
                            break;
                    }
                }
                _nodes.Add(_node);
            }

            return Json(_nodes);
        }

        /// <summary>
        /// 下拉树【常规栏目】
        /// </summary>
        /// <returns></returns>
        public ActionResult DropdownTree()
        {
            //栏目并进行排序使最深的节点排在最前
            var _categories = categoryManager.FindList(0, CategoryType.General, new OrderParam[] { new OrderParam() { Method = OrderMethod.ASC, PropertyName = "ParentPath" }, new OrderParam() { Method = OrderMethod.ASC, PropertyName = "Order" } });
            List<TreeNode> _nodes = new List<TreeNode>();
            //遍历常规栏目
            foreach (var _category in _categories)
            {
                _nodes.Add(new TreeNode() { parentid = _category.ParentID, id = _category.CategoryID.ToString(), label = _category.Name });
            }

            return Json(_nodes);
        }

        #region 修改

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id">栏目ID</param>
        /// <returns></returns>
        public ActionResult Modify(int id)
        {
            var _category = categoryManager.Find(id);
            if (_category == null) return View("Prompt", new Prompt() { Title = "错误", Message = "栏目不存在！" });
            return View(_category);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modify(int id, FormCollection form)
        {
            Category _category = categoryManager.Find(id);
            if (_category == null) return View("Prompt", new Prompt() { Title = "错误", Message = "栏目不存在！" });
            if (TryUpdateModel(_category, new string[] { "Type", "ParentID", "Name", "Description", "Order", "Target" }))
            {
                //检查父栏目
                if (_category.ParentID > 0)
                {
                    var _parentCategory = categoryManager.Find(_category.ParentID);
                    if (_parentCategory == null) ModelState.AddModelError("ParentID", "父栏目不存在，请刷新后重新添加");
                    else if (_parentCategory.Type != CategoryType.General) ModelState.AddModelError("ParentID", "父栏目不允许添加子栏目");
                    else if (_parentCategory.ParentPath.IndexOf(_category.ParentPath) >= 0) ModelState.AddModelError("ParentID", "父栏目不能是其本身或其子栏目");
                    else
                    {
                        _category.ParentPath = _parentCategory.ParentPath + "," + _parentCategory.CategoryID;
                        _category.Depth = _parentCategory.Depth + 1;
                    }
                }
                else
                {
                    _category.ParentPath = "0";
                    _category.Depth = 0;
                }
                //栏目基本信息保存
                Response _response = new Response() { Code = 0, Message = "初始失败信息" };
                //根据栏目类型进行处理
                switch (_category.Type)
                {
                    case CategoryType.General:
                        var _generalManager = new CategoryGeneralServices();
                        var _general = _generalManager.Find(id);
                        if (_general == null) _general = new CategoryGeneral() { CategoryID = id, View = "Index", ContentView = "Index" };
                        if (TryUpdateModel(_general)) _response = categoryManager.Update(_category, _general);
                        break;
                    case CategoryType.Page:
                        var _pageManager = new CategoryPageServices();
                        var _page = _pageManager.Find(id);
                        if (_page == null) _page = new CategoryPage() { CategoryID = id, View = "index" };
                        if (TryUpdateModel(_page)) _response = categoryManager.Update(_category, _page);
                        break;
                    case CategoryType.Link:
                        var _linkManager = new CategoryLinkServices();
                        var _link = _linkManager.Find(id);
                        if (_link == null) _link = new CategoryLink() { CategoryID = id, Url = "http://" };
                        if (TryUpdateModel(_link)) _response = categoryManager.Update(_category, _link);
                        break;
                }
                if (ModelState.IsValid)
                {
                    if (_response.Code == 1) return View("Prompt", new Prompt() { Title = "修改栏目成功", Message = "修改栏目【" + _category.Name + "】成功" });
                    else return View("Prompt", new Prompt() { Title = "修改栏目失败", Message = "修改栏目【" + _category.Name + "】时发生系统错误，未能保存到数据库，请重试" });
                }
            }
            return View(_category);
        }

        /// <summary>
        /// 常规栏目
        /// </summary>
        /// <param name="id">栏目ID</param>
        /// <returns></returns>
        public ActionResult ModifyGeneral(int id)
        {
            List<SelectListItem> _contentTypeItems = new List<SelectListItem>() { new SelectListItem { Text = "无", Value = "0" } };
            ContentTypeServices _contentTypeManager = new ContentTypeServices();
            var _contentTypes = _contentTypeManager.FindList();
            foreach (var contentType in _contentTypes)
            {
                _contentTypeItems.Add(new SelectListItem() { Value = contentType.ContentTypeID.ToString(), Text = contentType.Name });
            }
            ViewBag.ContentTypeItems = _contentTypeItems;
            var _generalManager = new CategoryGeneralServices();
            var _general = _generalManager.Find(id);
            if (_general == null) _general = new CategoryGeneral() { ContentView = "index", View = "index" };
            return PartialView(_general);
        }

        /// <summary>
        /// 单页栏目
        /// </summary>
        /// <param name="id">栏目ID</param>
        /// <returns></returns>
        public ActionResult ModifyPage(int id)
        {
            var _pageManager = new CategoryPageServices();
            var _page = _pageManager.Find(id);
            if (_page == null) _page = new CategoryPage() { View = "index" };
            return PartialView(_page);
        }

        /// <summary>
        /// 链接栏目
        /// </summary>
        /// <param name="id">栏目ID</param>
        /// <returns></returns>
        public ActionResult ModifyLink(int id)
        {
            var _linkManager = new CategoryLinkServices();
            var _link = _linkManager.Find(id);
            if (_link == null) _link = new CategoryLink() { Url = "http://" };
            return PartialView(_link);
        }


        #endregion

        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <param name="id">栏目ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Response _resp = new Response();
            var _category = categoryManager.Find(id);
            if (_category == null)
            {
                _resp.Code = 0;
                _resp.Message = "栏目不存在";
            }
            else
            {
                if (categoryManager.Count(c => c.ParentID == _category.CategoryID) > 0)
                {
                    _resp.Code = 0;
                    _resp.Message = "该栏目栏目有子栏目，请先删除子栏目";
                }
                else
                {
                    switch (_category.Type)
                    {
                        case CategoryType.General:
                            new CategoryGeneralServices().DeleteByCategoryID(_category.CategoryID);
                            break;
                        case CategoryType.Page:
                            new CategoryPageServices().DeleteByCategoryID(_category.CategoryID);
                            break;
                        case CategoryType.Link:
                            new CategoryLinkServices().DeleteByCategoryID(_category.CategoryID);
                            break;
                    }
                    _resp = categoryManager.Delete(_category.CategoryID);
                }
            }
            return Json(_resp);

        }

    }
}