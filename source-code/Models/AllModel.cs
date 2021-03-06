﻿using System;
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
        public barcode tblBarcode { get; set;}
       
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
        public shippingfee tblShipping { get; set;}
        public vocabulary[] tblVocabularyArray { get; set;}
        public vocabulary  tblVocabulary { get; set;}
        public List<vocabulary>   listVocabulary { get; set;}

        public item[] tblProductArray { get; set; }
        public List<item> listProduct { get; set; }

        public metagrup[] tblMasterMetaGroupArray { get; set; }
        public List<metagrup> listMasterMetaGroup { get; set; }

        public metagrup[] tblMetaGroupArray { get; set; }
        public List<metagrup> listMetaGroup { get; set; }


        public artgrp tblGroup { get; set; }
        public artgrp[] tblGroupArray { get; set; }
        public List<artgrp> listGroup { get; set; }

        public stockcod tblStockCod { get; set; }
        public stockcod[] tblStockCodArray { get; set; }
        public List<stockcod> listStockCod { get; set; }

        public stock tblStock { get; set; }
        public stock[] tblStockArray { get; set; }
        public List<stock> listStock { get; set; }
        public string messError { get; set; }

        public store tblStore { get; set; }
        public store[] tblStoreArray { get; set; }
        public List<store> listStore { get; set; }


        public file tblFile { get; set; }
        public file[] tblFileArray { get; set; }
        public List<file> listFile { get; set; }
    }
}