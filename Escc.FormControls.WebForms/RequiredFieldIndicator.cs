using System;
using System.Web.UI.HtmlControls;
using Escc.FormControls.WebForms.Properties;

namespace Escc.FormControls.WebForms
{
	/// <summary>
	/// An marker which indicates that a form field is a required field
	/// </summary>
	public class RequiredFieldIndicator : HtmlGenericControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RequiredFieldIndicator"/> class.
		/// </summary>
		public RequiredFieldIndicator() : base("span")
		{
			
		}

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Init"/>
		/// event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnInit(EventArgs e)
		{
			this.Attributes["class"] = "requiredField";
            this.InnerText = EsccWebTeam_FormControls.RequiredFieldIndicator;

			base.OnInit (e);
		}


	}
}
