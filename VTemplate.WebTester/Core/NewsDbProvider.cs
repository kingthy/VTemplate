using System;
using System.Collections.Generic;
using System.Web;

namespace VTemplate.WebTester.Core
{
    /// <summary>
    /// 新闻数据驱动
    /// </summary>
    public class NewsDbProvider
    {
        #region 数据源
        /// <summary>
        /// 根据类型获取新闻数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<News> GetNewsData(string type)
        {
            //以下数据只是测试.在实际项目中可从数据库获取
            List<News> data = new List<News>();

            switch (type)
            {
                case "relating":
                    data.Add(new News() { Id = 48791, Title = "消息称盛大收购成都星漫科技 涉资1.4亿元", Visible = true });
                    data.Add(new News() { Id = 48785, Title = "巴伦周刊：暴雪称游戏有望5年内击败电影电视业", Visible = true });
                    data.Add(new News() { Id = 48774, Title = "盛大称私服运营商煽动酿成游戏堵门事件已报案", Visible = true });
                    data.Add(new News() { Id = 48734, Title = "开心农场今年有望盈利300万 活跃玩家达1600万", Visible = true });
                    data.Add(new News() { Id = 48673, Title = "暴雪的秘密：游戏首重平衡性训练创意思考", Visible = true });
                    //
                    data.Add(new News() { Id = 11111, Title = "这是一条多余的数据,所有显示时不可见", Visible = false });
                    data.Add(new News() { Id = 48670, Title = "魔兽世界第三资料片官方中文介绍视频", Visible = true });
                    break;
                case "hoting":
                    data.Add(new News() { Id = 48741, Title = "全球网速排名揭晓 中国仅排71", Visible = true });
                    data.Add(new News() { Id = 48742, Title = "[视频]傲游陈明杰:只有IE配做我们的竞争对手", Visible = true });
                    data.Add(new News() { Id = 48574, Title = "魔兽玩家驻留网易 每天烧钱百万叫苦不迭", Visible = true });
                    data.Add(new News() { Id = 48781, Title = "专家称白领\"偷菜\"不能排解焦虑 最好融入生活", Visible = true });
                    data.Add(new News() { Id = 48780, Title = "英网站评出IT业十大错误决策", Visible = true });
                    data.Add(new News() { Id = 48702, Title = "中国雅虎近百北京员工可能离职", Visible = true });
                    //
                    data.Add(new News() { Id = 11111, Title = "这是一条多余的数据,所有显示时不可见", Visible = false });
                    break;
            }
            return data;
        }

        /// <summary>
        /// 获取新闻的访问地址
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public static string GetNewsUrl(News news)
        {
            return string.Format("http://news.cnblogs.com/n/{0}", news.Id);
        }
        #endregion
    }
}
