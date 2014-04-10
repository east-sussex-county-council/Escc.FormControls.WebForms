using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace EsccWebTeam.FormControls
{
	/// <summary>
	/// A standard <see cref="RadioButtonList"/> with properties set to produce accessible XHTML
	/// </summary>
	[ToolboxData("<{0}:EsccRadioButtonList runat=server></{0}:EsccRadioButtonList>")]
	public class EsccRadioButtonList : System.Web.UI.WebControls.RadioButtonList
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EsccRadioButtonList"/> class.
		/// </summary>
		public EsccRadioButtonList() : base()
		{
			this.RepeatDirection = RepeatDirection.Horizontal;
			this.RepeatLayout = RepeatLayout.Flow;
		}
	}
}
