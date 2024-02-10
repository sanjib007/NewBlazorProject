﻿using L3T.OAuth2DotNet7.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace L3T.OAuth2DotNet7.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
	public class ClientController : ControllerBase
	{
		private readonly IAccountService _accountService;
        public ClientController(
            IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("ClientGetAllUsersForSearch")]
		public async Task<IActionResult> ClientGetAllUsersForSearch(string searchText)
		{
			return Ok(await _accountService.GetAllUsersForSearchAsync(searchText));
		}

    }
}
