using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using eastsussexgovuk.webservices.TextXhtml.HouseStyle;


namespace eastsussexgovuk.webservices.FormControls.genericforms
{
	/// <summary>
	/// The GenericForm class represents an html page
	/// including the meta data EGMS
	/// </summary>
	public class GenericForm
	{
		
		#region Private fields
		private string description = "";
		private string creator = "";
		private string gclCategories = "";
		private string lgclCategories = "";
		private string ipsvPreferredTerms = "";
		private string ipsvNonPreferredTerms = "";
		private string lgalTypes = "";
		private string publisher = "";
		private string spatialCoverage = "";
		private string keywords = "";
		private string lgtlType = "";
		private string dateCreated = "";
		private string dateModified = "";
		private string dateIssued = "";
		private string lgslNumbers = "";
		private string lgilType = "";
		private bool isInSearch = true;
		private string title = "";
		private FormQuestionCollection formQuestionCollection = new FormQuestionCollection();
		private bool showSummary = false;
		private bool isIntranet = false;
		private string departmentName = "";
		private bool isSurvey = false;
/*	*/
		//CODE_CHANGE 24/02/2006
	//	private string cmsChannelGUID = "";
	

		#endregion // Private fields

		#region Constructor
		
		/// <summary>
		///  Create a form with no Metadata, used in eform admin system
		/// </summary>
		public GenericForm()
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericForm"/> class.
        /// </summary>
        /// <param name="FormID">The form ID.</param>
		public GenericForm(int FormID)
		{
			DataSet ds = null;

			ds = GetMetadata(FormID);

			if (ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					this.lgalTypes = dr["LGALTypes"].ToString();
					this.spatialCoverage = dr["SpatialCoverage"].ToString();

					this.creator = dr["Creator"].ToString();
					
					if (dr["DateCreated"].ToString().Length > 0 )
					{
						this.dateCreated =  DateTimeFormatter.ISODate(DateTime.Parse(dr["DateCreated"].ToString()));
					}
					else
					{
						this.dateCreated = DateTime.Now.ToShortDateString();
					}
				
					 
					if (dr["DateIssued"].ToString().Length > 0 )
					{
						this.dateIssued =  DateTimeFormatter.ISODate(DateTime.Parse(dr["DateIssued"].ToString()));
					}
					else
					{
						this.dateIssued = DateTime.Now.ToShortDateString();
					}

                    this.title = dr["DisplayName"].ToString();
					this.description = dr["Description"].ToString();
					this.ipsvPreferredTerms = dr["IPSVPreferredTerms"].ToString();
					this.ipsvNonPreferredTerms = dr["IPSVNonPreferredTerms"].ToString();
					this.keywords = dr["Keywords"].ToString();
					this.lgslNumbers = dr["LGSLNumbers"].ToString();
					this.lgtlType= dr["LGTLType"].ToString();
					this.lgilType = dr["LGILType"].ToString();
					this.gclCategories = dr["GCLCategories"].ToString();

					//New field 25/1/2007 - summary page
					this.showSummary = Convert.ToBoolean(dr["ShowSummary"]);

					//New field 15/1/2007 - intranet genric form
					this.isIntranet = Convert.ToBoolean(dr["IsIntranet"]);
					this.departmentName = dr["Department"].ToString();
					this.isSurvey = Convert.ToBoolean(dr["IsSurvey"]);

				
					//CODE_CHANGE 24/02/2006
					//this.cmsChannelGUID = dr["CMSChannelGUID"].ToString();
				}
				
			}
			else
			{
				
			}

			

			
		}

        /// <summary>
        /// Gets metadata about the specified form
        /// </summary>
        /// <param name="FormID">The form ID.</param>
        /// <returns></returns>
		private DataSet GetMetadata(int FormID)
		{
				string Conn = ConfigurationManager.AppSettings["DbConnectionStringEformsReader"];
				SqlConnection	cn = new SqlConnection(Conn);
				SqlParameter prmFormID = new SqlParameter("@FormID", SqlDbType.Int);
				prmFormID.Value = FormID;
				
				DataSet ds  = null;	
				
			return ds = SqlHelper.ExecuteDataset(cn, CommandType.StoredProcedure, ConfigurationManager.AppSettings["EformsGetFormMetadata"], prmFormID);
		}



		#endregion

		#region Properties

		/// <summary>
		/// Flag to decide if the form is a survey, used to hide ref code on confirmation screen
		/// </summary>
		public bool IsSurvey
		{
			get
			{
				return this.isSurvey;
			}
			set
			{
				this.isSurvey = value;
			}
		}

		/// <summary>
		/// Title of page
		/// </summary>
		public string Title
		{

			get
			{
				return this.title;
			}
			set
			{
				this.title = value;
			}
		}
			
		/// <summary>
		/// Property Description (string)
		/// </summary>
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		/// <summary>
		/// Gets or sets a semi-colon separated list of audience types or profiles from the Local Government Audience List (LGAL)
		/// </summary>
		public string LgalTypes
		{
			get
			{
				return this.lgalTypes;
			}
			set
			{
				this.lgalTypes = value;
			}
		}
		
		

	
		
