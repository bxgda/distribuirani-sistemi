using Grpc.Core;
using Zad1_Service;

namespace Zad1_Service.Services;

public class CalcServiceImpl : CalcService.CalcServiceBase
{
    private static int acc = 0;
    private static int callCount = 0;

    // Procedura a: parni poziv -> acc += value, neparni poziv -> acc -= value
    public override Task<Empty> UpdateAcc(NumberRequest request, ServerCallContext context)
    {
        callCount++;

        if (callCount % 2 == 0)
        {
            acc += request.Value;
            Console.WriteLine($"Poziv #{callCount} (parni): acc += {request.Value} => acc = {acc}");
        }
        else
        {
            acc -= request.Value;
            Console.WriteLine($"Poziv #{callCount} (neparni): acc -= {request.Value} => acc = {acc}");
        }

        return Task.FromResult(new Empty());
    }

    // Procedura b: svaki 3. element toka sabira sa acc, ostali oduzimaju acc od sebe
    public override async Task ProcessStream(
        IAsyncStreamReader<NumberRequest> requestStream,
        IServerStreamWriter<NumberResponse> responseStream,
        ServerCallContext context)
    {
        int counter = 0;

        await foreach (var item in requestStream.ReadAllAsync())
        {
            counter++;
            int result;

            if (counter % 3 == 0)
            {
                acc += item.Value;
                result = acc;
                Console.WriteLine($"Element #{counter} (svaki 3.): acc += {item.Value} => acc = {acc}");
            }
            else
            {
                result = item.Value - acc;
                Console.WriteLine($"Element #{counter}: {item.Value} - acc({acc}) = {result}");
            }

            await responseStream.WriteAsync(new NumberResponse { Value = result });
        }
    }
}
