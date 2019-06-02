using System;

namespace Bau.Libraries.LibJob.Core.Models.Context
{
	/// <summary>
	///		Modelo para los datos de benchmark
	/// </summary>
	public class JobBenchmarkModel
	{
		/// <summary>
		///		Nombre del trabajo
		/// </summary>
		public string Job { get; set; }

		/// <summary>
		///		Nombre del paso
		/// </summary>
		public string Step { get; set; }

		/// <summary>
		///		Título
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		///		Tiempo utilizado
		/// </summary>
		public TimeSpan Elapsed { get; set; }

		/// <summary>
		///		Número de registros
		/// </summary>
		public long Records { get; set; }
	}
}
