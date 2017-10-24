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
    /// 测试function标签
    /// </summary>
    public class functiontag_test : PageBase
    {
        /// <summary>
        /// 获取热门的新闻数据
        /// </summary>
        /// <returns></returns>
        public static List<News> GetHotingNews()
        {
            throw new Exception("ddd");
            return NewsDbProvider.GetNewsData("hoting");
        }

        /// <summary>
        /// 初始化当前页面模板数据
        /// </summary>
        protected override void InitPageTemplate()
        {
            //注册一个自定义函数
            this.Document.SetValue("Page", this);
            this.Document.RegisterGlobalFunction(this.GetNewsUrl);
            this.Document.SetValue("newsdata", NewsDbProvider.GetNewsData("relating"));
        }

        public string GetNewsUrl2(byte v)
        {
            return "aaa";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        private object GetNewsUrl(object[] news)
        {
            if (news.Length > 0 && news[0] is News)
            {
                return NewsDbProvider.GetNewsUrl((News)news[0]);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
