using Zad1_Client;
using Grpc.Net.Client;
using Grpc.Core;

var channel = GrpcChannel.ForAddress("http://localhost:5178");
var client = new CalcService.CalcServiceClient(channel);

// Procedura a: testiranje unary poziva
Console.WriteLine("### Procedura a: UpdateAcc ###");

await client.UpdateAccAsync(new NumberRequest { Value = 5 });
Console.WriteLine("Poziv 1 (neparni): acc -= 5");

await client.UpdateAccAsync(new NumberRequest { Value = 3 });
Console.WriteLine("Poziv 2 (parni): acc += 3");

await client.UpdateAccAsync(new NumberRequest { Value = 10 });
Console.WriteLine("Poziv 3 (neparni): acc -= 10");

await client.UpdateAccAsync(new NumberRequest { Value = 7 });
Console.WriteLine("Poziv 4 (parni): acc += 7");

Console.WriteLine("Unary pozivi zavrseni.\n");

// Procedura b: bidirectional stream
Console.WriteLine("### Procedura b: ProcessStream ###");

using var call = client.ProcessStream();

var responseTask = Task.Run(async () =>
{
    await foreach (var response in call.ResponseStream.ReadAllAsync())
    {
        Console.WriteLine($"Server vraca: {response.Value}");
    }
});

for (int i = 1; i <= 6; i++)
{
    await call.RequestStream.WriteAsync(new NumberRequest { Value = i });
    Console.WriteLine($"Poslat: {i}");
}

await call.RequestStream.CompleteAsync();
await responseTask;

Console.WriteLine("\nPritisni Enter za izlaz...");
Console.ReadLine();
