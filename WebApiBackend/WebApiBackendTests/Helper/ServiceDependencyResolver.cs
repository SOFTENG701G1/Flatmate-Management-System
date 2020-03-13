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

        public T GetService<T>()
        {
            using (var serviceScope = _webHost.Services.CreateScope())
            {
                return serviceScope.ServiceProvider.GetRequiredService<T>();
            };
        }
    }
}
