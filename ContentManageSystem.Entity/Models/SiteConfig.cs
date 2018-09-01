﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManageSystem.Entity.Models
{
    /// <summary>
    /// 网站配置类
    /// </summary>
    public class SiteConfig : ConfigurationSection
    {
        private static ConfigurationProperty _property = new ConfigurationProperty(string.Empty, typeof(KeyValueConfigurationCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        private KeyValueConfigurationCollection keyValues
        {
            get { return (KeyValueConfigurationCollection)base[_property]; }
            set { base[_property] = value; }
        }


        /// <summary>
        ///网站名称
        /// </summary>
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "最多{1}个字符")]
        [Display(Name = "网站名称")]
        public string SiteName
        {
            get { return keyValues["SiteName"] == null ? string.Empty : keyValues["SiteName"].Value; }
            set { keyValues["SiteName"].Value = value; }
        }

        /// <summary>
        ///网站标题
        /// </summary>
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "最多{1}个字符")]
        [Display(Name = "网站标题")]
        public string SiteTitle
        {
            get { return keyValues["SiteTitle"] == null ? string.Empty : keyValues["SiteTitle"].Value; }
            set { keyValues["SiteTitle"].Value = value; }
        }

        /// <summary>
        ///网站地址
        /// </summary>
        [DataType(DataType.Url)]
        [Required(ErrorMessage = "*")]
        [StringLength(500, ErrorMessage = "最多{1}个字符")]
        [Display(Name = "网站地址")]
        public string SiteUrl
        {
            get { return keyValues["SiteUrl"] == null ? "http://" : keyValues["SiteUrl"].Value; }
            set { keyValues["SiteUrl"].Value = value; }
        }

        /// <summary>
        ///Meta关键词
        /// </summary>
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "最多{1}个字符")]
        [Display(Name = "Meta关键词")]
        public string MetaKeywords
        {
            get { return keyValues["MetaKeywords"] == null ? string.Empty : keyValues["MetaKeywords"].Value; }
            set { keyValues["MetaKeywords"].Value = value; }
        }

        /// <summary>
        ///Meta描述
        /// </summary>
        [DataType(DataType.MultilineText)]
        [StringLength(1000, ErrorMessage = "最多{1}个字符")]
        [Display(Name = "Meta描述")]
        public string MetaDescription
        {
            get { return keyValues["MetaDescription"] == null ? string.Empty : keyValues["MetaDescription"].Value; }
            set { keyValues["MetaDescription"].Value = value; }
        }

        /// <summary>
        ///版权信息
        /// </summary>
        [DataType(DataType.MultilineText)]
        [StringLength(1000, ErrorMessage = "最多{1}个字符")]
        [Display(Name = "版权信息")]
        public string Copyright
        {
            get { return keyValues["Copyright"] == null ? "Ninesky 版权所有" : keyValues["Copyright"].Value; }
            set { keyValues["Copyright"].Value = value; }
        }

    }
}
