using InvestControl.Producer.Model;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace InvestControl.Producer.Services
{
    public class CotacaoApiService
    {
        private readonly HttpClient _http;
        private readonly string _token;

        public CotacaoApiService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _token = config["CotacaoAPI:Token"] ?? string.Empty;

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        public async Task<CotacaoDTO?> ObterCotacaoAsync(string ticker)
        {
            try
            {
                var url = $"https://brapi.dev/api/quote/{ticker}";
                var response = await _http.GetFromJsonAsync<CotacaoApiResponse>(url);

                var item = response?.Results?.FirstOrDefault();

                if (item == null)
                    return null;

                return new CotacaoDTO
                {
                    Ticker = item.Symbol,
                    Preco = item.RegularMarketPrice,
                    DataHora = item.RegularMarketTime.ToLocalTime()
                };
            }
            catch(Exception)
            {
                return null;
            }
        }

    }
}
