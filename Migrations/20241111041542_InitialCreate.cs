using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployeesCh12.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Benefits",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<int>(type: "int", nullable: true),
                    Dental = table.Column<bool>(type: "bit", nullable: false),
                    Vision = table.Column<bool>(type: "bit", nullable: false),
                    Health = table.Column<bool>(type: "bit", nullable: false),
                    LifeIns = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benefits", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Zipcode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DepartmentID = table.Column<int>(type: "int", nullable: true),
                    BenefitsID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Employees_Benefits_BenefitsID",
                        column: x => x.BenefitsID,
                        principalTable: "Benefits",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "DepartmentLocations",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(type: "int", nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentLocations", x => new { x.DepartmentID, x.LocationID });
                    table.ForeignKey(
                        name: "FK_DepartmentLocations_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentLocations_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Benefits",
                columns: new[] { "ID", "Category", "Dental", "Health", "LifeIns", "Vision" },
                values: new object[,]
                {
                    { 1, 0, false, false, 0m, false },
                    { 2, 2, true, true, 100000m, true },
                    { 3, 1, true, true, 100000m, true }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Accounting" },
                    { 2, "IT" },
                    { 3, "Marketing" },
                    { 4, "Sales" },
                    { 5, "Management" },
                    { 6, "Plant" },
                    { 7, "Shipping" },
                    { 8, "Warehouse" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "ID", "Address", "Type", "Zipcode" },
                values: new object[,]
                {
                    { 1, "2200 Park Ave", 1, "49696" },
                    { 2, "2200 Park Ave", 4, "49696" },
                    { 3, "2200 Park Ave", 0, "49696" },
                    { 4, "2100 Park Ave", 3, "49696" },
                    { 5, "6 Hickory Blvd.", 3, "49696" },
                    { 6, "6 Hickory Blvd.", 2, "49696" }
                });

            migrationBuilder.InsertData(
                table: "DepartmentLocations",
                columns: new[] { "DepartmentID", "LocationID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 6, 4 },
                    { 7, 2 },
                    { 7, 5 },
                    { 8, 1 }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "BenefitsID", "DepartmentID", "FirstName", "HireDate", "LastName" },
                values: new object[,]
                {
                    { 100, 1, 1, "Judy", new DateTime(2019, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jetson" },
                    { 101, 2, 1, "Daphne", new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Blake" },
                    { 1012, 1, 1, "Freddie", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Flintstone" },
                    { 1067, 1, 2, "Wilma", new DateTime(2005, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Flintstone" },
                    { 1098, 2, 3, "Barney", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rubble" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentLocations_LocationID",
                table: "DepartmentLocations",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_BenefitsID",
                table: "Employees",
                column: "BenefitsID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentID",
                table: "Employees",
                column: "DepartmentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentLocations");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Benefits");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
