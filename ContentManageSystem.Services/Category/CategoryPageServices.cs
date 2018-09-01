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
    /// 单页栏目管理
    /// </summary>
    public class CategoryPageServices : BaseServices<CategoryPage>
    {
        /// <summary>
        /// 删除单页栏目-根据栏目ID
        /// </summary>
        /// <param name="categoryID">栏目ID</param>
        /// <returns></returns>
        public Response DeleteByCategoryID(int categoryID)
        {
            return base.Delete(categoryID);
        }
    }
}
