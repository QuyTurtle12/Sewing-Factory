using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SewingFactory.Models;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SewingFactory.Services.Service
{
    public class AuthService
    {
        private readonly DatabaseContext _dbContext;
        private readonly ITokenService _tokenService;

        // Constructor for dependency injection
        public AuthService(DatabaseContext dbContext, ITokenService tokenService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        /// <summary>
        /// Authenticates a user based on login credentials and generates a JWT token if successful.
        /// </summary>
        /// <param name="loginDto">The login credentials.</param>
        /// <returns>A JWT token if authentication is successful.</returns>
        public async Task<string> LoginAsync(UserLoginDto loginDto)
        {
            // Retrieve the user from the database based on the provided username
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .Include(g => g.Group)
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            // Validate the user credentials
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            // Check if the user's status is active
            if (user.Status != true)
            {
                throw new UnauthorizedAccessException("User account is not active.");
            }

            // Generate and return the JWT token
            return _tokenService.GenerateJwtToken(user);
        }

    }

    public interface ITokenService
    {
        string GenerateJwtToken(User user);
        ClaimsPrincipal DecodeJwtToken(string token);
        Guid GetUserIdFromTokenHeader(String? token);
    }

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        // Constructor for dependency injection
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Generates a JWT token for a given user.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <returns>A JWT token as a string.</returns>
        public string GenerateJwtToken(User user)
        {
            // Retrieve the JWT secret from configuration
            var secret = _configuration["JwtSettings:Secret"] ?? throw new ArgumentNullException("JwtSettings:Secret");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Ensure Role and Group are not null
            var roleName = user.Role?.Name ?? "DefaultRole";
            var groupName = user.Group?.Name ?? "DefaultGroup";


            // Create claims based on user information
            var claims = new List<Claim>
            {
                new Claim("username", user.Username),
                new Claim("userId", user.ID.ToString()),
                new Claim("roleName", roleName),
                new Claim("groupName", groupName)

            };

            // Retrieve the token expiry period from configuration
            var expiryInDays = int.Parse(_configuration["JwtSettings:ExpiryInDays"] ?? "1");

            // Create and return the JWT token
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(expiryInDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Decodes and validates a JWT token, returning the claims principal.
        /// </summary>
        /// <param name="token">The JWT token to decode.</param>
        /// <returns>The claims principal representing the token's claims.</returns>
        public ClaimsPrincipal DecodeJwtToken(string token)
        {
            // Retrieve the JWT secret from configuration
            var secret = _configuration["JwtSettings:Secret"] ?? throw new ArgumentNullException("JwtSettings:Secret");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            // Set up token validation parameters
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };

            try
            {
                // Validate the token and return the claims principal
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return principal;
            }
            catch (SecurityTokenExpiredException)
            {
                throw new SecurityTokenException("Token has expired");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                throw new SecurityTokenException("Invalid token signature");
            }
            catch (Exception)
            {
                throw new SecurityTokenException("Invalid token");
            }
        }

        public Guid GetUserIdFromTokenHeader(String? token)
        {
            // Decode the JWT token and extract claims
            var principal = DecodeJwtToken(token);
            var claims = principal.Claims.Select(c => new { c.Type, c.Value }).ToList();

            foreach (var claim in claims)
            {
                if (claim.Type == "userId")
                {
                    if (Guid.TryParse(claim.Value, out Guid parsedUserID))
                    {
                        return parsedUserID;
                    }
                    break;
                }
            }

            return Guid.Empty;
        }
    }

    public class UserStatusRequirement : IAuthorizationRequirement
    {
        // No additional properties needed for a boolean status check
    }

    public class UserStatusHandler : AuthorizationHandler<UserStatusRequirement>
    {
        private readonly DatabaseContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserStatusHandler> _logger;

        public UserStatusHandler(DatabaseContext dbContext, ITokenService tokenService, ILogger<UserStatusHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async System.Threading.Tasks.Task HandleRequirementAsync(AuthorizationHandlerContext context, UserStatusRequirement requirement)
        {
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "userId");

                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    var user = await _dbContext.Users.FindAsync(userId);

                    if (user != null && user.Status == true)
                    {
                        context.Succeed(requirement);
                        return;
                    }
                    else
                    {
                        _logger.LogWarning($"Authorization failed for user {userId}: User status is not true.");
                    }
                }
                else
                {
                    _logger.LogWarning($"Authorization failed: Invalid userId claim.");
                }
            }
            else
            {
                _logger.LogWarning("Authorization failed: User is not authenticated.");
            }

            context.Fail();
        }
    }



}