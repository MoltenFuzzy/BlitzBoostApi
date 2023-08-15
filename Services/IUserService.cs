using ExerciseApi.Models;

public interface IUserService
{
    Task<bool> CreateUser(User employee);
    Task<List<User>> GetUserList();
    Task<User> GetUser(int key);
    Task<User> UpdateUser(User employee);
    Task<bool> DeleteUser(int key);
}
