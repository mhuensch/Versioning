﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Run00.Versioning.Link
{
	public sealed class Installer : IWindsorInstaller
	{
		void IWindsorInstaller.Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Classes.FromAssembly(this.GetType().Assembly)
				.BasedOn(typeof(ISymbolLinkFactory<>))
				.WithService.FromInterface());

			container.Register(Component.For<SolutionLinker>());
		}
	}
}
