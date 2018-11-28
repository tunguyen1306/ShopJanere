using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class AllModel
    {
        public category tblCategory { get; set;}
        public catalogue tblCatalog { get; set;}
        public metagrup tblMetaGroup { get; set;}
        public item tblitem { get; set;}
        public artgrp tblGroup { get; set;}
        public order tblOrder { get; set;}
        public orderstatu tblOrderStatus { get; set;}
        public orderdetail tblOrderDetail { get; set;}
        public setting tblSetting { get; set;}
        public settingtype tblSettingType { get; set;}
        public menu tblMenu { get; set;}
        public menu tblParentMenu { get; set;}
       
    }
}