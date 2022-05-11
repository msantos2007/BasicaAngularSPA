namespace webapibasica.Entities
{
    public class AlunoNota
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public int Nota { get; set; }

        public DateTime DtInclusao { get; set; }
        public DateTime DtModificacao { get; set; } = new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc);

        public Aluno Aluno { get; set; } = new Aluno();
    }
}