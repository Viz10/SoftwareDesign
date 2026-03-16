using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Warehouse.Migrations
{
    /// <inheritdoc />
    public partial class AddedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedTime",
                table: "Items",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "DeletedAtTime", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, null, "OLED SAMSUNG TV", false, "TV" },
                    { 2, null, "240Hz Asus", false, "Gaming Monitor" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "LastModifiedTime",
                table: "Items");

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "DeletedAtTime", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { -2, null, "240Hz Asus", false, "Gaming Monitor" },
                    { -1, null, "OLED SAMSUNG TV", false, "TV" }
                });
        }
    }
}
