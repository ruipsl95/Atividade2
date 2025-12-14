
using Grpc.Net.Client;
using VotingSystem; 

//Permissão de HTTP2 sem TLS (desenvolvimento local no mac)
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);


var channel = GrpcChannel.ForAddress("http://localhost:5137");
var client = new VoterRegistrationService.VoterRegistrationServiceClient(channel);

Console.WriteLine("=== Registo (Cliente) ===");
Console.Write("Insira o Nº Cartão Cidadão: ");
var cc = Console.ReadLine();

try {
    var reply = await client.IssueVotingCredentialAsync(new VoterRequest { CitizenCardNumber = cc });
    
    if (reply.IsEligible) {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nSucesso! Credencial emitida: {reply.VotingCredential}");
        Console.WriteLine("Reserve esta credencial para utilizar na App de Votação.");
    } else {
        Console.WriteLine("Cidadão não elegível.");
    }
} catch (Exception ex) {
    Console.WriteLine("Ocorreu um erro a obter a credencial!")
    Console.WriteLine($"Erro: {ex.Message}");
}
Console.ResetColor();

