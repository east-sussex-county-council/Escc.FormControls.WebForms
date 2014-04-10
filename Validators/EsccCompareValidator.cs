using System;
using System.Web.UI.WebControls;

namespace eastsussexgovuk.webservices.FormControls.Validators
{
	/// <summary>
	/// EsccCompareValidator with defaults used by ESCC
	/// </summary>
	public class EsccCompareValidator : CompareValidator
	{
		/// <summary>
		/// Compare one control to another, using default settings used by ESCC Web Services
		/// </summary>
		public EsccCompareValidator()
		{
			this.Display = ValidatorDisplay.None;
			this.EnableClientScript = false;
		}

		/// <summary>
		/// Compare one control to another, using default settings used by ESCC Web Services
		/// </summary>
		/// <param name="controlToValidateId">Id of the TextBox in which the email address is being entered</param>
		/// <param name="controlToCompareId">Id of the TextBox in which the email address is being entered</param>
		/// <param name="errorMessage">The error message to display in the validation summary</param>
		public EsccCompareValidator(string controlToValidateId, string controlToCompareId, string errorMessage)
		{
			this.Display = ValidatorDisplay.None;
			this.EnableClientScript = false;
			this.ControlToValidate = controlToValidateId;
			this.ControlToCompare = controlToCompareId;
			this.ErrorMessage = errorMessage;
		}


		/// <summary>
		/// Compare one control to another, using default settings used by ESCC Web Services
		/// </summary>
		/// <param name="controlToValidateId">Id of the TextBox in which the email address is being entered</param>
		/// <param name="dataType">The value must be valid for this datatype</param>
		/// <param name="errorMessage">The error message to display in the validation summary</param>
		public EsccCompareValidator(string controlToValidateId, ValidationDataType dataType, string errorMessage)
		{
			this.Display = ValidatorDisplay.None;
			this.EnableClientScript = false;
			this.ControlToValidate = controlToValidateId;
			this.Operator = ValidationCompareOperator.DataTypeCheck;
			this.Type = dataType;
			this.ErrorMessage = errorMessage;
		}

	}
}
