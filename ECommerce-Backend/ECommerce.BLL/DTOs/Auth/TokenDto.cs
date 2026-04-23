

namespace ECommerce.BLL
{
    public record TokenDto
        (
            string AccessToken,
            int ExpirationInMinutes,
            string TokenType = "Bearer"
        );
}
