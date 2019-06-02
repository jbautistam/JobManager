using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibDataStructures.Collections;
using Bau.Libraries.LibJob.Core.Models;
using Bau.Libraries.LibJob.Core.Models.Processes;

namespace Bau.Libraries.LibJob.Application.Repository
{
	/// <summary>
	///		Clase de lectura de <see cref="JobModel"/>
	/// </summary>
	internal class JobRepository
	{
		// Constantes privadas
		private const string TagRoot = "Job";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		private const string TagBlock = "Block";
		private const string TagStep = "Step";
		private const string TagScript = "Script";
		private const string TagPluginKey = "Plugin";
		private const string TagParameter = "Parameter";
		private const string TagKey = "Key";
		private const string TagType = "Type";
		private const string TagValue = "Value";
		private const string TagNow = "Now";
		private const string TagIncrement = "Increment";
		private const string TagInterval = "Interval";
		private const string TagMode = "Mode";
		private const string TagStartWithPreviousError = "StartWithPreviousError";
		// Enumerados privados
		/// <summary>
		///		Tipo de parámetro (para no estar pasando de mayúsculas a minúsculas por el XML)
		///	</summary>
		private enum ParameterType
		{
			/// <summary>Desconocido. No se debería utilizar</summary>
			Unknown,
			/// <summary>Numérico</summary>
			Numeric,
			/// <summary>Fecha</summary>
			DateTime,
			/// <summary>Cadena</summary>
			String,
			/// <summary>Lógico</summary>
			Boolean
		}
		/// <summary>
		///		Tipo de intervalo
		/// </summary>
		private enum IntervalType
		{
			/// <summary>Año</summary>
			Year,
			/// <summary>Mes</summary>
			Month,
			/// <summary>Día</summary>
			Day
		}
		/// <summary>Modo de ajuste del intervalo</summary>
		private enum IntervalMode
		{
			/// <summary>Desconocido. No se debería utilizar</summary>
			Unknown,
			/// <summary>Ajusta el intervalo al siguiente lunes</summary>
			NextMonday,
			/// <summary>Ajusta el intervalo al lunes anterior</summary>
			PreviousMonday,
			/// <summary>Ajusta el intervalo a final de mes</summary>
			MonthEnd,
			/// <summary>Ajusta el intervalo a inicio de mes</summary>
			MonthStart
		}

		/// <summary>
		///		Carga los datos de un trabajo
		/// </summary>
		internal JobModel Load(string fileName)
		{
			JobModel job = new JobModel();

				// Carga los datos del archivo
				if (System.IO.File.Exists(fileName))
				{
					MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);

						if (fileML != null)
							foreach (MLNode rootML in fileML.Nodes)
								if (rootML.Name == TagRoot)
								{
									// Carga los parámetros básicos
									job.Name = rootML.Nodes[TagName].Value;
									job.Description = rootML.Nodes[TagDescription].Value;
									// Carga el resto de parámetros
									foreach (MLNode nodeML in rootML.Nodes)
										switch (nodeML.Name)
										{
											case TagBlock:
													job.Steps.Add(LoadBlock(job, nodeML));
												break;
											case TagStep:
													job.Steps.Add(LoadStep(job, nodeML));
												break;
											case TagParameter:
													LoadParameter(nodeML, job.Parameters);
												break;
										}
								}
				}
				// Devuelve los datos del trabajo
				return job;
		}

		/// <summary>
		///		Carga los datos de un bloque
		/// </summary>
		private ProcessBaseModel LoadBlock(JobModel job, MLNode rootML)
		{
			ProcessBlockModel block = new ProcessBlockModel(job);

				// Asigna las propiedades básicas
				block.StartWithPreviousError = rootML.Attributes[TagStartWithPreviousError].Value.GetBool(false);
				block.Name = rootML.Nodes[TagName].Value;
				block.Description = rootML.Nodes[TagDescription].Value;
				// Carga los datos
				foreach (MLNode nodeML in rootML.Nodes)
					switch (nodeML.Name)
					{
						case TagStep:
								block.Steps.Add(LoadStep(job, nodeML));
							break;
					}
				// Devuelve los datos del bloque
				return block;
		}

