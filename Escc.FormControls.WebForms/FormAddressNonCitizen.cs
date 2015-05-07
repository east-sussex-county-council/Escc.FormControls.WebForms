using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Escc.AddressAndPersonalDetails;
using Escc.FormControls.WebForms.Properties;
using Escc.FormControls.WebForms.Validators;

namespace Escc.FormControls.WebForms
{
    /// <summary>
    /// Summary description for FormAddressNonCitizen.
    /// </summary>
    [DefaultProperty("QuestionID"), ToolboxData("<{0}:FormAddressNonCitizen runat=server></{0}:FormAddressNonCitizen>")]
    public class FormAddressNonCitizen : WebControl, INamingContainer
    {
        #region private fields
        private int questionID;
        private Label lblMessage;
        private Label lblAddress;
        private TextBox tbxSaon;
        private TextBox tbxPaon;
        private TextBox tbxStreetDescriptor;
        private TextBox tbxLocality;
        private TextBox tbxTown;
        private TextBox tbxAdministrativeArea;
        private TextBox tbxPostcode;
        private HtmlInputHidden hidEasting;
        private HtmlInputHidden hidNorthing;
        private string saon;
        private string paon;
        private string streetDescriptor;
        private string locality;
        private string town;
        private string administrativeArea;
        private string postcode;
        private string easting;
        private string northing;
        private string oa;
        private string oid;
        private bool pafCheckValid;
        private EsccButton imgbtnConfirmAddress;
        private ListBox lbxAddressChoices;
        private EsccButton imgbtnFind;
        private LiteralControl lc2a;
        private LiteralControl lc2b;
        private LiteralControl lcfp1a;
        private LiteralControl lcfp1b;
        private LiteralControl lcfp2a;
        private LiteralControl lcfp2b;
        private LiteralControl lcfp3a;
        private LiteralControl lcfp3b;
        private LiteralControl lcfp4a;
        private LiteralControl lcfp4b;
        private LiteralControl lcfp5a;
        private LiteralControl lcfp5b;
        private LiteralControl lcfp6a;
        private LiteralControl lcfp6b;
        private LiteralControl lcfp7a;
        private LiteralControl lcfp7b;
        private LiteralControl lcpca;
        private LiteralControl lcpcb;
        private bool isRequired = false;
        private EsccButton imbAddressToggle;
        private bool toggleStatus;
        private string toggleStatusSessionName;
        private HtmlInputHidden inpGuid;
        private LiteralControl orText;
        private bool enableValidaton = true;
        private string messageAddressRequired = string.Empty;
        private string messageFullAddressRequired = string.Empty;
        private string messageSaonLength;
        private string messagePaonLength;
        private string messageStreetDescriptorLength;
        private string messageLocalityLength;
        private string messageTownLength;
        private string messageAdministrativeAreaLength;
        private string messageSaonFormat;
        private string messagePaonFormat;
        private string messageStreetDescriptorFormat;
        private string messageLocalityFormat;
        private string messageTownFormat;
        private string messageAdministrativeAreaFormat;
        private int maxLengthSaon = 100;
        private int maxLengthPaon = 100;
        private int maxLengthStreetDescriptor = 100;
        private int maxLengthLocality = 35;
        private int maxLengthTown = 30;
        private int maxLengthAdministrativeArea = 30;
        private int maxLengthPostcode = 8;
        private string textIntro;
        private PlaceHolder selectAddress;
        private PlaceHolder addressFields;

        // The first four labels are hidden by default by using css class 'aural'.
        // Czone.EventsCalendar requires to override the labels so that they are seen.
        private Label labelSaon;
        private Label labelPaon;
        private Label labelStreetDescriptor;
        private Label labelLocality;
        private string resourceFileName = typeof(EsccWebTeam_FormControls).Name;
        #endregion

