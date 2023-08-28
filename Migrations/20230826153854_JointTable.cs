using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bangzon.Migrations
{
    public partial class JointTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OrderProduct",
                columns: new[] { "OrdersId", "ProductsId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderProduct",
                keyColumns: new[] { "OrdersId", "ProductsId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "OrderProduct",
                keyColumns: new[] { "OrdersId", "ProductsId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "OrderProduct",
                keyColumns: new[] { "OrdersId", "ProductsId" },
                keyValues: new object[] { 1, 3 });
        }
    }
}
