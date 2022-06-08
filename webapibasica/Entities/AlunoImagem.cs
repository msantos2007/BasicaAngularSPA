namespace webapibasica.Entities
{
    public class AlunoImagem
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public string ImagemId { get; set; } = "";
        public bool Ativo { get; set; }

        public Aluno Aluno { get; set; } = new Aluno();
    }
}