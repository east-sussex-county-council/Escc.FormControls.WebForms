using System.Web.UI.WebControls;

namespace Escc.FormControls.WebForms.Validators
{
    /// <summary>
    /// Extended drop down list validator to support eforms system
    /// </summary>
    public class EformsRequiredDropDownListValidator: RequiredFieldValidator
    {
        /// <summary>
        /// Constructor sets display to allow validation summary to display errors
        /// </summary>
        public EformsRequiredDropDownListValidator()
        {
            this.Display = ValidatorDisplay.None;
            this.EnableClientScript = false;
        }

        /// <summary>
        /// Constructor overriden to allow setting of error message and control to validate
        /// </summary>
        /// <param name="controlToValidateId"></param>
        /// <param name="errorMessage"></param>
        public EformsRequiredDropDownListValidator(string controlToValidateId, string errorMessage)
        {
            this.Display = ValidatorDisplay.None;
            this.EnableClientScript = false;
            this.ControlToValidate = controlToValidateId;
            this.ErrorMessage = errorMessage;
            
            
        }
       


        protected override bool EvaluateIsValid()
        {
            string currentSelection = string.Empty;
            currentSelection = this.Context.Request.Form[ControlToValidate];

            if (currentSelection == this.InitialValue | currentSelection == "No selection")
            {
                this.IsValid = false;
            }
            else
            {
                this.IsValid = true;
            }

            return this.IsValid;
        }
    }
}
