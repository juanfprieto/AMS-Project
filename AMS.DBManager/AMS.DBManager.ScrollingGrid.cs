/*
 * Copyright © 2005, Ashley van Gerven (ashley.vg@gmail.com)
 * All rights reserved.
 *
 * Use in source and binary forms, with or without modification, is permitted 
 * provided that the above copyright notice and disclaimer below is not removed.
 * 
 * However if you wish to include this control as part of a redistributable 
 * project, contact the author.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
 * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
 * OF SUCH DAMAGE.
 */


using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;


namespace ScrollingGridDemo
{
	/// <summary>
	/// Cross-browser container control for a DataGrid to freeze the header row and sync the header when the DataGrid is scrolled horizontally
	/// </summary>
	[ToolboxData("<{0}:ScrollingGrid runat=server></{0}:ScrollingGrid>")]
	public class ScrollingGrid : Panel
	{
		private DataGrid grid = null;

		/// <summary>
		/// Overflow style setting. Can be auto, scroll, hidden
		/// </summary>
		public string Overflow = "scroll";

		/// <summary>
		/// Pixels to reduce the header and pager tables by (e.g. 17 for scrollbar width if you don't want the header to extend the full width of the control)
		/// </summary>
		public int HeaderWidthReduction = 0;

		/// <summary>
		/// True if scrolling should be enabled
		/// </summary>
		public bool ScrollingEnabled = true;



		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			// bg color style
			string style = "";
			if (!this.BackColor.IsEmpty)
			{
				string colorValue = this.BackColor.Name;
				if (colorValue.StartsWith("ff"))
					colorValue = "#" + colorValue.Substring(2);
				style = string.Format("background-color:{0};", colorValue);
			}

			if (ScrollingEnabled)
				style += string.Format("width:{0};", this.Width);

			// render the DIV slightly differently - since height style messes up FF layout
			string div = string.Format("<div id={0} style='{1}'", this.UniqueID, style);
			if (this.CssClass != null && this.CssClass.Length > 0)
				div += string.Format(" class='{0}'", this.CssClass);
			div += ">\r\n";
			writer.Write(div);
		}


		public override void RenderEndTag(HtmlTextWriter writer)
		{
			writer.Write("</div>\r\n");
		}


		protected override void OnInit(EventArgs e)
		{
			Page.RegisterClientScriptBlock("ScrollingGrid_js", "<script language=JavaScript src=\"../js/ScrollingGrid.js\" type=\"text/javascript\"></script>");

			// find the DataGrid control
			foreach (Control c in this.Controls)
			{
				if (c is DataGrid)
				{
					grid = (DataGrid)c;
					break;
				}
			}

			if (grid != null && ScrollingEnabled)
			{
				StringBuilder html = new StringBuilder();

				string widthStyleHdr = "width:" + (Width.Value - HeaderWidthReduction) + ";";
				string widthStyle = "width:" + Width + ";";

				if (grid.ShowHeader)
				{
					// header div
					html.AppendFormat("<div id={0}$divHdr style='{1}overflow:hidden;'>\r\n", this.UniqueID, widthStyleHdr);

					// header table borders
					string border = "0";
					string style = " style='border-collapse:collapse;'";
					if (!grid.BorderWidth.IsEmpty)
						border = grid.BorderWidth.Value.ToString();
					else if (grid.GridLines == GridLines.Both)
						border = "1";

					if (grid.CellSpacing > 0) // FF doesn't display cellspacing correctly with border-collapse style
						style = "";

					string borderColor = "";
					if (!grid.BorderColor.IsEmpty)
					{
						string colorValue = grid.BorderColor.Name;
						if (colorValue.StartsWith("ff"))
							colorValue = "#" + colorValue.Substring(2);
						borderColor = " borderColor='" + colorValue + "'";
					}

					// container table + header table
					html.AppendFormat("<table cellpadding=0 cellspacing=0 id={3}$headerCntr><tr><td><table id={3}$tblHdr border='{0}' cellpadding='{1}' cellspacing='{2}'{4}{5}>", border, grid.CellPadding, grid.CellSpacing, this.UniqueID, borderColor, style );
					html.Append(" <tr></tr></table></td></tr></table>\r\n");

					//close header div
					html.Append("</div>\r\n");
				}


				// scrolling div + 2nd container table
				html.AppendFormat("<div id={3}$divContent style='{4}height:{1};overflow:{2};' onscroll='updateScroll(this, \"{3}\")'><table cellpadding=0 cellspacing=0 id={3}$contentCntr><tr><td>", Width, Height, this.Overflow, this.UniqueID, widthStyle);


				// insert our html as the first control
				this.Controls.AddAt(0, new LiteralControl(html.ToString()));


				// close container table & scrolling div (appended to end)
				string appendHtml = "";
				if (grid.ShowHeader)
					appendHtml += "</td></tr></table>";
				appendHtml += "</div>\r\n";
				this.Controls.Add(new LiteralControl(appendHtml));


				// check for datagrid pager
				bool lastRowIsPager = false;
				if (grid.AllowPaging && grid.PagerStyle.Visible && (grid.PagerStyle.Position == PagerPosition.Bottom || grid.PagerStyle.Position == PagerPosition.TopAndBottom))
				{
					lastRowIsPager = true;

					// pager table underneath scrolling grid
					this.Controls.Add(new LiteralControl( string.Format("<table id={0}$tblPager cellpadding={1} cellspacing={2} style='" + widthStyleHdr + "'> <tr></tr></table>\r\n", this.UniqueID, grid.CellPadding, grid.CellSpacing) ));
				}


				// javacript to initialise grid
				if (grid.ShowHeader)
					this.Controls.Add( new LiteralControl(string.Format("<script language=javascript>\r\n<!--\r\n setTimeout(\"initScrollingGrid('{0}', '{1}', {2})\", 50) \r\n//--></script>", this.ClientID, grid.ClientID, lastRowIsPager.ToString().ToLower())) );
			}

			base.OnInit(e);
		}

	}
}
