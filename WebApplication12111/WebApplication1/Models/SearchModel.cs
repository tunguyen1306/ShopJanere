using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class SearchModel
    {
        public int CatalogueId { get; set; }
        public string OrderBy { get; set; }
        public string GroupName { get; set; }
        public string CategoryName { get; set; }
        public string ItemName { get; set; }
        public string DINCode { get; set; }
        public string Dimension { get; set; }
        public string Length { get; set; }
        public string Material { get; set; }

        public SearchModel()
        {
            this.CatalogueId = 0;
            this.OrderBy = "esc";
            this.GroupName = "";
            this.CategoryName = "";
            this.ItemName = "";
            this.DINCode = "";
            this.Dimension = "";
            this.Length = "0";
            this.Material = "";
        }

    }
}