        #region Protected properties and methods
        /// <summary>
        /// Gets or sets the TextBox control used for editing the postcode
        /// </summary>
        public TextBox TbxPostcode
        {
            get { return tbxPostcode; }
            set { tbxPostcode = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether address fields are displayed.
        /// </summary>
        /// <value><c>true</c> if displayed; otherwise, <c>false</c>.</value>
        protected bool ToggleStatus
        {
            get { return this.toggleStatus; }
            set { this.toggleStatus = value; }
        }

        /// <summary>
        /// Helper method toggles visibility of address fields excl postcode
        /// </summary>
        protected void ToggleAddressFields(bool state)
        {
            this.EnsureChildControls();

            this.addressFields.Visible = state;

            HttpContext.Current.Session[toggleStatusSessionName] = state;

            this.ShowToggleButton(!state);
        }

        /// <summary>
        /// Shows or hides the toggle button.
        /// </summary>
        /// <param name="state">if set to <c>true</c> shows the toggle button; otherwise hides it.</param>
        protected void ShowToggleButton(bool state)
        {
            if (this.imbAddressToggle != null) this.imbAddressToggle.Visible = state;
            if (this.orText != null) this.orText.Visible = state;
        }

        #endregion

        #region public properties
        /// <summary>
        /// Gets or sets the question ID.
        /// </summary>
        /// <value>The question ID.</value>
        /// <remarks>Setting a question id enables to have two address controls on the same page which behave independently</remarks>
        [Bindable(true),
        Category("Misc"),
        DefaultValue(0)]
        public int QuestionID
        {
            get
            {
                return questionID;
            }
            set
            {
                questionID = value;
            }
        }

        /// <summary>
        /// Gets the easting coordinate of the selected address
        /// </summary>
        public string Easting
        {
            get
            {
                return easting;
            }
            set
            {
                easting = value;
            }
        }

        /// <summary>
        /// Gets the northing coordinate of the selected address
        /// </summary>
        public string Northing
        {
            get
            {
                return northing;
            }
            set
            {
                northing = value;
            }
        }

        public string OA
        {
            get
            {
                return oa;
            }
        }
        public string OID
        {
            get
            {
                return oid;
            }
        }

        /// <summary>
        /// Gets or sets whether the selected address has been validated against the Post Office Address file (PAF)
        /// </summary>
        public bool PafCheckValid
        {
            get
            {
                return pafCheckValid;
            }
            set
            {
                pafCheckValid = value;
            }
        }

        /// <summary>
        /// Gets the secondary addressable object name of the selected address
        /// </summary>
        public string Saon
        {
            get
            {
                return saon;
            }
        }

        /// <summary>
        /// Gets the primary addressable object name of the selected address
        /// </summary>
        public string Paon
        {
            get
            {
                return paon;
            }
        }

        /// <summary>
        /// Gets the street descriptor part of the selected address
        /// </summary>
        public string StreetDescriptor
        {
            get
            {
                return streetDescriptor;
            }
        }

        /// <summary>
        /// Gets the locality part of the selected address
        /// </summary>
        public string Locality
        {
            get
            {
                return this.locality;
            }
        }

        /// <summary>
        /// Gets the town part of the selected address
        /// </summary>
        public string Town
        {
            get
            {
                return town;
            }
        }

        /// <summary>
        /// Gets the administrative area part of the selected address
        /// </summary>
        public string AdministrativeArea
        {
            get
            {
                return administrativeArea;
            }
        }

        /// <summary>
        /// Gets the postcode from the selected address
        /// </summary>
        public string Postcode
        {
            get
            {
                return postcode;
            }
        }

        /// <summary>
        /// Gets or sets whether an address must be entered for this control to be valid
        /// </summary>
        public bool IsRequired
        {
            get
            {
                return this.isRequired;
            }
            set
            {
                this.isRequired = value;
            }
        }


        /// <summary>
        /// Gets or sets the selected address.
        /// </summary>
        /// <value>The selected address.</value>
        public BS7666Address BS7666Address
        {
            get
            {
                this.UpdateFields();
                BS7666Address bs7666Address = new BS7666Address(this.Paon, this.Saon, this.StreetDescriptor, this.Locality, this.Town, this.AdministrativeArea, this.Postcode);
                try
                {
                    bs7666Address.GridEasting = Convert.ToInt32(easting);
                    bs7666Address.GridNorthing = Convert.ToInt32(northing);
                }
                catch
                {
                    // We don't need to do anything here. We just need to cater for Easting and Northing strings being empty.
                }
                return bs7666Address;
            }
            set
            {
                if (value != null)
                {
                    this.paon = value.Paon;
                    this.saon = value.Saon;
                    this.streetDescriptor = value.StreetName;
                    this.locality = value.Locality;
                    this.town = value.Town;
                    this.administrativeArea = value.AdministrativeArea;
                    this.postcode = value.Postcode;
                    this.UpdateTextboxes();
                }
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FormAddressNonCitizen"/> class.
        /// </summary>
        /// <param name="formQuestionID">The form question ID.</param>
        public FormAddressNonCitizen(int formQuestionID)
            : base(HtmlTextWriterTag.Div)
        {
            questionID = formQuestionID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormAddressNonCitizen"/> class.
        /// </summary>
        public FormAddressNonCitizen()
            : base(HtmlTextWriterTag.Div)
        {

        }
        #endregion

        #region init

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/>
        /// event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {

            //Allow default messages to be bypased by eforms system setting them
            if (this.messageAddressRequired.Length == 0)
            {
                this.messageAddressRequired = LocalisedResourceReader.ResourceString(resourceFileName, "ErrorAddressBlank", EsccWebTeam_FormControls.ErrorAddressBlank);
            }
            if (this.messageFullAddressRequired.Length == 0)
            {
                this.messageFullAddressRequired = LocalisedResourceReader.ResourceString(resourceFileName, "ErrorAddressPostcodeOnly", EsccWebTeam_FormControls.ErrorAddressPostcodeOnly);
            }
            // Set up default messages
            //this.messageAddressRequired = TextUtilities.ResourceString(resourceFileName, "", EsccWebTeam_FormControls.ErrorAddressBlank;
            //this.messageFullAddressRequired = TextUtilities.ResourceString(resourceFileName, "", EsccWebTeam_FormControls.ErrorAddressPostcodeOnly;
            this.textIntro = LocalisedResourceReader.ResourceString(resourceFileName, "FindAddressIntro", LocalisedResourceReader.ResourceString(resourceFileName, "FindAddressIntro", EsccWebTeam_FormControls.FindAddressIntro));

            // Length
            this.messageSaonLength = LocalisedResourceReader.ResourceString(resourceFileName, "ErrorSaonLength", EsccWebTeam_FormControls.ErrorSaonLength);
            this.messagePaonLength = LocalisedResourceReader.ResourceString(resourceFileName, "ErrorPaonLength", EsccWebTeam_FormControls.ErrorPaonLength);
            this.messageStreetDescriptorLength = LocalisedResourceReader.ResourceString(resourceFileName, "ErrorStreetDescriptorLength", EsccWebTeam_FormControls.ErrorStreetDescriptorLength);
            this.messageLocalityLength = LocalisedResourceReader.ResourceString(resourceFileName, "ErrorLocalityLength", EsccWebTeam_FormControls.ErrorLocalityLength);
            this.messageTownLength = LocalisedResourceReader.ResourceString(resourceFileName, "ErrorTownLength", EsccWebTeam_FormControls.ErrorTownLength);
            this.messageAdministrativeAreaLength = LocalisedResourceReader.ResourceString(resourceFileName, "ErrorAdministrativeAreaLength", EsccWebTeam_FormControls.ErrorAdministrativeAreaLength);

            // Format
            this.messageSaonFormat = LocalisedResourceReader.ResourceString(resourceFileName, "ErrorSaonFormat", EsccWebTeam_FormControls.ErrorSaonFormat);
            this.messagePaonFormat = LocalisedResourceReader.ResourceString(resourceFileName, "ErrorPaonFormat", EsccWebTeam_FormControls.ErrorPaonFormat);
            this.messageStreetDescriptorFormat = LocalisedResourceReader.ResourceString(resourceFileName, "ErrorStreetDescriptorFormat", EsccWebTeam_FormControls.ErrorStreetDescriptorFormat);
            this.messageLocalityFormat = LocalisedResourceReader.ResourceString(resourceFileName, "ErrorLocalityFormat", EsccWebTeam_FormControls.ErrorLocalityFormat);
            this.messageTownFormat = LocalisedResourceReader.ResourceString(resourceFileName, "ErrorTownFormat", EsccWebTeam_FormControls.ErrorTownFormat);
            this.messageAdministrativeAreaFormat = LocalisedResourceReader.ResourceString(resourceFileName, "ErrorAdministrativeAreaFormat", EsccWebTeam_FormControls.ErrorAdministrativeAreaFormat);


            base.OnInit(e);
            this.EnableViewState = false;

            this.PostcodeSubmitted += new PostcodeSearchEventHandler(Message_PostcodeSubmitted);
            this.AddressConfirmed += new AddressConfirmedEventHandler(FormAddress_AddressConfirmed);

            // Create textboxes now, so that text can be set as soon as control is instantiated
            tbxSaon = new TextBox();
            tbxSaon.ID = "saon";
            tbxSaon.MaxLength = this.maxLengthSaon;

            tbxPaon = new TextBox();
            tbxPaon.ID = "paon";
            tbxPaon.MaxLength = this.maxLengthPaon;

            tbxStreetDescriptor = new TextBox();
            tbxStreetDescriptor.ID = "streetDescriptor";
            tbxStreetDescriptor.MaxLength = this.maxLengthStreetDescriptor;

            tbxLocality = new TextBox();
            tbxLocality.ID = "locality";
            tbxLocality.MaxLength = this.maxLengthLocality;

            tbxTown = new TextBox();
            tbxTown.ID = "town";
            tbxTown.MaxLength = this.maxLengthTown;

            tbxAdministrativeArea = new TextBox();
            tbxAdministrativeArea.ID = "administrativeArea";
            tbxAdministrativeArea.MaxLength = this.maxLengthAdministrativeArea;

            tbxPostcode = new TextBox();
            tbxPostcode.ID = "postcode";
            tbxPostcode.MaxLength = this.maxLengthPostcode;
            tbxPostcode.CssClass = "postcode";

            //EXTEND for Eastings and Northings

            hidEasting = new HtmlInputHidden();
            hidEasting.ID = "easting";
            hidNorthing = new HtmlInputHidden();
            hidNorthing.ID = "northing";

            // Restore posted values
            if (this.Page.IsPostBack)
            {
                string separator = ":"; // .NET 1.1 uses a colon
                if (this.Context.Request.Form[this.UniqueID + separator + this.tbxSaon.UniqueID] != null)
                {
                    // .NET 1.1, Address fields posted
                    this.tbxSaon.Text = this.Context.Request.Form[this.UniqueID + separator + this.tbxSaon.UniqueID];
                    this.tbxPaon.Text = this.Context.Request.Form[this.UniqueID + separator + this.tbxPaon.UniqueID];
                    this.tbxStreetDescriptor.Text = this.Context.Request.Form[this.UniqueID + separator + this.tbxStreetDescriptor.UniqueID];
                    this.tbxLocality.Text = this.Context.Request.Form[this.UniqueID + separator + this.tbxLocality.UniqueID];
                    this.tbxTown.Text = this.Context.Request.Form[this.UniqueID + separator + this.tbxTown.UniqueID];
                    this.tbxAdministrativeArea.Text = this.Context.Request.Form[this.UniqueID + separator + this.tbxAdministrativeArea.UniqueID];
                    this.UpdateFields();
                }
                else
                {
                    separator = "$"; // .NET 2 uses a dollar
                    if (this.Context.Request.Form[this.UniqueID + separator + this.tbxSaon.UniqueID] != null)
                    {
                        // .NET 2, Address fields posted
                        this.tbxSaon.Text = this.Context.Request.Form[this.UniqueID + separator + this.tbxSaon.UniqueID];
                        this.tbxPaon.Text = this.Context.Request.Form[this.UniqueID + separator + this.tbxPaon.UniqueID];
                        this.tbxStreetDescriptor.Text = this.Context.Request.Form[this.UniqueID + separator + this.tbxStreetDescriptor.UniqueID];
                        this.tbxLocality.Text = this.Context.Request.Form[this.UniqueID + separator + this.tbxLocality.UniqueID];
                        this.tbxTown.Text = this.Context.Request.Form[this.UniqueID + separator + this.tbxTown.UniqueID];
                        this.tbxAdministrativeArea.Text = this.Context.Request.Form[this.UniqueID + separator + this.tbxAdministrativeArea.UniqueID];
                        this.hidEasting.Value = this.Context.Request.Form[this.UniqueID + separator + this.hidEasting.UniqueID];
                        this.hidNorthing.Value = this.Context.Request.Form[this.UniqueID + separator + this.hidNorthing.UniqueID];
                        this.UpdateFields();
                    }
                }
            }

        }
        #endregion

        #region public methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void imbAddressToggle_Click(Object sender, EventArgs e)
        {
            bool state = true;

            // if fields are not visible make them so
            if (!toggleStatus)
            {
                ToggleAddressFields(true);
            }

            // if fields are visible and are empty toggle visibility off
            if ((toggleStatus) & (this.tbxAdministrativeArea.Text.Length == 0) &
                (this.tbxLocality.Text.Length == 0) & (this.tbxPaon.Text.Length == 0) & (this.tbxPostcode.Text.Length == 0)
                & (this.tbxSaon.Text.Length == 0) & (this.tbxStreetDescriptor.Text.Length == 0) & (this.tbxTown.Text.Length == 0))
            {
                ToggleAddressFields(false);
                state = false;
            }

            HttpContext.Current.Session[toggleStatusSessionName] = state;
        }

        /// <summary>
        /// Allows the first four address labels to be seen and set to different text.
        /// </summary>
        /// <param name="SaonLabelText"></param>
        /// <param name="PaonLabelText"></param>
        /// <param name="StreetDescriptorLabelText"></param>
        /// <param name="LocalityLabelText"></param>
        public void showAddressLineLabelling(
            string SaonLabelText,
            string PaonLabelText,
            string StreetDescriptorLabelText,
            string LocalityLabelText)
        {
            if (!string.IsNullOrEmpty(SaonLabelText))
            {
                this.labelSaon.CssClass = "formLabel";
                this.labelSaon.Text = SaonLabelText;
            }

            if (!string.IsNullOrEmpty(PaonLabelText))
            {
                this.labelPaon.CssClass = "formLabel";
                this.labelPaon.Text = PaonLabelText;
            }

            if (!string.IsNullOrEmpty(StreetDescriptorLabelText))
            {
                this.labelStreetDescriptor.CssClass = "formLabel";
                this.labelStreetDescriptor.Text = StreetDescriptorLabelText;
            }

            if (!string.IsNullOrEmpty(LocalityLabelText))
            {
                this.labelLocality.CssClass = "formLabel";
                this.labelLocality.Text = LocalityLabelText;
            }
        }
        #endregion

        #region private methods

        /// <summary>
        /// Formats addresses for display in a list box.
        /// </summary>
        /// <param name="ds">A DataSet containing paf addresses.</param>
        private void FormatDataSet(DataSet ds)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                StringBuilder sb = new StringBuilder();

                if (dr["BD"] != null & dr["BD"].ToString() != "")
                {
                    sb.Append(dr["BD"]).ToString();
                }

                if (dr["BN"] != null & dr["BN"].ToString() != "" & dr["BN"].ToString() != "0")
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(dr["BN"]).ToString();
                }

                if (dr["DL"] != null & dr["DL"].ToString() != "")
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(dr["DL"]).ToString();
                }

                if (dr["TN"] != null & dr["TN"].ToString() != "")
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(dr["TN"]).ToString();
                }

                if (dr["PT"] != null & dr["PT"].ToString() != "")
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(dr["PT"]).ToString();
                }

