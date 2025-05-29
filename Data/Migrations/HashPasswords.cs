using Microsoft.EntityFrameworkCore.Migrations;
using AutismEducationPlatform.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AutismEducationPlatform.Data.Migrations
{
    public partial class HashPasswords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Önce geçici bir kolon oluştur
            migrationBuilder.AddColumn<string>(
                name: "HashedPassword",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: true);

            // Raw SQL ile şifreleri hash'le
            migrationBuilder.Sql(@"
                UPDATE Kullanicilar 
                SET HashedPassword = CASE 
                    WHEN Sifre IS NOT NULL THEN '10000.' + CONVERT(varchar(max), CRYPT_GEN_RANDOM(16), 2) + '.' + CONVERT(varchar(max), HASHBYTES('SHA2_512', Sifre), 2)
                    ELSE NULL 
                END");

            // Eski şifre kolonunu sil
            migrationBuilder.DropColumn(
                name: "Sifre",
                table: "Kullanicilar");

            // Yeni kolonu yeniden adlandır
            migrationBuilder.RenameColumn(
                name: "HashedPassword",
                table: "Kullanicilar",
                newName: "Sifre");

            // Şifre kolonunu required yap
            migrationBuilder.AlterColumn<string>(
                name: "Sifre",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
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