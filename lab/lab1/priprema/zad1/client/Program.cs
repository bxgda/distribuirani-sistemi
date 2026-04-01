using Grpc.Net.Client;
using Zad1_Client;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
var channel = GrpcChannel.ForAddress("http://localhost:5178");
var client = new UserService.UserServiceClient(channel);

static void PrintUser(String label, User user)
{
    Console.WriteLine($"{label}: Id={user.Id}, \n{user.FirstName} {user.LastName}, \nAddress={user.Address}, \nPhones=[{string.Join(", ", user.Phone)}]");
}

Console.WriteLine("### CREATE ###");

var user1 = new User
{
    Id = 1,
    FirstName = "Jovan",
    LastName = "Bogdanovic",
    Address = "Bor",
    Phone = { "1234567" }
};

await client.CreateUserAsync(user1);

PrintUser("Created", user1);

var user2 = new User 
{ 
    Id = 2, 
    FirstName = "Aleksandar", 
    LastName = "Gospavić",
    Phone = { "11111", "22222" } 
};

await client.CreateUserAsync(user2);

PrintUser("Created", user2);

var user3 = new User 
{ 
    Id = 3, 
    FirstName = "Marko", 
    LastName = "Petrovic", 
    Address = "Beograd" 
};

await client.CreateUserAsync(user3);

PrintUser("Created", user3);

Console.WriteLine("\n### GET (single) ###");

var get1 = await client.GetUserAsync(new UserId { Id = 1 });
PrintUser("GetUser(1)", get1);

Console.WriteLine("\n### UPDATE ###");

var updated1 = new User
{
    Id = 1,
    FirstName = "Jovan (Updated)",
    LastName = "Bogda",
    Address = "BORR - Updated",
    Phone = { "123-456", "999-999" }
};

await client.UpdateUserAsync(updated1);

var get1AfterUpdate = await client.GetUserAsync(new UserId { Id = 1 });
PrintUser("After update", get1AfterUpdate);

Console.WriteLine("\n### GET USERS IN RANGE ###");

var range = await client.GetUsersInRangeAsync(new IdRangeRequest { StartId = 1, EndId = 3 });

foreach (var u in range.Users)
{
    PrintUser("Range item", u);
}

Console.WriteLine("\n### DELETE (single) ###");

await client.DeleteUserAsync(new UserId { Id = 2 });

var get2AfterDelete = await client.GetUserAsync(new UserId { Id = 2 });
PrintUser("GetUser(2) after delete", get2AfterDelete);

Console.WriteLine("\n== DELETE MANY ==");


var deleteManyResp = await client.DeleteManyAsync(new DeleteManyRequest { Ids = { 1, 2, 999 } });

foreach (var r in deleteManyResp.Results)
{
    Console.WriteLine($"DeleteMany: Id={r.Id} => {r.Message}");
}

Console.WriteLine("\n### FINAL STATE (range 1..3) ###");

var finalRange = await client.GetUsersInRangeAsync(new IdRangeRequest { StartId = 1, EndId = 3 });

Console.WriteLine($"Remaining users: {finalRange.Users.Count}");

foreach (var u in finalRange.Users)
{
    PrintUser("Remaining", u);
}

Console.WriteLine("\nPritisni Enter za izlaz...");
Console.ReadLine();
