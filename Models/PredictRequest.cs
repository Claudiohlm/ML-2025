namespace ML_2025.Models
{
    public class CompareRequest
{
    public string NomeProduto { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty; // opcional
    public string LojaPreferencial { get; set; } = string.Empty; // opcional
}

}
