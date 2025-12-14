using System.Security.Cryptography;
using System.Threading.Tasks;
using Grpc.Core;
using VotingSystem;

public class VoterRegistrationServiceImpl : VoterRegistrationService.VoterRegistrationServiceBase
{
    // As 3 credenciais válidas usadas pelo Serviço B
    private static readonly string[] ValidCredentials =
    {
        "CRED-ABC-123",
        "CRED-DEF-456",
        "CRED-GHI-789"
    };

    public override Task<VoterResponse> IssueVotingCredential(
        VoterRequest request,
        ServerCallContext context)
    {
        string credential;

        // Gerar número aleatório entre 0 e 1
        double p = RandomNumberGenerator.GetInt32(0, 100) / 100.0;

        if (p < 0.30)
        {
            // 30% das vezes → enviar credencial inválida
            credential = GenerateInvalidCredential();
            Console.WriteLine(">> Enviada credencial **inválida** para teste.");
        }
        else
        {
            // 70% das vezes → credencial válida
            credential = PickValidCredential();
            Console.WriteLine(">> Enviada credencial válida.");
        }

        var response = new VoterResponse
        {
            IsEligible = true,
            VotingCredential = credential
        };

        return Task.FromResult(response);
    }

    private static string PickValidCredential()
    {
        int idx = RandomNumberGenerator.GetInt32(ValidCredentials.Length);
        return ValidCredentials[idx];
    }

    private static string GenerateInvalidCredential()
    {
        // Gera uma credencial aleatória que não coincide com a lista válida
        var bytes = RandomNumberGenerator.GetBytes(6);
        return "INVALID-" + Convert.ToHexString(bytes);
    }
}
