namespace GG_shopping_cart.Helpers
{
	public static class ConfigurationHelpers
	{
        public static string GetConfigurationValue(IConfiguration configuration, string key, string defaultValue)
        {
            return configuration[key] ?? defaultValue;
        }
    }
}

