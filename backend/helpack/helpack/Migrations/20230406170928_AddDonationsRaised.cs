using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace helpack.Migrations
{
    /// <inheritdoc />
    public partial class AddDonationsRaised : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DonationsRaised",
                table: "Profiles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonationsRaised",
                table: "Profiles");
        }
    }
}
