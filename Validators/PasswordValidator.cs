using System;
using System.Web.UI.WebControls;


namespace eastsussexgovuk.webservices.FormControls.Validators
{
	/// <summary>
	///  Validate a password against the ESCC website standard
	/// </summary>
	public class PasswordValidator : RegularExpressionValidator
	{
		/// <summary>
		/// Validate an password against the ESCC website standard
		/// </summary>
		/// <param name="controlToValidateId">Id of the TextBox in which the password is being entered</param>
		/// <param name="errorMessage">The error message to display in the validation summary</param>
		public PasswordValidator(string controlToValidateId, string errorMessage)
		{
			this.Display = ValidatorDisplay.None;
			this.EnableClientScript = false;
			this.ControlToValidate = controlToValidateId;
			this.ErrorMessage = errorMessage;
			this.ValidationExpression = @"^[0-9A-Za-z]{6,16}$";
		}
	}
}
