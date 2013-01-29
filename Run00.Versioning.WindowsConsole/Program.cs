using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System;
using System.IO;
using System.Reflection;

namespace Run00.Versioning.WindowsConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var assembly = Assembly.ReflectionOnlyLoad(args[0]);

			var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			_container = new WindsorContainer();
			_container.Kernel.Resolver.AddSubResolver(new ArrayResolver(_container.Kernel, true));
			_container.Install(
				FromAssembly.This(),
				FromAssembly.InDirectory(new AssemblyFilter(workingDirectory, "*.dll"))
			);

			var assemblyVersioning = default(IAssemblyVersioning);
			try
			{
				assemblyVersioning = _container.Resolve<IAssemblyVersioning>();
				var version = assemblyVersioning.Calculate(args[0], args[1]);
				foreach (var file in Directory.GetFiles(args[3], "AssemblyInfo.cs", SearchOption.AllDirectories))
				{
					var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
					assemblyVersioning.UpdateAssemblyInfo(stream, version);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			finally
			{
				if (_container != null)
					_container.Release(assemblyVersioning);
			}
		}

		private static IWindsorContainer _container;
	}
}
