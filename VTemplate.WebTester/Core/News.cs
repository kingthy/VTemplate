using System;
using System.Collections.Generic;
using System.Web;

namespace VTemplate.WebTester.Core
{
    /// <summary>
    /// 新闻实体
    /// </summary>
    public class News
    {
        /// <summary>
        /// 新闻Id
        /// </summary>
        public int Id;
        /// <summary>
        /// 新闻标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible;
    }
}
