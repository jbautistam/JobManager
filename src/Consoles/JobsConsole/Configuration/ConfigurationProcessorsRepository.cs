using System;
using System.Collections.Generic;

using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Applications.JobsManager.Configuration
{
	/// <summary>
	///		Repositorio para la carga de archivos de configuración
	/// </summary>
	internal class ConfigurationProcessorsRepository
	{
		// Constantes privadas
		private const string TagRoot = "JobManager";
		private const string TagProcessor = "Processor";

		/// <summary>
		///		Carga la configuración
		/// </summary>
		internal List<string> LoadPathPlugins(string fileName)
		{
			List<string> pathPlugins = new List<string>();
			MLFile fileML = new Libraries.LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);

				// Carga los directorios de plugins
				if (fileML != null)
					foreach (MLNode rootML in fileML.Nodes)
						if (rootML.Name == TagRoot)
							foreach (MLNode nodeML in rootML.Nodes)
								switch (nodeML.Name)
								{
									case TagProcessor:
											if (!string.IsNullOrEmpty(nodeML.Value))
												pathPlugins.Add(nodeML.Value.Trim());
										break;
								}
				// Devuelve los directorios de plugins
				return pathPlugins;
		}
	}
}
