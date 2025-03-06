using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using REGLOGPROP_LEASNOTIF.Models;

namespace REGLOGPROP_LEASNOTIF
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RLP_LN;Encrypt=False");
        }

        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Prop> Props { get; set; }
        public DbSet<Lease> Leases { get; set; }
        public DbSet<Notification> notifications { get; set; }
        public DbSet<Paymennts> payments { get; set; }
        public DbSet<Maintenance> Maintainances { get; set; }

        //public DbSet<history> histories { get; set; }   



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prop>()
                .HasOne(p => p.Registration)
                .WithMany()
                .HasForeignKey(p => p.Owner_Id);
        }
    }
}
