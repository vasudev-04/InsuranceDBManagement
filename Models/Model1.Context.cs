﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InsuranceMgmtDB.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class InsuranceMgtDbEntities1 : DbContext
    {
        public InsuranceMgtDbEntities1()
            : base("name=InsuranceMgtDbEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbl_advisor> tbl_advisor { get; set; }
        public virtual DbSet<tbl_advisor_experience_level> tbl_advisor_experience_level { get; set; }
        public virtual DbSet<tbl_users> tbl_users { get; set; }
    }
}
