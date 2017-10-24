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
    public class cnblogs_newslist : PageBase
    {
        /// <summary>
        /// 初始化当前页面模板数据
        /// </summary>
        protected override void InitPageTemplate()
        {
            //获取所有名称为topnews的模板块
            ElementCollection<Template> templates = this.Document.GetChildTemplatesByName("topnews");
            foreach (Template template in templates)
            {
                //根据模板块里定义的type属性条件取得新闻数据
                List<News> newsData = NewsDbProvider.GetNewsData(template.Attributes.GetValue("type"));
                //设置变量newsdata的值
                template.Variables.SetValue("newsdata", newsData);

                //取得模板块下Id为newslist的标签(也即是在cnblogs_newsdata.html文件中定义的foreach标签)
                Tag tag = template.GetChildTagById("newslist");
                if (tag is ForEachTag)
                {
                    //如果标签为foreach标签则设置其BeforeRender事件用于设置变量表达式{$:#.news.url}的值
                    tag.BeforeRender += new System.ComponentModel.CancelEventHandler(Tag_BeforeRender);
                }
            }
        }

        /// <summary>
        /// 在标签每次呈现数据前触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Tag_BeforeRender(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ForEachTag t = (ForEachTag)sender;

            #region 方法一: 根据foreach标签的Item属性取得变量
            //取得当前项的值(因为foreach标签的数据源是List<News>集合,所以当前项的值类型为News实体)
            News news = (News)t.Item.Value;
            //设置当前项的变量表达式的值.也即是"{$:#.news.url}"变量表达式
            t.Item.SetExpValue("url", NewsDbProvider.GetNewsUrl(news));
            #endregion

            #region 方法二: 直接获取news变量
            //或者也可以直接取得news变量
            //Variable newsVar = t.OwnerTemplate.Variables["news"];
            //News news = (News)newsVar.Value;
            //newsVar.SetExpValue("url", NewsDbProvider.GetNewsUrl(news));
            #endregion

            //当新闻不可见时.你可以取消本次输出
            if (!news.Visible) e.Cancel = true;
        }
    }
}
