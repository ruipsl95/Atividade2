using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using VotingSystem.Voting; // do csharp_namespace do .proto

public class VotingServiceImpl : VotingService.VotingServiceBase
{
    // Lista fixa de candidatos
    private static readonly List<Candidate> Candidates = new()
    {
        new Candidate { Id = 1, Name = "Candidato A" },
        new Candidate { Id = 2, Name = "Candidato B" },
        new Candidate { Id = 3, Name = "Candidato C" },
    };

    // Três credenciais válidas (mock)
    private static readonly HashSet<string> ValidCredentials = new()
    {
        "CRED-ABC-123",
        "CRED-DEF-456",
        "CRED-GHI-789"
    };

    // Contagem de votos em memória: candidateId -> nº de votos
    private static readonly Dictionary<int, int> VoteCounts = new();

    public override Task<GetCandidatesResponse> GetCandidates(
        GetCandidatesRequest request,
        ServerCallContext context)
    {
        var response = new GetCandidatesResponse();
        response.Candidates.AddRange(Candidates);
        return Task.FromResult(response);
    }

    public override Task<VoteResponse> Vote(
        VoteRequest request,
        ServerCallContext context)
    {
        var credential = request.VotingCredential;
        var candidateId = request.CandidateId;

        // 1. Validar credencial
        if (!ValidCredentials.Contains(credential))
        {
            return Task.FromResult(new VoteResponse
            {
                Success = false,
                Message = "Credencial de voto inválida ou não autorizada."
            });
        }

        // 2. Validar candidato
        var candidate = Candidates.FirstOrDefault(c => c.Id == candidateId);
        if (candidate == null)
        {
            return Task.FromResult(new VoteResponse
            {
                Success = false,
                Message = "Candidato inexistente."
            });
        }

        // 3. Atualizar contagem de votos em memória
        if (!VoteCounts.ContainsKey(candidateId))
        {
            VoteCounts[candidateId] = 0;
        }

        VoteCounts[candidateId]++;

        return Task.FromResult(new VoteResponse
        {
            Success = true,
            Message = $"Voto registado para o {candidate.Name}."
        });
    }

    public override Task<GetResultsResponse> GetResults(
        GetResultsRequest request,
        ServerCallContext context)
    {
        var response = new GetResultsResponse();

        foreach (var candidate in Candidates)
        {
            VoteCounts.TryGetValue(candidate.Id, out int votes);

            var result = new CandidateResult
            {
                Id = candidate.Id,
                Name = candidate.Name,
                Votes = votes
            };

            response.Results.Add(result);
        }

        return Task.FromResult(response);
    }
}
