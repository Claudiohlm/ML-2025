using Microsoft.ML.Data;


namespace ML_2025.Models
{
    public class Produto
    {
        public string Nome { get; set; } = string.Empty;
        public string Loja { get; set; } = string.Empty;
        public float Preco { get; set; }          // float, não decimal
        public string Text { get; set; } = string.Empty;
        public float Score { get; set; }
        public int Desconto { get; set; }

        public float PrecoFinal => Preco - (Preco * Desconto / 100f); // float
    }

}



