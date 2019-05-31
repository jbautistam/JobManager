using System;

namespace Bau.Libraries.LibJob.Application.Interfaces
{
	/// <summary>
	///		Salida de la consola
	/// </summary>
	public interface IConsoleOutput
	{
		/// <summary>
		///		Escribe un mensaje
		/// </summary>
		void Write(string message);

		/// <summary>
		///		Escribe un error
		/// </summary>
		void WriteError(string message);
	}
}
