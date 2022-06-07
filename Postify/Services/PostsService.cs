
namespace Postify.Services;

public class PostsService
{

    private readonly HttpClient _http;

    private readonly ProtectedLocalStorage _localStorage;

    public PostsService(HttpClient http,
                        ILogger<PostsService> logger,
                        ProtectedLocalStorage localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    public async Task<List<Post>> GetAllAsync()
    {

        var result = await _http.GetFromJsonAsync<PostsResponse>("posts");

        if (result is null)
            return new();

        return result.Posts;

    }

    public async Task<Post> GetPostAsync(string? id)
    {
        var result = await _http.GetFromJsonAsync<PostResponse>($"posts/{id}");

        if (result is null)
            return new();

        return result.Post;
    }

    public async Task<List<Post>> GetUserPostsAsync(string userId)
    {

        var result = await _http.GetFromJsonAsync<PostsResponse>($"posts/user/{userId}");

        if (result is null)
            return new();

        return result.Posts;

    }

    public async Task AddPostAsync(PostRequest request)
    {

        var token = await _localStorage.GetAsync<string>("access_token");

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
        (
            "Bearer",
            token.Value
        );

        await _http.PostAsJsonAsync("posts", request);
    }

    public async Task EditPostAsync(string id, PostRequest request)
        => await _http.PutAsJsonAsync($"posts/{id}", request);

    public async Task RemovePostAsync(string id)
        => await _http.DeleteAsync($"posts/{id}");

}
