using System.Diagnostics;
using OpenTelemetry; // Necessário para o Sdk.CreateMeterProvider
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ChatWebApp.Api
{
    public class Program
    {
        public static readonly ActivitySource MyActivitySource = new ActivitySource("ChatWebApp.Api");

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Cria um Span de inicialização para teste
            using (var activity = MyActivitySource.StartActivity("ApplicationStarting"))
            {
                activity?.SetTag("status", "starting");
                activity?.AddEvent(new ActivityEvent("Host construído. Executando a aplicação..."));
                Console.WriteLine("OpenTelemetry: Span de inicialização 'ApplicationStarting' criado.");
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                // ********** INÍCIO DA CONFIGURAÇÃO COMPLETA DO OPENTELEMETRY **********
                .ConfigureServices((hostContext, services) =>
                {
                    var serviceName = "ChatWebApp.Api";
                    var serviceVersion = "1.0.0";

                    // Lê o endpoint do appsettings.json ou usa um padrão
                    var otlpEndpoint = hostContext.Configuration.GetValue<string>("Otlp:Endpoint") ?? "http://localhost:4317";

                    services.AddOpenTelemetry( )
                        // 1. CONFIGURAÇÃO DO RECURSO (comum para traces e métricas)
                        .ConfigureResource(resource => resource
                            .AddService(serviceName: serviceName, serviceVersion: serviceVersion))

                        // 2. CONFIGURAÇÃO DE TRACING (equivalente ao traceExporter e instrumentações)
                        .WithTracing(tracing =>
                        {
                            tracing
                                .AddSource(MyActivitySource.Name)
                                .AddAspNetCoreInstrumentation() // Equivalente a @opentelemetry/instrumentation-express
                                .AddHttpClientInstrumentation(); // Equivalente a @opentelemetry/instrumentation-http

                            tracing.AddOtlpExporter(otlpOptions =>
                            {
                                otlpOptions.Endpoint = new Uri(otlpEndpoint );
                                otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                                // A compressão GZIP é habilitada por padrão no exportador gRPC do .NET
                                // A conexão insegura é o padrão para endpoints http://
                            } );
                        })

                        // 3. CONFIGURAÇÃO DE MÉTRICAS (equivalente ao metricExporter e metricReader)
                        .WithMetrics(metrics =>
                        {
                            metrics
                                .AddAspNetCoreInstrumentation()
                                .AddHttpClientInstrumentation()
                                .AddRuntimeInstrumentation(); // Coleta métricas de runtime do .NET

                            metrics.AddOtlpExporter(otlpOptions =>
                            {
                                otlpOptions.Endpoint = new Uri(otlpEndpoint);
                                otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                                // Equivalente a PeriodicExportingMetricReader
                                // O exportador OTLP no .NET já usa um leitor periódico por padrão.
                                // As opções abaixo ajustam os intervalos para corresponder ao seu código Node.js.
                                otlpOptions.ExportProcessorType = ExportProcessorType.Batch;
                                otlpOptions.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>
                                {
                                    // Equivalente a exportIntervalMillis
                                    ScheduledDelayMilliseconds = 10000,
                                    // Equivalente a exportTimeoutMillis
                                    ExporterTimeoutMilliseconds = 3000
                                };
                            });
                        });
                });
        // ********** FIM DA CONFIGURAÇÃO COMPLETA DO OPENTELEMETRY **********
    }
}
