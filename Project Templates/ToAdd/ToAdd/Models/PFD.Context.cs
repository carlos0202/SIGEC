﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DGII_PFD.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PFDContext : DbContext
    {
        public PFDContext()
            : base("name=PFDContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<PFD_PARAMETROS> PFD_PARAMETROS { get; set; }
        public DbSet<PFD_PROCEDIMIENTOS> PFD_PROCEDIMIENTOS { get; set; }
        public DbSet<PFD_ROLES> PFD_ROLES { get; set; }
        public DbSet<PFD_USUARIOS> PFD_USUARIOS { get; set; }
        public DbSet<PFD_LOG> PFD_LOG { get; set; }
    }
}