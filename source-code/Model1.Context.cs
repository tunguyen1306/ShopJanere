﻿

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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class veebdbEntities : DbContext
{
    public veebdbEntities()
        : base("name=veebdbEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<artdimension> artdimensions { get; set; }

    public virtual DbSet<artgrp> artgrps { get; set; }

    public virtual DbSet<artlink> artlinks { get; set; }

    public virtual DbSet<artname> artnames { get; set; }

    public virtual DbSet<artsize> artsizes { get; set; }

    public virtual DbSet<barcode> barcodes { get; set; }

    public virtual DbSet<boundart> boundarts { get; set; }

    public virtual DbSet<card> cards { get; set; }

    public virtual DbSet<catalogue> catalogues { get; set; }

    public virtual DbSet<category> categories { get; set; }

    public virtual DbSet<color> colors { get; set; }

    public virtual DbSet<cumain> cumains { get; set; }

    public virtual DbSet<cuperson> cupersons { get; set; }

    public virtual DbSet<custgroup> custgroups { get; set; }

    public virtual DbSet<eligibility> eligibilities { get; set; }

    public virtual DbSet<file> files { get; set; }

    public virtual DbSet<invoice> invoices { get; set; }

    public virtual DbSet<invoice_log> invoice_log { get; set; }

    public virtual DbSet<item> items { get; set; }

    public virtual DbSet<langname> langnames { get; set; }

    public virtual DbSet<language> languages { get; set; }

    public virtual DbSet<metagrup> metagrups { get; set; }

    public virtual DbSet<orderdetail> orderdetails { get; set; }

    public virtual DbSet<ordritm> ordritms { get; set; }

    public virtual DbSet<outitem> outitems { get; set; }

    public virtual DbSet<payment> payments { get; set; }

    public virtual DbSet<pricelist> pricelists { get; set; }

    public virtual DbSet<price> prices { get; set; }

    public virtual DbSet<producer> producers { get; set; }

    public virtual DbSet<promotion> promotions { get; set; }

    public virtual DbSet<promotionrule> promotionrules { get; set; }

    public virtual DbSet<role> roles { get; set; }

    public virtual DbSet<stgrp> stgrps { get; set; }

    public virtual DbSet<stgrp2> stgrp2 { get; set; }

    public virtual DbSet<stgrpmap> stgrpmaps { get; set; }

    public virtual DbSet<stgrpmap2> stgrpmap2 { get; set; }

    public virtual DbSet<stock> stocks { get; set; }

    public virtual DbSet<stockcod> stockcods { get; set; }

    public virtual DbSet<store> stores { get; set; }

    public virtual DbSet<supplycycle> supplycycles { get; set; }

    public virtual DbSet<trans_log> trans_log { get; set; }

    public virtual DbSet<user> users { get; set; }

    public virtual DbSet<user_role> user_role { get; set; }

    public virtual DbSet<userdata> userdatas { get; set; }

    public virtual DbSet<warehouse> warehouses { get; set; }

    public virtual DbSet<setting> settings { get; set; }

    public virtual DbSet<settingtype> settingtypes { get; set; }

    public virtual DbSet<menu> menus { get; set; }

    public virtual DbSet<order> orders { get; set; }

    public virtual DbSet<slider> sliders { get; set; }

    public virtual DbSet<orderstatu> orderstatus { get; set; }

}

}

