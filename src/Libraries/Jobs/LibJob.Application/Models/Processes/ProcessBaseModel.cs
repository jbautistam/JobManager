using System;

namespace Bau.Libraries.LibJob.Application.Models.Processes
{
	/// <summary>
	///		Base para los procesos de un trabajo
	/// </summary>
	public abstract class ProcessBaseModel : LibDataStructures.Base.BaseExtendedModel
	{
		public ProcessBaseModel(JobModel job)
		{
			Job = job;
		}

		/// <summary>
		///		Depuración de los datos de paso
		/// </summary>
		public string Debug(int indent)
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder();

				// Añade los datos
				CreateDebug(indent, builder);
				// Devuelve la cadena de depuración
				return builder.ToString();
		}

		/// <summary>
		///		Obtiene la cadena de indentación
		/// </summary>
		protected string GetIndentString(int indent)
		{
			if (indent > 0)
				return new string(' ', 5 * indent);
			else
				return string.Empty;
		}

		/// <summary>
		///		Obtiene una cadena de depuración
		/// </summary>
		protected abstract void CreateDebug(int indent, System.Text.StringBuilder builder);

		/// <summary>
		///		Trabajo al que pertenece el proceso
		/// </summary>
		public JobModel Job { get; }

		/// <summary>
		///		Indica si se debe iniciar el paso si el anterior ha devuelto algún error
		/// </summary>
		public bool StartWithPreviousError { get; set; }

		/// <summary>
		///		Nombre del archivo del script
		/// </summary>
		public string ScriptFileName { get; set; }
	}
}
