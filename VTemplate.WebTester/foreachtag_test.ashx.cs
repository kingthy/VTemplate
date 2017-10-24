using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using VTemplate.Engine;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace VTemplate.WebTester
{
    /// <summary>
    /// 测试foreach标签
    /// </summary>
    public class foreachtag_test : PageBase
    {
        /// <summary>
        /// 初始化当前页面模板数据
        /// </summary>
        protected override void InitPageTemplate()
        {
            //集合数据
            List<object> users = new List<object>()
            {
                new {Name = "张三", age=20},
                new {Name = "李四", age=24},
                new {Name = "王五", age=30}
            };
            this.Document.Variables.SetValue("users", users);

            //DataTable 数据
            //注: 进入foreach循环后的数据是DataTable对象的DefaultView属性.所以item里的数据对象是DataRowView类型
            DataTable userTable = new DataTable();
            userTable.Columns.Add("Name", typeof(string));
            userTable.Columns.Add("Age", typeof(int));
            userTable.Rows.Add("张三", 20);
            userTable.Rows.Add("李四", 24);
            userTable.Rows.Add("王五", 30);
            this.Document.Variables.SetValue("usertable", userTable);


            //字典
            //注: 进入foreach循环后的数据是KeyValuePair<T,T>对象.所以模板里是通过Key与Value属性取得键与值.
            Dictionary<string, int> userDict = new Dictionary<string, int>();
            userDict.Add("张三", 20);
            userDict.Add("李四", 24);
            userDict.Add("王五", 30);
            this.Document.Variables.SetValue("userdict", userDict);
            

            //数组
            object[] userArray = new object[]{
                new {Name = "张三", age=20},
                new {Name = "李四", age=24},
                new {Name = "王五", age=30}
            };
            this.Document.Variables.SetValue("userarray", userArray);
        }
    }
}
