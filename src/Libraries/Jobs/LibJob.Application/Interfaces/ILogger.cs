using System;

namespace Bau.Libraries.LibJob.Application.Interfaces
{
	/// <summary>
	///		Interface para tratamiento de log
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		///		Escribe un mensaje informativo
		/// </summary>
		void WriteInfo(string source, string message);

		/// <summary>
		///		Escribe un mensaje de depuración
		/// </summary>
		void WriteDebug(string source, string message);

		/// <summary>
		///		Escribe un mensaje de advertencia
		/// </summary>
		void WriteWarning(string source, string message);

		/// <summary>
		///		Escribe un mensaje de error
		/// </summary>
		void WriteError(string source, string message, Exception exception = null);

		/// <summary>
		///		Escribe un error
		/// </summary>
		void WriteError(string source, Exception exception);
	}
}
