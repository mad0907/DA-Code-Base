using System;
using System.Net;
using System.Net.NetworkInformation;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

namespace DAWebAPIs.Model
{
    public class AgencyDBContext : DbContext
    {
        public AgencyDBContext(DbContextOptions<AgencyDBContext> option) : base(option)
        {
            agencyDatas = base.Set<AgencyData>();

        }

        public DbSet<AgencyData> agencyDatas { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgencyData>()
                .ToContainer("agency_data")
                .HasPartitionKey(c => c.id);


            //modelBuilder.Entity<Customer>().OwnsMany(p => p.Orders);
        }
    }
}

