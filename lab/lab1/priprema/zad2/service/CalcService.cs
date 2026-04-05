using Grpc.Core;
using Zad2_Service;

namespace Zad2_Service.Services;

public class CalcServiceImpl : CalcService.CalcServiceBase
{
    private static double acc = 1;
    
    // Procedura 1
    public override Task<Empty> AddAverage(NumberRequest request, ServerCallContext context)
    {
        int n = request.Value;
        if (n <= 0) return Task.FromResult<Empty>(new Empty());


        double avg = (n + 1) / 2.0;
        acc += avg;
        Console.WriteLine($"Added average of first {n} natural numbers: {avg}. Current accumulated value: {acc}");
        return Task.FromResult<Empty>(new Empty());
    }

    //Procedura 2
    public override async Task ProcessStream( IAsyncStreamReader<NumberRequest> requestStream, 
                                        IServerStreamWriter<NumberResponse> responseStream, 
                                        ServerCallContext context)
    {

        int counter = 0;

        await foreach (var item in requestStream.ReadAllAsync())
        {
            counter++;
            int result;

            if (counter % 3 == 0)
                result = (int)(item.Value * acc);
            else
                result = (int)(item.Value - acc);
            
            await responseStream.WriteAsync(new NumberResponse {Value = result});
        }
    }

}
