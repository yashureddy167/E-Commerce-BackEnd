namespace AuthService.Application.Interfaces.Services
{
    public interface IPasswordService
    {
        Task<string> HashPasswordAsync(string password);
        Task<bool> VerfiyPasswordAsync(string hashedPassword, string providedPassword);
    }
}
