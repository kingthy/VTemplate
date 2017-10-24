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
    /// CurrentRenderTag属性的使用
    /// </summary>
    public class currentrendertag_test : PageBase
    {
        /// <summary>
        /// 初始化当前页面模板数据
        /// </summary>
        protected override void InitPageTemplate()
        {
            this.Document.Variables.SetValue("this", this);
        }

        /// <summary>
        /// 用于给模板调用的方法
        /// </summary>
        /// <returns></returns>
        public List<News> GetNewsData()
        {
            //获取当前正在呈现数据的标签,也就是正在调用此函数的标签
            Tag tag = this.Document.CurrentRenderingTag;
            string type = "relating"; //默认的
            if (tag != null)
            {
                //标签不为null.也就是此函数是通过标签调用的
                type = tag.Attributes.GetValue("dbtype");
            }
            return NewsDbProvider.GetNewsData(type);
        }

        /// <summary>
        /// 用于给模板调用的方法(静态方法)
        /// </summary>
        /// <returns></returns>
        public static List<News> GetStaticNewsData()
        {
            //获取正在呈现数据的文档
            TemplateDocument document = TemplateDocument.CurrentRenderingDocument;
            string type = "relating"; //默认的
            if (document != null)
            {
                if (document.CurrentRenderingTag != null)
                {
                    //正在呈现的标签不为null.也就是此函数是通过标签调用的
                    type = document.CurrentRenderingTag.Attributes.GetValue("dbtype");
                }
            }
            return NewsDbProvider.GetNewsData(type);
        }
    }
}
