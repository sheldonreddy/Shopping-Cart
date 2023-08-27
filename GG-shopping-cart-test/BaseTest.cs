using System;
using Microsoft.Extensions.Configuration;

namespace GG_shopping_cart_test
{
	public class BaseTest
	{
        protected IConfiguration configuration { get; }

		public BaseTest()
		{
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: true)
                .Build();
    }
	}
}

