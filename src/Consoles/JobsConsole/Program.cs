using System;
using System.Threading.Tasks;

using Bau.Libraries.LibJob.Application;

namespace Bau.Applications.JobsManager
{
	/// <summary>
	///		Clase de inicio de la aplicación
	/// </summary>
	internal static class Program
	{
		internal async static Task Main(string[] args)
		{
			Configuration.ConfigurationModel configuration = new Configuration.ConfigurationModel();

				// Carga los datos de la configuración
				configuration.Parse(args);
				// Valida la configuración antes de continuar
				if (!configuration.Validate(out string error))
				{
					Console.WriteLine("Error when load configuration");
					Console.WriteLine(error);
				}
				else 
				{
					JobManager manager = new JobManager(new Logger(Logger.LogItemType.Debug), new ConsoleOutput());

						// Inicializa el manager
						if (manager.Initialize(configuration.ProcessorsDefinitionFileName))
							await manager.ProcessAsync(configuration.ScriptFileName);
						else
							Console.WriteLine("Error while initialize job manager");
				}
				// Espera a que el usuario pulse una tecla
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine();
				Console.WriteLine("Press any key...");
				Console.ReadKey();
		}
	}
}
