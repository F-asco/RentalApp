using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Equipment");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Equipment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EquipmentCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_CategoryId",
                table: "Equipment",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_EquipmentCategories_CategoryId",
                table: "Equipment",
                column: "CategoryId",
                principalTable: "EquipmentCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_EquipmentCategories_CategoryId",
                table: "Equipment");

            migrationBuilder.DropTable(
                name: "EquipmentCategories");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_CategoryId",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Equipment");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Equipment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
