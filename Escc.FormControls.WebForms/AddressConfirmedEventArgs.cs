using System;

namespace Escc.FormControls.WebForms
{
    /// <summary>
    /// Custom Event Arguments for AddressConfirmed event.
    /// </summary>
    public class AddressConfirmedEventArgs : EventArgs
    {
        #region Fields

        /// <summary>
        /// Store submitted oid
        /// </summary>
        private string oid;
        #endregion
        #region properties
        /// <summary>
        /// Public property.
        /// </summary>
        public string Oid
        {
            get
            {
                return oid;
            }
            set
            {
                oid = value;
            }
        }
        #endregion
        #region constructors
        /// <summary>
        /// Event arguments for <c>AddressConfirmed</c> event
        /// </summary>
        /// <param name="oid"></param>
        public AddressConfirmedEventArgs(string oid)
        {
            this.Oid = oid;
        }
        #endregion
    }
}