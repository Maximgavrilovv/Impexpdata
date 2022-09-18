using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Impexpdata.Models;

namespace Impexpdata.DatabaseContext
{
    public class CustomerContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CodeLookup> CodeLookup { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO: move connection string to config
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-FALDG67\SEMKASCIRPT;Database=CustomerDB;Trusted_Connection=True;");
        }
    }
}
