using BusinessObject.Common;
using BusinessObject.Common.Enums;
using BusinessObject.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _accountService.Login(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Search([FromQuery] SearchBaseReq req)
        {
            var response = await _accountService.Search(req);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("{accountID}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Search(int accountID)
        {
            var response = await _accountService.GetAccount(accountID);
            return StatusCode((int)response.StatusCode, response);
        }


        [HttpPut("{accountID}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int accountID, [FromBody] AccountReq req)
        {
            var response = await _accountService.UpdateAccount(accountID, req);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] AccountReq req)
        {
            var response = await _accountService.CreateAccount(req);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("{accountID}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int accountID)
        {
            var response = await _accountService.DeleteAccount(accountID);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
