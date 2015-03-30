using System.Web.UI.WebControls;

namespace Escc.FormControls.WebForms.Validators
{
    /// <summary>
    /// Validates the length of a string once white space has been trimmed from each end
    /// </summary>
    public class LengthValidator : BaseValidator
    {
        private int minimumLength;
        private int maximumLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="LengthValidator"/> class.
        /// </summary>
        public LengthValidator()
        {
            this.Display = ValidatorDisplay.None;
            this.EnableClientScript = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LengthValidator"/> class.
        /// </summary>
        /// <param name="controlToValidateId">The control to validate ID.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="minimumLength">The minimum length.</param>
        /// <param name="maximumLength">The maximum length.</param>
        public LengthValidator(string controlToValidateId, string errorMessage, int minimumLength, int maximumLength)
        {
            this.Display = ValidatorDisplay.None;
            this.EnableClientScript = false;
            this.ControlToValidate = controlToValidateId;
            this.ErrorMessage = errorMessage;
            this.minimumLength = minimumLength;
            this.maximumLength = maximumLength;
        }

        /// <summary>
        /// Gets or sets the minimum length.
        /// </summary>
        /// <value>The minimum length.</value>
        public int MinimumLength
        {
            get { return this.minimumLength; }
            set { this.minimumLength = value; }
        }

        /// <summary>
        /// Gets or sets the maximum length.
        /// </summary>
        /// <value>The maximum length.</value>
        public int MaximumLength
        {
            get { return this.maximumLength; }
            set { this.maximumLength = value; }
        }



        /// <summary>
        /// When overridden in a derived class, this method contains the code to determine
        /// whether the value in the input
        /// control is valid.
        /// </summary>
        /// <returns>
        /// 	<see langword="true"/> if the value
        /// in the input control is valid; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        /// <exception cref="System.NullReferenceException">Thrown if the TextBox specified by the ControlToValidate property is not found</exception>
        protected override bool EvaluateIsValid()
        {
            TextBox textboxToValidate = this.Parent.FindControl(this.ControlToValidate) as TextBox;
            string text = textboxToValidate.Text.Trim();
            return (text.Length >= this.minimumLength && text.Length <= this.maximumLength);
        }


    }
}
