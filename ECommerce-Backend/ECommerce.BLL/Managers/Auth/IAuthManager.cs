using ECommerce.Common;

namespace ECommerce.BLL
{
    public interface IAuthManager
    {
        Task<GeneralResult<TokenDto>> LoginUserAsync(LoginDto loginDto);
        Task<GeneralResult> RegisterUserAsync(RegisterDto registerDto);
    }
}