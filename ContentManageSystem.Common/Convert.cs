using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContentManageSystem.Common
{
    /// <summary>
    /// 转换类
    /// </summary>
    public class Convert
    {
        /// <summary>
        /// 转换枚举类型为详细信息列表
        /// </summary>
        /// <param name="_enum">枚举类型</param>
        /// <returns></returns>
        public static List<EnumItemDetails> EnumToDetailsList(Enum _enum)
        {
            List<EnumItemDetails> _itemDetails = new List<EnumItemDetails>();
            //字段元数据
            FieldInfo _fileInfo;
            //显示属性
            DisplayAttribute _displayAttribute;
            foreach (var _item in Enum.GetValues(_enum.GetType()))
            {
                try
                {
                    _fileInfo = _item.GetType().GetField(_item.ToString());
                    _displayAttribute = (DisplayAttribute)_fileInfo.GetCustomAttribute(typeof(DisplayAttribute));
                    if (_displayAttribute != null) _itemDetails.Add(new EnumItemDetails() { Text = _item.ToString(), Value = (int)_item, Name = _displayAttribute.Name, Description = _displayAttribute.Description });
                    else _itemDetails.Add(new EnumItemDetails() { Text = _item.ToString(), Value = (int)_item });
                }
                catch
                {
                    _itemDetails.Add(new EnumItemDetails() { Text = _item.ToString(), Value = (int)_item });
                }
            }
            return _itemDetails;
        }
    }
}
