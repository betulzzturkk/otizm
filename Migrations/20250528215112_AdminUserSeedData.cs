using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutismEducationPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AdminUserSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Adminler",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "ModulIcerikleri");

            migrationBuilder.DropColumn(
                name: "ResimYolu",
                table: "ModulIcerikleri");

            migrationBuilder.RenameColumn(
                name: "SesYolu",
                table: "ModulIcerikleri",
                newName: "Icerik");

            migrationBuilder.InsertData(
                table: "Kullanicilar",
                columns: new[] { "Id", "Ad", "Email", "KullaniciTipi", "Sifre", "Soyad" },
                values: new object[] { 1, "Admin", "admin@gmail.com", "Admin", "admin1234", "User" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Kullanicilar",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "Icerik",
                table: "ModulIcerikleri",
                newName: "SesYolu");

            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "ModulIcerikleri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResimYolu",
                table: "ModulIcerikleri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Adminler",
                columns: new[] { "Id", "KullaniciAdi", "Sifre" },
                values: new object[] { 1, "admin", "admin123" });
        }
    }
}
