using ContentManageSystem.Common;
using ContentManageSystem.Entity.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManageSystem.Services.Category
{
    /// <summary>
    /// 链接栏目管理
    /// </summary>
    public class CategoryLinkServices : BaseServices<CategoryLink>
    {
        /// <summary>
        /// 删除链接栏目-根据栏目ID
        /// </summary>
        /// <param name="categoryID">栏目ID</param>
        /// <returns></returns>
        public Response DeleteByCategoryID(int categoryID)
        {
            return base.Delete(categoryID);
        }
    }
}
