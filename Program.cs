using InvestControl.Producer;
using InvestControl.Producer.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddHttpClient<CotacaoApiService>();
builder.Services.AddSingleton<KafkaProducerService>();

var host = builder.Build();
host.Run();
