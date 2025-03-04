using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REGLOGPROP_LEASNOTIF.Migrations
{
    /// <inheritdoc />
    public partial class ss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tenant");

            migrationBuilder.AddColumn<string>(
                name: "TenantDetailsID",
                table: "payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_TenantDetailsID",
                table: "payments",
                column: "TenantDetailsID");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_Registrations_TenantDetailsID",
                table: "payments",
                column: "TenantDetailsID",
                principalTable: "Registrations",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_Registrations_TenantDetailsID",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "IX_payments_TenantDetailsID",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "TenantDetailsID",
                table: "payments");

            migrationBuilder.CreateTable(
                name: "Tenant",
                columns: table => new
                {
                    TenantID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tenant_Adhar = table.Column<int>(type: "int", nullable: false),
                    Tenant_History = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tenant_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tenant_Pan = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenant", x => x.TenantID);
                });
        }
    }
}
