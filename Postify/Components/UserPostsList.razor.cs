namespace Postify.Components;

public partial class UserPostsList
{

    [Parameter] public string Id { get; set; } = string.Empty;

    [Inject] public PostsService? Posts { get; set; }

    List<Post> userPosts = new();

    protected override async Task OnInitializedAsync()
    {

        if (Posts is null)
            return;

        userPosts = await Posts.GetUserPostsAsync(Id);

    }

}
