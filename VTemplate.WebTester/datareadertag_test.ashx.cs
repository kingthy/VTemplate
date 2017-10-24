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
    /// 测试DataReader标签
    /// </summary>
    public class datareadertag_test : PageBase
    {
        /// <summary>
        /// 重载父级方法.返回一个可使用DataReader标签的文档配置
        /// </summary>
        protected override TemplateDocumentConfig DocumentConfig
        {
            get
            {
                return new TemplateDocumentConfig(TagOpenMode.Full);
            }
        }
        /// <summary>
        /// 初始化当前页面模板数据
        /// </summary>
        protected override void InitPageTemplate()
        {
        }
    }
}
