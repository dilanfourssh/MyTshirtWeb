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
        public DbSet<ColingOff> ColingOffs { get; set; }
        public DbSet<Login> logins { get; set; }
        public DbSet<Offer> offers { get; set; }
        public DbSet<TshirtImage> tshirtImages { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            

        }
    }
        
}