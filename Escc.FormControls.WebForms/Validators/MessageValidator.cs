using System.Web.UI;
using System.Web.UI.WebControls;

namespace Escc.FormControls.WebForms.Validators
{
    /// <summary>
    /// Dummy validator to use to highlight to the validation framework a situation which we know is invalid
    /// </summary>
    public class MessageValidator : System.Web.UI.WebControls.BaseValidator
    {
        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        public static void Create(string message, Control container, string validationGroup)
        {
            // Inject a control that acts purely as a hook to add a validator to. It'll still do that
            // job without being displayed, so set Visible to false.
            TextBox controlToValidate = new TextBox();
            controlToValidate.ID = "controlToValidate";
            controlToValidate.ValidationGroup = validationGroup;
            container.Controls.Add(controlToValidate);
            controlToValidate.Visible = false;

            // Since we've already confirmed that we're in an invalid scenario, just add a dummy
            // validator that always returns false, which will cause the page to be invalid and 
            // force its error message to appear.
            MessageValidator v = new MessageValidator();
            v.ControlToValidate = controlToValidate.ID;
            v.ValidationGroup = validationGroup;
            v.ErrorMessage = message;
            container.Controls.Add(v);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
        /// </summary>
        private MessageValidator()
        {
            this.Display = ValidatorDisplay.None;
            this.EnableClientScript = false;
        }

        /// <summary>
        /// This validator will always return <c>false</c>
        /// </summary>
        /// <returns>false</returns>
        protected override bool EvaluateIsValid()
        {
            return false;
        }
    }
}
