using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Run00.Versioning.Compare
{
	public sealed class Installer : IWindsorInstaller
	{
		void IWindsorInstaller.Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Classes.FromAssembly(this.GetType().Assembly)
				.BasedOn(typeof(ISymbolComparisonFactory<>))
				.WithService.FromInterface());

			container.Register(Component.For<SymbolChangeCalculator>());
			container.Register(Component.For<SolutionComparison>());
		}
	}
}
