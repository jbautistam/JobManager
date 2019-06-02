using System;
using System.Threading.Tasks;

using Bau.Libraries.LibDataStructures.Collections;
using Bau.Libraries.LibJob.Core.Interfaces;
using Bau.Libraries.LibJob.Core.Models;
using Bau.Libraries.LibJob.Core.Models.Context;
using Bau.Libraries.LibJob.Core.Models.Processes;

namespace Bau.Libraries.LibJob.Application
{
	/// <summary>
	///		Manager de trabajos
	/// </summary>
	public class JobManager
	{
		public JobManager(ILogger logger, IConsoleOutput consoleOutput)
		{
			Context = new JobContextModel(logger, consoleOutput);
		}

		/// <summary>
		///		Inicializa el manager de trabajo con los plugins
		/// </summary>
		public bool Initialize(System.Collections.Generic.List<string> pathPlugins)
		{
			bool initialized = false;

				// Carga los plugins
				if (!ValidateData(pathPlugins, out string error))
					Context.Logger.WriteError("JobManager - Initialize plugins", error);
				else
				{
					Processor.Controllers.PluginsManager<IJobStepProcessor> pluginsManager = new Processor.Controllers.PluginsManager<IJobStepProcessor>();

						// Inicializa el manejador de plugins
						pluginsManager.Initialize(pathPlugins, ".dll");
						// Controla los errores
						if (pluginsManager.Errors.Count == 0)
						{
							// Añade los procesadores
							foreach (IJobStepProcessor processor in pluginsManager.Plugins)
								Processors.Add(processor.Key, processor);
							// Indica que se ha inicializado correctamente
							initialized = true;
						}
						else
							foreach (string pluginError in pluginsManager.Errors)
								Context.Logger.WriteError("JobManager - Initialize plugins", pluginError);
				}
				// Devuelve el valor que indica si se ha inicializado
				return initialized;
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		private bool ValidateData(System.Collections.Generic.List<string> pathPlugins, out string error)
		{
			// Inicializa los argumentos de salida
			error = string.Empty;
			// Comprueba los datos
			if (pathPlugins == null || pathPlugins.Count == 0)
				error = "Don't exists plugin defined in configuration file";
			else
				foreach (string path in pathPlugins)
					if (string.IsNullOrWhiteSpace(path))
						error += "There is a path empty in the plugins definition.";
					else if (!System.IO.Directory.Exists(path))
						error += $"Can't find the directory '{path}'.";
			// Devuelve el valor que indica si ha habido algún error
			return string.IsNullOrWhiteSpace(error);
		}

		/// <summary>
		///		Procesa un trabajo
		/// </summary>
		public async Task ProcessAsync(string fileName)
		{
			JobModel job = new Repository.JobRepository().Load(fileName);

				// Log
				Context.WriteDebug($"JobManager - Load {fileName}", job.Debug());
				// Procesa el archivo cargado
				await ProcessAsync(job);
		}

		/// <summary>
		///		Procesa un trabajo
		/// </summary>
		public async Task ProcessAsync(JobModel job)
		{
			foreach (ProcessBaseModel baseStep in job.Steps)
				if (Context.Errors.Count == 0 || baseStep.StartWithPreviousError)
					switch (baseStep)
					{
						case ProcessBlockModel block:
								await ProcessBlockAsync(block);
							break;
						case ProcessStepModel step:
								await ProcessStepAsync(step);
							break;
					}
				else
					Context.WriteError("JobManager - Process", $"Don't process the step {baseStep.Name} because exist a previous error");
		}

		/// <summary>
		///		Procesa un bloque de pasos
		/// </summary>
		private async Task ProcessBlockAsync(ProcessBlockModel block)
		{
			System.Collections.Generic.List<Task> stepTasks = new System.Collections.Generic.List<Task>();

				// Añade los procesos de ejecución
				foreach (ProcessStepModel step in block.Steps)
					stepTasks.Add(ProcessStepAsync(step));
				// Espera a que se ejecuten todas las tareas
				await Task.WhenAll(stepTasks);
		}

		/// <summary>
		///		Procesa un paso
		/// </summary>
		private async Task ProcessStepAsync(ProcessStepModel step)
		{
			IJobStepProcessor processor = Processors[step.PluginKey];

				// Procesa el trabajo
				if (processor == null)
					Context.WriteError("JobManager - Process", $"Can't find the processor for step {step.PluginKey}");
				else
				{
					// Ejecuta el trabajo
					try
					{
						// Inicializa el procesador
						processor.Initialize(Context, step);
						// Procesa
						await processor.ProcessAsync();
					}
					catch (Exception exception)
					{
						Context.WriteError("JobManager - Process", exception.Message);
					}
					// Muestra los benchmarks
					ShowBenchmarks(Context.Benchmarks);
				}
		}

		/// <summary>
		///		Muestra la información de tiempos
		/// </summary>
		private void ShowBenchmarks(System.Collections.Generic.List<JobBenchmarkModel> benchmarks)
		{
			string lastJob = string.Empty, lastStep = string.Empty;

				// Recorre la información
				foreach (JobBenchmarkModel benchmark in benchmarks)
				{
					// Información del trabajo
					if (!lastJob.Equals(benchmark.Job, StringComparison.CurrentCultureIgnoreCase))
					{
						Context.WriteInfo(benchmark.Job);
						lastJob = benchmark.Job;
					}
					// Información del paso
					if (!lastStep.Equals(benchmark.Step, StringComparison.CurrentCultureIgnoreCase))
					{
						Context.WriteInfo(new string(' ', 5) + benchmark.Step);
						lastStep = benchmark.Step;
					}
					// Información de medida
					Context.WriteInfo(new string(' ', 10) + $"{benchmark.Title}\t{benchmark.Elapsed:HH:mm:ss}\t{benchmark.Records:#,##0}");
				}
		}

		/// <summary>
		///		Procesadores
		/// </summary>
		internal NormalizedDictionary<IJobStepProcessor> Processors { get; } = new NormalizedDictionary<IJobStepProcessor>();

		/// <summary>
		///		Contexto
		/// </summary>
		private JobContextModel Context { get; }
	}
}
