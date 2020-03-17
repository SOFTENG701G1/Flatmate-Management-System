using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiBackendTests
{
    class ServiceDependencyResolver
    {
        private readonly IWebHost _webHost;
        public ServiceDependencyResolver(IWebHost WebHost)
        {
            _webHost = WebHost;
        }

        /// <summary>
        /// Returns a service from the web host's service provider
        /// </summary>
        /// <typeparam name="T">The type of the service to be returned from the service providers</typeparam>
        /// <returns>A service</returns>
        public T GetService<T>()
        {
            using (var serviceScope = _webHost.Services.CreateScope())
            {
                return serviceScope.ServiceProvider.GetRequiredService<T>();
            };
        }
    }
}
