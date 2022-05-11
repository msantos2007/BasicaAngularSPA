using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace webapibasica.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "aluno_tb",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "text", nullable: false),
                    dt_nascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dt_inclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dt_alteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aluno_tb", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "aluno_nota_tb",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_aluno = table.Column<int>(type: "integer", nullable: false),
                    nota = table.Column<int>(type: "integer", nullable: false),
                    dt_inclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dt_modificacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aluno_nota_tb", x => x.id);
                    table.ForeignKey(
                        name: "FK_aluno_nota_tb_aluno_tb_id_aluno",
                        column: x => x.id_aluno,
                        principalTable: "aluno_tb",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_aluno_nota_tb_id_aluno",
                table: "aluno_nota_tb",
                column: "id_aluno");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aluno_nota_tb");

            migrationBuilder.DropTable(
                name: "aluno_tb");
        }
    }
}
