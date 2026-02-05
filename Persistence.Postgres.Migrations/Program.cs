using Persistence.Postgres;
using Persistence.Postgres.Migrations;

using ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

var pg = new RegisterPostgresServices();
pg.RegisterServices(builder, true);

builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
		.WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

var host = builder.Build();

host.Run();