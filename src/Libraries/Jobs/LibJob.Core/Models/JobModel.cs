using System;

using Bau.Libraries.LibDataStructures.Collections;
using Bau.Libraries.LibJob.Core.Models.Processes;

namespace Bau.Libraries.LibJob.Core.Models
{
	/// <summary>
	///		Modelo con los datos necesarios para ejecutar un trabajo
	/// </summary>
	public class JobModel : LibDataStructures.Base.BaseExtendedModel
	{
		/// <summary>
		///		Obtiene una cadena de depuración con el contenido
		/// </summary>
		public string Debug()
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder();

				// Añade los datos a la cadena de depuración
				builder.AppendLine($"{nameof(Name)}: {Name}");
				builder.AppendLine($"{nameof(Description)}: {Description}");
				// Añade los pasos
				foreach (ProcessStepModel step in Steps)
					builder.Append(step.Debug(0));
				// Devuelve la cadena de depuración
				return builder.ToString();
		}

		/// <summary>
		///		Pasos
		/// </summary>
		public ProcessModelCollection Steps { get; } = new ProcessModelCollection();

		/// <summary>
		///		Parámetros de ejecución del trabajo
		/// </summary>
		public NormalizedDictionary<object> Parameters { get; } = new NormalizedDictionary<object>();
	}
}
