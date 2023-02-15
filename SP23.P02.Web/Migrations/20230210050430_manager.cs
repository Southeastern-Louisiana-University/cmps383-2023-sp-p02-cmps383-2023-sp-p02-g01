using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SP23.P02.Web.Migrations
{
    /// <inheritdoc />
    public partial class manager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "TrainStation",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainStation_ManagerId",
                table: "TrainStation",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainStation_AspNetUsers_ManagerId",
                table: "TrainStation",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainStation_AspNetUsers_ManagerId",
                table: "TrainStation");

            migrationBuilder.DropIndex(
                name: "IX_TrainStation_ManagerId",
                table: "TrainStation");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "TrainStation");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }
    }
}
