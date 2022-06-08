namespace webapibasica.Models
{
    public class AlunoImagemViewModel
    {
        public int AlunoId { get; set; }
        public string ImagemId { get; set; } = "";
        public Byte[]? ImagemByte { get; set; }
    }
}