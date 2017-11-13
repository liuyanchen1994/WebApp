using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.DB
{
    /// <summary>
    /// 会员
    /// </summary>
    public class Member
    {
        public Guid Id { get; set; }

        public User User { get; set; }
        /// <summary>
        /// 会员积分
        /// </summary>
        public int? Score { get; set; }

        public string Level { get; set; }
        /// <summary>
        /// 会员等级名称
        /// </summary>
        public string LevelName { get; set; }

        public DateTime ExpiredTime { get; set; }


    }
}
