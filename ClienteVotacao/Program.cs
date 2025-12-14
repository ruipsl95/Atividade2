using Grpc.Net.Client;
using VotingSystem.Voting;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

// Endereço do Mockup de Votação 
var channel = GrpcChannel.ForAddress("http://localhost:5159");
var client = new VotingService.VotingServiceClient(channel);

Console.WriteLine("=== ENTIDADE DE VOTAÇÃO (CLIENTE) ===");

while (true)
{
    Console.WriteLine("\n1. Ver Candidatos\n2. Votar\n3. Ver Resultados\n0. Sair");
    Console.Write("Opção: ");
    var op = Console.ReadLine();
    if (op == "0") break;

    try
    {
        if (op == "1")
        {

            var reply = await client.GetCandidatesAsync(new GetCandidatesRequest());
            Console.WriteLine("\n--- CANDIDATOS ---");
            foreach (var c in reply.Candidates) Console.WriteLine($"ID: {c.Id} | Nome: {c.Name}");
        }
        else if (op == "2")
        {

            Console.Write("Insira a Credencial de Voto: ");
            var cred = Console.ReadLine();
            var candidates = await client.GetCandidatesAsync(new GetCandidatesRequest());
           foreach (var c in candidates.Candidates) Console.Write($"ID: {c.Id} ");
            Console.Write("\nID do Candidato: ");
            var id = int.Parse(Console.ReadLine() ?? "0");

            var reply = await client.VoteAsync(new VoteRequest { VotingCredential = cred, CandidateId = id });
            Console.WriteLine(reply.Success ? "Voto Aceite!" : $"ERRO: {reply.Message}");
        }
        else if (op == "3")
        {

            var reply = await client.GetResultsAsync(new GetResultsRequest());
            Console.WriteLine("\n--- RESULTADOS ---");
            foreach (var r in reply.Results) Console.WriteLine($"{r.Name}: {r.Votes} votos");
        }
    }
    catch (Exception ex)
    {
        
        Console.WriteLine($"Erro de comunicação: {ex.Message}");
    }
}