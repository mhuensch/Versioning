using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Run00.Versioning.Sequence
{
	public sealed class Installer : IWindsorInstaller
	{
		void IWindsorInstaller.Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<IAssemblyVersioning>().ImplementedBy<AssemblyVersioning>());
		}
	}
}
