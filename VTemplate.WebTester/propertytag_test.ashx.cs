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
    /// 测试property标签
    /// </summary>
    public class propertytag_test : PageBase
    {
        /// <summary>
        /// 获取热门的新闻数据
        /// </summary>
        /// <returns></returns>
        public static List<News> HotingNews
        {
            get
            {
                return NewsDbProvider.GetNewsData("hoting");
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
