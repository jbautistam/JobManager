using System;

namespace Bau.Applications.JobsManager
{
	/// <summary>
	///		Clase para manejo de log
	/// </summary>
	internal class Logger : Libraries.LibJob.Core.Interfaces.ILogger
	{
		/// <summary>
		///		Tipo de log
		/// </summary>
		internal enum LogItemType
		{
			/// <summary>Mensaje de depuración</summary>
			Debug,
			/// <summary>Mensaje informativo</summary>
			Info,
			/// <summary>Mensaje de advertencia</summary>
			Warning,
			/// <summary>Mensaje de error</summary>
			Error
		}

		internal Logger(LogItemType minimumLog)
		{
			MinimumLog = minimumLog;
		}

		/// <summary>
		///		Escribe un mensaje informativo
		/// </summary>
		public void WriteInfo(string source, string message)
		{
			if (MustShow(LogItemType.Info))
				Write(ConsoleColor.White, "INFO: " + source, message);
		}

		/// <summary>
		///		Escribe un mensaje de depuración
		/// </summary>
		public void WriteDebug(string source, string message)
		{
			if (MustShow(LogItemType.Debug))
				Write(ConsoleColor.Green, "DEBUG: " + source, message);
		}

		/// <summary>
		///		Escribe un mensaje de advertencia
		/// </summary>
		public void WriteWarning(string source, string message)
		{
			if (MustShow(LogItemType.Warning))
				Write(ConsoleColor.Gray, "WARNING: " + source, message);
		}

		/// <summary>
		///		Escribe un mensaje de error
		/// </summary>
		public void WriteError(string source, string message, Exception exception = null)
		{
			if (MustShow(LogItemType.Error))
				Write(ConsoleColor.Red, "ERROR: " + source, message + Environment.NewLine + exception?.Message);
		}

		/// <summary>
		///		Escribe un mensaje de error
		/// </summary>
		public void WriteError(string source, Exception exception)
		{
			if (MustShow(LogItemType.Error))
				Write(ConsoleColor.Red, "ERROR: " + source, exception.Message);
		}

		/// <summary>
		///		Escribe un mensaje
		/// </summary>
		private void Write(ConsoleColor color, string source, string message)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(source);
			Console.WriteLine(new string(' ', 5) + message);
		}

		/// <summary>
		///		Indica si se debe mostrar el elemento de log
		/// </summary>
		private bool MustShow(LogItemType itemType)
		{
			return (int) itemType >= (int) MinimumLog;
		}

		/// <summary>
		///		Log mínimo para impresión
		/// </summary>
		internal LogItemType MinimumLog { get; }
	}
}
