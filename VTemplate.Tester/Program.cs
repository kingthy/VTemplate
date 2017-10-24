using System;
using System.Collections.Generic;
using System.Text;
using VTemplate.Engine;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections;
using System.Windows.Forms;

namespace TemplateTester
{
    class Program
    {
        static object CreateType(string typeName)
        {
            Type type;
            //取当前执行代码所在的程序集
            Assembly assembly = Assembly.GetExecutingAssembly();
            object value = assembly.CreateInstance(typeName, true);
            if (value == null)
            {
                //搜索程序域下的各个程序集
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly item in assemblies)
                {
                    value = item.CreateInstance(typeName, true);
                    if (value != null) break;
                }
            }
            return value;
        }
        /// <summary>
        /// 获取某个属性的值
        /// </summary>
        /// <param name="container">数据源</param>
        /// <param name="propName">属性名</param>
        /// <param name="exist">是否存在此属性</param>
        /// <returns>属性值</returns>
        internal static object GetPropertyValue(object container, string propName, out bool exist)
        {
            exist = false;
            object value = null;
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            if (string.IsNullOrEmpty(propName))
            {
                throw new ArgumentNullException("propName");
            }
            PropertyDescriptor descriptor = TypeDescriptor.GetProperties(container).Find(propName, true);
            if (descriptor != null)
            {
                exist = true;
                value = descriptor.GetValue(container);
            }
            else if (container is IDictionary)
            {
                //是IDictionary集合
                IDictionary idic = (IDictionary)container;
                if (idic.Contains(propName))
                {
                    exist = true;
                    value = idic[propName];
                }
            }
            else if (container is NameObjectCollectionBase)
            {
                NameObjectCollectionBase nob = (NameObjectCollectionBase)container;
                MethodInfo method = nob.GetType().GetMethod("BaseGet", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(string) }, new ParameterModifier[] { new ParameterModifier(1) });
                if (method != null)
                {
                    value = method.Invoke(container, new object[] { propName });
                    exist = value == null;
                }
            }
            else if (container is Type)
            {
                Type type = (Type)container;
                //查找字段
                FieldInfo field = type.GetField(propName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                if (field != null)
                {
                    exist = true;
                    value = field.GetValue(container);
                }
                else
                {
                    //查找属性
                    PropertyInfo property = type.GetProperty(propName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase, null, null, new Type[0], new ParameterModifier[0]);
                    if (property != null)
                    {
                        exist = true;
                        value = property.GetValue(container, null);
                    }
                    else
                    {
                        //查找方法
                        MethodInfo method = type.GetMethod(propName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase, null, new Type[0], new ParameterModifier[0]);
                        if (method != null)
                        {
                            value = method.Invoke(container, null);
                            exist = value == null;
                        }
                    }
                }
            }
            return value;
        }
        static void Main(string[] args)
        {
            //Application.
            Type e = typeof(System.Environment);
            //Environment.CurrentDirectory
            //Environment.GetCommandLineArgs();
            bool exist;
            object value = GetPropertyValue(e, "GetCommandLineArgs", out exist);
            Console.WriteLine(value);

            object type = CreateType("System.DateTime");
            //type.GetType().f
            //string args
            //for (var j = 1; j < 100; j++)
            //{
            //    Stopwatch watcher = new Stopwatch();
            //    watcher.Start();
            //    TemplateDocument document = TemplateDocument.FromFileCache(@"template1.html", Encoding.Default);
            //    document.Variables.SetValue("users", new List<object>(){  new {name="张三",age=12},
            //                                                        new {name="李四",age=20},
            //                                                        new {name="王五",age=12},});
            //    document.SetValue("user", new { name = "李四", age = 20 });
            //    document.SetValue("user1", new { name = "李四", age = 25 });

                
            //    var tags = document.GetChildTagsByTagName("for");
            //    foreach (var t in tags)
            //    {
            //        t.BeforeRender += (sender, e) =>
            //        {
            //            ForTag tag = (ForTag)sender;
            //            LoopIndex index = (LoopIndex)tag.Index.Value;
            //            if (index.IsEven) e.Cancel = true;     
                   
            //            tag.OwnerTemplate.Variables.SetExpValue("i.max", int.MaxValue / index.ToInt32(null));
            //        };
                   
            //    }
            //    string dt = document.GetRenderText();
            //    if(j < 3)Console.WriteLine(dt);
            //    watcher.Stop();
            //    Console.WriteLine("{0} 次运行时间: {1} ms.", j, watcher.ElapsedMilliseconds);
            //}
            //Stopwatch watcher = new Stopwatch();

            //for (var j = 1; j < 5; j++)
            //{
            //    watcher.Reset();
            //    watcher.Start();
            //    for (var i = 0; i < 1000; i++)
            //    {
            //        TemplateDocument document = TemplateDocument.FromFileCache(@"template1.html", Encoding.Default);
            //        document.SetValue("users", new List<object>(){  new {name="张三",age=12},
            //                                                        new {name="李四",age=20},
            //                                                        new {name="王五",age=12},});
            //        document.SetValue("user", new { name = "李四", age = 20 });
            //        string text = document.RenderText;
            //    }
            //    watcher.Stop();
            //    Console.WriteLine("{0} 次运行时间: {1} ms.", j, watcher.ElapsedMilliseconds);
            //}
            Console.Read();
        }
    }
}
