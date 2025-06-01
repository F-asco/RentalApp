using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalApp.Data.Migrations
{
    public partial class AddRentalStatusFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Rentals",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<bool>(
                name: "IsReturned",
                table: "Rentals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Rentals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Rentals",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "RentDate", table: "Rentals");
            migrationBuilder.DropColumn(name: "DueDate", table: "Rentals");
            migrationBuilder.DropColumn(name: "IsReturned", table: "Rentals");
            migrationBuilder.DropColumn(name: "IsConfirmed", table: "Rentals");
            migrationBuilder.DropColumn(name: "IsCanceled", table: "Rentals");
        }
    }
}