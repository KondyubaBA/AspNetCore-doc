public static class Host
{
	public static IHostBuilder CreateDefaultBuilder()
	{
		return CreateDefaultBuilder(null);
	}

	public static IHostBuilder CreateDefaultBuilder(string[]? args)
	{
		return new HostBuilder().ConfigureDefaults(args);
	}

	public static HostApplicationBuilder CreateApplicationBuilder()
	{
		return new HostApplicationBuilder();
	}

	public static HostApplicationBuilder CreateApplicationBuilder(string[]? args)
	{
		return new HostApplicationBuilder(args);
	}

	public static HostApplicationBuilder CreateApplicationBuilder(HostApplicationBuilderSettings? settings)
	{
		return new HostApplicationBuilder(settings);
	}

	public static HostApplicationBuilder CreateEmptyApplicationBuilder(HostApplicationBuilderSettings? settings)
	{
		return new HostApplicationBuilder(settings, empty: true);
	}
}
