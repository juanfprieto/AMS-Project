namespace AMS.Tools
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Descripción breve de AMS_Tools_Seleccionar.
	/// </summary>
	public partial class Seleccionar : System.Web.UI.UserControl
	{
	//	int indiceo = 0;
	//	int indiced = 0;
	//	int i = 0;
		protected ListItem It = null;
		protected string tem = "";

        public Button BotonAccion{
            set{Button4 = value;}
            get{return Button4;}
        }

		public delegate void DCargarG(System.Web.UI.WebControls.ListBox Inicio,System.Web.UI.WebControls.ListBox Final);
		public event DCargarG ECargarG;

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		protected void Button4_Click(object sender, System.EventArgs e)
		{
			//Cargar valores
			ListItemCollection lstTodos=new ListItemCollection();
			ArrayList arlValoresDestino=new ArrayList();
			arlValoresDestino.AddRange(hdn_values.Value.Split(','));
            if (!hdn_values.Value.Equals(""))
            {
                for (int n = 0; n < Origen.Items.Count; n++)
                    lstTodos.Add(new ListItem(Origen.Items[n].Text, Origen.Items[n].Value));
                for (int n = 0; n < Destino.Items.Count; n++)
                    lstTodos.Add(new ListItem(Destino.Items[n].Text, Destino.Items[n].Value));
                Origen.Items.Clear();
                Destino.Items.Clear();
                for (int n = 0; n < lstTodos.Count; n++)
                    if (arlValoresDestino.Contains(lstTodos[n].Value))
                        Destino.Items.Add(new ListItem(lstTodos[n].Text, lstTodos[n].Value));
                    else
                        Origen.Items.Add(new ListItem(lstTodos[n].Text, lstTodos[n].Value));
            }
            
			ECargarG(Origen,Destino);
		}

		public void CargarOrigen(int indice,string texto,string valor,string color)
		{
			It = new ListItem(texto,valor);
			tem = "background-color: " + color;
			System.Web.UI.AttributeCollection Ac = It.Attributes;
			Ac.Add("BackColor","Color.blue");

			It.Attributes.Add("BackColor","Color.blue");
			Origen.Items.Insert(indice,It);
		}

		public void CargarDestino(int indice,string texto,string valor,string color)
		{
			It = new ListItem(texto,valor);
			tem = "background-color: " + color;
			It.Attributes.Add("style",tem);
			Destino.Items.Insert(indice,It);
		}

		public void LimpiarTodo()
		{
			Destino.Items.Clear();
			Origen.Items.Clear();
		}
		
		public void AsignarEstados(bool pp1,bool pp2,bool pp3,bool pp4)
		{
			Todos.Visible = pp1;
			Pasar.Visible = pp2;
			Quitar.Visible = pp3;
			Limpiar.Visible = pp4;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				this.Todos.Attributes.Add("onclick","fnR_"+this.ClientID+"(false);return(false);");
				this.Pasar.Attributes.Add("onclick","fnR_"+this.ClientID+"(true);return(false);");
				this.Quitar.Attributes.Add("onclick","fnL_"+this.ClientID+"(true);return(false);");
				this.Limpiar.Attributes.Add("onclick","fnL_"+this.ClientID+"(false);return(false);");
			}
		}

		public void AsignarTextoLabels(string texto1,string texto2)
		{
			lbTitulo1.Text=texto1;
			lbTitulo2.Text=texto2;
		}

        public void AsignarTextoBoton(string texto1)
        {
            Button4.Text = texto1;
         }
    }
}
