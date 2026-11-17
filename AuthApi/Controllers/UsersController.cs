using AuthApi.Models;
using AuthApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AuthuserContext _context;

        public UsersController(AuthuserContext context)
        {
            _context = context;
        }

        // Összes user lekérdezése (jelszó nélkül, szerepkörökkel)
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Include(u => u.Roles)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.RegTime,
                    Roles = u.Roles.Select(r => new { r.Id, r.Name })
                })
                .ToListAsync();

            return Ok(users);
        }

        // Egy user lekérdezése ID alapján (jelszó nélkül)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.RegTime,
                    Roles = u.Roles.Select(r => new { r.Id, r.Name })
                })
                .FirstOrDefaultAsync();

            if (user is null)
            {
                return NotFound(new { message = "Nincs ilyen felhasználó!" });
            }

            return Ok(user);
        }

        // Új user felvétele (órán írt verzió)
        [HttpPost("newUser")]
        public async Task<IActionResult> NewUser([FromBody] NewUserDto newUserDto)
        {
            if (newUserDto is null)
            {
                return BadRequest(new { message = "Nincs user akit hozzáadjak!" });
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = newUserDto.Name,
                Email = newUserDto.Email,
                Password = newUserDto.Password,
                RegTime = DateTime.Now
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Felvéve" });
        }

        // User módosítása
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (existingUser is null)
            {
                return NotFound(new { message = "Nincs ilyen felhasználó!" });
            }

            existingUser.Name = updateUserDto.Name;
            existingUser.Email = updateUserDto.Email;
            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                existingUser.Password = updateUserDto.Password;
            }

            _context.Users.Update(existingUser); // Ez szinkron!
            await _context.SaveChangesAsync();

            return Ok(new { message = "Sikeres módosítás!" });
        }

        // User törlése
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (existingUser is null)
            {
                return NotFound(new { message = "Nincs ilyen felhasználó!" });
            }

            _context.Users.Remove(existingUser); // Ez szinkron!
            await _context.SaveChangesAsync();

            return Ok(new { message = "Sikeres törlés!" });
        }
    }
}