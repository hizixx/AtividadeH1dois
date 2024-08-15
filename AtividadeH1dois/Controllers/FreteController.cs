using Microsoft.AspNetCore.Mvc;

namespace AtividadeH1dois.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FreteController : ControllerBase
    {
        // Taxa fixa por centímetro cúbico
        private const double TaxaPorCm3 = 0.01;

        // Tabela de tarifas por estado
        private static readonly Dictionary<string, double> TarifasPorEstado = new()
        {
            { "SP", 50.00 },
            { "RJ", 60.00 },
            { "MG", 55.00 },
            { "OUTROS", 70.00 }
        };

        // Ação para calcular o frete
        [HttpPost("calcular-frete")]
        public IActionResult CalcularFrete([FromBody] ProdutoDto produto)
        {
            if (produto == null || produto.Peso <= 0 || produto.Altura <= 0 || produto.Largura <= 0 || produto.Comprimento <= 0 || string.IsNullOrWhiteSpace(produto.UF))
            {
                return BadRequest("Dados inválidos.");
            }

            // Calcular o volume do produto em cm³
            var volume = produto.Altura * produto.Largura * produto.Comprimento;

            // Calcular o valor do frete baseado no volume
            var valorFrete = volume * TaxaPorCm3;

            // Adicionar a taxa por estado
            var tarifaPorEstado = TarifasPorEstado.TryGetValue(produto.UF.ToUpper(), out var tarifa) ? tarifa : TarifasPorEstado["OUTROS"];
            valorFrete += tarifaPorEstado;

            return Ok(new { ValorFrete = valorFrete });
        }
    }

    public class ProdutoDto
    {
        public string Nome { get; set; }
        public float Peso { get; set; }
        public float Altura { get; set; }
        public float Largura { get; set; }
        public float Comprimento { get; set; }
        public string UF { get; set; }
    }
}
