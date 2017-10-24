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
    /// 测试Output标签
    /// </summary>
    public class outputtag_test : PageBase
    {
        /// <summary>
        /// 栏目
        /// </summary>
        public class Category
        {
            public int Id;
            public string Path;
            public string Name;
        }
        /// <summary>
        /// 栏目列表
        /// </summary>
        public List<Category> Categories;

        /// <summary>
        /// 获取某个栏目下的子级栏目列表
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public List<Category> GetCategories(Category category)
        {
            string path = category.Path + category.Id.ToString() + "/";

            return this.Categories.FindAll(x => x.Path == path);
        }
        /// <summary>
        /// 初始化当前页面模板数据
        /// </summary>
        protected override void InitPageTemplate()
        {
            //构造一个多级的栏目列表
            this.Categories = new List<Category>();
            this.Categories.Add(new Category() { Id = 1, Path = "/", Name = "A:根级栏目" });
            this.Categories.Add(new Category() { Id = 2, Path = "/", Name = "B:根级栏目" });
            this.Categories.Add(new Category() { Id = 3, Path = "/", Name = "C:根级栏目" });
            this.Categories.Add(new Category() { Id = 4, Path = "/", Name = "D:根级栏目" });

            this.Categories.Add(new Category() { Id = 5, Path = "/1/", Name = "A:一级栏目" });
            this.Categories.Add(new Category() { Id = 6, Path = "/2/", Name = "B:一级栏目" });
            this.Categories.Add(new Category() { Id = 7, Path = "/3/", Name = "C:一级栏目" });
            this.Categories.Add(new Category() { Id = 8, Path = "/4/", Name = "D:一级栏目" });

            this.Categories.Add(new Category() { Id = 9, Path = "/1/5/", Name = "A:二级栏目" });
            this.Categories.Add(new Category() { Id = 10, Path = "/2/6/", Name = "B:二级栏目" });
            this.Categories.Add(new Category() { Id = 11, Path = "/3/7/", Name = "C:二级栏目" });
            this.Categories.Add(new Category() { Id = 12, Path = "/4/8/", Name = "D:二级栏目" });

            this.Categories.Add(new Category() { Id = 9, Path = "/1/5/9/", Name = "A:三级栏目" });
            this.Categories.Add(new Category() { Id = 11, Path = "/3/7/11/", Name = "C:三级栏目" });

            
            this.Document.Variables.SetValue("this", this);
            //输出根级目录
            this.Document.SetValue("categories", this.Categories.FindAll(x => x.Path == "/"));
        }
    }
}
