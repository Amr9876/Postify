namespace Postify.API.Controllers;

public class PostsController : ApiController
{

    private readonly AppDbContext _db;

    public PostsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Post>>> GetPosts()
        => Ok(new PostsResponse(await _db.Posts.ToListAsync()));

    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPost([FromRoute] string id)
    {

        if (string.IsNullOrEmpty(id))
            return BadRequest(new ErrorResponse("Invalid ID!"));

        var post = await _db.Posts.SingleOrDefaultAsync(x => x.Id == id);

        if (post is null)
            return NotFound(new ErrorResponse("Post not found!", 404));

        return Ok(new PostResponse(post));
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<Post>> GetUserPosts([FromRoute] string userId)
    {

        if (userId is null)
            return NotFound(new ErrorResponse("User Id not found!", 404));

        var user = await _db.Users.SingleOrDefaultAsync(x => x.Id == userId);

        if (user is null)
            return NotFound(new ErrorResponse("User not found!", 404));

        var posts = await _db.Posts.Where(x => x.Creator == user.FullName).ToListAsync();

        if (posts is null)
            return NotFound(new ErrorResponse("There is not posts to show!", 404));

        return Ok(new PostsResponse(posts));

    }

    [HttpPost]
    public async Task<ActionResult> AddPost([FromBody] PostRequest request)
    {
        if (string.IsNullOrEmpty(HttpContext.Request.Headers["Authorization"]))
            return BadRequest(new ErrorResponse("Bearer Token required!"));

        string tokenString = HttpContext.Request.Headers["Authorization"];

        var jwtToken = new JwtSecurityTokenHandler()
                           .ReadJwtToken(tokenString.Split(" ")[1]);

        string userName = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;

        var post = request.ToPost(Guid.NewGuid().ToString(), userName);

        _db.Posts.Add(post);

        await _db.SaveChangesAsync();

        return Ok();
        
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Post>> UpdatePost([FromRoute] string id,
                                                     [FromBody] PostRequest request)
    {

        if (string.IsNullOrEmpty(id))
            return BadRequest(new ErrorResponse("Invalid ID"));

        var post = await _db.Posts.SingleOrDefaultAsync(x => x.Id == id);

        if (post is null)
            return NotFound(new ErrorResponse("post not found!", 404));

        post.Title = request.Title;
        post.ImageUrl = request.ImageUrl;
        post.Description = request.Description;
        post.Date = DateOnly.FromDateTime(DateTime.Now).ToString();

        _db.Posts.Update(post);

        await _db.SaveChangesAsync();

        return Ok(new PostResponse(post));

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost([FromRoute] string id)
    {
        if (string.IsNullOrEmpty(id))
            return BadRequest(new ErrorResponse("Invalid ID"));

        var post = await _db.Posts.SingleOrDefaultAsync(x => x.Id == id);

        if (post is null)
            return NotFound(new ErrorResponse("Post is not found!"));

        _db.Posts.Remove(post);

        await _db.SaveChangesAsync();

        return Ok();
    }

}
