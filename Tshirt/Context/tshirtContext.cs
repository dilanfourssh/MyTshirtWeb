using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Tshirt.Migrations;
using Tshirt.Models;
using user = Tshirt.Migrations.user;

namespace Tshirt.Context
{
    public class tshirtContext : DbContext

    {

        public tshirtContext() : base("connectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<tshirtContext, Configuration>());
        }
        public DbSet<bootstrap_type> bootstrap_Types { get; set; }
        public DbSet<Register> registers { get; set; }
        public DbSet<addproject> addprojects { get; set; }
        public DbSet<Downloahistory> downloahistories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            

        }
    }
        
}