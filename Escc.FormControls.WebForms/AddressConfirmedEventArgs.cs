using System;

namespace Escc.FormControls.WebForms
{
    /// <summary>
    /// Custom Event Arguments for AddressConfirmed event.
    /// </summary>
    public class AddressConfirmedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the Unique Property Reference Number.
        /// </summary>
        /// <value>
        /// The uprn.
        /// </value>
        public string Uprn { get; set; }
        
        /// <summary>
        /// Event arguments for <c>AddressConfirmed</c> event
        /// </summary>
        /// <param name="uprn"></param>
        public AddressConfirmedEventArgs(string uprn)
        {
            this.Uprn = uprn;
        }
    }
}