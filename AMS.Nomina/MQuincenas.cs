using System;

namespace AMS.Nomina
{
	/// <summary>
	/// Descripción breve de MQuincenas.
	/// </summary>
	public class MQuincenas
	{
		public const string CODIGO="MQUI_CODIQUIN";
		public const string ANO="MQUI_ANOQUIN"; 
		public const string MES="MQUI_MESQUIN"; 
		public const string PERIODONOMINA="MQUI_TPERNOMI"; 
		public const string ESTADO="MQUI_ESTADO";

		private int codigo;
		private int ano;
		private int mes;
		private int periodoNomina;
		private int estado;

		public MQuincenas()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}

		public int Codigo
		{
			get
			{
				return this.codigo;
			}
			set
			{
				this.codigo=value;
			}
		}
		
		public int Ano
		{
			get
			{
				return this.ano;
			}
			set
			{
				this.ano=value;
			}
		}

		public int Mes
		{
			get
			{
				return this.mes;
			}
			set
			{
				this.mes=value;
			}
		}

		public int PeriodoNomina
		{
			get
			{
				return this.periodoNomina;
			}
			set
			{
				this.periodoNomina=value;
			}
		}

		public int Estado
		{
			get
			{
				return this.estado;
			}
			set
			{
				this.estado=value;
			}
		}

	}
}
