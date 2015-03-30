using System.Web.UI.WebControls;

namespace Escc.FormControls.WebForms.Validators
{
	/// <summary>
	/// ValidationSummary with defaults used by ESCC Web Services
	/// </summary>
	public class EsccValidationSummary : ValidationSummary
	{
		/// <summary>
		/// Display server-side-only validation error messages in a bulleted list
		/// </summary>
		public EsccValidationSummary()
		{
			this.DisplayMode = ValidationSummaryDisplayMode.BulletList;
			this.EnableClientScript = false;
			this.CssClass = "validationSummary";

		}

				
	}
}
