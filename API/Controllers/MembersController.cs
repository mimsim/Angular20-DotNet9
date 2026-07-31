using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<AppUser>> GetMembers()
        {
            var members = context.Users.ToList();
            return Ok(members);
        }

        [HttpGet("{id}")]
        public ActionResult<AppUser> GetMember(string id)
        {
            var member = context.Users.Find(id);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }
    }
}