using System;

namespace Bau.Applications.JobsManager
{
	/// <summary>
	///		Consola de ejecución
	/// </summary>
	internal class ConsoleOutput : Libraries.LibJob.Application.Interfaces.IConsoleOutput
	{
		/// <summary>
		///		Escribe un mensaje en la consola
		/// </summary>
		public void Write(string message)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(message);
		}

		/// <summary>
		///		Escribe un mensaje de error en la consola
		/// </summary>
		public void WriteError(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
		}
	}
}
