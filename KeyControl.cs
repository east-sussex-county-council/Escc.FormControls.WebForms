using System;
using System.Resources;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EsccWebTeam.FormControls.Properties;
using EsccWebTeam.FormControls;
using EsccWebTeam.HouseStyle;

namespace EsccWebTeam.FormControls
{
	/// <summary>
	/// Standard key for markings used on forms
	/// </summary>
	public class KeyControl: PlaceHolder
	{
		/// <summary>
		/// Standard key for markings used on forms
		/// </summary>
		public KeyControl()
		{
		}

		/// <summary>
		/// Build controls for key
		/// </summary>
		protected override void CreateChildControls()
		{
			HtmlGenericControl reqPara = new HtmlGenericControl("p");
			reqPara.Controls.Add(new RequiredFieldIndicator());
			reqPara.Controls.Add(new LiteralControl(TextUtilities.ResourceString(typeof(EsccWebTeam_FormControls).Name, "RequiredFieldIndicatorHelp", EsccWebTeam_FormControls.RequiredFieldIndicatorHelp)));
			this.Controls.Add(reqPara);
		}

	}
}
