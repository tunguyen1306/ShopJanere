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
        public metagrup tblMasterMetaGroup { get; set;}
        public item tblitem { get; set;}
        public artgrp tblGroup { get; set;}
        public order tblOrder { get; set;}
        public orderstatu tblOrderStatus { get; set;}
        public orderdetail tblOrderDetail { get; set;}
        public setting tblSetting { get; set;}
        public settingtype tblSettingType { get; set;}
        public menu tblMenu { get; set;}
        public menu tblParentMenu { get; set;}
        public promotion tblPromotion { get; set;}
        public ordersetting tblOrderSetting { get; set;}
        public user tblUser { get; set;}
        public userdata tblUserData { get; set;}
        public country tblCountry { get; set;}
        public city tblCity { get; set;}
        public vocabulary[] tblVocabularyArray { get; set;}
        public vocabulary  tblVocabulary { get; set;}
        public List<vocabulary>   listVocabulary { get; set;}

        public item[] tblProductArray { get; set; }
        public List<item> listProduct { get; set; }

        public metagrup[] tblMasterMetaGroupArray { get; set; }
        public List<metagrup> listMasterMetaGroup { get; set; }

        public metagrup[] tblMetaGroupArray { get; set; }
        public List<metagrup> listMetaGroup { get; set; }


    }
}