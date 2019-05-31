using System;

using Bau.Libraries.LibJob.Application.Models.Configuration;
using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Libraries.LibJob.Application.Repository
{
	/// <summary>
	///		Repositorio para la carga de archivos de configuración
	/// </summary>
	internal class JobManagerRepository
	{
		// Constantes privadas
		private const string TagRoot = "JobManager";
		private const string TagProcessor = "Processor";

		/// <summary>
		///		Carga la configuración
		/// </summary>
		internal ConfigurationModel LoadConfiguration(string fileName)
		{
			ConfigurationModel configuration = new ConfigurationModel();
			MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);

				// Carga los datos del archivo
				if (fileML != null)
					foreach (MLNode rootML in fileML.Nodes)
						if (rootML.Name == TagRoot)
							foreach (MLNode nodeML in rootML.Nodes)
								switch (nodeML.Name)
								{
									case TagProcessor:
											if (!string.IsNullOrEmpty(nodeML.Value))
												configuration.PathPlugins.Add(nodeML.Value.Trim());
										break;
								}
				// Devuelve la configuración
				return configuration;
		}
	}
}
