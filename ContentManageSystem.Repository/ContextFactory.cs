using ContentManageSystem.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ContentManageSystem.Repository
{
    /// <summary>
    /// 数据上下文工厂
    /// </summary>
    public class ContextFactory
    {
        /// <summary>
        /// 获取当前线程的数据上下文
        /// </summary>
        /// <returns>数据上下文</returns>
        public static ContentManageSystemContext CurrentContext()
        {
            ContentManageSystemContext _context = CallContext.GetData("ContentManageSystemContext") as ContentManageSystemContext;
            if (_context == null)
            {
                _context = new ContentManageSystemContext();
                CallContext.SetData("ContentManageSystemContext", _context);
            }
            return _context;
        }
    }
}
