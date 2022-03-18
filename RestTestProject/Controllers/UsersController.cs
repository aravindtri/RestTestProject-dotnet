using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Http.Headers;

namespace RestTestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public UsersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            return Ok(await _dataContext.Users.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var usr = await _dataContext.Users.FindAsync(id);
            if (usr == null)
            {
                return BadRequest("User Not Found");
            }
            return Ok(usr);
        }
        /// <summary>
        /// Saves user in request and returns all users.
        /// </summary>
        /// <returns>All users</returns>
        /// <responseType>
        ///     <see cref="List{User}" />
        /// </responseType>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<User>), Description = "Returns all users")]
        [HttpPost]
        public async Task<ActionResult<List<User>>> AddUser(User user)
        {
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.Users.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<User>>> UpdateUser(User user)
        {
            var usr = await _dataContext.Users.FindAsync(user.Id);
            if (usr == null)
            {
                return BadRequest("User Not Found");
            }
            usr.Name = user.Name;
            usr.Age = user.Age;
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.Users.ToListAsync());

        }

        [HttpDelete]
        public async Task<ActionResult<List<User>>> DeleteUser(int id)
        {
            var usr = await _dataContext.Users.FindAsync(id);
            if (usr == null)
            {
                return BadRequest("User Not Found");
            }

            _dataContext.Users.Remove(usr);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.Users.ToListAsync());

        }

    }
}
