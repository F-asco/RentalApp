using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalStatusFields2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Rentals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Rentals");
        }
    }
}
