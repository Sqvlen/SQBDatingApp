using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Entities;

namespace WebAPI.Controllers
{
    public class BuggyController : BaseAPIController
    {
        public readonly DataBaseContext _dataBaseContext;

        public BuggyController(DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }

        [Authorize]
        [HttpGet("auth")]
        public async Task<ActionResult<string>> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public async Task<ActionResult<AppUser>> GetNotFound()
        {
            var thing = await _dataBaseContext.Users.FindAsync(-1);

            if (thing == null)
                return NotFound();

            return thing;
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            try
            {
                var thing = _dataBaseContext.Users.Find(-1);

                if (thing == null)
                {
                    return StatusCode(500, "Server Error");
                }
                else
                {
                    var thingToReturn = thing.ToString();

                    return thingToReturn;
                }
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was a bad request");
        }
    }
}
