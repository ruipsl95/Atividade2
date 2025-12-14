using VotingSystem.Voting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<VotingServiceImpl>();

app.MapGet("/", () => "Servidor gRPC - Serviço de Votação ativo.");

app.Run();

