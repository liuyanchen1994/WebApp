using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Entity
{
    public class Model
    {
        /// <summary>
        /// Guid
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 状态值
        /// </summary>
        public int? Status { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedTime { get; set; }
    }
}
