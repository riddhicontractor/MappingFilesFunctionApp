using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MappingFilesFunctionApp.Model
{
    public class Product
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "PoNo")]
        public string PoNo { get; set; }

        public string SupplierName  { get; set; }

        public DateTime Date { get; set; }

        public string OrderValue { get; set; }

        public string Status { get; set; }
    }
}
