namespace ML_2025.Models
{
    public class ComparisonResponse
    {
        public string Produto { get; set; } = string.Empty;

        public string Loja { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int Desconto { get; set; }

        public decimal PrecoFinal => Preco * (1 - Desconto / 100m); // cálculo do preço final

        public string Resultado => $"R$ {PrecoFinal:F2} na loja {Loja}";
    }
}
