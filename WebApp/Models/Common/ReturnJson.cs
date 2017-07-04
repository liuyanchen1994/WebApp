using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Common
{
    public class ReturnJson<T>
    {
        /// <summary>错误代码</summary>
        public int ErrorCode { get; set; }

        /// <summary>消息</summary>
        public string Msg { get; set; }

        /// <summary>数据</summary>
        public T Data { get; set; }

        /// <summary>分页信息</summary>
        public PagerOption PageOption { get; set; }

        /// <summary>返回时间</summary>
        public DateTime DateTime => DateTime.Now.ToLocalTime();
    }
    public class PagerOption
    {
        /// <summary>
        /// 当前页  必传
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 总条数  必传
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 分页记录数（每页条数 默认每页15条）
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 路由地址(格式如：/Controller/Action) 默认自动获取
        /// </summary>
        public string RouteUrl { get; set; }
        /// <summary>
        /// 样式 默认 bootstrap样式 1
        /// </summary>
        public int StyleNum { get; set; }

    }
}
