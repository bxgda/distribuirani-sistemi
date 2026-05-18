using Grpc.Core;
using grpc2026;

/*

	1. U .NET-u, koristeći gRPC, kreirati servis za upravljanje listom poruka. Servis treba da omogući klijentima da 
	dodaju i brišu poruke, i prikažu sve poruke.
	Definisati proto fajl (message.proto) koji daje specifikaciju servisa i poruka. Servis treba da podržava sledeće
	operacije:
		- SendMessage: dodaje novu poruku na listu.
		- DeleteMessage: briše poruku sa zadatim ID-jem.
		- ListMessages: Dobije tok podataka koji sadrži sve poruke.
	Implementirati gRPC server definisan u message.proto fajlu.

*/

/*
    grpcurl -plaintext -d '{\"tekst\": \"Zdravo!\"}' localhost:5110 messages.Poruke/SendMessage
    grpcurl -plaintext localhost:5110 messages.Poruke/ListMessages
    grpcurl -plaintext -d '{\"id\": 1}' localhost:5110 messages.Poruke/DeleteMessage
*/

// NE MORA lock-ovi da se koriste ipak, nego to nisam znao kad sam radio ovaj zadatak
// tako da ispada jos prostije onda

public class PorukeService : Poruke.PorukeBase
{
    private static readonly List<Poruka> _poruke = new();
    private static int _nextId = 1;
    private readonly object _lock = new();

    public override Task<Google.Protobuf.WellKnownTypes.Empty> SendMessage(
        PorukaRequest request,
        ServerCallContext context
    )
    {
        lock (_lock)
        {
            var poruka = new Poruka { Id = _nextId++, Tekst = request.Tekst };

            _poruke.Add(poruka);

            return Task.FromResult(new Google.Protobuf.WellKnownTypes.Empty());
        }
    }

    public override Task<PorukaStatus> DeleteMessage(PorukaId request, ServerCallContext context)
    {
        lock (_lock)
        {
            var poruka = _poruke.FirstOrDefault(p => p.Id == request.Id);
            if (poruka != null)
            {
                _poruke.Remove(poruka);
                return Task.FromResult(
                    new PorukaStatus
                    {
                        Status = true,
                        PovratnaPoruka = "Message deleted successfully.",
                    }
                );
            }
            else
            {
                return Task.FromResult(
                    new PorukaStatus { Status = false, PovratnaPoruka = "Message not found." }
                );
            }
        }
    }

    public override async Task ListMessages(
        Google.Protobuf.WellKnownTypes.Empty request,
        IServerStreamWriter<Poruka> responseStream,
        ServerCallContext context
    )
    {
        foreach (var poruka in _poruke)
        {
            // if (context.CancellationToken.IsCancellationRequested)
            //     break;

            await responseStream.WriteAsync(poruka);
        }
    }
}
