using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace eastsussexgovuk.webservices.FormControls
{
	/// <summary>
	/// Container for buttons at the bottom of a form
	/// </summary>
	[DefaultProperty(""), 
		ToolboxData("<{0}:FormButtons runat=server></{0}:FormButtons>")]
	public class FormButtons : System.Web.UI.WebControls.WebControl
	{
		/// <summary>
		/// Constructor creates this control as a <div></div>
		/// </summary>
		public FormButtons() : base(HtmlTextWriterTag.Div) 
		{
			this.CssClass = "formButtons";
		}
		
	}
}
