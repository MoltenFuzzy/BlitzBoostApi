namespace ExerciseApi.Models;

public class UserService : IUserService
{
    private readonly IDbService _dbService;

    public UserService(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<bool> CreateUser(User user)
    {
        var result = await _dbService.EditData(
            "INSERT INTO public.employee (id,name, age, address, mobile_number) VALUES (@Id, @Name, @Age, @Address, @MobileNumber)",
            user
        );
        return true;
    }

    public async Task<List<User>> GetUserList()
    {
        var userList = await _dbService.GetAll<User>("SELECT * FROM public.employee", new { });
        return userList;
    }

    public async Task<User> GetUser(int id)
    {
        var userList = await _dbService.GetAsync<User>(
            "SELECT * FROM public.employee where id=@id",
            new { id }
        );
        return userList;
    }

    public async Task<User> UpdateUser(User user)
    {
        var updateUser = await _dbService.EditData(
            "Update public.employee SET name=@Name, age=@Age, address=@Address, mobile_number=@MobileNumber WHERE id=@Id",
            user
        );
        return user;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var deleteUser = await _dbService.EditData(
            "DELETE FROM public.employee WHERE id=@Id",
            new { id }
        );
        return true;
    }
}
