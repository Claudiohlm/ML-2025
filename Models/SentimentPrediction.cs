using Microsoft.ML.Data;

namespace ML_2025.Models
{
    public class ProductPrediction
    {
        public string Nome { get; set; } = string.Empty;
        public string Loja { get; set; } = string.Empty;
        public float Preco { get; set; }
        public int Desconto { get; set; }

        public float PrecoFinal => Preco - (Preco * Desconto / 100f); // float
        public string MelhorOferta => $"{Nome} por R$ {PrecoFinal:F2} na loja {Loja}";
    }

}
