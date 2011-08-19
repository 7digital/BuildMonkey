namespace BuildMonkey.TeamCity
{
	public interface IBuildTypeRunner
	{
		void Run(BuildType buildType);
	}
}