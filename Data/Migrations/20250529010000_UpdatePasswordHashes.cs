using Microsoft.EntityFrameworkCore.Migrations;
using AutismEducationPlatform.Helpers;

namespace AutismEducationPlatform.Data.Migrations
{
    public partial class UpdatePasswordHashes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Önce geçici bir kolon oluştur
            migrationBuilder.AddColumn<string>(
                name: "TempPassword",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: true);

            // Mevcut şifreleri geçici kolona kopyala
            migrationBuilder.Sql(@"
                UPDATE Kullanicilar 
                SET TempPassword = Sifre");

            // Eski şifre kolonunu sil
            migrationBuilder.DropColumn(
                name: "Sifre",
                table: "Kullanicilar");

            // Yeni şifre kolonunu ekle
            migrationBuilder.AddColumn<string>(
                name: "Sifre",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // Admin kullanıcısının şifresini güncelle
            migrationBuilder.Sql($@"
                UPDATE Kullanicilar 
                SET Sifre = '{PasswordHasher.HashPassword("admin1234")}'
                WHERE KullaniciTipi = 'Admin'");

            // Geçici kolonu sil
            migrationBuilder.DropColumn(
                name: "TempPassword",
                table: "Kullanicilar");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Geri alma işlemi gerekirse
            migrationBuilder.AlterColumn<string>(
                name: "Sifre",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
} 