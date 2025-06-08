using Confluent.Kafka;
using InvestControl.Producer.Model;

namespace InvestControl.Producer.Services
{
    public class KafkaProducerService
    {
        private readonly string _broker;
        private readonly string _topic;

        public KafkaProducerService(IConfiguration config)
        {
            _broker = config["Kafka:Broker"];
            _topic = config["Kafka:Topic"];
        }

        public async Task EnviarAsync(CotacaoDTO cotacao)
        {
            var config = new ProducerConfig { BootstrapServers = _broker };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            try
            {
                var message = new Message<Null, string>
                {
                    Value = System.Text.Json.JsonSerializer.Serialize(cotacao)
                };
                var deliveryResult = await producer.ProduceAsync(_topic, message);
                Console.WriteLine($"Mensagem enviada para o tópico '{_topic}' com offset {deliveryResult.Offset}");
            }
            catch (ProduceException<Null, string> ex)
            {
                Console.WriteLine($"Erro ao enviar mensagem: {ex.Error.Reason}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }
        }
    }
}
