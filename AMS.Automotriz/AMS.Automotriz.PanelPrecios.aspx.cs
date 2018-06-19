using System;
using System.Collections;
using System.Data;
using AMS.DB;

namespace AMS.Automotriz
{
    public partial class PanelPrecios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Automotriz.PanelPrecios));
        }

        [Ajax.AjaxMethod]
        public ArrayList GetGrupos()
        {
            ArrayList arrGrupos = new ArrayList();

            //Retornar los grupos que tengas kits unicamente.
            arrGrupos = DBFunctions.RequestAsCollection(
                    @"(select  p.pgru_grupo as grupo_cat 
                    from pgrupocatalogo p, pkit k, MKITITEM mk, mitems i
                    where
                    p.pgru_grupo = k.pgru_grupo
                    and mk.mkit_codikit = k.pkit_codigo
                    and i.mite_codigo = mk.mkit_coditem
                    union
                    select p.pgru_grupo as grupo_cat 
                    from pgrupocatalogo p, pkit k, MKIToperacion mo, ptempario o
                    where
                    p.pgru_grupo = k.pgru_grupo
                    and mo.mkit_codikitoper = k.pkit_codigo
                    and o.ptem_operacion = mo.mkit_operacion); ");

            return arrGrupos;
        }
        

        [Ajax.AjaxMethod]
        public DataTable GetListaPrecios(string codigoGrupo)
        {
            DataSet dsListaPrecios = new DataSet();
            //Obtener consulta con todos los kits posibles segun el grupo seleccionado.
            DBFunctions.Request(dsListaPrecios, IncludeSchema.NO,
              @"(select  p.pgru_nombre as grupo_cat, k.pkit_codigo as codigo_kit, k.pkit_nombre as nombre_kit, i.mite_codigo as codigo_elemento,
                i.mite_nombre as elemento,
                CASE i.mite_indigeneric
                WHEN 'S' THEN(1 + (i.piva_porciva * 0.01)) * (mp.mpre_precio * (select mg.mig_cantidaduso from mitemsgrupo mg where mg.mite_codigo = i.mite_codigo and mg.pgru_grupo = p.pgru_grupo))
                WHEN 'N' THEN(1 + (i.piva_porciva * 0.01)) * (mp.mpre_precio * i.mite_usoxvehi)
                END as valor,
                k.pkit_kilometr as kilometraje, pgru_imagen as imagen
                from pgrupocatalogo p, pkit k, MKITITEM mk, mitems i, mprecioitem mp
                where p.pgru_grupo = '" + codigoGrupo + @"'
                and p.pgru_grupo = k.pgru_grupo
                and mk.mkit_codikit = k.pkit_codigo
                and i.mite_codigo = mk.mkit_coditem
                and i.mite_codigo = mp.mite_codigo
                and mp.ppre_codigo = k.ppre_codigo
                union
                select p.pgru_nombre as grupo_cat, k.pkit_codigo as codigo_kit, k.pkit_nombre as nombre_kit, o.ptem_operacion as codigo_elemento, 
                o.ptem_descripcion as elemento,
                (select p1.ppreta_valohoraclie from ppreciotaller p1 where p1.ppreta_codigo = p.ppreta_codigo) * o.ptem_tiempoestandar as valor,
                k.pkit_kilometr as kilometraje, pgru_imagen as imagen
                from pgrupocatalogo p, pkit k, MKIToperacion mo, ptempario o
                where p.pgru_grupo = '" + codigoGrupo + @"'
                and p.pgru_grupo = k.pgru_grupo
                and mo.mkit_codikitoper = k.pkit_codigo
                and o.ptem_operacion = mo.mkit_operacion)
                order by codigo_kit, codigo_elemento; ");

                //@"(select  p.pgru_nombre as grupo_cat, k.pkit_codigo as codigo_kit, k.pkit_nombre as nombre_kit, i.mite_codigo as codigo_elemento, 
                //i.mite_nombre as elemento, mite_costrepo as valor, k.pkit_kilometr as kilometraje, pgru_imagen as imagen
                //from pgrupocatalogo p, pkit k, MKITITEM mk, mitems i
                //where p.pgru_grupo = '" + codigoGrupo + @"'
                //and p.pgru_grupo = k.pgru_grupo
                //and mk.mkit_codikit = k.pkit_codigo
                //and i.mite_codigo = mk.mkit_coditem
                //union
                //select p.pgru_nombre as grupo_cat, k.pkit_codigo as codigo_kit, k.pkit_nombre as nombre_kit, o.ptem_operacion as codigo_elemento, 
                //o.ptem_descripcion as elemento, ptem_valooper as valor, k.pkit_kilometr as kilometraje, pgru_imagen as imagen
                //from pgrupocatalogo p, pkit k, MKIToperacion mo, ptempario o
                //where p.pgru_grupo = '" + codigoGrupo + @"'
                //and p.pgru_grupo = k.pgru_grupo
                //and mo.mkit_codikitoper = k.pkit_codigo
                //and o.ptem_operacion = mo.mkit_operacion)
                //order by codigo_kit, codigo_elemento;");

            string codigoKit = "";
            DataTable dtFichas = new DataTable();
            dtFichas.Columns.Add("GRUPO");
            dtFichas.Columns.Add("CODIGO");
            dtFichas.Columns.Add("ELEMENTO");
            dtFichas.Columns.Add("VALOR");
            dtFichas.Columns.Add("KM");
            dtFichas.Columns.Add("IMAGEN");

            //Se recorren todos los tados para formar un kit entero por registro, y obtener los nombres de cada uno para ver suales
            //kits se repiten en sus elementos.
            for (int k=0; k < dsListaPrecios.Tables[0].Rows.Count; k++)
            {
                DataRow drowFicha = dtFichas.NewRow();
                drowFicha["GRUPO"] = dsListaPrecios.Tables[0].Rows[k]["GRUPO_CAT"].ToString();
                drowFicha["CODIGO"] = dsListaPrecios.Tables[0].Rows[k]["CODIGO_KIT"].ToString();
                codigoKit = dsListaPrecios.Tables[0].Rows[k]["CODIGO_KIT"].ToString();
                drowFicha["ELEMENTO"] = "<li>" + dsListaPrecios.Tables[0].Rows[k]["ELEMENTO"].ToString() + "</li>";
                double sumValor = Convert.ToDouble(dsListaPrecios.Tables[0].Rows[k]["VALOR"].ToString());
                drowFicha["KM"] = dsListaPrecios.Tables[0].Rows[k]["KILOMETRAJE"].ToString().Replace("000","");
                drowFicha["IMAGEN"] = dsListaPrecios.Tables[0].Rows[k]["IMAGEN"].ToString();
                bool nextGrupo = false;

                while (nextGrupo == false && (k+1) < dsListaPrecios.Tables[0].Rows.Count)
                {
                    if(codigoKit == dsListaPrecios.Tables[0].Rows[k+1]["CODIGO_KIT"].ToString())
                    {
                        drowFicha["CODIGO"] += "," + dsListaPrecios.Tables[0].Rows[k+1]["CODIGO_KIT"].ToString();
                        drowFicha["ELEMENTO"] += "<li>" + dsListaPrecios.Tables[0].Rows[k+1]["ELEMENTO"].ToString() + "</li>";
                        sumValor += Convert.ToDouble(dsListaPrecios.Tables[0].Rows[k+1]["VALOR"].ToString());
                        k++;
                    }
                    else
                    {
                        nextGrupo = true;
                    }
                }

                drowFicha["VALOR"] = sumValor.ToString("C0");
                dtFichas.Rows.Add(drowFicha);
            }

            //Se recorre la tabla generada comparando si har kits con elementos identicos y volverlos asi una sola tarjeta.
            for(int t=0; t < dtFichas.Rows.Count; t++)
            {
                string elementoRef = dtFichas.Rows[t]["ELEMENTO"].ToString();
                for (int u = (t+1); u < dtFichas.Rows.Count; u++)
                {
                    string elementoNext = dtFichas.Rows[u]["ELEMENTO"].ToString();
                    if(elementoRef == elementoNext)
                    {
                        dtFichas.Rows[t]["KM"] += ", " + dtFichas.Rows[u]["KM"].ToString().Replace("000", "");
                        dtFichas.Rows.RemoveAt(u);
                        u--;
                    }
                }
            }

            return dtFichas;
        }

    }
}