using System;
using System.Threading.Tasks;

using Bau.Libraries.LibJob.Core.Models.Processes;

namespace Bau.Libraries.LibJob.Core.Interfaces
{
	/// <summary>
	///		Interface para los procesadores de un paso de un trabajo
	/// </summary>
	public interface IJobStepProcessor
	{
		/// <summary>
		///		Inicializa el proceso
		/// </summary>
		void Initialize(Models.Context.JobContextModel context, ProcessStepModel step);

		/// <summary>
		///		Procesa el paso
		/// </summary>
		Task<bool> ProcessAsync();

		/// <summary>
		///		Clave del procesador
		/// </summary>
		string Key { get; }
	}
}
