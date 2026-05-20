using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

/*
	U .NET-u, koristeći gRPC, napisati servis koji prihvata tok podataka, gde se svaki podatak sastoji od dva prirodna broja a i b. 
	Za svaku primljenu poruku, procedura ispituje da li je broj b kvadrat broja a, i vraća nazad klijentu tok poruka sa sadržajem "Da", 
	ukoliko broj b jeste kvadrat broja a, ili "Ne", ukoliko broj b nije kvadrat broja a.

	Napisati i proto fajl (okr2.proto) koji daje specifikaciju servisa i poruka.
*/

namespace grpc2026.Services;

public class Okt2Service : Okt2.Okt2Base
{
    public override async Task JeKvadrat(
        IAsyncStreamReader<Payload> requestStream,
        IServerStreamWriter<StringValue> responseStream,
        ServerCallContext context
    )
    {
        await foreach (var payload in requestStream.ReadAllAsync())
        {
            var a = payload.A;
            var b = payload.B;

            var result = a * a == b ? "Da" : "Ne";

            await responseStream.WriteAsync(new StringValue { Value = result });
        }
    }
}
