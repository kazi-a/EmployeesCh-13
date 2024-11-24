using Microsoft.EntityFrameworkCore;
namespace EmployeesCh12.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {

        }
        public DbSet<Benefits> Benefits { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<DepartmentLocation> DepartmentLocations { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Composite key for DepartmentLocation
            modelBuilder.Entity<DepartmentLocation>().HasKey(d => new { d.DepartmentID, d.LocationID });

            //one-to-many relationship between Department and DepartmentLocation
            modelBuilder.Entity<DepartmentLocation>().HasOne(d1 => d1.Department)
                .WithMany(d => d.DepartmentLocations).HasForeignKey(d1 => d1.DepartmentID);

            //one-to-many relationship between Location and DepartmentLocation
            modelBuilder.Entity<DepartmentLocation>().HasOne(dl => dl.Location)
                .WithMany(l => l.DepartmentLocations).HasForeignKey(dl => dl.LocationID);

            modelBuilder.Entity<Department>().HasData(
                new Department { ID = 1, Name = "Accounting" },
                new Department { ID = 2, Name = "IT" },
                new Department { ID = 3, Name = "Marketing" },
                new Department { ID = 4, Name = "Sales" },
                new Department { ID = 5, Name = "Management" },
                new Department { ID = 6, Name = "Plant" },
                new Department { ID = 7, Name = "Shipping" },
                new Department { ID = 8, Name = "Warehouse" }
            );
            modelBuilder.Entity<Location>().HasData(
                new Location { ID = 1, Type = Models.Type.Headquarters, Address = "2200 Park Ave", Zipcode = "49696" },
                new Location { ID = 2, Type = Models.Type.Warehouse, Address = "2200 Park Ave", Zipcode = "49696" },
                new Location { ID = 3, Type = Models.Type.Sales, Address = "2200 Park Ave", Zipcode = "49696" },
                new Location { ID = 4, Type = Models.Type.Plant, Address = "2100 Park Ave", Zipcode = "49696" },
                new Location { ID = 5, Type = Models.Type.Plant, Address = "6 Hickory Blvd.", Zipcode = "49696" },
                new Location { ID = 6, Type = Models.Type.Satellite, Address = "6 Hickory Blvd.", Zipcode = "49696" }
            );
            modelBuilder.Entity<Benefits>().HasData(
                new Benefits { ID = 1, Category = Category.none, Dental = false, Health = false, Vision = false, LifeIns = 0 },
                new Benefits { ID = 2, Category = Category.family, Dental = true, Health = true, Vision = true, LifeIns = 100000 },
                new Benefits { ID = 3, Category = Category.single, Dental = true, Health = true, Vision = true, LifeIns = 100000 }
            );
            modelBuilder.Entity<Employee>().HasData(
               new Employee { ID = 1012, FirstName = "Freddie", LastName = "Flintstone", HireDate = DateTime.Parse("2020-09-01"), DepartmentID = 1, BenefitsID = 1 },
               new Employee { ID = 1067, FirstName = "Wilma", LastName = "Flintstone", HireDate = DateTime.Parse("2005-07-01"), DepartmentID = 2, BenefitsID = 1 },
               new Employee { ID = 1098, FirstName = "Barney", LastName = "Rubble", HireDate = DateTime.Parse("2021-09-01"), DepartmentID = 3, BenefitsID = 2 },
               new Employee { ID = 100, FirstName = "Judy", LastName = "Jetson", HireDate = DateTime.Parse("2019-02-01"), DepartmentID = 1, BenefitsID = 1 },
               new Employee { ID = 101, FirstName = "Daphne", LastName = "Blake", HireDate = DateTime.Parse("2010-01-01"), DepartmentID = 1, BenefitsID = 2 }
            );

            modelBuilder.Entity<DepartmentLocation>().HasData(
                new DepartmentLocation { DepartmentID = 1, LocationID = 1 },
                new DepartmentLocation { DepartmentID = 2, LocationID = 1 },
                new DepartmentLocation { DepartmentID = 3, LocationID = 1 },
                new DepartmentLocation { DepartmentID = 4, LocationID = 1 },
                new DepartmentLocation { DepartmentID = 5, LocationID = 1 },
                new DepartmentLocation { DepartmentID = 8, LocationID = 1 },
                new DepartmentLocation { DepartmentID = 6, LocationID = 4 },
                new DepartmentLocation { DepartmentID = 7, LocationID = 5 },
                new DepartmentLocation { DepartmentID = 7, LocationID = 2 }
           );

        }

    }
}