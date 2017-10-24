using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using VTemplate.Engine;
using System.Text;
using System.Collections.Generic;

namespace VTemplate.WebTester
{
    /// <summary>
    /// 测试template标签
    /// </summary>
    public class templatetag_test : PageBase
    {
        /// <summary>
        /// 初始化当前页面模板数据
        /// </summary>
        protected override void InitPageTemplate()
        {
            List<object> users1 = new List<object>()
            {
                new {Name = "张三", age="20"},
                new {Name = "李四", age="24"},
                new {Name = "王五", age="30"}
            };

            List<object> users2 = new List<object>()
            {
                new {Name = "黄丽", age="18"},
                new {Name = "张燕", age="20"},
                new {Name = "陈梅", age="23"}
            };
            this.Document.Variables.SetValue("users", users1);
            this.Document.GetChildTemplateById("female").Variables.SetValue("users", users2);
        }
    }
}
