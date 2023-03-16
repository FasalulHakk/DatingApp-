using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    [ApiController]
    [Route("api/users")] //  /api/users

    public class UsersController
    {
        private readonly DataConetxt _conetxt;

        public UsersController(DataConetxt conetxt)
        {
            _conetxt = conetxt;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _conetxt.Users.ToListAsync();

            return users;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await _conetxt.Users.FindAsync(id);
            return user;
        }

    }
}