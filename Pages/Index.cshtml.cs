using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.ML;
using Microsoft.ML;
using ML_2025.Models;

public class IndexModel : PageModel
{
 

    private readonly PredictionEngine<Produto, ProductPrediction> _predictionEngine; 
    public IndexModel(PredictionEngine<Produto, ProductPrediction> predictionEngine)
        {
        _predictionEngine = predictionEngine;
    }
    public List<Produto> Resultados { get; set; }

    [BindProperty]
    public string InputText { get; set; }

    public ProductPrediction PredictionResult { get; set; }

    public void OnPost()
    {
        if (!string.IsNullOrWhiteSpace(InputText))
        {
            var input = new Produto { Text = InputText };
            PredictionResult = _predictionEngine.Predict(input);
        }
    }
}
