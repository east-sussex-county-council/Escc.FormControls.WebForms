using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using eastsussexgovuk.webservices.FormControls.Validators;
using EsccWebTeam.Exceptions.Soap;
using EsccWebTeam.FormControls.AddressFinder;
using EsccWebTeam.FormControls.Properties;
using EsccWebTeam.HouseStyle;
using Microsoft.ApplicationBlocks.Data;

namespace eastsussexgovuk.webservices.FormControls.CustomControls
{
    /// <summary>
    /// Summary description for FormAddress.
    /// </summary>
    [DefaultProperty("QuestionID"), ToolboxData("<{0}:FormAddress runat=server></{0}:FormAddress>")]
    public class FormAddress : WebControl, INamingContainer
    {
        #region private fields
        private int questionID;
        private CustomValidator error = null;
        private Label lblMessage;
        private Label lblAddress;
        private TextBox tbxSaon;
        private TextBox tbxPaon;
        private TextBox tbxStreetDescriptor;
        private TextBox tbxLocality;
        private TextBox tbxTown;
        private TextBox tbxAdministrativeArea;
        private TextBox tbxPostcode;
        private string saon;
        private string paon;
        private string streetDescriptor;
        private string locality;
        private string town;
        private string administrativeArea;
        private string postcode;
        private string easting;
        private string northing;
        //		private string wardCode;
        private string oa;
        private string oid;
        private bool pafCheckValid;
        private EsccButton imgbtnConfirmAddress = new EsccButton();
        private ListBox lbxAddressChoices;
        private EsccButton imgbtnFind = new EsccButton();
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
        private EsccButton imbAddressToggle = new EsccButton();
        private bool toggleStatus;
        private string toggleStatusSessionName;
        private HtmlInputHidden inpGuid;
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
        private PlaceHolder selectAddress;
        private PlaceHolder addressFields;

        //Validation Controls

        EsccCustomValidator requiredValidator = new EsccCustomValidator();
        EsccCustomValidator addressRequiredValidator = new EsccCustomValidator();
        EsccCustomValidator notRequiredButHasPostcodeRequiredValidator = new EsccCustomValidator();
        private string resourceFileName = typeof(EsccWebTeam_FormControls).Name;



