using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ExportProduct
    {
        public string MetaGroup { get; set; }
        public string Group { get; set; }
        public string DinCode { get; set; }
        public string Name { get; set; }      
        public string Description { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Length { get; set; }
        public decimal? Weight { get; set; }
        public string Unit { get; set; }
        public string UnitDescription { get; set; }
    }
}