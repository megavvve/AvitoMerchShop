using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvitoMerchShop.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Purchases",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionDate",
                table: "CoinTransactions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_UserId",
                table: "Purchases",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CoinTransactions_FromUserId",
                table: "CoinTransactions",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CoinTransactions_ToUserId",
                table: "CoinTransactions",
                column: "ToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoinTransactions_Users_FromUserId",
                table: "CoinTransactions",
                column: "FromUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoinTransactions_Users_ToUserId",
                table: "CoinTransactions",
                column: "ToUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Users_UserId",
                table: "Purchases",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoinTransactions_Users_FromUserId",
                table: "CoinTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_CoinTransactions_Users_ToUserId",
                table: "CoinTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Users_UserId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_UserId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_CoinTransactions_FromUserId",
                table: "CoinTransactions");

            migrationBuilder.DropIndex(
                name: "IX_CoinTransactions_ToUserId",
                table: "CoinTransactions");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "CoinTransactions");
        }
    }
}
