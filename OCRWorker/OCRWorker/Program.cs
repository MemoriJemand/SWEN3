/*using OCRWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();*/
using OCRWorker;
using OCRWorker.Interfaces;
using OCRWorker.Services;
using Nest;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IQueueClient, RabbitMqQueueClient>();
        services.AddSingleton<IOcrEngine, TesseractEngineOcr>();
        services.AddHostedService<Worker>();
        services.AddSingleton<IPdfToImageConverter, PdfToImageConverter>();
        services.AddSingleton<ITesseractEngineWrapper, TesseractEngineWrapper>();
        services.AddSingleton<IElasticClient>(sp =>
        {
            var settings = new ConnectionSettings(new Uri("http://elasticsearch:9200"))
                .DefaultIndex("documents");
            return new ElasticClient(settings);
        });
    })
    .Build()
    .Run();