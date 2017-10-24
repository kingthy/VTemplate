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
    /// 博客日记页面测试
    /// </summary>
    public class blogarchive : PageBase
    {
        /// <summary>
        /// 获取博客日记数据
        /// </summary>
        /// <returns></returns>
        private object GetBlogArchive()
        {
            //示例里我们只随便构造一个对象.在实际项目里你可以从数据库获取博客日记的真正数据
            return new
            {
                title = "这只是一篇测试的博客日记",
                content = "你好,这只是用于测试VTemplate模板引擎所用的博客日记",
                author = "kingthy",
                time = DateTime.Now,
                comments = new object[]{
                    new {author = "张三", time = DateTime.Now.AddDays(-1), content = "沙发"},
                    new {author = "李四", time = DateTime.Now.AddDays(0), content = "顶楼主"},
                    new {author = "王五", time = DateTime.Now.AddDays(1), content = "板凳啊"}
                }
            };
        }
        /// <summary>
        /// 初始化当前页面模板数据
        /// </summary>
        protected override void InitPageTemplate()
        {
            //对VT模板里的blogarchive变量赋值
            this.Document.Variables.SetValue("blogarchive", this.GetBlogArchive());
        }
    }
}
