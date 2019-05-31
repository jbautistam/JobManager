using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;

namespace Bau.Libraries.LibJob.Application.Processor.Controllers
{
	/// <summary>
	///		Manager para plugins MEF
	/// </summary>
	internal class PluginsManager<TypePlugin> : IDisposable
	{
		/// <summary>
		///		Inicializa los datos
		/// </summary>
		public void Initialize(List<string> pathPlugins, string extensionPlugins = ".plugin.dll")
		{
			// Crea el contenedor
			Container = new ContainerConfiguration().WithAssemblies(GetPluginsAssemblies(pathPlugins, NormalizeExtension(extensionPlugins)), 
																	GetConventionsBuilder()).CreateContainer();
			// Obtiene el manager de plugins
			Plugins = Container.GetExports<TypePlugin>().ToList();
		}

		/// <summary>
		///		Normaliza una extensión para que siempre tenga un punto por delante
		/// </summary>
		private string NormalizeExtension(string extension)
		{
			// Crea una extensión si no existía y quita los espacios
			if (string.IsNullOrWhiteSpace(extension))
				extension = ".plugin.dll";
			extension = extension.Trim();
			// Añade el punto inicial
			if (!extension.StartsWith("."))
				extension = "." + extension;
			// Devuelve la extensión normalizada
			return extension;
		}

		/// <summary>
		///		Obtiene los plugins de los ensamblados
		/// </summary>
		private IEnumerable<Assembly> GetPluginsAssemblies(List<string> pathPlugins, string extensionPlugins)
		{
			List<Assembly> assemblies = new List<Assembly>();

				// Añade los directorios de plugins
				foreach (string path in pathPlugins)
					if (System.IO.Directory.Exists(path))
						foreach (string fileName in System.IO.Directory.GetFiles(path, "*" + extensionPlugins))
							if (System.IO.File.Exists(fileName) && fileName.EndsWith(extensionPlugins, StringComparison.CurrentCultureIgnoreCase))
								try
								{
									assemblies.Add(Assembly.LoadFrom(fileName));
								}
								catch (Exception exception)
								{
									Errors.Add($"Error al cargar la librería del plugin {fileName}{Environment.NewLine}{exception.Message}");
								}
				// Devuelve la lista de ensamblados
				return assemblies;
		}

		/// <summary>
		///		Obtiene las convenciones
		/// </summary>
		private System.Composition.Convention.ConventionBuilder GetConventionsBuilder()
		{
			System.Composition.Convention.ConventionBuilder builder = new System.Composition.Convention.ConventionBuilder();

				// Inicializa las convenciones
				builder.ForTypesDerivedFrom<TypePlugin>().Export<TypePlugin>().Shared();
				// Devuelve las convenciones
				return builder;
		}

		/// <summary>
		///		Libera el objeto
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (!Disposed)
			{
				// Libera la memoria
				if (disposing)
					Container = null;
				// Indica que se ha liberado
				Disposed = true;
			}
		}

		/// <summary>
		///		Libera el objeto
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		///		Manager de plugins
		/// </summary>
		[ImportMany]
		public IEnumerable<TypePlugin> Plugins { get; private set; }

		/// <summary>
		///		Contenedor de MEF
		/// </summary>
		private CompositionContext Container { get; set; }

		/// <summary>
		///		Errores
		/// </summary>
		public List<string> Errors { get; } = new List<string>();

		/// <summary>
		///		Indica si se ha liberado el objeto
		/// </summary>
		public bool Disposed { get; private set; }
	}
}
