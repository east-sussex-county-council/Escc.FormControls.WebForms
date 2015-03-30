using System;
using System.Web.UI;

namespace Escc.FormControls.WebForms.Validators
{
    /// <summary>
    /// Adds a message to the validation summary on the current page
    /// </summary>
    /// <example>
    /// <para>Just create an instance of this class to add a message to the validation summary. 
    /// You don't need to do anything with the instance - just create it. In the following example
    /// <c>this</c> refers to the current page.</para>
    /// <code>new ValidationMessage("This is the error message", this);</code>
    /// </example>
    /// <remarks>Code is from <a href="http://www.leastprivilege.com/AddingErrorMessagesToAValidationSummary.aspx">http://www.leastprivilege.com/AddingErrorMessagesToAValidationSummary.aspx</a></remarks>
    [Obsolete ("Use Escc.FormControls.WebForms.Validators.MessageValidator instead")]
    internal class ValidationMessage : IValidator
    {

        string _message;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="page">The page.</param>
        public ValidationMessage(string message, Page page)
        {
            _message = message;
            page.Validators.Add(this);
        }

        /// <summary>
        /// Gets or sets the error message text generated 
        /// </summary>
        /// <value></value>
        /// <returns>The error message to generate.</returns>
        public string ErrorMessage
        {
            get { return _message; }
            set { this._message = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the user-entered content in the specified control passes validation. Setter does nothing.
        /// </summary>
        /// <value></value>
        /// <returns>Always returns <c>false</c>.</returns>
        public bool IsValid
        {
            get { return false; }
            set { }
        }

        /// <summary>
        /// Empty implementation of method required by abstract base class
        /// </summary>
        public void Validate()
        { }

    }
}
