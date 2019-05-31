using System;
using System.Collections.Generic;

using Bau.Libraries.LibJob.Application.Models.Processes;

namespace Bau.Libraries.LibJob.Application.Models.Context
{
	/// <summary>
	///		Contexto del trabajo
	/// </summary>
	public class JobContextModel
	{
		public JobContextModel(JobManager jobManager, Interfaces.ILogger logger, Interfaces.IConsoleOutput consoleOutput)
		{
			JobManager = jobManager;
			Logger = logger;
			ConsoleOutput = consoleOutput;
		}

		/// <summary>
		///		Actualiza el trabajo y paso actual
		/// </summary>
		internal void UpdateJobStep(string processorKey, ProcessStepModel step)
		{
			ActualProcessorKey = processorKey;
			ActualStep = step;
		}

		/// <summary>
		///		Escribe una advertencia en el log
		/// </summary>
		public void WriteWarning(string message)
		{
			Logger.WriteWarning(GetSource(), message);
		}

		/// <summary>
		///		Escribe un mensaje de depuración en el log
		/// </summary>
		public void WriteDebug(string message)
		{
			WriteDebug(GetSource(), message);
		}

		/// <summary>
		///		Escribe un mensaje de depuración en el log
		/// </summary>
		public void WriteDebug(string source, string message)
		{
			Logger.WriteDebug(source, message);
		}

		/// <summary>
		///		Escribe un error en el log
		/// </summary>
		public void WriteError(string message)
		{
			WriteError(GetSource(), message);
		}

		/// <summary>
		///		Escribe un mensaje de depuración en el log
		/// </summary>
		public void WriteError(string source, string message)
		{
			// Escribe el error en el log
			Logger.WriteError(source, message);
			// Añade el error a la lista
			Errors.Add(message);
		}

		/// <summary>
		///		Escribe información en el log
		/// </summary>
		public void WriteInfo(string message)
		{
			Logger.WriteInfo(GetSource(), message);
		}

		/// <summary>
		///		Escribe un mensaje en la consola
		/// </summary>
		public void WriteConsole(string message)
		{
			// Log
			WriteDebug(message);
			// Consola
			ConsoleOutput.Write(message);
		}

		/// <summary>
		///		Escribe un mensaje en la consola
		/// </summary>
		public void WriteConsoleError(string message)
		{
			ConsoleOutput.WriteError(message);
		}

		/// <summary>
		///		Añade la información de tiempo
		/// </summary>
		public void AddBenchmark(string message, TimeSpan elapsed, long records)
		{
			// Añade la información a la lista
			Benchmarks.Add(new JobBenchmarkModel
										{
											Job = ActualStep?.Job?.Name,
											Step = ActualStep?.Name,
											Title = message,
											Elapsed = elapsed,
											Records = records
										}
						  );
			// Escribe la información
			WriteInfo($"Benchmark\t{message}\tElapsed: {elapsed:HH:mm:ss.ms}\tRecords: {records}");
		}

		/// <summary>
		///		Obtiene el origen para el log
		/// </summary>
		private string GetSource()
		{
			return $"{ActualStep.Job.Name} - {ActualStep.Name}";
		}

		/// <summary>
		///		Manager de trabajos
		/// </summary>
		public JobManager JobManager { get; }

		/// <summary>
		///		Clave del procesador actual
		/// </summary>
		public string ActualProcessorKey { get; private set; }

		/// <summary>
		///		Paso actual
		/// </summary>
		public ProcessStepModel ActualStep { get; private set; }

		/// <summary>
		///		Tratamiento de log
		/// </summary>
		public Interfaces.ILogger Logger { get; }

		/// <summary>
		///		Consola de salida de resultados
		/// </summary>
		public Interfaces.IConsoleOutput ConsoleOutput { get; } 

		/// <summary>
		///		Lista de errores
		/// </summary>
		public List<string> Errors { get; } = new List<string>();

		/// <summary>
		///		Benchmarks
		/// </summary>
		public List<JobBenchmarkModel> Benchmarks { get; } = new List<JobBenchmarkModel>();
	}
}
