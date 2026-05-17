using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppForSEII2526.API.Migrations
{
    /// <inheritdoc />
    public partial class FixReportCustomerRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportCustomers_AspNetUsers_UserId",
                table: "ReportCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportCustomers_BanReports_BanReportId",
                table: "ReportCustomers");

            migrationBuilder.DropIndex(
                name: "IX_ReportCustomers_UserId",
                table: "ReportCustomers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReportCustomers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportCustomers",
                table: "ReportCustomers");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "ReportCustomers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportCustomers",
                table: "ReportCustomers",
                columns: new[] { "BanReportId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_ReportCustomers_CustomerId",
                table: "ReportCustomers",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportCustomers_AspNetUsers_CustomerId",
                table: "ReportCustomers",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportCustomers_BanReports_BanReportId",
                table: "ReportCustomers",
                column: "BanReportId",
                principalTable: "BanReports",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportCustomers_AspNetUsers_CustomerId",
                table: "ReportCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportCustomers_BanReports_BanReportId",
                table: "ReportCustomers");

            migrationBuilder.DropIndex(
                name: "IX_ReportCustomers_CustomerId",
                table: "ReportCustomers");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "ReportCustomers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ReportCustomers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCustomers_UserId",
                table: "ReportCustomers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportCustomers_AspNetUsers_UserId",
                table: "ReportCustomers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportCustomers_BanReports_BanReportId",
                table: "ReportCustomers",
                column: "BanReportId",
                principalTable: "BanReports",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
