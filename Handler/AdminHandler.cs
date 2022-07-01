﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using STJWebAppAPI.Data;

namespace STJWebAppAPI.Handler
{
    public class AdminHandler: AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IDbRepo _repository;

        public AdminHandler(
            IDbRepo repository,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            ) : base(options, logger, encoder, clock)
        {
            _repository = repository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                Response.Headers.Add("WWW-Authenticate", "Basic");
                return AuthenticateResult.Fail("Authorization header not found");
            }
            else
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(":");
                var username = credentials[0];
                var password = credentials[1];
                if (_repository.ValidLogin(username, password))
                {
                    if (_repository.GetUserByEmail(username).IsAdmin == true)
                    {
                        var claims = new[] { new Claim("admin", username) };
                        ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");
                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                        AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);
                        return AuthenticateResult.Success(ticket);
                    }
                    else
                    {
                        return AuthenticateResult.Fail("user is not admin");
                    }
                }
                else
                {
                    return AuthenticateResult.Fail("userName and password do not match");
                }
            }
        }
    }
}
