using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApp.DB
{
    public class MvaDetails : Model
    {

        [MaxLength(32)]
        public string MvaId { get; set; }

        public MvaVideos MvaVideo { get; set; }

        [MaxLength(128)]
        public string  Title { get; set; }

        [MaxLength(500)]
        public string SourceUrl { get; set; }

        [MaxLength(500)]
        public string LowDownloadUrl { get; set; }
        [MaxLength(500)]

        public string MidDownloadUrl { get; set; }
        [MaxLength(500)]

        public string HighDownloadUrl { get; set; }

        [DataType(DataType.Duration)]
        public DateTime Duration { get; set; }

    }
}
