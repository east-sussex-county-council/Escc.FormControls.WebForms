using System;
using System.Web.UI.WebControls;


namespace eastsussexgovuk.webservices.FormControls.Validators
{
	/// <summary>
	/// RegularExpressionValidator with defaults used by ESCC
	/// </summary>
	public class EsccRegularExpressionValidator : RegularExpressionValidator
	{
		
		/// <summary>
		/// Creates a RegularExpressionValidator with defaults used by ESCC
		/// </summary>
		public EsccRegularExpressionValidator()
		{
			this.Display = ValidatorDisplay.None;
			this.EnableClientScript = false;
		}
		
		/// <summary>
		/// Creates a RegularExpressionValidator with defaults used by ESCC
		/// </summary>
		/// <param name="controlToValidateId"></param>
		/// <param name="errorMessage"></param>
		/// <param name="validationExpression"></param>
		public EsccRegularExpressionValidator(string controlToValidateId, string errorMessage, string validationExpression)
		{
			this.Display = ValidatorDisplay.None;
			this.EnableClientScript = false;
			this.ControlToValidate = controlToValidateId;
			this.ErrorMessage = errorMessage;
			this.ValidationExpression = validationExpression;
		}
	}
}
