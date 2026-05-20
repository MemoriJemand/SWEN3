/*using OCRWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();*/
using OCRWorker;
using OCRWorker.Interfaces;
using OCRWorker.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IQueueClient, RabbitMqQueueClient>();
        services.AddSingleton<IOcrEngine, TesseractEngineOcr>();
        services.AddSingleton<PdfToImageConverter>();
        services.AddHostedService<Worker>();
    })
    .Build()
    .Run();