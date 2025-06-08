using InvestControl.Producer.Services;

namespace InvestControl.Producer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        private readonly CotacaoApiService _api;
        private readonly KafkaProducerService _kafka;

        public Worker(ILogger<Worker> logger, IConfiguration config, CotacaoApiService api, KafkaProducerService kafka)
        {
            _logger = logger;
            _config = config;
            _api = api;
            _kafka = kafka;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var ativos = _config.GetSection("TickerSettings:Ativos").Get<List<string>>() ?? new List<string>();
            var intervalo = int.Parse(_config["TickerSettings:Intervalo"] ?? "60");

            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var ticker in ativos)
                {
                    var cotacao = await _api.ObterCotacaoAsync(ticker);
                    if (cotacao != null)
                    {
                        await _kafka.EnviarAsync(cotacao);
                        _logger.LogInformation($"Cotação de {ticker} enviada: {cotacao.Preco} às {cotacao.DataHora}");
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(intervalo), stoppingToken);
            }
        }
    }
}
