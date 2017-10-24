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
    /// 测试分页的输出
    /// </summary>
    public class pagedata_test : PageBase
    {
        /// <summary>
        /// 获取分页数据.
        /// </summary>
        /// <param name="pageNumber">要获取的页码</param>
        /// <param name="pageSize">分页的大小</param>
        /// <param name="pageCount">返回页码总数</param>
        /// <returns></returns>
        public List<string> GetPageData(int pageNumber, int pageSize, out int pageCount)
        {
            //这个函数只是测试数据.在实际项目时,你可以通过从数据库获取分页数据
            pageCount = 100;
            List<string> datas = new List<string>();
            for (int i = 0; i < pageSize; i++)
            {
                //随机构造分页的列表数据
                datas.Add(new String('A', (i + 1) % 20));
            }
            return datas;
            
        }

        /// <summary>
        /// 获取导航的地址.通过方法返回是以便进行地址重写,或者参数构造等.
        /// </summary>
        /// <param name="nav"></param>
        /// <returns></returns>
        public string GetPageUrl(string nav)
        {
            if ("first".Equals(nav, StringComparison.InvariantCultureIgnoreCase))
            {
                return "?page=1";
            }
            else if ("previous".Equals(nav, StringComparison.InvariantCultureIgnoreCase))
            {
                return "?page=" + (this.PageNumber - 1).ToString();
            }
            else if ("next".Equals(nav, StringComparison.InvariantCultureIgnoreCase))
            {
                return "?page=" + (this.PageNumber + 1).ToString();
            }
            else if ("last".Equals(nav, StringComparison.InvariantCultureIgnoreCase))
            {
                return "?page=" + this.PageCount.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 获取导航的地址.通过方法返回是以便进行地址重写,或者参数构造等.
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public string GetPageNavigation(object[] data)
        {
            object pageNo = data[0];
            if (pageNo is LoopIndex)
            {
                return "?page=" + ((LoopIndex)pageNo).Value.ToString();
            }
            else if (pageNo is int)
            {
                return "?page=" + ((int)pageNo).ToString();
            }
            else
            {
                return "?page=" + pageNo.ToString();
            }
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNumber;
        /// <summary>
        /// 页码大小
        /// </summary>
        public int PageSize;
        /// <summary>
        /// 页码总数
        /// </summary>
        public int PageCount;


        /// <summary>
        /// 初始化当前页面模板数据
        /// </summary>
        protected override void InitPageTemplate()
        {
            //注册一个变量函数.是给样式一中的{$:#.i call='GetPageNavigation'} 调用
            //样式二是通过<vt:function>标签调用GetPageNavigation函数.这两种方法你可以根据需要灵活使用.VT就是这么灵活:)
            this.Document.UserDefinedFunctions.Add(this.GetPageNavigation);


            this.Document.Variables.SetValue("this", this);

            this.PageNumber = 1;            
            this.PageSize = 10;
            //在实际项目中.PageSize的值最好也定制在模板中, 比如下面的代码
            Tag tag = this.Document.GetChildTagById("pagetable");
            if (tag != null)
            {
                this.PageSize = int.Parse(tag.Attributes.GetValue("pagesize", "10"));
            }

            if (!string.IsNullOrEmpty(this.Request.QueryString["page"])) this.PageNumber = int.Parse(this.Request.QueryString["page"]);

            //输出分页数据
            this.Document.SetValue("pagedata", this.GetPageData(this.PageNumber, this.PageSize, out this.PageCount));
        }
    }
}
