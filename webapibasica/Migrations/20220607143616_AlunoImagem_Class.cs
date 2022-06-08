using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace webapibasica.Migrations
{
    public partial class AlunoImagem_Class : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "aluno_imagem_tb",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_aluno = table.Column<int>(type: "integer", nullable: false),
                    id_imagem = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    ativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aluno_imagem_tb", x => x.id);
                    table.ForeignKey(
                        name: "FK_aluno_imagem_tb_aluno_tb_id_aluno",
                        column: x => x.id_aluno,
                        principalTable: "aluno_tb",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_aluno_imagem_tb_id_aluno",
                table: "aluno_imagem_tb",
                column: "id_aluno");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aluno_imagem_tb");
        }
    }
}
