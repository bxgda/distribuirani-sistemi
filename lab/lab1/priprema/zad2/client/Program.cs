using Zad2_Client;

using Grpc.Net.Client;
using Grpc.Core;

var channel = GrpcChannel.ForAddress("http://localhost:5227");
var client = new CalcService.CalcServiceClient(channel);

await client.AddAverageAsync(
    new NumberRequest { Value = 5 });

Console.WriteLine("Unary poziv zavrsen");


using var call = client.ProcessStream();

var responseTask = Task.Run(async () =>
{
    await foreach (var response in call.ResponseStream.ReadAllAsync())
    {
        Console.WriteLine($"Server vraca: {response.Value}");
    }
});

// slanje brojeva
for (int i = 1; i <= 6; i++)
{
    await call.RequestStream.WriteAsync(
        new NumberRequest { Value = i });
}

await call.RequestStream.CompleteAsync();

await responseTask;