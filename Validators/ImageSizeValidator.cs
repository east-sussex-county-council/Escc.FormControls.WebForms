using System;
using System.Drawing;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using eastsussexgovuk.webservices.FormControls.Validators;

namespace EsccWebTeam.FormControls.Validators
{
    /// <summary>
    /// Validates that an image being uploaded is less than a given size in pixels
    /// </summary>
    public class ImageSizeValidator : EsccCustomValidator
    {
        #region Public properties

        /// <summary>
        /// Gets or sets the maximum width in pixels. Use 0 for unlimited.
        /// </summary>
        /// <value>The maximum width.</value>
        public int MaximumWidth { get; set; }

        /// <summary>
        /// Gets or sets the maximum height in pixels. Use 0 for unlimited.
        /// </summary>
        /// <value>The maximum height.</value>
        public int MaximumHeight { get; set; }

        /// <summary>
        /// Gets or sets the file size above which the image is not checked and the validator silently passes.
        /// </summary>
        /// <value>The maximum bytes.</value>
        public int MaximumBytes { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSizeValidator"/> class.
        /// </summary>
        public ImageSizeValidator() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ImageSizeValidator"/> class.
        /// </summary>
        /// <param name="controlToValidateId">The id of the control to validate.</param>
        /// <param name="errorMessage">The error message.</param>
        public ImageSizeValidator(string controlToValidateId, string errorMessage)
            : base(controlToValidateId, errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ImageSizeValidator"/> class.
        /// </summary>
        /// <param name="controlToValidateId">The id of the control to validate.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="maximumWidth">The maximum width in pixels. Use 0 for unlimited.</param>
        /// <param name="maximumHeight">The maximum height in pixels. Use 0 for unlimited.</param>
        public ImageSizeValidator(string controlToValidateId, string errorMessage, int maximumWidth, int maximumHeight)
            : base(controlToValidateId, errorMessage)
        {
            this.MaximumWidth = maximumWidth;
            this.MaximumHeight = maximumHeight;
        }

        #endregion Constructors

        #region Validation

        /// <summary>
        /// Overrides the <see cref="M:System.Web.UI.BaseValidator.EvaluateIsValid"></see> method.
        /// </summary>
        /// <returns>
        /// true if the value in the input control is valid; otherwise, false.
        /// </returns>
        protected override bool EvaluateIsValid()
        {
            // Get control to validate
            HtmlInputFile inputToValidate = this.Parent.FindControl(this.ControlToValidate) as HtmlInputFile;
            FileUpload uploadToValidate = this.Parent.FindControl(this.ControlToValidate) as FileUpload;

            // Get the uploaded file
            HttpPostedFile postedFile;
            if (inputToValidate != null) postedFile = inputToValidate.PostedFile;
            else if (uploadToValidate != null) postedFile = uploadToValidate.PostedFile;
            // If the control doesn't exist, that's an error
            else return false;

            // If no file was uploaded that's not an error (this isn't a RequiredFieldValidator)
            if (postedFile == null || postedFile.ContentLength == 0) return true;

            // If file too big that's not an error (this isn't an UploadSizeValidator)
            // but it is a reason not to check the image size, because it might generate an OutOfMemoryException
            if (this.MaximumBytes > 0 && postedFile.ContentLength > this.MaximumBytes) return true;

            // If max pixel size, check it
            if (this.MaximumWidth > 0 || this.MaximumHeight > 0)
            {
                Bitmap image;
                try
                {
                    image = new Bitmap(postedFile.InputStream);
                }
                catch (ArgumentException)
                {
                    // Not an image, but this validator isn't checking for that.
                    // No positive match for an image above the maximum size, so return true.
                    // 
                    // (If we were to return false you'd get a message saying "Your image is too big" 
                    //  when someone uploads a Word doc, and that doesn't make sense.)
                    return true;
                }

                // Check width if max specified
                if (this.MaximumWidth > 0)
                {
                    bool widthValid = (this.MaximumWidth >= image.Width);
                    if (!widthValid) return false;
                }

                // Check height if max specified
                if (this.MaximumHeight > 0)
                {
                    bool heightValid = (this.MaximumHeight >= image.Height);
                    if (!heightValid) return false;
                }
            }

            return true;
        }

        #endregion Validation

    }

}
