using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Community.Data.Entities
{
    public class Config : BaseModel
    {
        [MaxLength(64)]
        public string Key { get; set; }
        public string Value { get; set; }
        [MaxLength(64)]
        public string Type { get; set; }
    }
}
