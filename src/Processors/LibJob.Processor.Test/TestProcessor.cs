using System;
using System.Threading.Tasks;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibJob.Processor.Test
{
	/// <summary>
	///		Procesador de prueba
	/// </summary>
	public class TestProcessor : Core.Processors.JobStepProcessorBase
	{
		public TestProcessor() : base("TestProcessor") {}

		/// <summary>
		///		Procesa los comandos
		/// </summary>
		protected async override Task ExecuteStepAsync()
		{
			// Carga el archivo de script
			if (!System.IO.File.Exists(Step.ScriptFileName))
				Context.WriteError($"Can't find the file {Step.ScriptFileName}");
			else
			{
				string text = System.IO.File.ReadAllText(Step.ScriptFileName);

					// Reemplaza los parámetros
					text = ReplaceParameters(text);
					// Escribe la información
					Context.WriteInfo(text);
			}
			// Espera un momento (por el async)
			await Task.Delay(100);
		}

		/// <summary>
		///		Carga los mensajes del archivo de script
		/// </summary>
		private string ReplaceParameters(string text)
		{
			// Reemplaza la cadena de parámetros
			foreach ((string key, object value) in Step.GetCombinedParameters().Enumerate())
				if (value == null)
					text = text.ReplaceWithStringComparison("{{" + key + "}}", "NULL");
				else
					text = text.ReplaceWithStringComparison("{{" + key + "}}", value.ToString());
			// Devuelve el texto
			return text;
		}
	}
}
