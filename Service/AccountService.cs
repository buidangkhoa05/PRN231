using BusinessObject;
using BusinessObject.Common;
using BusinessObject.Dto;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Repository;
using Repository.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Service
{
    public class AccountService : BaseService, IAccountService
    {
        protected readonly IConfiguration _config;
        public AccountService(IUnitOfWork unitOfWork, IConfiguration config) : base(unitOfWork)
        {
            _config = config;
        }

        public async Task<ApiResponse<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _uOW.Resolve<Account>().FindAsync(a => a.Username == request.Username && a.Password == request.Password);

            if (user == null)
            {
                return Failed<LoginResponse>("Username or password is incorrect", HttpStatusCode.BadRequest);
            }

            var token = GenerateJSONWebToken(user);

            return Success(new LoginResponse
            {
                AccessToken = token,
                Fullname = user.Fullname,
                Role = user.Role
            });
        }

        private string GenerateJSONWebToken(Account userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            IEnumerable<Claim> claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim(ClaimTypes.Name, userInfo.Username),
                new Claim("fullname", userInfo.Fullname),
                new Claim(ClaimTypes.Role, userInfo.Role),
                new Claim("accountID", userInfo.AccountId.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddDays(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IEnumerable<Claim>> GetAuthenticatedAccount(string accessToken)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var tokenHandler = new JwtSecurityTokenHandler();

            tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            await Task.CompletedTask;

            return jwtToken.Claims;
        }

        public async Task<PagingApiResponse<Account>> Search(SearchBaseReq req)
        {
            try
            {
                var result = await _uOW.Resolve<Account, IAccountRepository>()
               .SearchAsync(req.KeySearch, req.PagingQuery, req.OrderBy);

                return Success(result);
            }
            catch (Exception ex)
            {
                return PagingFailed<Account>(ex.Message);
            }
        }

        public async Task<ApiResponse<Account>> GetAccount(int accountID)
        {
            try
            {
                var account = await _uOW.Resolve<Account>().FindAsync(accountID);

                if (account == null)
                    return Failed<Account>("Account not found", HttpStatusCode.NotFound);

                return Success(account);
            }
            catch (Exception ex)
            {
                return Failed<Account>(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAccount(int accountID, AccountReq req)
        {
            try
            {
                var account = await _uOW.Resolve<Account, IAccountRepository>().FindAsync(accountID);

                if (account == null)
                    return Failed<bool>("Account not found", HttpStatusCode.NotFound);

                req.Adapt(account);

                await _uOW.Resolve<Account>().UpdateAsync(account);
                await _uOW.SaveChangesAsync();

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> CreateAccount(AccountReq req)
        {
            try
            {
                var account = req.Adapt<Account>();

                await _uOW.Resolve<Account>().CreateAsync(account);
                await _uOW.SaveChangesAsync();

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAccount(int accountID)
        {
            try
            {
                var account = await _uOW.Resolve<Account, IAccountRepository>().FindAsync(accountID);

                if (account == null)
                    return Failed<bool>("Account not found", HttpStatusCode.NotFound);

                await _uOW.Resolve<Account>().DeleteAsync(account);
                await _uOW.SaveChangesAsync();

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.Message);
            }
        }
    }
}
