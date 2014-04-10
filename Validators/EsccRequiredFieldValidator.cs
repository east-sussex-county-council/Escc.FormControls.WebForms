using System;
using System.Web.UI.WebControls;

namespace eastsussexgovuk.webservices.FormControls.Validators
{
	/// <summary>
	/// RequiredFieldValidator with defaults used by ESCC
	/// </summary>
	public class EsccRequiredFieldValidator : RequiredFieldValidator
	{
		/// <summary>
		/// RequiredFieldValidator with defaults used by ESCC
		/// </summary>
		public EsccRequiredFieldValidator()
		{
			this.Display = ValidatorDisplay.None;
			this.EnableClientScript = false;
		}
		/// <summary>
		/// Overriden constructor that accepts a error message and control to validate against
		/// </summary>
		/// <param name="controlToValidateId">string</param>
		/// <param name="errorMessage">string</param>
		public EsccRequiredFieldValidator(string controlToValidateId, string errorMessage)
		{
			this.Display = ValidatorDisplay.None;
			this.EnableClientScript = false;
			this.ControlToValidate = controlToValidateId;
			this.ErrorMessage = errorMessage;
		}

	}
}
