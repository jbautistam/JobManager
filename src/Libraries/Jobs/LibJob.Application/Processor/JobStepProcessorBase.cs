using System;
using System.Threading.Tasks;

using Bau.Libraries.LibJob.Application.Models.Processes;

namespace Bau.Libraries.LibJob.Application.Processor
{
	/// <summary>
	///		Base para los procesadores de trabajos
	/// </summary>
	public abstract class JobStepProcessorBase : Interfaces.IJobStepProcessor
	{
		public JobStepProcessorBase(string key)
		{
			Key = key;
		}

		/// <summary>
		///		Inicializa el procesador
		/// </summary>
		public void Initialize(Models.Context.JobContextModel context, ProcessStepModel step)
		{
			Context = context;
			Step = step;
		}

		/// <summary>
		///		Procesa el paso
		/// </summary>
		public async Task<bool> ProcessAsync()
		{
			// Asigna el trabajo y el paso actual
			Context.UpdateJobStep(Key, Step);
			// Log
			Context.WriteDebug("Start process");
			// Ejecuta el paso
			await ExecuteStepAsync();
			// Log
			Context.WriteDebug("End process");
			// Devuelve el valor que indica si es correcto
			return Context.Errors.Count == 0;
		}

		/// <summary>
		///		Procesa el paso
		/// </summary>
		protected abstract Task ExecuteStepAsync();

		/// <summary>
		///		Clave del procesador
		/// </summary>
		public string Key { get; }

		/// <summary>
		///		Datos del proceso
		/// </summary>
		public ProcessStepModel Step { get; private set; }

		/// <summary>
		///		Contexto de ejecución
		/// </summary>
		protected Models.Context.JobContextModel Context { get; private set; }
	}
}