		/// <summary>
		/// Gets or sets the Local Government Interaction Type
		/// </summary>
		/// <remarks>The Local Government Interaction Type List is defined at http://www.esd.org.uk/standards/lgil/</remarks>
		public string LgilType
		{
			get
			{
				return this.lgilType;
			}
			set
			{
				this.lgilType = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the numeric process identifiers from the Local Government Service List (LGSL)
		/// </summary>
		/// <remarks>The LGSL is known on the ESD toolkit as the PID List</remarks>
		public string LgslNumbers
		{
			get
			{
				return this.lgslNumbers;
			}
			set
			{
				this.lgslNumbers = value;
			}
		}
		
	
	
		/// <summary>
		/// Gets or sets the date the document was published in ISO ccyy-mm-dd format
		/// </summary>
		public string DateIssued
		{
			get
			{
				return this.dateIssued;
			}
			set
			{
				this.dateIssued = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the date the document was modified in ISO ccyy-mm-dd format
		/// </summary>
		public string DateModified
		{
			get
			{
				return this.dateModified;
			}
			set
			{
				this.dateModified = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the date the document was created in ISO ccyy-mm-dd format
		/// </summary>
		public string DateCreated
		{
			get
			{
				return this.dateCreated;
			}
			set
			{
				this.dateCreated = value;
			}
		}

		
		
		/// <summary>
		/// Gets or sets the type of content - eg maps, minutes, graphs etc
		/// </summary>
		public string LgtlType
		{
			get
			{
				return this.lgtlType;
			}
			set
			{
				this.lgtlType = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the Integrated Public Sector Vocabulary (IPSV) preferred terms for the web page
		/// </summary>
		/// <remarks>Required in e-GMS 4. Terms should be separated using semi-colons.</remarks>
		public string IpsvPreferredTerms
		{
			get
			{
				return this.ipsvPreferredTerms;
			}
			set
			{
				this.ipsvPreferredTerms = value;
			}
		}

		/// <summary>
		/// Gets or sets the Integrated Public Sector Vocabulary (IPSV) non-preferred terms for the web page
		/// </summary>
		/// <remarks>Optional in e-GMS 4. Terms should be separated using semi-colons.</remarks>
		public string IpsvNonPreferredTerms
		{
			get
			{
				return this.ipsvNonPreferredTerms;
			}
			set
			{
				this.ipsvNonPreferredTerms = value;
			}
		}

		/// <summary>
		/// Gets or sets the additional keywords - those not taken from a controlled vocabulary
		/// </summary>
		/// <remarks>Keywords should be separated using semi-colons</remarks>
		public string Keywords
		{
			get
			{
				return this.keywords;
			}
			set
			{
				this.keywords = value;
			}
		}
		
		
		
		/// <summary>
		/// Gets or sets the geographical area covered by the web page
		/// </summary>
		public string SpatialCoverage
		{
			get
			{
				return this.spatialCoverage;
			}
			set
			{
				this.spatialCoverage = value;
			}
		}
		
		/// <summary>
		/// Gets or sets a semi-colon separated list of publishers - the entities who need to give permission should someone want to republish the data
		/// </summary>
		/// <remarks>This property is called "Publisher" rather than "Publishers" for backwards compatibility - it used only to accept one publisher.</remarks>
		public string Publisher
		{
			get
			{
				return this.publisher;
			}
			set
			{
				this.publisher = value;
			}
		}
		
		
		
		
		
		/// <summary>
		/// Gets or sets a semi-colon separated list of LAWs LGCL categories
		/// </summary>
		public string LgclCategories
		{
			get
			{
				return this.lgclCategories;
			}
			set
			{
				this.lgclCategories = value;
			}
		}


		/// <summary>
		/// Gets or sets a semi-colon separated list of Government Category List (GCL) categories
		/// </summary>
		public string GclCategories
		{
			get
			{
				return this.gclCategories;
			}
			set
			{
				this.gclCategories = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the entity primarily responsible for the content of the web page
		/// </summary>
		public string Creator
		{
			get
			{
				return this.creator;
			}
			set
			{
				this.creator = value;
			}
		}

        /// <summary>
        /// Gets or sets a value indicating whether this form should be indexed by search engines
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this form can be indexed by search engines; otherwise, <c>false</c>.
        /// </value>
		public bool IsInSearch
		{
			get
			{
				return this.isInSearch;
			}
			set
			{
				this.isInSearch = value;
			}
		}



        /// <summary>
        /// Gets or sets the form question collection.
        /// </summary>
        /// <value>The form question collection.</value>
		public FormQuestionCollection FormQuestionCollection
		{
			get
			{
				return this.formQuestionCollection;
			}
			set
			{
				this.formQuestionCollection = value;
			}
		}

        /// <summary>
        /// Gets or sets a value indicating whether to show a summary page
        /// </summary>
        /// <value><c>true</c> to show a summary page; otherwise, <c>false</c>.</value>
		public bool ShowSummary
		{
			get
			{
				return this.showSummary;
			}
			set
			{
				this.showSummary = value;
			}
		}

        /// <summary>
        /// Gets or sets a value indicating whether this instance is an intranet form.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is an intranet form; otherwise, <c>false</c>.
        /// </value>
		public bool IsIntranet
		{
			get
			{
				return this.isIntranet;
			}
			set
			{
				this.isIntranet = value;
			}
		}

        /// <summary>
        /// Gets or sets the name of the department.
        /// </summary>
        /// <value>The name of the department.</value>
		public string DepartmentName
		{
			get
			{
				return this.departmentName;
			}
			set
			{
				this.departmentName = value;
			}
		}
		//CODE_CHANGE 24/02/2006
		/*public string CMSChannelGUID
		{
			get
			{
				return this.cmsChannelGUID;
			}
		}*/
		#endregion // Properties
		
	}		
}
