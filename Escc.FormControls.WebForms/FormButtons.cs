using System.ComponentModel;
using System.Web.UI;

namespace Escc.FormControls.WebForms
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
