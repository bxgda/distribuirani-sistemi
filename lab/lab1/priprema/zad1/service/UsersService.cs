using Grpc.Core;

namespace Zad1_Service.Services;

public class UsersService : UserService.UserServiceBase
{
    private static List<User> users = new();

    public override Task<User> CreateUser(User request, ServerCallContext context)
    {
        users.Add(request);
        return Task.FromResult(request);
    }

    public override Task<User> GetUser(UserId request, ServerCallContext context)
    {
        var user = users.FirstOrDefault(u => u.Id == request.Id);
        return Task.FromResult(user ?? new User());
    }

    public override Task<User> UpdateUser(User request, ServerCallContext context)
    {
        var user = users.FirstOrDefault(u => u.Id == request.Id);

        if (user != null)
        {
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Address = request.Address;
            user.Phone.Clear();
            user.Phone.AddRange(request.Phone);
        }

        return Task.FromResult(request);
    }

    public override Task<Empty> DeleteUser(UserId request, ServerCallContext context)
    {
        users.RemoveAll(u => u.Id == request.Id);
        return Task.FromResult(new Empty());
    }

    public override Task<UserList> GetUsersInRange(
        IdRangeRequest request,
        ServerCallContext context)
    {
        var result = new UserList();

        result.Users.AddRange(
            users.Where(u => u.Id >= request.StartId &&
                             u.Id <= request.EndId));

        return Task.FromResult(result);
    }

    public override Task<DeleteManyResponse> DeleteMany(
        DeleteManyRequest request,
        ServerCallContext context)
    {
        var response = new DeleteManyResponse();

        foreach (var id in request.Ids)
        {
            var removed = users.RemoveAll(u => u.Id == id) > 0;

            response.Results.Add(new DeleteResult
            {
                Id = id,
                Message = removed ? "Deleted" : "Not found"
            });
        }

        return Task.FromResult(response);
    }
}
