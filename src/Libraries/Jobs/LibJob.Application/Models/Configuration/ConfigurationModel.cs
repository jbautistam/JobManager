using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibJob.Application.Models.Configuration
{
	/// <summary>
	///		Datos de configuración
	/// </summary>
	public class ConfigurationModel
	{
		/// <summary>
		///		Directorios de plugins
		/// </summary>
		public List<string> PathPlugins { get; } = new List<string>();
	}
}
