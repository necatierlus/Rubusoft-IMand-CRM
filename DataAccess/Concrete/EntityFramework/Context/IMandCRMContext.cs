using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework.Context
{
    public class IMandCRMContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(connectionString: @"Data Source=89.19.24.243;Initial Catalog=Rubusoft_Ss;persist security info=True;user id=Login_Ss;Password=mexETH0JauSt7W9b;");
            optionsBuilder.UseSqlServer(connectionString: @"Data Source=DESKTOP-DHUQA7J\SQLEXPRESS;Initial Catalog=SmartScreen;persist security info=True;user id=frk;Password=123456;");
            //optionsBuilder.UseSqlServer(connectionString: @"Data Source=LAPTOP-Q5KQP3V5\SQLEXPRESS;Initial Catalog=Rubusoft_Ss;persist security info=True;user id=qbot;Password=123;");
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Firm> Firms { get; set; }
        public DbSet<FirmManager> FirmManagers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<BidProduct> BidProducts { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<GeneralRequirement> GeneralRequirements { get; set; }
        public DbSet<CustomerRequest>  CustomerRequests { get; set; }
        public DbSet<TechSupport> TechSupports { get; set; }
        public DbSet<TechSupportDetail> TechSupportDetails { get; set; }
        public DbSet<StockPoint> StockPoints { get; set; }
        public DbSet<DeviceStock> DeviceStocks { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }


    }
}
