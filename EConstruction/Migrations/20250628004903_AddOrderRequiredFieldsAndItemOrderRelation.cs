using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EConstruction.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderRequiredFieldsAndItemOrderRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "Orders");
        }
    }
}
