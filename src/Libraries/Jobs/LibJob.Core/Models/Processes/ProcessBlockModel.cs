using System;

namespace Bau.Libraries.LibJob.Core.Models.Processes
{
	/// <summary>
	///		Clase con los datos de un bloque de procesos: los procesos de un bloque se ejecutan en paralelo
	/// </summary>
	public class ProcessBlockModel : ProcessBaseModel
	{
		public ProcessBlockModel(JobModel job) : base(job) {}

		/// <summary>
		///		Depuración de los datos de paso
		/// </summary>
		protected override void CreateDebug(int indent, System.Text.StringBuilder builder)
		{
			string indentString = GetIndentString(indent);

				// Añade los datos
				builder.AppendLine($"{indentString} Block {nameof(Name)}: {Name}");
				builder.AppendLine($"{indentString}    {nameof(StartWithPreviousError)}: {StartWithPreviousError}");
				builder.AppendLine($"{indentString}    Steps");
				// Añade la depuración del árbol
				foreach (ProcessBaseModel step in Steps)
					builder.AppendLine(step.Debug(indent + 1));
		}

		/// <summary>
		///		Pasos del bloque
		/// </summary>
		public ProcessModelCollection Steps { get; } = new ProcessModelCollection();
	}
}
