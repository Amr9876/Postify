using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Postify.Pages;

public partial class PostOpen : ComponentBase
{

    [Parameter] public string? Id { get; set; }

    [Inject] public PostsService? Post { get; set; }

    [Inject] public CommentsService? Comments { get; set; }

    [Inject] public AuthenticationStateProvider? AuthStateProvider { get; set; }

    Post postItem = new();

    List<Comment> comments = new();

    string? creatorName;

    protected override async Task OnInitializedAsync()
    {
        if (Post is null)
            return;

        if (Comments is null)
            return;

        if (AuthStateProvider is null)
            return;

        if (string.IsNullOrEmpty(Id))
            return;

        postItem = await Post.GetPostAsync(Id);

        var authState = await AuthStateProvider.GetAuthenticationStateAsync();

        creatorName = postItem.Creator == authState!.User!.Identity!.Name! ? "YOU" : postItem.Creator;

        comments = await Comments.GetCommentsAsync(Id);

    }

}
