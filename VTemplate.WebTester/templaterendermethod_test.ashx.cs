using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using VTemplate.Engine;
using System.Text;
using System.Collections.Generic;
using VTemplate.WebTester.Core;

namespace VTemplate.WebTester
{
    /// <summary>
    /// 博客园新闻列表测试例子
    /// </summary>
    public class templaterendermethod_test : PageBase
    {
        /// <summary>
        /// 初始化当前页面模板数据
        /// </summary>
        protected override void InitPageTemplate()
        {
            /**
             * 
             * 模板块的解析权已交给Core.CnblogsNewsRenderMethod.cs文件.所以页面程序里就不需要再对模板块进行解析了
             * 
             **/            
        }
    }
}
