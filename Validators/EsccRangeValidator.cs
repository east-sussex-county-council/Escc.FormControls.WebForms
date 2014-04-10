using System;
using System.Web.UI.WebControls;

namespace eastsussexgovuk.webservices.FormControls.Validators
{
	/// <summary>
	/// RangeValidator with defaults used by ESCC
	/// </summary>
	public class EsccRangeValidator : RangeValidator
	{
		/// <summary>
		/// RangeValidator with defaults used by ESCC
		/// </summary>
		public EsccRangeValidator()
		{
			this.Display = ValidatorDisplay.None;
			this.EnableClientScript = false;
		}
		/// <summary>
		/// Overriden constructor that accepts a error message and control to validate against
		/// </summary>
		/// <param name="controlToValidateId">string</param>
		/// <param name="errorMessage">string</param>
		public EsccRangeValidator(string controlToValidateId, string errorMessage)
		{
			this.Display = ValidatorDisplay.None;
			this.EnableClientScript = false;
			this.ControlToValidate = controlToValidateId;
			this.ErrorMessage = errorMessage;
		}

	}
}
