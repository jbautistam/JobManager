using System;

namespace Bau.Applications.JobsManager.Configuration
{
	/// <summary>
	///		Modelo con los datos de configuración
	/// </summary>
	internal class ConfigurationModel
	{
		/// <summary>
		///		Interpreta los argumentos de entrada
		/// </summary>
		internal void Parse(string[] args)
		{
			// Obtiene el nombre del archivo que contiene los directorios de los diferentes procesadores
			ProcessorsDefinitionFileName = System.IO.Path.Combine(PathBaseApplication, "Data\\ProcessorPaths.xml");
			// Obtiene el nombre del script que se debe ejecutar
			if (args.Length > 0)
				ScriptFileName = args[0];
		}

		/// <summary>
		///		Valida la configuración
		/// </summary>
		internal bool Validate(out string error)
		{
			// Inicializa los argumentos de salida
			error = string.Empty;
			// Comprueba los datos
			if (string.IsNullOrWhiteSpace(ProcessorsDefinitionFileName))
				error = "Processor path file undefined";
			else if (!System.IO.File.Exists(ProcessorsDefinitionFileName))
				error = $"Cant find the processors path file: {ProcessorsDefinitionFileName}";
			else if (string.IsNullOrWhiteSpace(ScriptFileName))
				error = "Script file name undefined";
			else if (!System.IO.File.Exists(ScriptFileName))
				error = $"Cant find the script {ScriptFileName}";
			// Devuelve el valor que indica si los datos son correctos
			return string.IsNullOrWhiteSpace(error);
		}

		/// <summary>
		///		Directorio base de la aplicación
		/// </summary>
		internal string PathBaseApplication
		{
			get
			{
				return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			}
		}

		/// <summary>
		///		Nombre del archivo que define los directorios donde se encuentran los diferentes procesadores
		/// </summary>
		internal string ProcessorsDefinitionFileName { get; private set; }

		/// <summary>
		///		Nombre del archivo con el script que se va a ejecutar
		/// </summary>
		internal string ScriptFileName { get; private set; }
	}
}
