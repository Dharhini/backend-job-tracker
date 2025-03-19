using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using server.Models;
using System.Threading.Tasks;
using System.Data;
using server.Data;


[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase {
    private readonly DatabaseContext _context;

    public AuthController(DatabaseContext context) {
        _context = context;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User user) {
        if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password)) {
            return BadRequest(new { success = false, message = "Email and Password are required" });
        }

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

        if (existingUser == null) {
            return Unauthorized(new { success = false, message = "User not found" });
        }

        // Ensure password matches (if using hashing, update this logic)
        if (existingUser.Password != user.Password) {
            return Unauthorized(new { success = false, message = "Invalid password" });
        }

        return Ok(new { success = true, token = "fake-jwt-token" });
    }


    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] User user) {
        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password)) {
            return BadRequest(new { success = false, message = "Email and Password are required" });
        }

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser != null) {
            return BadRequest(new { success = false, message = "User already exists" });
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(new { success = true });
    }
}
