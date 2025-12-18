using Grpc.Net.Client;
using System.Net.Http; 
using VotingSystem; 

var httpHandler = new HttpClientHandler();
httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

var channel = GrpcChannel.ForAddress("https://ken01.utad.pt:9091", new GrpcChannelOptions
{
    HttpHandler = httpHandler
});

var client = new VoterRegistrationService.VoterRegistrationServiceClient(channel);

Console.WriteLine("=== Registo (Cliente) ===");
Console.Write("Insira o Nº Cartão Cidadão: ");
var cc = Console.ReadLine();

try {
    var reply = await client.IssueVotingCredentialAsync(
        new VoterRequest { CitizenCardNumber = cc },
        deadline: DateTime.UtcNow.AddSeconds(5)
    );
    
    if (reply.IsEligible) {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nSucesso! Credencial emitida: {reply.VotingCredential}");
        Console.WriteLine("Reserve esta credencial para utilizar na App de Votação.");
    } else {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Cidadão não elegível.");
    }
} catch (Grpc.Core.RpcException rpcEx) {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Erro gRPC: {rpcEx.Status.StatusCode} - {rpcEx.Status.Detail}");
 
} catch (Exception ex) {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Ocorreu um erro a obter a credencial!");
    Console.WriteLine($"Erro: {ex.Message}");
}
Console.ResetColor();