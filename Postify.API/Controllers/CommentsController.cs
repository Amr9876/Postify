
namespace Postify.API.Controllers;

public class CommentsController : ApiController
{

    private readonly AppDbContext _db;

    public CommentsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("{postId}")]
    public async Task<ActionResult<List<Comment>>> GetComments([FromRoute] string postId)
    {

        if (string.IsNullOrEmpty(postId))
            return BadRequest(new ErrorResponse("Invalid Post ID"));

        var postComments = await _db.PostComments.Where(x => x.PostId == postId).ToListAsync();

        if (postComments is null)
            return NotFound(new ErrorResponse("This post have no comments", 404));

        var comments = new List<Comment>();

        foreach (var item in postComments)
        {
            var comment = await _db.Comments.SingleOrDefaultAsync(x => x.Id == item.CommentId);

            if (comment is not null)
                comments.Add(comment);
        }

        return Ok(new CommentsResponse(comments));

    }

    [HttpPost("{postId}")]
    public async Task<ActionResult> AddComment([FromBody] CommentRequest request,
                                               [FromRoute] string postId)
    {

        if (string.IsNullOrEmpty(HttpContext.Request.Headers["Authorization"]))
            return NotFound(new ErrorResponse("bearer token header is not found", 404));

        string tokenString = HttpContext.Request.Headers["Authorization"];

        var jwtToken = new JwtSecurityTokenHandler()
                           .ReadJwtToken(tokenString.Split(" ")[1]);

        string userName = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;

        var comment = request.ToComment(Guid.NewGuid().ToString(),
                                        userName);

        var post = await _db.Posts.SingleOrDefaultAsync(x => x.Id == postId);

        if (post is null)
            return NotFound(new ErrorResponse("post is not found", 404));

        var postComments = new PostComment
        {
            CommentId = comment.Id,
            PostId = post.Id
        };

        _db.Comments.Add(comment);
        _db.PostComments.Add(postComments);

        await _db.SaveChangesAsync();

        return Ok();
    }

}
