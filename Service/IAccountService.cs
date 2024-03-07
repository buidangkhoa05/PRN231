using BusinessObject;
using BusinessObject.Common;
using BusinessObject.Dto;
using System.Security.Claims;

namespace Service
{
    public interface IAccountService
    {
        Task<ApiResponse<Account>> GetAccount(int accountID);
        Task<IEnumerable<Claim>> GetAuthenticatedAccount(string accessToken);
        Task<ApiResponse<LoginResponse>> Login(LoginRequest request);
        Task<PagingApiResponse<Account>> Search(SearchBaseReq req);
        Task<ApiResponse<bool>> UpdateAccount(int accountID, AccountReq req);
        Task<ApiResponse<bool>> CreateAccount(AccountReq req);
        Task<ApiResponse<bool>> DeleteAccount(int accountID);
    }
}
