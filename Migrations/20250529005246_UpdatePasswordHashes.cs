using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutismEducationPlatform.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePasswordHashes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adminler");

            migrationBuilder.DropTable(
                name: "Ogrenciler");

            migrationBuilder.RenameColumn(
                name: "Ad",
                table: "EgitimModulleri",
                newName: "ModulAdi");

            migrationBuilder.AddColumn<bool>(
                name: "AktifMi",
                table: "ModulIcerikleri",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ResimYolu",
                table: "ModulIcerikleri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SesYolu",
                table: "ModulIcerikleri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Sira",
                table: "ModulIcerikleri",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "KullaniciAdi",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "AktifMi",
                table: "EgitimModulleri",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "OlusturulmaTarihi",
                table: "EgitimModulleri",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Cocuklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Yas = table.Column<int>(type: "int", nullable: false),
                    VeliId = table.Column<int>(type: "int", nullable: false),
                    DogumTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OzelDurum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EgitimDurumu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cocuklar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cocuklar_Kullanicilar_VeliId",
                        column: x => x.VeliId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VeliBilgilendirmeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VeliId = table.Column<int>(type: "int", nullable: false),
                    Mesaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarihSaat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Okundu = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VeliBilgilendirmeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VeliBilgilendirmeler_Kullanicilar_VeliId",
                        column: x => x.VeliId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CocukDurumlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CocukId = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarihSaat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CocukDurumlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CocukDurumlari_Cocuklar_CocukId",
                        column: x => x.CocukId,
                        principalTable: "Cocuklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Kullanicilar",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "KullaniciAdi", "Soyad" },
                values: new object[] { "", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_CocukDurumlari_CocukId",
                table: "CocukDurumlari",
                column: "CocukId");

            migrationBuilder.CreateIndex(
                name: "IX_Cocuklar_VeliId",
                table: "Cocuklar",
                column: "VeliId");

            migrationBuilder.CreateIndex(
                name: "IX_VeliBilgilendirmeler_VeliId",
                table: "VeliBilgilendirmeler",
                column: "VeliId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CocukDurumlari");

            migrationBuilder.DropTable(
                name: "VeliBilgilendirmeler");

            migrationBuilder.DropTable(
                name: "Cocuklar");

            migrationBuilder.DropColumn(
                name: "AktifMi",
                table: "ModulIcerikleri");

            migrationBuilder.DropColumn(
                name: "ResimYolu",
                table: "ModulIcerikleri");

            migrationBuilder.DropColumn(
                name: "SesYolu",
                table: "ModulIcerikleri");

            migrationBuilder.DropColumn(
                name: "Sira",
                table: "ModulIcerikleri");

            migrationBuilder.DropColumn(
                name: "KullaniciAdi",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "AktifMi",
                table: "EgitimModulleri");

            migrationBuilder.DropColumn(
                name: "OlusturulmaTarihi",
                table: "EgitimModulleri");

            migrationBuilder.RenameColumn(
                name: "ModulAdi",
                table: "EgitimModulleri",
                newName: "Ad");

            migrationBuilder.CreateTable(
                name: "Adminler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sifre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adminler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ogrenciler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VeliId = table.Column<int>(type: "int", nullable: false),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuncelModulId = table.Column<int>(type: "int", nullable: false),
                    SonAktiviteTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ogrenciler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ogrenciler_Kullanicilar_VeliId",
                        column: x => x.VeliId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Kullanicilar",
                keyColumn: "Id",
                keyValue: 1,
                column: "Soyad",
                value: "User");

            migrationBuilder.CreateIndex(
                name: "IX_Ogrenciler_VeliId",
                table: "Ogrenciler",
                column: "VeliId");
        }
    }
}
