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
    /// template与include标签的差别
    /// </summary>
    public class template_include_test : PageBase
    {
        /// <summary>
        /// 初始化当前页面模板数据
        /// </summary>
        protected override void InitPageTemplate()
        {
            this.Document.Variables.SetValue("var1", 1);
            this.Document.Variables.SetValue("names", new string[] { "张三", "李四", "王五" });
        }
    }
}
