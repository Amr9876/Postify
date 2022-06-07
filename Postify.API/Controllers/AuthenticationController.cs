
namespace Postify.API.Controllers;

public class AuthenticationController : ApiController
{

    private readonly AppDbContext _db;

    private readonly IConfiguration _config;

    public AuthenticationController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }


    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginRequest request)
    {
        var userPassword = Utils.Hash(request.Password);

        var user = await _db.Users.SingleOrDefaultAsync(x => x.Email == request.Email);

        if (user is null)
            return NotFound(new ErrorResponse("User not found!", 404));
        

        if (!Utils.CompareHash(userPassword, user.Password))
            return BadRequest(new ErrorResponse("Invalid Password!"));

        var claims = new List<Claim>();
        claims.Add(new(ClaimTypes.NameIdentifier, user.Id));
        claims.Add(new(ClaimTypes.Name, user.FullName));
        claims.Add(new(ClaimTypes.Email, user.Email));
        if (user.PhoneNumber is not null)
        {
            claims.Add(new(ClaimTypes.MobilePhone, user.PhoneNumber));
        }
        claims.Add(new("FirstName", user.FirstName));
        claims.Add(new("LastName", user.LastName));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecretKey"]));

        var credetnials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(signingCredentials: credetnials,
                                         claims: claims,
                                         expires: DateTime.Now.AddDays(5));

        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new LoginResponse(tokenString));

    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = request.ToUser();

        var userExists = await _db.Users.AnyAsync(x => x.Email == user.Email);

        if (userExists)
        {
            return BadRequest(new ErrorResponse("User arleady exists!"));
        }

        _db.Users.Add(user);

        await _db.SaveChangesAsync();

        return Ok();
    }

}
