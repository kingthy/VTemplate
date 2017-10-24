using System;
using System.Collections.Generic;
using System.Web;
using VTemplate.Engine;

namespace VTemplate.WebTester.Core
{
    /// <summary>
    /// 模板块解析器
    /// </summary>
    public class CnblogsNewsRender : ITemplateRender
    {
        #region ITemplateRender 成员
        /// <summary>
        /// 解析某个模板块的数据
        /// </summary>
        /// <param name="template"></param>
        public void PreRender(Template template)
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
                tag.BeforeRender += (sender, e) =>
                {
                    ForEachTag t = (ForEachTag)sender;

                    //取得当前项的值(因为foreach标签的数据源是List<News>集合,所以当前项的值类型为News实体)
                    News news = (News)t.Item.Value;
                    //设置当前项的变量表达式的值.也即是"{$:#.news.url}"变量表达式
                    t.Item.SetExpValue("url", NewsDbProvider.GetNewsUrl(news));

                    //当新闻不可见时.你可以取消本次输出
                    if (!news.Visible) e.Cancel = true;
                };
            }
        }
        #endregion
    }
}
