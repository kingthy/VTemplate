using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using VTemplate.Engine;
using System.Text;

namespace VTemplate.WebTester
{
    /// <summary>
    /// 测试属性的变量表达式
    /// </summary>
    public class attributeexp_test : PageBase
    {
        /// <summary>
        /// 初始化当前页面模板数据
        /// </summary>
        protected override void InitPageTemplate()
        {
            this.Document.Variables.SetValue("this", this);
            this.Document.Variables.SetValue("users", new[] { new { Id = 1, Name = "张三" }, new { Id = 2, Name = "李四" }, new { Id = 3, Name = "王五" } });
        }

        /// <summary>
        /// 获取用户的资料
        /// </summary>
        /// <returns></returns>
        public object GetUserProfile()
        {
            Tag tag = this.Document.CurrentRenderingTag;

            //获取自定义属性
            var attribute = tag.Attributes["userid"];
            if (attribute == null) return null;

            //获取自定义属性的值
            int userId = Convert.ToInt32(attribute.Value.GetValue());

            //取得了用户的userid，所以后面可以通过其它方式取得对应用户的资料。
            //此处是演示用，所以直接返回不同的资料
            if (userId == 1)
            {
                return new { Level = "管理员", City = "广州市", Province = "广东省"};
            }
            else if (userId == 2)
            {
                return new { Level = "版主", City = "上海市", Province = "上海" };
            }
            else
            {
                return new { Level = "普通会员", City = "未知", Province = "未知" };
            }
        }
    }
}
