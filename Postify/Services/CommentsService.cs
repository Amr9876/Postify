
namespace Postify.Services;

public class CommentsService
{

    private readonly HttpClient _http;

    private readonly ILogger<CommentsService> _logger;

    private readonly ProtectedLocalStorage _localStorage;

    public CommentsService(HttpClient http,
                           ILogger<CommentsService> logger, 
                           ProtectedLocalStorage localStorage)
    {
        _http = http;
        _logger = logger;
        _localStorage = localStorage;
    }

    public async Task<List<Comment>> GetCommentsAsync(string postId)
    {
        var result = await _http.GetFromJsonAsync<CommentsResponse>($"comments/{postId}");

        if (result is null)
            return new();

        return result.Comments;
    }


    public async Task AddCommentAsync(CommentRequest request, string postId)
    {
        var token = await _localStorage.GetAsync<string>("access_token");

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
        (
            "Bearer", 
            token.Value
        );

        var result = await _http.PostAsJsonAsync($"comments/{postId}", request);

        if (result.IsSuccessStatusCode == false)
        {
            var response = await result.Content.ReadFromJsonAsync<ErrorResponse>();

            if (response is null)
                return;

            _logger.LogError(response.ErrorMessage);
        }

    }

}
