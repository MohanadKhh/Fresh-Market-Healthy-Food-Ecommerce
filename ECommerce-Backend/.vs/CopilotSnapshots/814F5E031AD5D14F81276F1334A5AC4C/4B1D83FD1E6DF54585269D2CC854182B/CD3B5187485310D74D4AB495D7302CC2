using ECommerce.Common;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECommerce.BLL
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSetting;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        public AuthManager(UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager
            , IUnitOfWork unitOfWork
            , IOptions<JwtSettings> jwtSettings
            , IValidator<RegisterDto> registerValidator
            , IValidator<LoginDto> loginValidator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _jwtSetting = jwtSettings.Value;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        public async Task<GeneralResult> RegisterUserAsync(RegisterDto registerDto)
        {
            var registerValidationResult = await _registerValidator.ValidateAsync(registerDto);
            if (!registerValidationResult.IsValid)
            {
                Dictionary<string, List<Error>> errors = registerValidationResult.ToError();
                return GeneralResult.FailedResult(errors);
            }

            var applicationUser = new ApplicationUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName
            };
            IdentityResult result = await _userManager.CreateAsync(applicationUser, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return GeneralResult.FailedResult($"User registration failed: " + string.Join(" | ", errors));
            }
            IdentityResult addRoleResult = await _userManager.AddToRoleAsync(applicationUser, "User");

            if (!addRoleResult.Succeeded)
            {
                return GeneralResult.FailedResult("Role assignment failed");
            }

            var cart = new Cart
            {
                UserId = applicationUser.Id,
                User = applicationUser
            };

            _unitOfWork.CartRepository.Add(cart);
            await _unitOfWork.SaveAsync();

            return GeneralResult.SuccessedResult("User registered successfully");
        }

        public async Task<GeneralResult<TokenDto>> LoginUserAsync(LoginDto loginDto)
        {
            var loginValidationResult = await _loginValidator.ValidateAsync(loginDto);
            if (!loginValidationResult.IsValid)
            {
                Dictionary<string, List<Error>> errors = loginValidationResult.ToError();
                return GeneralResult<TokenDto>.FailedResult(errors);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return GeneralResult<TokenDto>.NotFound("Invalid Email or Password");
            }

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(
                user, loginDto.Password, lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);

                var remainingTime = lockoutEnd.HasValue
                    ? (lockoutEnd.Value - DateTimeOffset.UtcNow).TotalSeconds
                    : 0;

                return GeneralResult<TokenDto>.FailedResult(
                    $"Account locked. Try again after {Math.Ceiling(remainingTime)} seconds");
            }

            if (result.IsNotAllowed)
            {
                return GeneralResult<TokenDto>.FailedResult("User is not allowed to login (email or phone not confirmed)");
            }

            if (!result.Succeeded)
            {
                return GeneralResult<TokenDto>.FailedResult("Invalid Email or Password");
            }

            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.FullName)
            ];

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = GenerateToken(claims);
            return GeneralResult<TokenDto>.SuccessedResult(token, "User logged in successfully");
        }

        private TokenDto GenerateToken(List<Claim> claims)
        {
            byte[] keyInBytes;
            try
            {
                keyInBytes = Convert.FromBase64String(_jwtSetting.SecretKey);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("JwtSettings:SecretKey must be a valid Base64 string.");
            }
            if (keyInBytes.Length < 16)
            {
                throw new InvalidOperationException("JwtSettings:SecretKey must decode to at least 16 bytes (128 bits).");
            }
            var key = new SymmetricSecurityKey(keyInBytes);

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            if (_jwtSetting.ExpirationInMinutes <= 0)
            {
                throw new InvalidOperationException("JwtSettings:ExpirationInMinutes must be > 0.");
            }
            var expiration = DateTime.UtcNow.AddMinutes(_jwtSetting.ExpirationInMinutes);

            var jwt = new JwtSecurityToken(
                issuer: _jwtSetting.Issuer,
                audience: _jwtSetting.Audience,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(jwt);
            var tokenDto = new TokenDto(tokenHandler,
                            _jwtSetting.ExpirationInMinutes);

            return tokenDto;
        }
    }
}
