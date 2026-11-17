using AuthApi.Models;
using AuthApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Controllers
{
    [Route("roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AuthuserContext _context;

        public RolesController(AuthuserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _context.Roles
                .Select(r => new { r.Id, r.Name, r.RegTime })
                .ToListAsync();

            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            var role = await _context.Roles
                .Where(r => r.Id == id)
                .Select(r => new { r.Id, r.Name, r.RegTime })
                .FirstOrDefaultAsync();

            if (role is null)
            {
                return NotFound(new { message = "Nincs ilyen szerepkör!" });
            }

            return Ok(role);
        }

        [HttpPost("newRole")]
        public async Task<IActionResult> NewRole([FromBody] RoleDto roleDto)
        {
            if (roleDto is null || string.IsNullOrWhiteSpace(roleDto.Name))
            {
                return BadRequest(new { message = "Hiányzó szerepkör név!" });
            }

            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = roleDto.Name,
                RegTime = DateTime.Now
            };

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Szerepkör felvéve!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] RoleDto roleDto)
        {
            var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);

            if (existingRole is null)
            {
                return NotFound(new { message = "Nincs ilyen szerepkör!" });
            }

            existingRole.Name = roleDto.Name;

            _context.Roles.Update(existingRole);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Szerepkör módosítva!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);

            if (existingRole is null)
            {
                return NotFound(new { message = "Nincs ilyen szerepkör!" });
            }

            _context.Roles.Remove(existingRole);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Szerepkör törölve!" });
        }
    }
}