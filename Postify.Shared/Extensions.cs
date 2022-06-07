
namespace Postify.Shared;

using Requests;

public static class Extensions
{

    public static Comment ToComment(this CommentRequest request, string id, string creator)
    {

        return new Comment
        {
            Id = id,
            Body = request.Body,
            Creator = creator,
            Date = DateOnly.FromDateTime(DateTime.Now).ToString()
        };

    }

    public static Post ToPost(this PostRequest request, string id, string creator)
    {

        return new Post
        {
            Id = id,
            Title = request.Title,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Creator = creator,
            Date = DateOnly.FromDateTime(DateTime.Now).ToString()
        };
    }

    public static User ToUser(this RegisterRequest request)
    {

        if (string.IsNullOrEmpty(request.PhoneNumber))
        {
            request.PhoneNumber = "000-0000-0000";
        }

        return new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            FullName = request.FullName,
            Password = Utils.Hash(request.Password),
            PhoneNumber = request.PhoneNumber
        };
    }

}