                if (dr["PC"] != null & dr["PC"].ToString() != "")
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(dr["PC"]).ToString();
                }
                ListItem item = new ListItem(sb.ToString(), dr["OID"].ToString());
                lbxAddressChoices.Items.Add(item);
            }
        }


        /// <summary>
        /// Updates the textboxes from the internal fields which hold the address.
        /// </summary>
        private void UpdateTextboxes()
        {
            // TODO: Is there any point to storing vars like this? Wouldn't it be better to use the TextBox.Text property as 
            // the only internal source of the data and avoid synchronisation issues. - RM July 06

            this.tbxSaon.Text = this.saon;
            this.tbxPaon.Text = this.paon;
            this.tbxStreetDescriptor.Text = this.streetDescriptor;
            this.tbxLocality.Text = this.locality;
            this.tbxTown.Text = this.town;
            this.tbxAdministrativeArea.Text = this.administrativeArea;
            this.tbxPostcode.Text = this.postcode;
            this.hidEasting.Value = this.easting;
            this.hidNorthing.Value = this.northing;


            bool addressPresent = (
                (this.tbxSaon.Text.Length > 0) ||
                (this.tbxPaon.Text.Length > 0) ||
                (this.tbxStreetDescriptor.Text.Length > 0) ||
                (this.tbxLocality.Text.Length > 0) ||
                (this.tbxTown.Text.Length > 0) ||
                (this.tbxAdministrativeArea.Text.Length > 0) ||
                (this.tbxPostcode.Text.Length > 0)
                );

            this.ToggleAddressFields(addressPresent);
        }

        /// <summary>
        /// Updates the internal fields which hold the address from the textboxes.
        /// </summary>
        private void UpdateFields()
        {
            this.saon = this.tbxSaon.Text;
            this.paon = this.tbxPaon.Text;
            this.streetDescriptor = this.tbxStreetDescriptor.Text;
            this.locality = this.tbxLocality.Text;
            this.town = this.tbxTown.Text;
            this.administrativeArea = this.tbxAdministrativeArea.Text;
            this.postcode = this.tbxPostcode.Text;
            this.easting = this.hidEasting.Value;
            this.northing = this.hidNorthing.Value;
        }

        #endregion

        #region overridden methods
        /// <summary>
        /// Override creates the form address controls.
        /// </summary>
        protected override void CreateChildControls()
        {
            lc2a = new LiteralControl("<div class=\"formPart\">");
            lc2b = new LiteralControl("</div>");

            lcfp1a = new LiteralControl("<div class=\"formPart\">");
            lcfp1b = new LiteralControl("</div>");

            lcfp2a = new LiteralControl("<div class=\"formPart\">");
            lcfp2b = new LiteralControl("</div>");

            lcfp3a = new LiteralControl("<div class=\"formPart\">");
            lcfp3b = new LiteralControl("</div>");

            lcfp4a = new LiteralControl("<div class=\"formPart\">");
            lcfp4b = new LiteralControl("</div>");

            lcfp5a = new LiteralControl("<div class=\"formPart\">");
            lcfp5b = new LiteralControl("</div>");

            lcfp6a = new LiteralControl("<div class=\"formPart\">");
            lcfp6b = new LiteralControl("</div>");

            lcfp7a = new LiteralControl("<div class=\"formPartAddress\">");
            lcfp7b = new LiteralControl("</div>");

            lcpca = new LiteralControl("<div class=\"formPart\">");
            lcpcb = new LiteralControl("</div>");



            lblMessage = new Label();
            lblAddress = new Label();
            imgbtnConfirmAddress = new EsccButton();
            lbxAddressChoices = new ListBox();
            imgbtnFind = new EsccButton();

            imbAddressToggle = new EsccButton();

            inpGuid = new HtmlInputHidden();



            if (this.textIntro != null && this.textIntro.Length > 0)
            {
                HtmlGenericControl pcDesc = new HtmlGenericControl("p");
                pcDesc.Attributes["class"] = "pcDesc";
                pcDesc.InnerText = this.textIntro;
                this.Controls.Add(pcDesc);
            }


            //			lblAddress.ID = "lblAddress";
            //			lblAddress.Text = "Address";
            //			lblAddress.CssClass="formLabel";
            //			this.Controls.Add(lblAddress);


            // Add address fields
            this.addressFields = new PlaceHolder();
            this.Controls.Add(this.addressFields);

            // Add SAON field
            this.labelSaon = new Label();
            this.labelSaon.Text = LocalisedResourceReader.ResourceString(this.resourceFileName, "SaonLabel", EsccWebTeam_FormControls.SaonLabel);
            this.labelSaon.CssClass = "aural";
            tbxSaon.CssClass = "saon";
            this.addressFields.Controls.Add(lcfp1a);
            this.addressFields.Controls.Add(this.labelSaon);
            this.addressFields.Controls.Add(tbxSaon);
            this.addressFields.Controls.Add(lcfp1b);
            this.labelSaon.AssociatedControlID = tbxSaon.ID;

            // Add PAON field
            this.labelPaon = new Label();
            this.labelPaon.Text = LocalisedResourceReader.ResourceString(this.resourceFileName, "PaonLabel", EsccWebTeam_FormControls.PaonLabel);
            this.labelPaon.CssClass = "aural";
            tbxPaon.CssClass = "paon";
            this.addressFields.Controls.Add(lcfp2a);
            this.addressFields.Controls.Add(this.labelPaon);
            this.addressFields.Controls.Add(tbxPaon);
            this.addressFields.Controls.Add(lcfp2b);
            this.labelPaon.AssociatedControlID = tbxPaon.ID;

            // Add street descriptor field
            this.labelStreetDescriptor = new Label();
            this.labelStreetDescriptor.Text = LocalisedResourceReader.ResourceString(this.resourceFileName, "StreetLabel", EsccWebTeam_FormControls.StreetLabel);
            this.labelStreetDescriptor.CssClass = "aural";
            tbxStreetDescriptor.CssClass = "street";
            this.addressFields.Controls.Add(lcfp3a);
            this.addressFields.Controls.Add(this.labelStreetDescriptor);
            this.addressFields.Controls.Add(tbxStreetDescriptor);
            this.addressFields.Controls.Add(lcfp3b);
            this.labelStreetDescriptor.AssociatedControlID = tbxStreetDescriptor.ID;

            // Add locality field
            this.labelLocality = new Label();
            this.labelLocality.Text = LocalisedResourceReader.ResourceString(this.resourceFileName, "LocalityLabel", EsccWebTeam_FormControls.LocalityLabel);
            this.labelLocality.CssClass = "aural";
            tbxLocality.CssClass = "locality";
            this.addressFields.Controls.Add(lcfp4a);
            this.addressFields.Controls.Add(this.labelLocality);
            this.addressFields.Controls.Add(tbxLocality);
            this.addressFields.Controls.Add(lcfp4b);
            this.labelLocality.AssociatedControlID = tbxLocality.ID;

            // Add town field
            Label townLabel = new Label();
            townLabel.Text = LocalisedResourceReader.ResourceString(this.resourceFileName, "TownLabel", EsccWebTeam_FormControls.TownLabel);
            townLabel.CssClass = "formLabel";
            tbxTown.CssClass = "town";
            this.addressFields.Controls.Add(lcfp5a);
            this.addressFields.Controls.Add(townLabel);
            this.addressFields.Controls.Add(tbxTown);
            this.addressFields.Controls.Add(lcfp5b);
            townLabel.AssociatedControlID = tbxTown.ID;

            // Add administrative area field
            Label adminLabel = new Label();
            adminLabel.Text = LocalisedResourceReader.ResourceString(this.resourceFileName, "AdministrativeAreaLabel", EsccWebTeam_FormControls.AdministrativeAreaLabel);
            adminLabel.CssClass = "formLabel";
            tbxAdministrativeArea.CssClass = "administrative-area";
            this.addressFields.Controls.Add(lcfp6a);
            this.addressFields.Controls.Add(adminLabel);
            this.addressFields.Controls.Add(tbxAdministrativeArea);
            this.addressFields.Controls.Add(lcfp6b);
            adminLabel.AssociatedControlID = tbxAdministrativeArea.ID;

            // Add postcode field
            Label postcodeLabel = new Label();
            postcodeLabel.Text = LocalisedResourceReader.ResourceString(this.resourceFileName, "PostcodeLabel", EsccWebTeam_FormControls.PostcodeLabel);
            postcodeLabel.CssClass = "formLabel";
            this.Controls.Add(lcpca);
            this.Controls.Add(postcodeLabel);
            this.Controls.Add(tbxPostcode);
            this.Controls.Add(lcpcb);
            postcodeLabel.AssociatedControlID = tbxPostcode.ID;


            //EXTEND to hold Eastings and Northings



            this.Controls.Add(hidEasting);
            this.Controls.Add(hidNorthing);

            // Add a dummy textbox because when the address fields are hidden, postcode is the only textbox, 
            // and that means IE won't fire the .NET click event
            TextBox ieBugFix = new TextBox();
            ieBugFix.CssClass = "ieBugFix";
            this.Controls.Add(ieBugFix);

            this.Controls.Add(lcfp7a);




            imgbtnFind.ID = "findAddress"; // ID added to avoid ctl number mismatches on postback - RM 13/10/06
            imgbtnFind.CssClass = LocalisedResourceReader.ResourceString(this.resourceFileName, "ButtonFindAddressClass", EsccWebTeam_FormControls.ButtonFindAddressClass);
            this.Controls.Add(imgbtnFind);
            this.imgbtnFind.CausesValidation = false;
            this.imgbtnFind.Text = LocalisedResourceReader.ResourceString(this.resourceFileName, "ButtonFindAddress", EsccWebTeam_FormControls.ButtonFindAddress);
            this.imgbtnFind.Click += new EventHandler(imgbtnFind_Click);

            this.orText = new LiteralControl("<span class=\"addressButton\">&nbsp;" + LocalisedResourceReader.ResourceString(this.resourceFileName, "FindAddressOr", EsccWebTeam_FormControls.FindAddressOr) + "&nbsp;</span>");
            this.Controls.Add(this.orText);


            this.Controls.Add(imbAddressToggle);
            this.imbAddressToggle.ID = "manualAddress"; // ID added to avoid ctl number mismatches on postback - RM 13/10/06
            this.imbAddressToggle.CssClass = LocalisedResourceReader.ResourceString(this.resourceFileName, "ButtonManualAddressClass", EsccWebTeam_FormControls.ButtonManualAddressClass);
            this.imbAddressToggle.CausesValidation = false;
            this.imbAddressToggle.Text = LocalisedResourceReader.ResourceString(this.resourceFileName, "ManualAddress", EsccWebTeam_FormControls.ManualAddress);
            this.imbAddressToggle.Click += new EventHandler(imbAddressToggle_Click);

            this.Controls.Add(lcfp7b);


            // Add choice of addresses following postcode search 

            this.selectAddress = new PlaceHolder();
            this.selectAddress.Visible = false;
            this.Controls.Add(this.selectAddress);

            lbxAddressChoices.ID = "lbxAddressChoices";
            lbxAddressChoices.CssClass = "listbox";

            Label choiceLabel = new Label();
            choiceLabel.CssClass = "formPart";
            choiceLabel.Text = LocalisedResourceReader.ResourceString(this.resourceFileName, "SelectAddress", EsccWebTeam_FormControls.SelectAddress);
            this.selectAddress.Controls.Add(choiceLabel);

            this.selectAddress.Controls.Add(lc2a);

            this.selectAddress.Controls.Add(lbxAddressChoices);
            choiceLabel.AssociatedControlID = lbxAddressChoices.ID;

            imgbtnConfirmAddress.ID = "imgbtnConfirmAddress";
            imgbtnConfirmAddress.CssClass = LocalisedResourceReader.ResourceString(this.resourceFileName, "ButtonConfirmAddressClass", EsccWebTeam_FormControls.ButtonConfirmAddressClass);
            this.selectAddress.Controls.Add(imgbtnConfirmAddress);
            this.imgbtnConfirmAddress.CausesValidation = false;
            this.imgbtnConfirmAddress.Text = LocalisedResourceReader.ResourceString(this.resourceFileName, "ConfirmAddress", EsccWebTeam_FormControls.ConfirmAddress);
            this.imgbtnConfirmAddress.Click += new EventHandler(imgbtnConfirmAddress_Click);

            this.selectAddress.Controls.Add(lc2b);

            // Finished adding choice of addresses 

            this.Controls.Add(inpGuid);

            lblMessage.CssClass = "validationSummary";
            this.Controls.Add(lblMessage);

            if (this.enableValidaton)
            {
                /*
                NameValueCollection validationConfig = ConfigurationSettings.GetConfig("EsccWebTeam.FormControls/ValidationSettings") as NameValueCollection;
                if (validationConfig != null && validationConfig["AddressFieldFormat"] != null)
                {
                    // Length
                    this.Controls.Add(new EsccRegularExpressionValidator(this.tbxSaon.ID, this.messageSaonLength, "^.{0," + this.maxLengthSaon + "}$"));
                    this.Controls.Add(new EsccRegularExpressionValidator(this.tbxPaon.ID, this.messagePaonLength, "^.{0," + this.maxLengthPaon + "}$"));
                    this.Controls.Add(new EsccRegularExpressionValidator(this.tbxStreetDescriptor.ID, this.messageStreetDescriptorLength, "^.{0," + this.maxLengthStreetDescriptor + "}$"));
                    this.Controls.Add(new EsccRegularExpressionValidator(this.tbxLocality.ID, this.messageLocalityLength, "^.{0," + this.maxLengthLocality + "}$"));
                    this.Controls.Add(new EsccRegularExpressionValidator(this.tbxTown.ID, this.messageTownLength, "^.{0," + this.maxLengthTown + "}$"));
                    this.Controls.Add(new EsccRegularExpressionValidator(this.tbxAdministrativeArea.ID, this.messageAdministrativeAreaLength, "^.{0," + this.maxLengthAdministrativeArea + "}$"));
					
                    // Format
                    this.Controls.Add(new EsccRegularExpressionValidator(this.tbxSaon.ID, this.messageSaonFormat, validationConfig["AddressFieldFormat"]));
                    this.Controls.Add(new EsccRegularExpressionValidator(this.tbxPaon.ID, this.messagePaonFormat, validationConfig["AddressFieldFormat"]));
                    this.Controls.Add(new EsccRegularExpressionValidator(this.tbxStreetDescriptor.ID, this.messageStreetDescriptorFormat, validationConfig["AddressFieldFormat"]));
                    this.Controls.Add(new EsccRegularExpressionValidator(this.tbxLocality.ID, this.messageLocalityFormat, validationConfig["AddressFieldFormat"]));
                    this.Controls.Add(new EsccRegularExpressionValidator(this.tbxTown.ID, this.messageTownFormat, validationConfig["AddressFieldFormat"]));
                    this.Controls.Add(new EsccRegularExpressionValidator(this.tbxAdministrativeArea.ID, this.messageAdministrativeAreaFormat, validationConfig["AddressFieldFormat"]));

                    // Postcode is validated by the web service
                }
                */

                // Create validators, though they will only be used if EnableValidation is set to true
                EsccCustomValidator requiredValidator = new EsccCustomValidator();
                requiredValidator.ErrorMessage = this.messageAddressRequired;
                requiredValidator.ServerValidate += new ServerValidateEventHandler(requiredValidator_ServerValidate);
                this.Controls.Add(requiredValidator);

                EsccCustomValidator addressRequiredValidator = new EsccCustomValidator();
                addressRequiredValidator.ErrorMessage = this.messageFullAddressRequired;
                addressRequiredValidator.ServerValidate += new ServerValidateEventHandler(addressRequiredValidator_ServerValidate);
                this.Controls.Add(addressRequiredValidator);

                EsccCustomValidator notRequiredButHasPostcodeRequiredValidator = new EsccCustomValidator();
                notRequiredButHasPostcodeRequiredValidator.ErrorMessage = this.messageFullAddressRequired;
                notRequiredButHasPostcodeRequiredValidator.ServerValidate += new ServerValidateEventHandler(notRequiredButHasPostcodeRequiredValidator_ServerValidate);
                this.Controls.Add(notRequiredButHasPostcodeRequiredValidator);

            }
            else
            {
                // If ASP.NET validation not deliberately enabled for this control, 
                // set up the old method of validation which ties this control into a "btnSubmit" button on the page.

                EsccButton imb = Page.FindControl("btnSubmit") as EsccButton;
                if (imb != null) imb.Click += new EventHandler(imb_Click);

            }


            base.CreateChildControls();


            toggleStatusSessionName = this.UniqueID + this.questionID + "togglestate";


            if (HttpContext.Current.Session[toggleStatusSessionName] != null)
            {
                toggleStatus = (bool)HttpContext.Current.Session[toggleStatusSessionName];
            }

            if (!toggleStatus)
            {
                ToggleAddressFields(false);
            }
            else
            {
                this.ShowToggleButton(false);
            }
        }

        #endregion

        #region Events
        /// <summary>
        /// Event indicating that a postcode and house name/number have been submitted and have passed validation
        /// </summary>
        public event PostcodeSearchEventHandler PostcodeSubmitted;

        /// <summary>
        /// Event handler delegate for <c>PostcodeSubmitted</c> event, allowing postcode and house name/number to be passed in <c>PostcodeSearchEventArgs</c>
        /// </summary>
        public delegate void PostcodeSearchEventHandler(object sender, PostcodeSearchEventArgs e);

        /// <summary>
        /// Raise event indicating that a postcode and house name/number have been submitted and have passed validation
        /// </summary>
        protected void OnPostcodeSubmitted(string postcode)
        {
            // check there are handlers for the event before raising
            if (this.PostcodeSubmitted != null)
            {
                // raise event
                this.PostcodeSubmitted(this, new PostcodeSearchEventArgs(postcode));
            }
        }
        /// <summary>
        /// Event indicating that the user has chose an address from the list box.
        /// </summary>
        public event AddressConfirmedEventHandler AddressConfirmed;
        /// <summary>
        /// Event handler delegate for <c>AddressConfirmed</c> event, allowing postcode and house name/number to be passed in <c>PostcodeSearchEventArgs</c>
        /// </summary>
        public delegate void AddressConfirmedEventHandler(object sender, AddressConfirmedEventArgs e);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oid"></param>
        public void OnAddressConfirmed(string oid)
        {
            //oid = oid;
            // check there are handlers for the event before raising
            if (this.AddressConfirmed != null)
            {
                this.AddressConfirmed(this, new AddressConfirmedEventArgs(oid));
            }
        }
        #endregion

        #region Event handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FormAddress_AddressConfirmed(object sender, AddressConfirmedEventArgs e)
        {

            DataSet ds = HttpContext.Current.Session[this.UniqueID + this.questionID + "addresses"] as DataSet;

            // If the addresses are not in session, perhaps because the session expired while the user was distracted,
            // try to get the addresses again to prevent an error. This happens surprisingly often.
            if (ds == null)
            {
                using (var addressfinder = new AddressFinder.AddressFinder())
                {
                    try
                    {
                        if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ConnectToCouncilWebServicesAccount"]) &&
                                    !String.IsNullOrEmpty(ConfigurationManager.AppSettings["ConnectToCouncilWebServicesPassword"]))
                        {
                            addressfinder.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["ConnectToCouncilWebServicesAccount"], ConfigurationManager.AppSettings["ConnectToCouncilWebServicesPassword"]);
                        }

                        ds = addressfinder.AddressesFromPostcode(this.tbxPostcode.Text);
                    }
                    catch (SoapException)
                    {
                        // web service returns the same message for all exceptions - we can localise
                        lblMessage.Text = LocalisedResourceReader.ResourceString(this.resourceFileName, "ErrorPostcodeNotFound", EsccWebTeam_FormControls.ErrorPostcodeNotFound);
                        return;
                    }
                }
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["oid"].ToString() == e.Oid)
                {

                    ToggleAddressFields(true);

                    this.saon = dr["BD"].ToString();
                    //amended to cope with building numbers of 0 instead of nulls in the underlying data tables RG 06/03/2006
                    if (dr["BN"].ToString() != "0")
                    {
                        this.paon = dr["BN"].ToString();
                    }
                    this.streetDescriptor = dr["TN"].ToString();
                    this.locality = dr["DL"].ToString();
                    this.town = dr["PT"].ToString();
                    this.administrativeArea = dr["CN"].ToString();
                    this.postcode = dr["PC"].ToString();
                    this.easting = dr["Easting"].ToString();
                    this.northing = dr["Northing"].ToString();
                    // removed due to absence of this field in the underlying data which has been changed 06/03/2006 RG
                    //this.wardCode = dr["Ward_Code"].ToString();
                    this.oa = dr["OA"].ToString();
                    this.oid = e.Oid;

                    this.UpdateTextboxes();

                    return;
                }
            }

        }

        /// <summary>
        /// Handles the PostcodeSubmitted event of the Message control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PostcodeSearchEventArgs"/> instance containing the event data.</param>
        protected void Message_PostcodeSubmitted(object sender, PostcodeSearchEventArgs e)
        {
            if (e.Postcode.Length > 0)
            {
                lblMessage.Text = String.Empty;
                DataSet ds = null;
                try
                {
                    using (var addressfinder = new AddressFinder.AddressFinder())
                    {
                        if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ConnectToCouncilWebServicesAccount"]) &&
                            !String.IsNullOrEmpty(ConfigurationManager.AppSettings["ConnectToCouncilWebServicesPassword"]))
                        {
                            addressfinder.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["ConnectToCouncilWebServicesAccount"], ConfigurationManager.AppSettings["ConnectToCouncilWebServicesPassword"]);
                        }

                        ds = addressfinder.AddressesFromPostcode(e.Postcode);
                    }

                }
                catch (SoapException)
                {
                    // web service returns the same message for all exceptions - we can localise
                    lblMessage.Text = LocalisedResourceReader.ResourceString(this.resourceFileName, "ErrorPostcodeNotFound", EsccWebTeam_FormControls.ErrorPostcodeNotFound);
                    return;
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    if (this.enableValidaton)
                    {
                        new ValidationMessage(LocalisedResourceReader.ResourceString(this.resourceFileName, "ErrorRecheckAddress", EsccWebTeam_FormControls.ErrorRecheckAddress), this.Page);
                    }
                    else
                    {
                        lblMessage.Text = LocalisedResourceReader.ResourceString(this.resourceFileName, "ErrorRecheckAddress", EsccWebTeam_FormControls.ErrorRecheckAddress);
                    }
                }
                else
                {
                    HttpContext.Current.Session[this.UniqueID + this.questionID + "addresses"] = ds;
                    FormatDataSet(ds);
                    this.selectAddress.Visible = true;
                    this.ToggleAddressFields(false);
                }
            }
            else
            {
                if (this.enableValidaton)
                {
                    new ValidationMessage(LocalisedResourceReader.ResourceString(this.resourceFileName, "ErrorPostcodeMissing", EsccWebTeam_FormControls.ErrorPostcodeRequired), this.Page);
                }
                else
                {
                    lblMessage.Text = LocalisedResourceReader.ResourceString(this.resourceFileName, "ErrorPostcodeMissing", EsccWebTeam_FormControls.ErrorPostcodeRequired);
                }
            }
        }

        /// <summary>
        /// Check that the form data is valid when it's submitted, and raise a <c>PostcodeSubmitted</c> event if it is
        /// </summary>
        /// <param name="sender">The submit button</param>
        /// <param name="e">The click coordinates</param>
        private void imgbtnFind_Click(object sender, EventArgs e)
        {
            //TODO: find a way of getting the button to submit after being tabbed to...see below.
            // this stops the click event for the button being fired if 
            // the user presses the enter key however it also prevents the button working if the user tabs to it before keying enter
            //			if(e.X > 0 & e.Y > 0)
            //			{	
            // raise event, and pass in valid data from textbox
            this.OnPostcodeSubmitted(this.tbxPostcode.Text);
            //			}
        }

        /// <summary>
        /// Raises an address confirmed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgbtnConfirmAddress_Click(Object sender, EventArgs e)
        {
            string[] keys = HttpContext.Current.Request.Form.AllKeys;
            foreach (string key in keys)
            {
                //				Regex regex = new Regex("_ctl[0-9]{1,4}:lbxAddress");
                Regex regex = new Regex(@"[A-Za-z0-9_]+(:|\$)lbxAddressChoices");
                if (regex.IsMatch(key))
                {
                    this.OnAddressConfirmed(HttpContext.Current.Request.Form[key]);
                    break;
                }
            }
        }

        #endregion

        #region Validation

        /// <summary>
        /// Handles the ServerValidate event of the requiredValidator control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.</param>
        private void requiredValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.enableValidaton && this.isRequired)
            {


                // Although we want people to complete more than the postcode, if only the postcode is
                // filled in this validator passes, because the next validator will fail and we don't want
                // both messages to display.

                args.IsValid = (
                    (this.tbxAdministrativeArea.Text.Trim().Length > 0) ||
                    (this.tbxLocality.Text.Trim().Length > 0) ||
                    (this.tbxPaon.Text.Trim().Length > 0) ||
                    (this.tbxSaon.Text.Trim().Length > 0) ||
                    (this.tbxStreetDescriptor.Text.Trim().Length > 0) ||
                    (this.tbxTown.Text.Trim().Length > 0) ||
                    (this.tbxPostcode.Text.Trim().Length > 0)
                                );
            }
            else
            {
                args.IsValid = true;
            }
        }

        /// <summary>
        /// Handles the ServerValidate event of the addressRequiredValidator control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.</param>
        private void addressRequiredValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.enableValidaton && this.isRequired)
            {
                // Check that it's not just the postcode completed. That probably means they didn't click "Find address".

                args.IsValid = !((this.tbxAdministrativeArea.Text.Trim().Length == 0) &&
                    (this.tbxLocality.Text.Trim().Length == 0) &&
                    (this.tbxPaon.Text.Trim().Length == 0) &&
                    (this.tbxSaon.Text.Trim().Length == 0) &&
                    (this.tbxStreetDescriptor.Text.Trim().Length == 0) &&
                    (this.tbxTown.Text.Trim().Length == 0) &&
                    (this.tbxPostcode.Text.Trim().Length > 0));
            }
            else
            {
                args.IsValid = true;
            }
        }


        private void notRequiredButHasPostcodeRequiredValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (enableValidaton && !this.IsRequired)
            {
                if (this.tbxPostcode.Text.Trim().Length > 0)
                {


                    args.IsValid = !((this.tbxAdministrativeArea.Text.Trim().Length == 0) &&
                              (this.tbxLocality.Text.Trim().Length == 0) &&
                              (this.tbxPaon.Text.Trim().Length == 0) &&
                              (this.tbxSaon.Text.Trim().Length == 0) &&
                              (this.tbxStreetDescriptor.Text.Trim().Length == 0) &&
                              (this.tbxTown.Text.Trim().Length == 0) &&
                              (this.tbxPostcode.Text.Trim().Length > 0));

                }
                else
                {
                    args.IsValid = true;
                }
            }
            else
            {
                //We tested this below so valid to stop repeating the error message twice
                args.IsValid = true;
            }

        }


        /// <summary>
        /// Handles the Click event of a submit button found on the page with an id of btnSubmit
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void imb_Click(Object sender, EventArgs e)
        {
            if (this.isRequired && !this.enableValidaton)
            {
                if ((Page.IsPostBack) & (this.tbxAdministrativeArea.Text.Length == 0) &
                    (this.tbxLocality.Text.Length == 0) & (this.tbxPaon.Text.Length == 0) & (this.tbxPostcode.Text.Length == 0)
                    & (this.tbxSaon.Text.Length == 0) & (this.tbxStreetDescriptor.Text.Length == 0) & (this.tbxTown.Text.Length == 0))
                {

                    Label error = new Label();
                    error.CssClass = "warning";
                    error.Text = LocalisedResourceReader.ResourceString(this.resourceFileName, "ErrorAddressBlank", EsccWebTeam_FormControls.ErrorAddressBlank);
                    this.Controls.AddAt(0, error);
                }
            }
        }

        /// <summary>
        /// When <see cref="IsRequired" /> is set to <c>true</c>, use ASP.NET validators rather than a custom label
        /// </summary>
        public bool EnableValidation
        {
            get { return this.enableValidaton; }
            set { this.enableValidaton = value; }
        }

        /// <summary>
        /// Gets or sets the introduction text below the address fields.
        /// </summary>
        /// <value>The introduction.</value>
        public string TextIntroduction
        {
            get { return this.textIntro; }
            set { this.textIntro = value; }
        }

        /// <summary>
        /// Gets or sets the error message to display when an address is required but not found
        /// </summary>
        public string MessageAddressRequired
        {
            get { return this.messageAddressRequired; }
            set { this.messageAddressRequired = value; }
        }

        /// <summary>
        /// Gets or sets the error message to display when a postcode is entered but the address is not completed
        /// </summary>
        public string MessageFullAddressRequired
        {
            get { return this.messageFullAddressRequired; }
            set { this.messageFullAddressRequired = value; }
        }

        /// <summary>
        /// Gets or sets the error message to display when the SAON is too long 
        /// </summary>
        /// <value>The error message.</value>
        public string MessageSaonLength
        {
            get { return this.messageSaonLength; }
            set { this.messageSaonLength = value; }
        }

        /// <summary>
        /// Gets or sets the error message to display when the PAON is too long 
        /// </summary>
        /// <value>The error message.</value>
        public string MessagePaonLength
        {
            get { return this.messagePaonLength; }
            set { this.messagePaonLength = value; }
        }

        /// <summary>
        /// Gets or sets the error message to display when the street descriptor is too long 
        /// </summary>
        /// <value>The error message.</value>
        public string MessageStreetDescriptorLength
        {
            get { return this.messageStreetDescriptorLength; }
            set { this.messageStreetDescriptorLength = value; }
        }

        /// <summary>
        /// Gets or sets the error message to display when the locality is too long 
        /// </summary>
        /// <value>The error message.</value>
        public string MessageLocalityLength
        {
            get { return this.messageLocalityLength; }
            set { this.messageLocalityLength = value; }
        }

        /// <summary>
        /// Gets or sets the error message to display when the town is too long 
        /// </summary>
        /// <value>The error message.</value>
        public string MessageTownLength
        {
            get { return this.messageTownLength; }
            set { this.messageTownLength = value; }
        }

        /// <summary>
        /// Gets or sets the error message to display when the administrative area is too long 
        /// </summary>
        /// <value>The error message.</value>
        public string MessageAdministrativeAreaLength
        {
            get { return this.messageAdministrativeAreaLength; }
            set { this.messageAdministrativeAreaLength = value; }
        }

        /// <summary>
        /// Gets or sets the error message to display when the SAON contains invalid characters
        /// </summary>
        /// <value>The error message.</value>
        public string MessageSaonFormat
        {
            get { return this.messageSaonFormat; }
            set { this.messageSaonFormat = value; }
        }

        /// <summary>
        /// Gets or sets the error message to display when the PAON contains invalid characters
        /// </summary>
        /// <value>The error message.</value>
        public string MessagePaonFormat
        {
            get { return this.messagePaonFormat; }
            set { this.messagePaonFormat = value; }
        }

        /// <summary>
        /// Gets or sets the error message to display when the street descriptor contains invalid characters
        /// </summary>
        /// <value>The error message.</value>
        public string MessageStreetDescriptorFormat
        {
            get { return this.messageStreetDescriptorFormat; }
            set { this.messageStreetDescriptorFormat = value; }
        }

        /// <summary>
        /// Gets or sets the error message to display when the locality contains invalid characters
        /// </summary>
        /// <value>The error message.</value>
        public string MessageLocalityFormat
        {
            get { return this.messageLocalityFormat; }
            set { this.messageLocalityFormat = value; }
        }

        /// <summary>
        /// Gets or sets the error message to display when the town contains invalid characters
        /// </summary>
        /// <value>The error message.</value>
        public string MessageTownFormat
        {
            get { return this.messageTownFormat; }
            set { this.messageTownFormat = value; }
        }

        /// <summary>
        /// Gets or sets the error message to display when the administrative area contains invalid characters
        /// </summary>
        /// <value>The error message.</value>
        public string MessageAdministrativeAreaFormat
        {
            get { return this.messageAdministrativeAreaFormat; }
            set { this.messageAdministrativeAreaFormat = value; }
        }

        #endregion

    }
}
