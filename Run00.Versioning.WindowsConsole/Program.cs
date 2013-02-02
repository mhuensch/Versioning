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
			var assemblyPath = args[0]; //@"C:\TeamCity\buildAgent\work\498206cac5de9896\Run00.Configuration\bin\Release\Run00.Configuration.dll";
			var previousPath = @"C:\TeamCity\Run00.Versioning\store\" + Path.GetFileName(assemblyPath);
			var workingFolder = Directory.GetParent(Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(assemblyPath)).FullName).FullName).FullName;
			var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			Console.WriteLine("assemblyPath" + assemblyPath);
			Console.WriteLine("previousPath" + previousPath);
			Console.WriteLine("workingFolder" + workingFolder);
			Console.WriteLine("workingDirectory" + workingDirectory);

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
				if (File.Exists(previousPath) == false)
					previousPath = string.Empty;

				var version = assemblyVersioning.Calculate(assemblyPath, previousPath);
				Console.WriteLine("Calculated Version:" + version);

				foreach (var file in Directory.GetFiles(workingFolder, "AssemblyInfo.cs", SearchOption.AllDirectories))
				{
					var contents = string.Empty;
					using (var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite))
					{
						contents = assemblyVersioning.UpdateAssemblyInfo(stream, version);
					}
					File.WriteAllText(file, contents);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
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
