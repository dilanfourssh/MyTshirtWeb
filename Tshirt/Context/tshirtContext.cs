using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        }
        public DbSet<user> user { get; set; }
    }
        
}