		/// <summary>
		///		Carga los datos de un paso
		/// </summary>
		private ProcessBaseModel LoadStep(JobModel job, MLNode rootML)
		{
			ProcessStepModel step = new ProcessStepModel(job);

				// Asigna las propiedades básicas
				step.Key = rootML.Attributes[TagKey].Value;
				step.PluginKey = rootML.Attributes[TagPluginKey].Value;
				step.StartWithPreviousError = rootML.Attributes[TagStartWithPreviousError].Value.GetBool(false);
				step.Name = rootML.Nodes[TagName].Value;
				step.Description = rootML.Nodes[TagDescription].Value;
				step.ScriptFileName = rootML.Nodes[TagScript].Value;
				// Carga los datos
				foreach (MLNode nodeML in rootML.Nodes)
					switch (nodeML.Name)
					{
						case TagParameter:
								LoadParameter(nodeML, step.Parameters);
							break;
					}
				// Devuelve los datos del paso
				return step;
		}

		/// <summary>
		///		Carga un parámetro
		/// </summary>
		private void LoadParameter(MLNode rootML, NormalizedDictionary<object> parameters)
		{
			object value;

				// Obtiene el valor
				switch (rootML.Attributes[TagType].Value.GetEnum(ParameterType.Unknown))
				{
					case ParameterType.Numeric:
							value = rootML.Attributes[TagValue].Value.GetDouble(0);
						break;
					case ParameterType.Boolean:
							value = rootML.Attributes[TagValue].Value.GetBool();
						break;
					case ParameterType.DateTime:
							value = ConvertDate(rootML);
						break;
					default:
							if (string.IsNullOrWhiteSpace(rootML.Attributes[TagValue].Value))
								value = rootML.Value;
							else
								value = rootML.Attributes[TagValue].Value;
						break;
				}
				// Añade el parámetro
				parameters.Add(rootML.Attributes[TagKey].Value, value);
		}

		/// <summary>
		///		Convierte una fecha
		/// </summary>
		private DateTime ConvertDate(MLNode nodeML)
		{
			int increment = nodeML.Attributes[TagIncrement].Value.GetInt(0);
			DateTime date = DateTime.Now.Date;

				// Se recoge la fecha (si se ha introducido alguna)
				if (!nodeML.Attributes[TagValue].Value.EqualsIgnoreCase(TagNow))
					date = nodeML.Attributes[TagValue].Value.GetDateTime(DateTime.Now);
				// Ajusta el valor con los parámetros del XML
				if (increment != 0)
					switch (nodeML.Attributes[TagInterval].Value.GetEnum(IntervalType.Day))
					{
						case IntervalType.Day:
								date = date.AddDays(increment);
							break;
						case IntervalType.Month:
								date = date.AddMonths(increment);
							break;
						case IntervalType.Year:
								date = date.AddYears(increment);
							break;
					}
				// Ajusta la fecha
				switch (nodeML.Attributes[TagMode].Value.GetEnum(IntervalMode.Unknown))
				{
					case IntervalMode.PreviousMonday:
							date = GetPreviousMonday(date);
						break;
					case IntervalMode.NextMonday:
							date = GetNextMonday(date);
						break;
					case IntervalMode.MonthStart:
							date = GetFirstMonthDay(date);
						break;
					case IntervalMode.MonthEnd:
							date = GetLastMonthDay(date);
						break;
				}
				// Devuelve la fecha calculada
				return date.Date;
		}

		/// <summary>
		///		Obtiene el lunes anterior a una fecha (o la misma fecha si ya es lunes)
		/// </summary>
		private DateTime GetPreviousMonday(DateTime date)
		{
			// Busca el lunes anterior
			while (date.DayOfWeek != DayOfWeek.Monday)
				date = date.AddDays(-1);
			// Devuelve la fecha
			return date;
		}

		/// <summary>
		///		Obtiene el lunes siguiente a una fecha (o la misma fecha si ya es lunes)
		/// </summary>
		private DateTime GetNextMonday(DateTime date)
		{
			// Busca el lunes anterior
			while (date.DayOfWeek != DayOfWeek.Monday)
				date = date.AddDays(1);
			// Devuelve la fecha
			return date;
		}

		/// <summary>
		///		Obtiene el primer día del mes
		/// </summary>
		private DateTime GetFirstMonthDay(DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1);
		}

		/// <summary>
		///		Obtiene el último día del mes
		/// </summary>
		private DateTime GetLastMonthDay(DateTime date)
		{
			return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
		}
	}
}
