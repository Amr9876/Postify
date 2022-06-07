
namespace Postify.API.Controllers;

public class UsersController : ApiController
{

    private readonly AppDbContext _db;

    public UsersController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
        => Ok(new UsersResponse(await _db.Users.ToListAsync()));

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser([FromRoute] string id)
    {

        if (string.IsNullOrEmpty(id))
            return BadRequest(new ErrorResponse("Invalid ID!"));

        var user = await _db.Users.SingleOrDefaultAsync(x => x.Id == id);

        if (user is null)
            return NotFound(new ErrorResponse("User not found!", 404));

        return Ok(new UserResponse(user));

    }

}
