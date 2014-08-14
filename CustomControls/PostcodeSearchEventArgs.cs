using System;

namespace EsccWebTeam.FormControls.CustomControls
{
    /// <summary>
    /// Event arguments for <c>PostcodeSearchControl.PostcodeSubmitted</c> event, allowing a postcode and house name/number to be passed
    /// </summary>
    public class PostcodeSearchEventArgs : EventArgs
    {
        #region Fields
        /// <summary>
        /// Store submitted postcode
        /// </summary>
        private string postcode;
        #endregion
        #region properties
        /// <summary>
        /// Public property.
        /// </summary>
        public string Postcode
        {
            get
            {
                return postcode;
            }
            set
            {
                postcode = value;
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// Event arguments for <c>PostcodeSearchControl.PostcodeSubmitted</c> event
        /// </summary>
        /// <param name="postcode">The validated postcode</param>
        public PostcodeSearchEventArgs(string postcode)
        {

            this.postcode = postcode;
        }
        #endregion
    }
}