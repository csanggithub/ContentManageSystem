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
    /// 常规栏目管理
    /// </summary>
    public class CategoryGeneralServices : BaseServices<CategoryGeneral>
    {
        /// <summary>
        /// 删除常规栏目-根据栏目ID
        /// </summary>
        /// <param name="categoryID">栏目ID</param>
        /// <returns></returns>
        public Response DeleteByCategoryID(int categoryID)
        {
            return base.Delete(categoryID);
        }
    }
}
