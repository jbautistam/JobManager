using System;

using Bau.Libraries.LibDataStructures.Collections;

namespace Bau.Libraries.LibJob.Application.Models.Processes
{
	/// <summary>
	///		Paso de un trabajo
	/// </summary>
	public class ProcessStepModel : ProcessBaseModel
	{
		public ProcessStepModel(JobModel job) : base(job) {}

		/// <summary>
		///		Depuración de los datos de paso
		/// </summary>
		protected override void CreateDebug(int indent, System.Text.StringBuilder builder)
		{
			string indentString = GetIndentString(indent);

				// Añade los datos
				builder.AppendLine($"{indentString} Step {nameof(Name)}: {Name} - {nameof(Key)}: {Key}");
				builder.AppendLine($"{indentString}    {nameof(Description)}: {Description}");
				builder.AppendLine($"{indentString}    {nameof(PluginKey)}: {PluginKey}");
				builder.AppendLine($"{indentString}    {nameof(StartWithPreviousError)}: {StartWithPreviousError}");
				builder.AppendLine($"{indentString}    {nameof(ScriptFileName)}: {ScriptFileName}");
				// Añade la depuración del árbol
				Debug(builder, indentString + new string(' ', 5), Parameters);
		}

		/// <summary>
		///		Depuración de los parámetros del árbol
		/// </summary>
		private void Debug(System.Text.StringBuilder builder, string indent, NormalizedDictionary<object> parameters)
		{
			// Añade los atributos
			foreach ((string key, object value) in parameters.Enumerate())
				builder.AppendLine($"{indent}     - Parameter: {key}: {value ?? "NULL"}");
		}

		/// <summary>
		///		Obtiene los parámetros combinados del paso y el trabajo
		/// </summary>
		public NormalizedDictionary<object> GetCombinedParameters()
		{
			NormalizedDictionary<object> parameters = new NormalizedDictionary<object>();

				// Asigna los parámetros del trabajo
				foreach ((string key, object value) in Job.Parameters.Enumerate())
					parameters.Add(key, value);
				// Asigna los parámetros del paso
				foreach ((string key, object value) in Parameters.Enumerate())
					parameters.Add(key, value);
				// Devuelve el diccionario de parámetros
				return parameters;
		}

		/// <summary>
		///		Clave del paso
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		///		Clave del plugin que procesa el paso
		/// </summary>
		public string PluginKey { get; set; }

		/// <summary>
		///		Parámetros del paso
		/// </summary>
		public NormalizedDictionary<object> Parameters { get; } = new NormalizedDictionary<object>();
	}
}
