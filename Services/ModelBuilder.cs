using Microsoft.ML;
using ClosedXML.Excel;
using ML_2025.Models;
using Microsoft.ML.Trainers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ML_2025.Services
{
    public static class ModelBuilder
    {
        public static void Treinar(string pastaModelos)
        {
            var ml = new MLContext(seed: 1);

            var caminhoExcel = Path.Combine(pastaModelos, "Comparativo_Precos_Pecas_PC.xlsx");

            if (!File.Exists(caminhoExcel))
            {
                CriarExcelExemplo(caminhoExcel);
            }

            var dados = CarregarDadosDoExcel(caminhoExcel);

            if (dados.Count < 2)
                return;

            var data = ml.Data.LoadFromEnumerable(
           dados.Select(d => new PrecoInput
           {
               Peca = d.Peca,
               Label = (float)((d.Loja1Preco + d.Loja2Preco + d.Loja3Preco) / 3.0)
           })
       );

            var pipeline = ml.Transforms.Text.FeaturizeText("Features", nameof(PrecoInput.Peca))
            .Append(ml.Regression.Trainers.FastTree(labelColumnName: "Label", featureColumnName: "Features"));

            var model = pipeline.Fit(data);

            var caminhoModelo = Path.Combine(pastaModelos, "model.zip");
            ml.Model.Save(model, data.Schema, caminhoModelo);
        }

        private class PrecoInput
        {
            public string Peca { get; set; } = string.Empty;
            public float Label { get; set; }
        }

        private static List<PrecoPeca> CarregarDadosDoExcel(string caminho)
        {
            var lista = new List<PrecoPeca>();

            using var workbook = new XLWorkbook(caminho);
            var ws = workbook.Worksheet(1);

            foreach (var row in ws.RangeUsed().RowsUsed().Skip(1))
            {
                var peca = row.Cell(1).GetString();
                double preco1 = ParseDoubleSafe(row.Cell(2));
                double preco2 = ParseDoubleSafe(row.Cell(4));
                double preco3 = ParseDoubleSafe(row.Cell(6));

                lista.Add(new PrecoPeca
                {
                    Peca = peca,
                    Loja1Preco = preco1,
                    Loja2Preco = preco2,
                    Loja3Preco = preco3,
                });
            }

            return lista;
        }

        private static double ParseDoubleSafe(IXLCell cell)
        {
            return double.TryParse(cell.GetString().Replace(",", "."), out double value) ? value : 0.0;
        }

        private static void CriarExcelExemplo(string caminho)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Precos");

            ws.Cell(1, 1).Value = "Peca";
            ws.Cell(1, 2).Value = "Loja1Preco";
            ws.Cell(1, 3).Value = "Loja1Desconto";
            ws.Cell(1, 4).Value = "Loja2Preco";
            ws.Cell(1, 5).Value = "Loja2Desconto";
            ws.Cell(1, 6).Value = "Loja3Preco";
            ws.Cell(1, 7).Value = "Loja3Desconto";

            ws.Cell(2, 1).Value = "Ryzen 5 5600X";
            ws.Cell(2, 2).Value = 1200.50;
            ws.Cell(2, 3).Value = "10%";
            ws.Cell(2, 4).Value = 1150.00;
            ws.Cell(2, 5).Value = "15%";
            ws.Cell(2, 6).Value = 1300.75;
            ws.Cell(2, 7).Value = "5%";

            ws.Cell(3, 1).Value = "RTX 3060";
            ws.Cell(3, 2).Value = 2500.00;
            ws.Cell(3, 3).Value = "0%";
            ws.Cell(3, 4).Value = 2400.00;
            ws.Cell(3, 5).Value = "8%";
            ws.Cell(3, 6).Value = 2600.00;
            ws.Cell(3, 7).Value = "2%";

            ws.Cell(4, 1).Value = "SSD 1TB";
            ws.Cell(4, 2).Value = 400.00;
            ws.Cell(4, 3).Value = "20%";
            ws.Cell(4, 4).Value = 380.00;
            ws.Cell(4, 5).Value = "25%";
            ws.Cell(4, 6).Value = 420.00;
            ws.Cell(4, 7).Value = "10%";

            workbook.SaveAs(caminho);
        }
    }
}