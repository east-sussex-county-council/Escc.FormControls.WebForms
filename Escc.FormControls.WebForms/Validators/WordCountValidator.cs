using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Escc.Html;

namespace Escc.FormControls.WebForms.Validators
{
    /// <summary>
    /// Checks that the number of words in a <see cref="System.Web.UI.WebControls.TextBox"/> is within a specified range
    /// </summary>
    public class WordCountValidator : EsccCustomValidator
    {

        #region Fields

        private int minimumWords = 0;
        private int maximumWords = 100;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the maximum word count.
        /// </summary>
        /// <value>The maximum word count.</value>
        public int MaximumWords
        {
            get
            {
                return this.maximumWords;
            }
            set
            {
                this.maximumWords = value;
            }
        }


        /// <summary>
        /// Gets or sets the minimum word count.
        /// </summary>
        /// <value>The minimum word count.</value>
        public int MinimumWords
        {
            get
            {
                return this.minimumWords;
            }
            set
            {
                this.minimumWords = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the property containing the string to validate
        /// </summary>
        public string PropertyToValidate { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WordCountValidator"/> class.
        /// </summary>
        public WordCountValidator()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WordCountValidator"/> class.
        /// </summary>
        /// <param name="controlToValidateId">The id of the control to validate.</param>
        /// <param name="errorMessage">The error message.</param>
        public WordCountValidator(string controlToValidateId, string errorMessage)
            : base()
        {
            this.ControlToValidate = controlToValidateId;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WordCountValidator"/> class.
        /// </summary>
        /// <param name="controlToValidateId">The id of the control to validate.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="minimumWords">The minimum word count.</param>
        /// <param name="maximumWords">The maximum word count.</param>
        public WordCountValidator(string controlToValidateId, string errorMessage, int minimumWords, int maximumWords)
            : base()
        {
            this.ControlToValidate = controlToValidateId;
            this.ErrorMessage = errorMessage;
            this.minimumWords = minimumWords;
            this.maximumWords = maximumWords;
        }
        #endregion Constructors

        #region Validation

        /// <summary>
        /// Checks that the number of words in a <see cref="System.Web.UI.WebControls.TextBox"/> is within a specified range
        /// </summary>
        /// <returns><c>true</c> if valid; otherwise <c>false</c></returns>
        protected override bool EvaluateIsValid()
        {
            string textToValidate = String.Empty;
            var control = this.Parent.FindControl(this.ControlToValidate);
            if (String.IsNullOrEmpty(PropertyToValidate))
            {
                TextBox textboxToValidate = control as TextBox;
                textToValidate = textboxToValidate.Text.Trim();
            }
            else
            {
                textToValidate = control.GetType().GetProperty(PropertyToValidate).GetValue(control, null) as string;
            }

            textToValidate = new HtmlTagSantiser().StripTags(textToValidate);

            // get rid of line breaks
            textToValidate = Regex.Replace(textToValidate, "\r", " ");
            textToValidate = Regex.Replace(textToValidate, "\n", " ");

            // get rid of multiple spaces
            while (textToValidate.IndexOf("  ") > -1) textToValidate = textToValidate.Replace("  ", " ");

            // split into words
            string[] words = textToValidate.Split(' ');

            // validate word count
            return ((words.Length >= this.minimumWords) && (words.Length <= this.maximumWords));
        }

        #endregion Validation

    }
}
