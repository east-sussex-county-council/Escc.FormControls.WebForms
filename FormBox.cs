using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using EsccWebTeam.HouseStyle;

namespace eastsussexgovuk.webservices.FormControls
{
	/// <summary>
	/// Presentation container for all controls on a form
	/// </summary>
	[DefaultProperty(""), 
		ToolboxData("<{0}:FormBox runat=server></{0}:FormBox>")]
	public class FormBox : System.Web.UI.WebControls.WebControl
	{
		/// <summary>
		/// Presentation container for all controls on a form
		/// </summary>
		public FormBox() : base(HtmlTextWriterTag.Div) 
		{
		}

		
		/// <summary>
		/// Build child controls
		/// </summary>
		protected override void CreateChildControls()
		{
			this.EnsureChildControls();

			// set CSS
		this.CssClass = "formBox";

			// add rounded corners
			RoundedBox topEdge = new RoundedBox();
		
			this.Controls.AddAt(0, topEdge);

			// add rounded corners
			RoundedBox bottomEdge = new RoundedBox();
        
			this.Controls.Add(bottomEdge);

		}

	}
}
