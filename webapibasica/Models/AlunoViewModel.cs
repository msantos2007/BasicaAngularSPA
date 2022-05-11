namespace webapibasica.Models
{
    public class AlunoViewModel
    {
        public string Nome { get; set; } = string.Empty;
        public DateTime DtNascimento { get; set; } = new DateTime(3000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}