        #endregion
        #region public properties
        /// <summary>
        /// Id referred to by the eforms system for each question component
        /// </summary>
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
        /// 
        /// </summary>
        public string Easting
        {
            get
            {
                return easting;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Northing
        {
            get
            {
                return northing;
            }
        }
        //		/// <summary>
        //		/// 
        //		/// </summary>
        //		public string WardCode
        //		{
        //			get
        //			{
        //				return wardCode;
        //			}
        //		}
        /// <summary>
        /// 
        /// </summary>
        public string OA
        {
            get
            {
                return oa;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OID
        {
            get
            {
                return oid;
            }
        }
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public string Saon
        {
            get
            {
                return saon;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Paon
        {
            get
            {
                return paon;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StreetDescriptor
        {
            get
            {
                return streetDescriptor;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Locality
        {
            get
            {
                return locality;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Town
        {
            get
            {
                return town;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AdministrativeArea
        {
            get
            {
                return administrativeArea;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Postcode
        {
            get
            {
                return postcode;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is required.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is required; otherwise, <c>false</c>.
        /// </value>
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



        #endregion
        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FormAddress"/> class.
        /// </summary>
        /// <param name="formQuestionID">The form question ID.</param>
        public FormAddress(int formQuestionID)
            : base(HtmlTextWriterTag.Div)
        {
            questionID = formQuestionID;

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FormAddress"/> class.
        /// </summary>
        public FormAddress()
            : base(HtmlTextWriterTag.Div)
        {

        }
        #endregion
        #region init
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {

            //Allow default messages to be bypased by eforms system setting them
            if (this.messageAddressRequired.Length == 0)
            {
                this.messageAddressRequired = TextUtilities.ResourceString(resourceFileName, "ErrorAddressBlank", EsccWebTeam_FormControls.ErrorAddressBlank);
            }
            if (this.messageFullAddressRequired.Length == 0)
            {
                this.messageFullAddressRequired = TextUtilities.ResourceString(resourceFileName, "ErrorAddressPostcodeOnly", EsccWebTeam_FormControls.ErrorAddressPostcodeOnly);
            }
            //this.textIntro = TextUtilities.ResourceString(resourceFileName, "", EsccWebTeam_FormControls.FindAddressIntro;

            // Length
            this.messageSaonLength = TextUtilities.ResourceString(resourceFileName, "ErrorSaonLength", EsccWebTeam_FormControls.ErrorSaonLength);
            this.messagePaonLength = TextUtilities.ResourceString(resourceFileName, "ErrorPaonLength", EsccWebTeam_FormControls.ErrorPaonLength);
            this.messageStreetDescriptorLength = TextUtilities.ResourceString(resourceFileName, "ErrorStreetDescriptorLength", EsccWebTeam_FormControls.ErrorStreetDescriptorLength);
            this.messageLocalityLength = TextUtilities.ResourceString(resourceFileName, "ErrorLocalityLength", EsccWebTeam_FormControls.ErrorLocalityLength);
            this.messageTownLength = TextUtilities.ResourceString(resourceFileName, "ErrorTownLength", EsccWebTeam_FormControls.ErrorTownLength);
            this.messageAdministrativeAreaLength = TextUtilities.ResourceString(resourceFileName, "ErrorAdministrativeAreaLength", EsccWebTeam_FormControls.ErrorAdministrativeAreaLength);

            // Format
            this.messageSaonFormat = TextUtilities.ResourceString(resourceFileName, "ErrorSaonFormat", EsccWebTeam_FormControls.ErrorSaonFormat);
            this.messagePaonFormat = TextUtilities.ResourceString(resourceFileName, "ErrorPaonFormat", EsccWebTeam_FormControls.ErrorPaonFormat);
            this.messageStreetDescriptorFormat = TextUtilities.ResourceString(resourceFileName, "ErrorStreetDescriptorFormat", EsccWebTeam_FormControls.ErrorStreetDescriptorFormat);
            this.messageLocalityFormat = TextUtilities.ResourceString(resourceFileName, "ErrorLocalityFormat", EsccWebTeam_FormControls.ErrorLocalityFormat);
            this.messageTownFormat = TextUtilities.ResourceString(resourceFileName, "ErrorTownFormat", EsccWebTeam_FormControls.ErrorTownFormat);
            this.messageAdministrativeAreaFormat = TextUtilities.ResourceString(resourceFileName, "ErrorAdministrativeAreaFormat", EsccWebTeam_FormControls.ErrorAdministrativeAreaFormat);


            base.OnInit(e);
            InitializeComponent();

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






        }
        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            this.PostcodeSubmitted += new PostcodeSearchEventHandler(Message_PostcodeSubmitted);
            this.AddressConfirmed += new AddressConfirmedEventHandler(FormAddress_AddressConfirmed);
        }
        #endregion
        #region public methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="citizenID">ID of the current user in the registration database</param>
        /// <param name="formInstanceID"></param>
        /// <param name="transaction"></param>
        /// <param name="spName"></param>
        /// <param name="paymentVerified"></param>
        public int SaveAddress(int citizenID, int formInstanceID, SqlTransaction transaction, string spName, bool paymentVerified)
        {
            // update non user viewable properties from the address dataset in session if this exists
            if (HttpContext.Current.Session["pafaddress"] != null)
            {
                DataSet ds = HttpContext.Current.Session["pafaddress"] as DataSet;
                this.oid = ds.Tables[0].Rows[0]["oid"].ToString();
                this.oa = ds.Tables[0].Rows[0]["oa"].ToString();
                //this.wardCode = ds.Tables[0].Rows[0]["wardCode"].ToString();
                //this.pafCheckValid = Convert.ToBoolean(ds.Tables[0].Rows[0]["pafCheckValid"]);
                this.easting = ds.Tables[0].Rows[0]["easting"].ToString();
                this.northing = ds.Tables[0].Rows[0]["northing"].ToString();
            }
            //REM validation/SQL injection...we are only concerned with character based fields but these issues are limited due 
            //to use of typed and sized params...do we need extra validation here?
            SqlParameter[] prms = new SqlParameter[16];

            SqlParameter prmSaon = new SqlParameter("@saon", SqlDbType.VarChar, 100);
            prmSaon.Value = this.tbxSaon.Text;
            if (this.tbxSaon.Text != null & this.tbxSaon.Text != "")
            {
                if (ValidateVarChar(prmSaon.Value.ToString()))
                {
                    prms[0] = prmSaon;
                }
                else
                {
                    throw new Exception("The entered value " + prmSaon.Value + " contains disallowed characters.");
                }
            }
            else
            {
                prms[0] = prmSaon;
            }

            SqlParameter prmPaon = new SqlParameter("@paon", SqlDbType.VarChar, 100);
            prmPaon.Value = this.tbxPaon.Text;
            if (this.tbxPaon.Text != null & this.tbxPaon.Text != "")
            {
                if (ValidateVarChar(prmPaon.Value.ToString()))
                {
                    prms[1] = prmPaon;

                }
                else
                {
                    throw new Exception("The entered value " + prmPaon.Value + " contains disallowed characters.");
                }
            }
            else
            {
                prms[1] = prmPaon;
            }

            SqlParameter prmStreetDescriptor = new SqlParameter("@streetDescriptor", SqlDbType.VarChar, 100);
            prmStreetDescriptor.Value = this.tbxStreetDescriptor.Text;
            if (this.tbxStreetDescriptor.Text != null & this.tbxStreetDescriptor.Text != "")
            {
                if (ValidateVarChar(prmStreetDescriptor.Value.ToString()))
                {
                    prms[2] = prmStreetDescriptor;
                }
                else
                {
                    throw new Exception("The entered value " + prmStreetDescriptor.Value + " contains disallowed characters.");
                }
            }
            else
            {
                prms[2] = prmStreetDescriptor;
            }

            SqlParameter prmLocality = new SqlParameter("@locality", SqlDbType.VarChar, 35);
            prmLocality.Value = this.tbxLocality.Text;
            if (this.tbxLocality.Text != null & this.tbxLocality.Text != "")
            {
                if (ValidateVarChar(prmLocality.Value.ToString()))
                {
                    prms[3] = prmLocality;
                }
                else
                {
                    throw new Exception("The entered value " + prmLocality.Value + " contains disallowed characters.");
                }
            }
            else
            {
                prms[3] = prmLocality;
            }

            SqlParameter prmTown = new SqlParameter("@town", SqlDbType.VarChar, 30);
            prmTown.Value = this.tbxTown.Text;
            if (this.tbxTown.Text != null & this.tbxTown.Text != "")
            {
                if (ValidateVarChar(prmTown.Value.ToString()))
                {
                    prms[4] = prmTown;
                }
                else
                {
                    throw new Exception("The entered value " + prmTown.Value + " contains disallowed characters.");
                }
            }
            else
            {
                prms[4] = prmTown;
            }

            SqlParameter prmAdministrativeArea = new SqlParameter("@administrativeArea", SqlDbType.VarChar, 30);
            prmAdministrativeArea.Value = this.tbxAdministrativeArea.Text;
            if (this.tbxAdministrativeArea.Text != null & this.tbxAdministrativeArea.Text != "")
            {
                if (ValidateVarChar(prmAdministrativeArea.Value.ToString()))
                {
                    prms[5] = prmAdministrativeArea;
                }
                else
                {
                    throw new Exception("The entered value " + prmAdministrativeArea.Value + " contains disallowed characters.");
                }
            }
            else
            {
                prms[5] = prmAdministrativeArea;
            }

            SqlParameter prmPostCode = new SqlParameter("@postcode", SqlDbType.VarChar, 8);
            prmPostCode.Value = this.tbxPostcode.Text;
            if (this.tbxPostcode.Text != null & this.tbxPostcode.Text != "")
            {
                if (ValidateAlphaNumeric(prmPostCode.Value.ToString()))
                {
                    prms[6] = prmPostCode;
                }
                else
                {
                    throw new Exception("The entered value " + prmPostCode.Value + " contains disallowed characters.");
                }
            }
            else
            {
                prms[6] = prmPostCode;
            }

            SqlParameter prmEasting = new SqlParameter("@easting", SqlDbType.Int, 4);
            prmEasting.Value = this.easting;
            if (this.easting != null & this.easting != "")
            {
                if (ValidateNumeric(prmEasting.Value.ToString()))
                {
                    prms[7] = prmEasting;

                }
                else
                {
                    throw new Exception("The entered value " + prmEasting.Value + " contains disallowed characters.");
                }
            }
            else
            {
                prms[7] = prmEasting;
            }

            SqlParameter prmNorthing = new SqlParameter("@northing", SqlDbType.Int, 4);
            prmNorthing.Value = this.northing;
            if (this.northing != null & this.northing != "")
            {
                if (ValidateNumeric(prmNorthing.Value.ToString()))
                {
                    prms[8] = prmNorthing;

                }
                else
                {
                    throw new Exception("The entered value " + prmNorthing.Value + " contains disallowed characters.");
                }
            }
            else
            {
                prms[8] = prmNorthing;
            }

            SqlParameter prmWardCode = new SqlParameter("@wardcode", SqlDbType.VarChar, 30);
            prmWardCode.Value = DBNull.Value;//this.wardCode;
            //			if(this.wardCode != null & this.wardCode != "")
            //			{
            //				if(ValidateVarChar(prmWardCode.Value.ToString()))
            //				{
            //					prms[9] = prmWardCode;
            //				}
            //				else
            //				{
            //					throw new Exception("The entered value " + prmWardCode.Value + " contains disallowed characters.");
            //				}
            //			}
            //			else
            //			{
            //				prms[9] = prmWardCode;
            //			}

            SqlParameter prmOA = new SqlParameter("@oa", SqlDbType.VarChar, 12);
            prmOA.Value = this.oa;
            if (this.oa != null & this.oa != "")
            {
                if (ValidateAlphaNumeric(prmOA.Value.ToString()))
                {
                    prms[10] = prmOA;
                }
                else
                {
                    throw new Exception("The entered value " + prmOA.Value + " contains disallowed characters.");
                }
            }
            else
            {
                prms[10] = prmOA;
            }

            SqlParameter prmPaymentVerified = new SqlParameter("@paymentverified", SqlDbType.Bit);
            prmPaymentVerified.Value = paymentVerified;
            prms[11] = prmPaymentVerified;

            SqlParameter prmPafCheckValid = new SqlParameter("@pafcheckvalid", SqlDbType.Bit);
            prmPafCheckValid.Value = pafCheckValid;
            prms[12] = prmPafCheckValid;

            SqlParameter prmOID = new SqlParameter("@OID", SqlDbType.Int, 4);
            prmOID.Value = oid;
            prms[13] = prmOID;

            SqlParameter prmAddressID = new SqlParameter("@addressid", SqlDbType.Int);
            prmAddressID.Direction = ParameterDirection.Output;
            prms[14] = prmAddressID;

            SqlParameter prmCitizenID = new SqlParameter("@citizenID", SqlDbType.Int);
            prmCitizenID.Value = citizenID;
            prms[15] = prmCitizenID;

            SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, prms);
            int addressID = Convert.ToInt32(prms[14].Value);
            // clear all session stuff
            HttpContext.Current.Session["addresses"] = null;
            HttpContext.Current.Session["pafAddress"] = null;
            return addressID;
        }

        /// <summary>
        /// Updates the form instance with address ID.
        /// </summary>
        /// <param name="formInstanceID">The form instance ID.</param>
        /// <param name="citizenID">The citizen ID.</param>
        /// <param name="addressID">The address ID.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="spName">Name of the stored procedure.</param>
        public void UpdateFormInstanceWithAddressID(int formInstanceID, int citizenID, int addressID, SqlTransaction transaction, string spName)
        {
            SqlParameter[] prms = new SqlParameter[3];

            prms[0] = new SqlParameter("@addressid", SqlDbType.Int, 4);
            prms[0].Value = addressID;


            prms[1] = new SqlParameter("@FormInstanceID", SqlDbType.Int, 4);
            prms[1].Value = formInstanceID;

            prms[2] = new SqlParameter("@citizenID", SqlDbType.Int, 4);
            prms[2].Value = citizenID;

            //TODO: check for problems when users use back button
            SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, prms);
        }
        #endregion
        #region private methods
        /// <summary>
        /// Helper method toggles visibility of address fields excl postcode
        /// </summary>
        private void ToggleAddressFields(bool state)
        {
            this.addressFields.Visible = state;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationChars"></param>
        /// <returns></returns>
        private bool ValidateVarChar(string validationChars)
        {
            bool retVal = false;
            // if the regular expression needs to be modified this should be done in web.config in the app root.
            Regex regx = new Regex(ConfigurationManager.AppSettings["RegexVarChar"]);
            if (regx.IsMatch(validationChars))
            {
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationChars"></param>
        /// <returns></returns>
        private bool ValidateAlphaNumeric(string validationChars)
        {
            bool retVal = false;
            // if the regular expression needs to be modified this should be done in web.config in the app root.
            Regex regx = new Regex(ConfigurationManager.AppSettings["RegexAlphaNum"]);
            if (regx.IsMatch(validationChars))
            {
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationChars"></param>
        /// <returns></returns>
        private bool ValidateNumeric(string validationChars)
        {
            bool retVal = false;
            // if the regular expression needs to be modified this should be done in web.config in the app root.
            Regex regx = new Regex(ConfigurationManager.AppSettings["RegexNumeric"]);
            if (regx.IsMatch(validationChars))
            {
                retVal = true;
            }
            return retVal;
        }
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

                if (dr["TN"] != null & dr["TN"].ToString() != "")
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(dr["TN"]).ToString();
                }

                if (dr["DL"] != null & dr["DL"].ToString() != "")
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(dr["DL"]).ToString();
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
            //imgbtnConfirmAddress = new EsccButton();
            lbxAddressChoices = new ListBox();
            //imgbtnFind = new EsccButton();

            //imbAddressToggle = new EsccButton();

            inpGuid = new HtmlInputHidden();



            this.Controls.Add(lcfp1a);

            HtmlGenericControl pcDesc = new HtmlGenericControl("p");
            pcDesc.Attributes["class"] = "pcDesc";
            pcDesc.InnerText = "If you have an East Sussex postcode type it in the text box below and use the 'Find Address' button to complete your address details.";
            this.Controls.Add(pcDesc);

            //			lblAddress.ID = "lblAddress";
            //			lblAddress.Text = "Address";
            //			lblAddress.CssClass="formLabel";
            //			this.Controls.Add(lblAddress);

            // Add address fields
            this.addressFields = new PlaceHolder();
            this.Controls.Add(this.addressFields);

            // Add SAON field
            Label saonLabel = new Label();
            saonLabel.Text = "Flat number or firm";
            saonLabel.CssClass = "aural";
            tbxSaon.CssClass = "saon";
            this.addressFields.Controls.Add(lcfp1a);
            this.addressFields.Controls.Add(saonLabel);
            this.addressFields.Controls.Add(tbxSaon);
            this.addressFields.Controls.Add(lcfp1b);
            saonLabel.AssociatedControlID = tbxSaon.ID;

            // Add PAON field
            Label paonLabel = new Label();
            paonLabel.Text = "Building name or number";
            paonLabel.CssClass = "aural";
            tbxPaon.CssClass = "paon";
            this.addressFields.Controls.Add(lcfp2a);
            this.addressFields.Controls.Add(paonLabel);
            this.addressFields.Controls.Add(tbxPaon);
            this.addressFields.Controls.Add(lcfp2b);
            paonLabel.AssociatedControlID = tbxPaon.ID;

            // Add street descriptor field
            Label streetLabel = new Label();
            streetLabel.Text = "Street name";
            streetLabel.CssClass = "aural";
            tbxStreetDescriptor.CssClass = "street";
            this.addressFields.Controls.Add(lcfp3a);
            this.addressFields.Controls.Add(streetLabel);
            this.addressFields.Controls.Add(tbxStreetDescriptor);
            this.addressFields.Controls.Add(lcfp3b);
            streetLabel.AssociatedControlID = tbxStreetDescriptor.ID;

            // Add locality field
            Label localityLabel = new Label();
            localityLabel.Text = "Village or part of town";
            localityLabel.CssClass = "aural";
            tbxLocality.CssClass = "locality";
            this.addressFields.Controls.Add(lcfp4a);
            this.addressFields.Controls.Add(localityLabel);
            this.addressFields.Controls.Add(tbxLocality);
            this.addressFields.Controls.Add(lcfp4b);
            localityLabel.AssociatedControlID = tbxLocality.ID;

            // Add town field
            Label townLabel = new Label();
            townLabel.Text = "Town";
            townLabel.CssClass = "formLabel";
            tbxTown.CssClass = "town";
            this.addressFields.Controls.Add(lcfp5a);
            this.addressFields.Controls.Add(townLabel);
            this.addressFields.Controls.Add(tbxTown);
            this.addressFields.Controls.Add(lcfp5b);
            townLabel.AssociatedControlID = tbxTown.ID;

            // Add administrative area field
            Label adminLabel = new Label();
            adminLabel.Text = "County";
            adminLabel.CssClass = "formLabel";
            tbxAdministrativeArea.CssClass = "administrative-area";
            this.addressFields.Controls.Add(lcfp6a);
            this.addressFields.Controls.Add(adminLabel);
            this.addressFields.Controls.Add(tbxAdministrativeArea);
            this.addressFields.Controls.Add(lcfp6b);
            adminLabel.AssociatedControlID = tbxAdministrativeArea.ID;

            // Add postcode field
            Label postcodeLabel = new Label();
            postcodeLabel.Text = "Postcode";
            postcodeLabel.CssClass = "formLabel";
            this.Controls.Add(lcpca);
            this.Controls.Add(postcodeLabel);
            this.Controls.Add(tbxPostcode);
            this.Controls.Add(lcpcb);
            postcodeLabel.AssociatedControlID = tbxPostcode.ID;

            // Add a dummy textbox because when the address fields are hidden, postcode is the only textbox, 
            // and that means IE won't fire the .NET click event
            TextBox ieBugFix = new TextBox();
            ieBugFix.CssClass = "ieBugFix";
            ieBugFix.Attributes["style"] = "display:none;";
            this.Controls.Add(ieBugFix);

            this.Controls.Add(lcfp7a);

            //lblMessage.CssClass = "validationSummary";
            //	this.Controls.Add(lblMessage);

            imgbtnFind.CssClass = "button";
            this.Controls.Add(imgbtnFind);
            this.imgbtnFind.Text = "Find address";
            this.imgbtnFind.CausesValidation = false;

            this.imgbtnFind.Click += new EventHandler(imgbtnFind_Click);

            LiteralControl lit = new LiteralControl("<span class=\"addressButton\">&nbsp;or&nbsp;</span>");
            this.Controls.Add(lit);

            this.Controls.Add(imbAddressToggle);
            this.imbAddressToggle.CssClass = "button buttonBigger";
            this.imbAddressToggle.CausesValidation = false;
            this.imbAddressToggle.Text = "Enter an address manually";
            this.imbAddressToggle.Click += new EventHandler(imbAddressToggle_Click);
            this.Controls.Add(lcfp7b);

            this.EnableViewState = false;



            // Add choice of addresses following postcode search 

            this.selectAddress = new PlaceHolder();
            this.selectAddress.Visible = false;
            this.Controls.Add(this.selectAddress);

            lbxAddressChoices.ID = "lbxAddressChoices";
            lbxAddressChoices.CssClass = "listbox";

            Label choiceLabel = new Label();
            choiceLabel.CssClass = "formPart";
            choiceLabel.Text = "Please select the correct address:";
            this.selectAddress.Controls.Add(choiceLabel);

            this.selectAddress.Controls.Add(lc2a);

            this.selectAddress.Controls.Add(lbxAddressChoices);
            choiceLabel.AssociatedControlID = lbxAddressChoices.ID;

            imgbtnConfirmAddress.ID = "imgbtnConfirmAddress";
            imgbtnConfirmAddress.CssClass = "button buttonBig";
            this.selectAddress.Controls.Add(imgbtnConfirmAddress);
            this.imgbtnConfirmAddress.CausesValidation = false;
            this.imgbtnConfirmAddress.Text = "Confirm address";
            this.imgbtnConfirmAddress.Click += new EventHandler(imgbtnConfirmAddress_Click);

            this.selectAddress.Controls.Add(lc2b);

            // Finished adding choice of addresses 

            this.Controls.Add(inpGuid);

            lblMessage.CssClass = "validationSummary";
            this.Controls.Add(lblMessage);

            if (this.enableValidaton)
            {
                #region old Ricks
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
					
				}*/
                #endregion

                // Create validators, though they will only be used if EnableValidation is set to true
                //EsccCustomValidator requiredValidator = new EsccCustomValidator();
                requiredValidator.ErrorMessage = this.messageAddressRequired;
                requiredValidator.ServerValidate += new ServerValidateEventHandler(requiredValidator_ServerValidate);
                this.Controls.Add(requiredValidator);

                //EsccCustomValidator addressRequiredValidator = new EsccCustomValidator();
                addressRequiredValidator.ErrorMessage = this.messageFullAddressRequired;
                addressRequiredValidator.ServerValidate += new ServerValidateEventHandler(addressRequiredValidator_ServerValidate);
                this.Controls.Add(addressRequiredValidator);

                // EsccCustomValidator notRequiredButHasPostcodeRequiredValidator = new EsccCustomValidator();
                notRequiredButHasPostcodeRequiredValidator.ErrorMessage = this.messageFullAddressRequired;
                notRequiredButHasPostcodeRequiredValidator.ServerValidate += new ServerValidateEventHandler(notRequiredButHasPostcodeRequiredValidator_ServerValidate);
                this.Controls.Add(notRequiredButHasPostcodeRequiredValidator);


            }
            else
            {
                // If ASP.NET validation not deliberately enabled for this control, 
                // set up the old method of validation which ties this control into a "btnSubmit" button on the page.
                EsccButton imb = Page.FindControl("btnSubmit") as EsccButton;
                imb.Click += new EventHandler(imb_Click);
            }


            base.CreateChildControls();


            toggleStatusSessionName = this.questionID + "togglestate";

            if (error != null)
            {
                error.Text = "";
            }

            if (HttpContext.Current.Session[toggleStatusSessionName] != null)
            {
                toggleStatus = (bool)HttpContext.Current.Session[toggleStatusSessionName];
            }

            if (!toggleStatus)
            {
                ToggleAddressFields(false);
                HttpContext.Current.Session[toggleStatusSessionName] = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void imbAddressToggle_Click(Object sender, EventArgs e)
        {

            bool state = false;
            // if fields are not visible make them so
            if (!toggleStatus)
            {
                ToggleAddressFields(true);
                state = true;

            }
            if (toggleStatus)
            {
                state = true;
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void imb_Click(Object sender, EventArgs e)
        {
            if (!this.enableValidaton)//this.isRequired &&
            {
                if ((Page.IsPostBack) & (this.tbxAdministrativeArea.Text.Length == 0) &
                    (this.tbxLocality.Text.Length == 0) & (this.tbxPaon.Text.Length == 0) & (this.tbxPostcode.Text.Length == 0)
                    & (this.tbxSaon.Text.Length == 0) & (this.tbxStreetDescriptor.Text.Length == 0) & (this.tbxTown.Text.Length == 0))
                {

                    Label error = new Label();
                    error.CssClass = "warning";
                    error.Text = "Please enter an address";
                    this.Controls.AddAt(0, error);

                }
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
        protected virtual void FormAddress_AddressConfirmed(object sender, AddressConfirmedEventArgs e)
        {

            DataSet ds = HttpContext.Current.Session["addresses"] as DataSet;

            // If the addresses are not in session, perhaps because the session expired while the user was distracted,
            // try to get the addresses again to prevent an error. This happens surprisingly often.
            if (ds == null)
            {
                using (AddressFinder addressfinder = new AddressFinder())
                {
                    try
                    {
                        ds = addressfinder.AddressesFromPostcode(this.tbxPostcode.Text);
                    }
                    catch (SoapException soapException)
                    {
                        SoapExceptionHelper helper = new SoapExceptionHelper(soapException);
                        lblMessage.Text = helper.Message;
                        return;
                    }
                }
            }


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["oid"].ToString() == e.Oid)
                {
                    // create a new dataset, add a table and a row with our address data
                    DataSet dsPafAddress = new DataSet();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("saon");
                    dt.Columns.Add("paon");
                    dt.Columns.Add("streetDescriptor");
                    dt.Columns.Add("locality");
                    dt.Columns.Add("town");
                    dt.Columns.Add("administrativeArea");
                    dt.Columns.Add("postcode");
                    dt.Columns.Add("northing");
                    dt.Columns.Add("easting");
                    dt.Columns.Add("wardCode");
                    dt.Columns.Add("oa");
                    dt.Columns.Add("oid");
                    dt.Columns.Add("pafCheckValid");

                    DataRow drPafAddress = dt.NewRow();

                    ToggleAddressFields(true);
                    HttpContext.Current.Session[toggleStatusSessionName] = true;

                    drPafAddress["saon"] = this.saon = tbxSaon.Text = dr["BD"].ToString();
                    //amended to cope with building numbers of 0 instead of nulls in the underlying data tables RG 06/03/2006
                    if (dr["BN"].ToString() != "0")
                    {
                        drPafAddress["paon"] = this.paon = tbxPaon.Text = dr["BN"].ToString();
                    }
                    else
                    {
                        drPafAddress["paon"] = this.paon = tbxPaon.Text = string.Empty;
                    }
                    drPafAddress["streetDescriptor"] = this.streetDescriptor = tbxStreetDescriptor.Text = dr["TN"].ToString();
                    // added locality 14/09/2006 as some postal addresses appear incorrect e.g. ringmer postcodes have ringmer as locality and lewes as the town
                    drPafAddress["locality"] = this.locality = tbxLocality.Text = dr["DL"].ToString();
                    drPafAddress["town"] = this.town = tbxTown.Text = dr["PT"].ToString();
                    drPafAddress["administrativeArea"] = this.administrativeArea = tbxAdministrativeArea.Text = dr["CN"].ToString();
                    drPafAddress["postcode"] = this.postcode = tbxPostcode.Text = dr["PC"].ToString();
                    drPafAddress["easting"] = this.easting = dr["Easting"].ToString();
                    drPafAddress["northing"] = this.northing = dr["Northing"].ToString();
                    drPafAddress["wardCode"] = DBNull.Value;//this.wardCode; //= dr["Ward_Code"].ToString(); removed due to missing column in new data RG 06/03/2006
                    drPafAddress["oa"] = this.oa = dr["OA"].ToString();
                    drPafAddress["oid"] = this.oid = e.Oid;
                    // at this point the address has been confirmed as a valid paf address although the user may subsequently change this before
                    // submitting the form
                    drPafAddress["pafCheckValid"] = this.pafCheckValid = true;

                    dt.Rows.Add(drPafAddress);
                    dsPafAddress.Tables.Add(dt);
                    HttpContext.Current.Session["pafaddress"] = dsPafAddress;
                    return;
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Message_PostcodeSubmitted(object sender, PostcodeSearchEventArgs e)
        {
            if (e.Postcode != "")
            {
                lblMessage.Text = "";
                AddressFinder addressfinder = new AddressFinder();
                DataSet ds = null;
                try
                {
                    ds = addressfinder.AddressesFromPostcode(e.Postcode);

                }
                catch (SoapException soapException)
                {
                    SoapExceptionHelper helper = new SoapExceptionHelper(soapException);
                    lblMessage.Text = helper.Message;
                    return;


                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    lblMessage.Text = "Please check that you have correctly entered an East Sussex postcode or add your address manually.";
                }
                else
                {
                    HttpContext.Current.Session["addresses"] = ds;
                    FormatDataSet(ds);
                    this.selectAddress.Visible = true;
                }
            }
            else
            {
                lblMessage.Text = "Please enter a postcode.";

            }
        }

        /// <summary>
        /// Check that the form data is valid when it's submitted, and raise a <c>PostcodeSubmitted</c> event if it is
        /// </summary>
        /// <param name="sender">The submit button</param>
        /// <param name="e">The click coordinates</param>
        private void imgbtnFind_Click(object sender, EventArgs e)
        {

            // this stops the click event for the button being fired if 
            // the user presses the enter key however it also prevents the button working if the user tabs to it before keying enter
            //if(e.X > 0 & e.Y > 0)
            //{			
            // raise event, and pass in valid data from textbox
            this.OnPostcodeSubmitted(this.tbxPostcode.Text);
            //}
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
                //Regex regex = new Regex("_ctl[0-9]{1,4}:lbxAddress");
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
                //We tested this above so valid to stop repeating the error message twice
                args.IsValid = true;
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

        /*		/// <summary>
                /// Gets or sets the introduction text above the address fields.
                /// </summary>
                /// <value>The introduction.</value>
                public string TextIntroduction
                {
                    get { return this.textIntro; }
                    set { this.textIntro = value; }
                }*/

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
    #region custom event argument classes
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
    #endregion
}
