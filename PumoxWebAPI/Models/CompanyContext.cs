using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PumoxWebAPI.Models
{

    public class CompanyContext : DbContext
    {
        public CompanyContext(DbContextOptions<CompanyContext> options)
            : base(options)
        {
            //InitData();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"server=(localdb)\MSSQLLocalDB;database=CompanyDB;Integrated Security=True;Trusted_Connection=true");
            // Data Source=DESKTOP-JQEEA1A;Initial Catalog=testy;Integrated Security=True;Pooling=False
            //Scaffold-DbContext “Server=DESKTOP-JQEEA1A;Database=Login;
            //options.UseSqlServer("Data Source=Companies.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().ToTable("Company");
            modelBuilder.Entity<Employee>().ToTable("Employee");

            //modelBuilder.Entity<Company>()
            //.HasMany(c => c.Employees);
            //.WithOne(e => e.Company);


            //modelBuilder.Entity<Company>().HasMany(g => g.Employees);
        }

       


        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        private void InitData()
        {
            if (Companies.Any())
            {
                return;
            }
            var company = new Company
            {
                Name = "Bisk",
                EstablishmentYear = 1991,
                Employees = new List<Employee>
                {
                    new Employee { FirstName = "Yoop", LastName = "Nod", DateOfBirth = DateTime.Parse("2019-07-26T00:00:00"), JobTitle = JobTitle.Developer }
                }
            };

            Companies.Add(company);
        }

    }
}