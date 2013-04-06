using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Run00.Versioning.Link
{
	public sealed class Installer : IWindsorInstaller
	{
		void IWindsorInstaller.Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<SolutionChangeCalculator>());
			container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
		}
	}
}
