using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebApp.DB
{
    [JsonObject(IsReference = true)]
    public partial class Catalog
    {
        public Catalog()
        {
            InverseTopCatalog = new HashSet<Catalog>();
            Resource = new HashSet<Resource>();
        }

        public Guid Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public int IsTop { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
        public Guid? TopCatalogId { get; set; }
        public string Type { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Value { get; set; }

        public Catalog TopCatalog { get; set; }

        [JsonIgnore]
        public ICollection<Catalog> InverseTopCatalog { get; set; }
        public ICollection<Resource> Resource { get; set; }
    }
}
