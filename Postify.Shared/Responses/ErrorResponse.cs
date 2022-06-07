
namespace Postify.Shared.Responses;

public record ErrorResponse(string ErrorMessage, int ErrorCode = 400);
