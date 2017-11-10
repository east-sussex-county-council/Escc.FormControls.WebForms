using System;
using System.Configuration;
using System.Net;
using System.Text;
using System.Web;
using Exceptionless;
using Newtonsoft.Json;

namespace Escc.FormControls.WebForms.Validators
{
    /// <summary>
    /// Validates the token submitted as part of a bot check by the invisible version of Google Recaptcha
    /// </summary>
    /// <seealso cref="Escc.FormControls.WebForms.Validators.EsccCustomValidator" />
    public class RecaptchaValidator : EsccCustomValidator
    {
        /// <summary>
        /// Overrides the <see cref="M:System.Web.UI.MobileControls.BaseValidator.EvaluateIsValid" /> method.
        /// </summary>
        /// <returns>
        /// true if the value in the input control is valid; otherwise, false.
        /// </returns>
        protected override bool EvaluateIsValid()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("secret", ConfigurationManager.AppSettings["GoogleRecaptchaSecretKey"]);
                    reqparm.Add("response", HttpContext.Current.Request.Form["g-recaptcha-response"]);
                    byte[] responsebytes = client.UploadValues("https://www.google.com/recaptcha/api/siteverify", "POST", reqparm);
                    string responsebody = Encoding.UTF8.GetString(responsebytes);
                    var result = JsonConvert.DeserializeObject<RecaptchaResult>(responsebody);
                    return result.success;
                }
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return true;
            }
        }

        /// <summary>
        /// Results returned by Google's Recaptcha API
        /// </summary>
        private class RecaptchaResult
        {
            public bool success { get; set; }
            public DateTime challenge_ts { get; set; }
            public string hostname { get; set; }
            public string[] errorcodes { get; set; }
        }
    }
}