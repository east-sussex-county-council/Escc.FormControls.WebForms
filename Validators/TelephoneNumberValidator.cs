using System;
using System.Web.UI.WebControls;

namespace eastsussexgovuk.webservices.FormControls.Validators
{
    /// <summary>
    /// Validate a telephone number according to the e-GIF standard
    /// </summary>
    /// <remarks>The e-GIF standard also allows for a country code and telephone extension</remarks>
    public class TelephoneNumberValidator : EsccRegularExpressionValidator
    {
        private const string VALIDATION_REGEX = @"^[0-9 ()\-]{0,20}$";

        /// <summary>
        /// Constructor sets the validation regular expression
        /// </summary>
        public TelephoneNumberValidator()
            : base()
        {
            this.ValidationExpression = VALIDATION_REGEX;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TelephoneNumberValidator"/> class.
        /// </summary>
        /// <param name="controlToValidateId">The id of the control to validate.</param>
        /// <param name="errorMessage">The error message.</param>
        public TelephoneNumberValidator(string controlToValidateId, string errorMessage)
            : base(controlToValidateId, errorMessage, VALIDATION_REGEX)
        {

        }
    }
}
