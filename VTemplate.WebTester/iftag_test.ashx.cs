using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using VTemplate.Engine;
using System.Text;

namespace VTemplate.WebTester
{
    /// <summary>
    /// 测试If标签
    /// </summary>
    public class iftag_test : PageBase
    {
        public enum UserTypes
        {
            A,
            B,
            C,
            D
        }
        /// <summary>
        /// 初始化当前页面模板数据
        /// </summary>
        protected override void InitPageTemplate()
        {
            this.Document.Variables.SetValue("user", new { name = "三王", age = 20, Type = UserTypes.B });
        }
    }
}
