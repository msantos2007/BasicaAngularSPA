namespace webapibasica.Entities
{
    public class Aluno
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public DateTime DtNascimento { get; set; } = new DateTime(3000, 01, 01, 0, 0, 0, DateTimeKind.Utc);

        public DateTime DtInclusao { get; set; }
        public DateTime DtModificacao { get; set; } = new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc);

        public ICollection<AlunoNota> AlunoNotas { get; set; } = new List<AlunoNota>();
    }
}