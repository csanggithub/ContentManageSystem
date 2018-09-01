using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContentManageSystem.Web.Models
{
    /// <summary>
    /// 树形节点
    /// </summary>
    public class TreeNode
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 父节点ID
        /// </summary>
        public int pId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        public string label { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int value { get; set; }
        /// <summary>
        /// HTML
        /// </summary>
        public string html { get; set; }

        /// <summary>
        /// 禁用
        /// </summary>
        public bool disabled { get; set; }

        public bool @checked { get; set; }

        public bool expanded { get; set; }

        public bool selected { get; set; }


        /// <summary>
        /// 子节点
        /// </summary>
        public List<TreeNode> items { get; set; }
    }
}