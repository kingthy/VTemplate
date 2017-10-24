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
    public class DataItem
    {
        public string Title;
        public int Id;
    }
    /// <summary>
    /// 测试template标签
    /// </summary>
    public class groupdata_test : PageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public List<string> GetItemData(DataItem o)
        {
            //你可以通过数据库返回对应栏目的数据.这里只是做测试.直接返回测试数据
            if (o.Id % 2 == 0)
            {
                return new List<string>() { "EEEEEEEEEEEE", "FFFFFFFFFFFFF", "GGGGGGGGGGG" };
            }
            else
            {
                return new List<string>() { "AAAAAAAAAAAA", "BBBBBBBBBBBBB", "CCCCCCCCCCC" };
            }
        }

        /// <summary>
        /// 初始化当前页面模板数据
        /// </summary>
        protected override void InitPageTemplate()
        {
            List<object> datalist = new List<object>();
            datalist.Add(new DataItem() { Title = "栏目A", Id = 1 });
            datalist.Add(new DataItem() { Title = "栏目B", Id = 2 });
            datalist.Add(new DataItem() { Title = "栏目C", Id = 3 });
            datalist.Add(new DataItem() { Title = "栏目D", Id = 4 });


            this.Document.Variables.SetValue("this", this);
            this.Document.Variables.SetValue("datalist", datalist);
        }
    }
}
