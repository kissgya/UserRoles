using AuthApi.Models;
using AuthApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Controllers
{
    [Route("userrole")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly AuthuserContext _context;

        public UserRoleController(AuthuserContext context)
        {
            _context = context;
        }

        // Szerepkör hozzárendelése felhasználóhoz
        [HttpPost]
        public async Task<IActionResult> AddRoleToUser([FromBody] UserRoleDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user is null)
            {
                return NotFound(new { message = "Nincs ilyen felhasználó!" });
            }

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == dto.RoleId);

            if (role is null)
            {
                return NotFound(new { message = "Nincs ilyen szerepkör!" });
            }

            if (user.Roles.Any(r => r.Id == dto.RoleId))
            {
                return BadRequest(new { message = "A felhasználó már rendelkezik ezzel a szerepkörrel!" });
            }

            user.Roles.Add(role);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Szerepkör sikeresen hozzárendelve!" });
        }

        // Szerepkör elvétele felhasználótól
        [HttpDelete]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] UserRoleDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user is null)
            {
                return NotFound(new { message = "Nincs ilyen felhasználó!" });
            }

            var role = user.Roles.FirstOrDefault(r => r.Id == dto.RoleId);

            if (role is null)
            {
                return NotFound(new { message = "A felhasználónak nincs ilyen szerepköre!" });
            }

            user.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Szerepkör sikeresen eltávolítva!" });
        }
    }
}