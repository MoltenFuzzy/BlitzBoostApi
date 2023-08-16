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
            "INSERT INTO \"User\" (id, name, email, created_at, updated_at) VALUES (@Id, @Name, @Email, @CreatedAt, @UpdatedAt)",
            user
        );
        return true;
    }

    public async Task<List<User>> GetUserList()
    {
        var userList = await _dbService.GetAll<User>("SELECT * FROM \"User\"", new { });
        return userList;
    }

    public async Task<User> GetUser(int id)
    {
        var userList = await _dbService.GetAsync<User>(
            "SELECT * FROM \"User\" where id=@id",
            new { id }
        );
        return userList;
    }

    public async Task<User> UpdateUser(User user)
    {
        var updateUser = await _dbService.EditData(
            "Update \"User\" SET name=@Name, email=@Email, WHERE id=@Id",
            user
        );
        return user;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var deleteUser = await _dbService.EditData("DELETE FROM \"User\" WHERE id=@Id", new { id });
        return true;
    }
}
