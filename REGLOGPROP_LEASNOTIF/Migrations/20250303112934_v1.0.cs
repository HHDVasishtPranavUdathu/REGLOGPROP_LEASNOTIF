using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REGLOGPROP_LEASNOTIF.Migrations
{
    /// <inheritdoc />
    public partial class v10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    Notification_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sendersId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    receiversId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    notification_Descpirtion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.Notification_Id);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: false),
                    D_O_B = table.Column<DateOnly>(type: "date", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Props",
                columns: table => new
                {
                    Property_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvailableStatus = table.Column<bool>(type: "bit", nullable: false),
                    Owner_Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Props", x => x.Property_Id);
                    table.ForeignKey(
                        name: "FK_Props_Registrations_Owner_Id",
                        column: x => x.Owner_Id,
                        principalTable: "Registrations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Leases",
                columns: table => new
                {
                    LeaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Property_Id = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tenant_Signature = table.Column<bool>(type: "bit", nullable: false),
                    Owner_Signature = table.Column<bool>(type: "bit", nullable: false),
                    Lease_status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leases", x => x.LeaseId);
                    table.ForeignKey(
                        name: "FK_Leases_Props_Property_Id",
                        column: x => x.Property_Id,
                        principalTable: "Props",
                        principalColumn: "Property_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leases_Registrations_ID",
                        column: x => x.ID,
                        principalTable: "Registrations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leases_ID",
                table: "Leases",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_Property_Id",
                table: "Leases",
                column: "Property_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Props_Owner_Id",
                table: "Props",
                column: "Owner_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leases");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "Props");

            migrationBuilder.DropTable(
                name: "Registrations");
        }
    }
}
