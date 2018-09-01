using ContentManageSystem.Entity.Models;
using ContentManageSystem.Entity.Models.Category;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManageSystem.Entity
{
    /// <summary>
    /// 数据上下文
    /// </summary>
    public class ContentManageSystemContext : DbContext
    {
        public ContentManageSystemContext() : base("ContentManageSystem")//base("DefaultConnection")
        {
            Database.SetInitializer<ContentManageSystemContext>(new CreateDatabaseIfNotExists<ContentManageSystemContext>());
        }

        /// <summary>
        /// 管理员集合
        /// </summary>
        public DbSet<Admin> Administrators { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<User> Users { get; set; }

        #region 栏目

        /// <summary>
        /// 栏目
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// 常规栏目
        /// </summary>
        public DbSet<CategoryGeneral> CategoryGeneral { get; set; }

        /// <summary>
        /// 单页栏目
        /// </summary>
        public DbSet<CategoryPage> CategoryPages { get; set; }

        /// <summary>
        /// 链接栏目
        /// </summary>
        public DbSet<CategoryLink> CategoryLinks { get; set; }

        #endregion

        #region 内容
        /// <summary>
        /// 内容类型
        /// </summary>
        public DbSet<ContentType> ContentTypes { get; set; }

        #endregion

    }
}
