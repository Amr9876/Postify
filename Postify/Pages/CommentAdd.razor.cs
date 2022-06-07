namespace Postify.Pages;

public partial class CommentAdd
{

    [Parameter] public string Id { get; set; } = string.Empty;

    [Inject] public CommentsService? Comments { get; set; }

    [Inject] public NavigationManager? NavManager { get; set; }

    CommentRequest request = new();

    async Task SubmitHandler()
    {
        if (Comments is null)
            return;

        if (NavManager is null)
            return;

        await Comments.AddCommentAsync(request, Id);

        NavManager.NavigateTo($"/post/open/{Id}");
    }

}
