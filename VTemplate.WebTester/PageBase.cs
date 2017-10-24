using System;
using System.Collections.Generic;
using System.Web;
using VTemplate.Engine;
using System.Text;
using System.IO;
using System.Web.SessionState;
using System.Diagnostics;

namespace VTemplate.WebTester
{
    /// <summary>
    /// 页面基类
    /// </summary>
    public abstract class PageBase : IHttpHandler
    {
        #region IHttpHandler 成员
        /// <summary>
        /// 
        /// </summary>
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            this.InitContext(context);
            this.TestType = context.Request.QueryString["testtype"];

            //判断是否是进行测试
            if (!string.IsNullOrEmpty(this.TestType))
            {
                Response.Buffer = true;
                StartTest(1, 1000);
                StartTest(2, 1000);
                StartTest(3, 1000);
            }
            else
            {
                //输出数据
                this.LoadCurrentTemplate();
                this.InitPageTemplate();
                this.Document.Render(this.Response.Output);
            }
        }
        protected HttpContext Context { get; private set; }
        protected HttpApplicationState Application { get; private set; }
        protected HttpRequest Request { get; private set; }
        protected HttpResponse Response { get; private set; }
        protected HttpServerUtility Server { get; private set; }
        protected HttpSessionState Session { get; private set; }

        /// <summary>
        /// 初始化上下文数据
        /// </summary>
        /// <param name="context"></param>
        private void InitContext(HttpContext context)
        {
            this.Context = context;
            this.Application = context.Application;
            this.Request = context.Request;
            this.Response = context.Response;
            this.Server = context.Server;
            this.Session = context.Session;
        }
        #endregion

        #region 当前页面的测试类型
        /// <summary>
        /// 测试类型
        /// </summary>
        private string TestType { get; set; }
        #endregion

        /// <summary>
        /// 进行测试
        /// </summary>
        /// <param name="num"></param>
        /// <param name="count"></param>
        protected void StartTest(int num, int count)
        {
            //进行测试
            Stopwatch watcher = new Stopwatch();
            watcher.Start();
            for (int i = 0; i < count; i++)
            {
                //装载当前页面的模板
                this.LoadCurrentTemplate();
                //初始化页面模板的数据
                this.InitPageTemplate();
                //不输出.只呈现
                string text = this.Document.GetRenderText();
            }
            watcher.Stop();
            Response.Write(string.Format("第{0}次测试(共运行{1}次)花费时间: {2} ms", num, count, watcher.ElapsedMilliseconds));
            Response.Write("<br />");
            Response.Flush();
        }

        /// <summary>
        /// 当前页面的模板文档对象
        /// </summary>
        protected TemplateDocument Document
        {
            get;
            private set;
        }
        /// <summary>
        /// 当前页面的模板文档的配置参数
        /// </summary>
        protected virtual TemplateDocumentConfig DocumentConfig
        {
            get
            {
                return TemplateDocumentConfig.Default;
            }
        }
        /// <summary>
        /// 是否读取缓存模板
        /// </summary>
        protected virtual bool IsLoadCacheTemplate
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 装载当前页面的模板文档
        /// </summary>
        public virtual void LoadCurrentTemplate()
        {
            string fileName = Path.GetFileNameWithoutExtension(this.Request.FilePath);
            this.LoadTemplateFile(this.Server.MapPath("/template/" + fileName + ".html"));
        }
        /// <summary>
        /// 装载模板文件
        /// </summary>
        /// <param name="fileName"></param>
        protected virtual void LoadTemplateFile(string fileName)
        {
            this.Document = null;
            if ("cache".Equals(this.TestType, StringComparison.InvariantCultureIgnoreCase) || this.IsLoadCacheTemplate)
            {
                //测试缓存模板文档
                this.Document = TemplateDocument.FromFileCache(fileName, Encoding.UTF8, this.DocumentConfig);
            }
            else
            {
                //测试实例模板文档
                this.Document = new TemplateDocument(fileName, Encoding.UTF8, this.DocumentConfig);
            }
        }

        /// <summary>
        /// 初始化当前页面模板数据
        /// </summary>
        protected abstract void InitPageTemplate();
    }
}
