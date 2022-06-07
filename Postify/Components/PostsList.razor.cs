
namespace Postify.Components;

public partial class PostsList : ComponentBase
{

    [Inject] public PostsService? Posts { get; set; }

    List<Post> postsList = new();

    protected override async Task OnInitializedAsync()
    {

        if (Posts is not null)
        {
            postsList = await Posts.GetAllAsync();
        }

    }

}
