//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1
{
    using System;
    using System.Collections.Generic;
    
    public partial class pricelist
    {
        public short PRICELISTNO { get; set; }
        public string PRICELISTCODE { get; set; }
        public string PRICELISTNAME { get; set; }
        public Nullable<System.DateTime> FIRSTDATE { get; set; }
        public Nullable<System.DateTime> LASTDATE { get; set; }
        public Nullable<int> CUSTNO { get; set; }
        public string PRICEBLOCKED { get; set; }
        public string CURRENCY_CODE { get; set; }
        public Nullable<short> PRICEGROUPNO { get; set; }
        public System.DateTime LASTCHANGE { get; set; }
        public System.DateTime CREATED { get; set; }
    }
}
