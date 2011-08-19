using Topshelf;

namespace BuildMonkey
{
	static class Program
	{
		static void Main(string[] args)
		{
			var host = HostFactory.New(hostConfigurator =>
			{
				hostConfigurator.Service<Service>(s =>
				{
					s.ConstructUsing(name => new Service());
					s.WhenStarted(tc => tc.Start());
					s.WhenStopped(tc => tc.Stop());
				});

				hostConfigurator.RunAsLocalSystem();

				hostConfigurator.SetDescription("Build Monkey like bananas");
				hostConfigurator.SetDisplayName("Build Monkey");
				hostConfigurator.SetServiceName("BuildMonkey");
			});

			host.Run();
		}
	}
}