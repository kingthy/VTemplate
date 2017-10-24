using System;
using System.Web;
using System.Web.SessionState;
using System.ComponentModel;
using System.Threading;
using System.Web.Caching;
using System.IO;
using VTemplate.Engine;
using System.Text;

namespace VTemplate.WebTester
{
    /// <summary>
    ///  模板页面基类
    ///  注: PageBase只是提供一个简单的用于测试VTemplate速度的基类,而TemplatePageBase则是根据某些朋友的需要而增加的.有需要的朋友则可以用此基类代替PageBase并可在实际项目中使用.
    ///      TemplatePageBase 提供了详细的页面流程事件控制和缓存处理等.其页面流程类似于System.Web.UI.Page的流程
    ///      页面流程： OnInit(页面初始化) -> OnLoad(页面装载) -> OnRender(页面数据呈现) -> OnUnLoad(页面卸载)
    ///      一般要处理页面模板数据则是在InitPageTemplate方法进行。而需要进行其它需要则可以重载上面的流程事件方法以便进行页面流程控制（比如需要进行页面权限判断，则可重载OnInit事件函数进行判断）
    ///      
    ///  注意： LoadCurrentTemplate 这个方法请根据你的实际项目编写！
    /// </summary>
    public abstract class TemplatePageBase
        : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 
        /// </summary>
        protected TemplatePageBase() { }

        #region IHttpHandler 成员
        /// <summary>
        /// 是否可重用
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// 处理每个请求
        /// </summary>
        /// <param name="context"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            this.InitHttpContext(context);

