using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ML_2025.Models;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ML_2025.Pages
{
    public class IndexModel : PageModel
    {
        public List<PrecoPeca> ListaPrecos { get; set; } = new();

        [BindProperty]
        public string ProductInput { get; set; }

        public void OnGet()
        {
            CarregarPlanilha();
        }

        public IActionResult OnPost()
        {
            CarregarPlanilha();

            if (!string.IsNullOrWhiteSpace(ProductInput))
            {
                ListaPrecos = ListaPrecos
                    .Where(p => p.Peca.Contains(ProductInput, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return Page();
        }

        private void CarregarPlanilha()
        {
            string caminho = Path.Combine(Directory.GetCurrentDirectory(), "MLModels", "Comparativo_Precos_Pecas_PC.xlsx");

            if (!System.IO.File.Exists(caminho)) return;

            ListaPrecos.Clear();

            try
            {
                using var workbook = new XLWorkbook(caminho);
                var ws = workbook.Worksheet(1);

                foreach (var row in ws.RangeUsed().RowsUsed().Skip(1))
                {
                    ListaPrecos.Add(new PrecoPeca
                    {
                        Peca = row.Cell(1).GetString(),
                        Loja1Preco = ParseDoubleSafe(row.Cell(2)),
                        Loja1Desconto = row.Cell(3).GetString(),
                        Loja2Preco = ParseDoubleSafe(row.Cell(4)),
                        Loja2Desconto = row.Cell(5).GetString(),
                        Loja3Preco = ParseDoubleSafe(row.Cell(6)),
                        Loja3Desconto = row.Cell(7).GetString()
                    });
                }
            }
            catch
            {
                // Ignora erros por simplicidade; adicione logging em produção
            }
        }

        private double ParseDoubleSafe(IXLCell cell)
        {
            return double.TryParse(cell.GetString(), out double value) ? value : 0.0;
        }
    }
}
