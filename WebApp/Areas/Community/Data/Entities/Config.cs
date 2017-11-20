using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Community.Data.Entities
{
    public class Config
    {
        public Guid Id { get; set; }
        [MaxLength(64)]
        public string Key { get; set; }
        public string Value { get; set; }
        [MaxLength(64)]
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime CreatedTime { get; set; }

    }
}