            try
            {
                this.OnInit(EventArgs.Empty);
                this.OnLoad(EventArgs.Empty);
                this.OnRender(EventArgs.Empty);
            }
            catch (ThreadAbortException)
            {
                if (this.IsValid && !_IsRenderPage && !Response.IsRequestBeingRedirected)
                {
                    //输出最后的数据
                    this.Document.Render(Response.Output);
                }
            }
            catch (Exception ex)
            {
                //触发错误通知事件
                this.Context.AddError(ex);
                this.OnError(this, EventArgs.Empty);
                if (this.Context.Error != null) throw;
            }
            finally
            {
                this.OnUnLoad(EventArgs.Empty);
            }
        }
        #endregion

        #region  Context、Request、Response、Server、Session、Application, Cache
        /// <summary>
        /// 初始化HTTP上下文参数
        /// </summary>
        /// <param name="context"></param>
        private void InitHttpContext(HttpContext context)
        {
            this.Context = context;
            this.Application = context.Application;
            this.Request = context.Request;
            this.Response = context.Response;
            this.Server = context.Server;
            this.Session = context.Session;
            this.Cache = context.Cache;
        }
        /// <summary>
        /// 获取与该页关联的 HttpContext 对象。
        /// </summary>
        public HttpContext Context { get; private set; }
        /// <summary>
        /// 获取请求的页的 HttpRequest 对象。
        /// </summary>
        public HttpRequest Request { get; private set; }
        /// <summary>
        /// 获取与该 Page 对象关联的 HttpResponse 对象。该对象使您得以将 HTTP 响应数据发送到客户端，并包含有关该响应的信息。
        /// </summary>
        public HttpResponse Response { get; private set; }
        /// <summary>
        /// 获取 Server 对象
        /// </summary>
        public HttpServerUtility Server { get; private set; }
        /// <summary>
        /// 获取 ASP.NET 提供的当前 Session 对象。
        /// </summary>
        public HttpSessionState Session { get; private set; }
        /// <summary>
        /// 为当前 Web 请求获取 HttpApplicationState 对象。
        /// </summary>
        public HttpApplicationState Application { get; private set; }

        /// <summary>
        /// 获取当前应用程序域的System.Web.Caching.Cache 对象
        /// </summary>
        public Cache Cache { get; private set; }
        #endregion

        #region 页面处理流程
        /// <summary>
        /// 页面初始化开始
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnInit(EventArgs e) { }

        /// <summary>
        /// 装载页面数据
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLoad(EventArgs e)
        {
            if (this.IsPostBack)
            {
                //POST方式则不处理缓存数据
                this.InitPageTemplate();
            }
            else
            {
                //优先装载缓存数据
                if (!this.LoadPageCache())
                {
                    this.InitPageTemplate();
                }
            }
        }
        /// <summary>
        /// 是否已呈现过页面
        /// </summary>
        private bool _IsRenderPage = false;
        /// <summary>
        /// 呈现页面数据
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRender(EventArgs e)
        {
            _IsRenderPage = true;
            if (!this.IsPostBack)
            {
                //GET方式访问才保存页面缓存
                this.SavePageCache();
            }
            //输出页面数据
            if (this.IsValid)
            {
                this.Document.Render(Response.Output);
            }
        }

        /// <summary>
        /// 页面卸载
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnUnLoad(EventArgs e)
        {

        }

        /// <summary>
        /// 初始化当前访问页面的模板数据
        /// </summary>
        public abstract void InitPageTemplate();


        /// <summary>
        /// 当页面发生错误时的事件
        /// </summary>
        protected virtual void OnError(object sender, EventArgs e)
        {
        }
        #endregion

        #region 模板处理
        private TemplateDocument _Document = null;
        /// <summary>
        /// 当前页面的模板文档对象
        /// </summary>
        public TemplateDocument Document
        {
            get
            {
                return _Document;
            }
            protected set
            {
                _Document = value;
            }
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
            //注: 以下模板路径只是参考!请根据你的现实情况进行修改.
            //    建议是配合配置文件一起使用.
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
            if (this.IsLoadCacheTemplate)
            {
                //缓存模板文档
                this.Document = TemplateDocument.FromFileCache(fileName, Encoding.UTF8, this.DocumentConfig);
            }
            else
            {
                //实例模板文档
                this.Document = new TemplateDocument(fileName, Encoding.UTF8, this.DocumentConfig);
            }

            //设置当前页面对象的值
            this.Document.SetValue("Page", this);
        }
        #endregion

        #region 常用属性
        /// <summary>
        /// 返回判断当前页面是否有效(模板是否已初始化)
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _Document != null;
            }
        }

        /// <summary>
        /// 当前页面是否是以POST方式访问
        /// </summary>
        public bool IsPostBack
        {
            get
            {
                return "POST".Equals(Request.RequestType, StringComparison.InvariantCultureIgnoreCase);
            }
        }
        #endregion

        #region 处理页面缓存
        /// <summary>
        /// 当前页面的缓存过期时效(单位:秒钟,默认为1小时,如果小于等于0则不缓存)
        /// </summary>
        protected virtual int PageCacheExpireTime
        {
            get
            {
                return 3600;
            }
        }
        /// <summary>
        /// 当前页面的缓存文件绝对地址(如果需要使用页面缓存则必须重写此属性并返回真实地址)
        /// </summary>
        protected virtual string PageCacheFileName
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 装载页面缓存,此方法默认在OnLoad事件发生时自动调用
        /// </summary>
        /// <returns>是否装载页面缓存成功</returns>
        protected virtual bool LoadPageCache()
        {
            if (this.PageCacheExpireTime > 0 && !string.IsNullOrEmpty(this.PageCacheFileName))
            {
                try
                {
                    FileInfo cacheFile = new FileInfo(this.PageCacheFileName);
                    if (cacheFile.Exists)
                    {
                        //判断文件是否没过期
                        TimeSpan span = DateTime.Now.Subtract(cacheFile.LastWriteTime);
                        if (span.TotalSeconds >= 0 && span.TotalSeconds <= this.PageCacheExpireTime)
                        {
                            //输出缓存文件数据
                            HttpResponse response = (HttpContext.Current != null ? HttpContext.Current.Response : null);
                            if (response != null)
                            {
                                response.Clear();
                                response.WriteFile(this.PageCacheFileName);
                                return true;
                            }
                        }
                    }
                }
                catch { }
            }
            return false;
        }

        /// <summary>
        /// 保存页面缓存,此方法默认在OnRender事件发生时自动调用
        /// </summary>
        protected virtual void SavePageCache()
        {
            if (this.PageCacheExpireTime > 0 && !string.IsNullOrEmpty(this.PageCacheFileName))
            {
                //获取模板文档的数据
                if (this.IsValid)
                {
                    try
                    {
                        string path = Path.GetDirectoryName(this.PageCacheFileName);
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                        this.Document.RenderTo(this.PageCacheFileName, this.Document.Charset);
                    }
                    catch { }
                }
            }
        }
        #endregion
    }
}
