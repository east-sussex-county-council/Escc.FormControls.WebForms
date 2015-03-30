using System;
using System.Web.UI.WebControls;

namespace Escc.FormControls.WebForms.Validators
{
	/// <summary>
	/// Base class for custom validators which default to valid XHTML settings
	/// </summary>
	public class EsccCustomValidator : CustomValidator
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EsccCustomValidator"/> class.
		/// </summary>
		public EsccCustomValidator() : base()
		{
		}

		
		/// <summary>
		/// Initializes a new instance of the <see cref="EsccCustomValidator"/> class.
		/// </summary>
		/// <param name="controlToValidateId">The control to validate ID.</param>
		/// <param name="errorMessage">The error message.</param>
		public EsccCustomValidator(string controlToValidateId, string errorMessage) : base()
		{
			this.ControlToValidate = controlToValidateId;
			this.ErrorMessage = errorMessage;
		}
		
		/// <summary>
		/// Raises the init event, and sets standard properties for the validator
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);

			this.Display = ValidatorDisplay.None;
			this.EnableClientScript = false;

		}

		
	}
}
