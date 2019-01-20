using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class SearchModel
    {
       
        public int? ddlPosition { get; set; }
        public string AllKey { get; set; }
        public string ddlGetGroup { get; set; }
        public string ddlGetMetaGroup { get; set; }
        public string ddlGetItem { get; set; }
        public string ddlGetDINCode { get; set; }
        public int? ddlGetDimension { get; set; }
        public int? ddlLength { get; set; }
        public int? ddlGetInterFace { get; set; }
        public int? ddlGetMaterial { get; set; }       
        public int? ddlGetCategory { get; set; }
        public int? ddlGetGroupno { get; set; }
    }
}