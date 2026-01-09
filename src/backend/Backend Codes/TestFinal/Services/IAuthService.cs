using Microsoft.EntityFrameworkCore;
using TestFinal.Data;
using TestFinal.Models;
using TestFinal.Services.Interfaces;

namespace TestFinal.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User?> ValidateUserAsync(string username, string password)
    {
        // Dummy yok: DB üzerinden user doğrulama
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username && u.IsActive == true);

        if (user == null) return null;

        // Senin DB scriptinde PasswordHash demo düz string (1234)
        // (İstersen ileride hash verify yapılır)
        return user.PasswordHash == password ? user : null;
    }
}
