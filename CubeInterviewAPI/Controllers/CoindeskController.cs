using CubeInterviewAPI.Data;
using CubeInterviewAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CubeInterviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoindeskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly ILogger<CoindeskController> _logger;

        public CoindeskController(ApplicationDbContext context, HttpClient httpClient, ILogger<CoindeskController> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestRates()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://api.coindesk.com/v1/bpi/currentprice.json");
                var coindeskResponse = System.Text.Json.JsonSerializer.Deserialize<CoindeskResponse>(response);

                if (coindeskResponse == null)
                {
                    return BadRequest("Unable to retrieve data from Coindesk.");
                }

                var currencies = await _context.Currencies.ToListAsync();

                var result = new
                {
                    UpdatedTime = DateTime.Parse(coindeskResponse.Time.Updated).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture),
                    Rates = coindeskResponse.Bpi.GetType().GetProperties().Select(p =>
                    {
                        var detail = (CurrencyDetail)p.GetValue(coindeskResponse.Bpi);
                        var currency = currencies.FirstOrDefault(c => c.Code == detail.Code);
                        return new
                        {
                            Code = detail.Code,
                            Name = currency?.Name ?? "Unknown",
                            Rate = detail.Rate
                        };
                    }).ToList()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving or processing data from Coindesk.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}