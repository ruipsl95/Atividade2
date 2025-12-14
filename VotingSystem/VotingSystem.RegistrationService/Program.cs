

using VotingSystem;

var builder = WebApplication.CreateBuilder(args);

// Regista gRPC
builder.Services.AddGrpc();

var app = builder.Build();

// Mapeia o serviço gRPC
app.MapGrpcService<VoterRegistrationServiceImpl>();

// Endpoint HTTP simples para veres se está vivo num browser
app.MapGet("/", () => "Servidor gRPC - Serviço de Registo de Eleitores ativo.");

app.Run();
