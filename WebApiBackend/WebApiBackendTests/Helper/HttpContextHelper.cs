using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace WebApiBackendTests.Helper
{
    class HttpContextHelper
    {
        private readonly DefaultHttpContext _httpContext;
        private readonly ClaimsIdentity _objClaim;

        public HttpContextHelper()
        {
            //Creates a new httpContext and adds a user identity to it, imitating being already logged in.
            _httpContext = new DefaultHttpContext();
            GenericIdentity MyIdentity = new GenericIdentity("User");
            _objClaim = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "1") });
        }

        public DefaultHttpContext GetHttpContext()
        {
            return _httpContext;
        }

        public ClaimsIdentity GetClaimsIdentity()
        {
            return _objClaim;
        }
    }
}
