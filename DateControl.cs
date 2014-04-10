using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using eastsussexgovuk.webservices.FormControls.Validators;
using EsccWebTeam.FormControls.Properties;
using EsccWebTeam.HouseStyle;

namespace eastsussexgovuk.webservices.FormControls
{
    /// <summary>
    /// Specify a date and time using dropdown lists
    /// </summary>
    [ToolboxData("<{0}:DateControl runat=server></{0}:DateControl>")]
    public class DateControl : System.Web.UI.WebControls.WebControl, INamingContainer
    {
        #region Fields
        private string questionId = "";
        private string questionRef = "";
        /// <summary>
        /// used to store eforms required value
        /// </summary>
        private bool required = false;
        private HtmlGenericControl legend;
        private HtmlGenericControl dateBox;
        private Label dayLabel;
        private Label monthLabel;
        private Label yearLabel;
        private bool enableValidaton = true;

        /// <summary>
        /// Store the CSS class for the whole container
        /// </summary>
        private string partCss = "formPart";

        /// <summary>
        /// Store the CSS class for the label
        /// </summary>
        private string labelCss = "formLabel";

        /// <summary>
        /// Store the CSS class for the control
        /// </summary>
        private string controlCss = "formControl";

        /// <summary>
        /// Store the first year to show in the year DropDownList
        /// </summary>
        private int firstYear = DateTime.Now.AddYears(-10).Year;

        /// <summary>
        /// Store the last year to show in the year DropDownList
        /// </summary>
        private int lastYear = DateTime.Now.Year;

        /// <summary>
        /// Store text legend for date fields
        /// </summary>
        private string label;

        /// <summary>
        /// Control to choose the days of the month
        /// </summary>
        private DropDownList dayList = new DropDownList();

        /// <summary>
        /// Control to choose the month of the year
        /// </summary>
        private DropDownList monthList = new DropDownList();

        /// <summary>
        /// Control to choose the year
        /// </summary>
        private DropDownList yearList = new DropDownList();

        /// <summary>
        /// Store date to select in case controls are repopulated
        /// </summary>
        /// <remarks>DateTime.MinValue used as stand-in for null, until we can use DateTime?</remarks>
        private DateTime dateToSelect = DateTime.MinValue;

        private bool showTime;
        private bool showDate = true;
        private Label hourLabel;
        private Label minLabel;
        private Label amPMLabel;
        private DropDownList hourList = new DropDownList();
        private DropDownList minList = new DropDownList();
        private DropDownList amPMList = new DropDownList();
        private int minInterval = 1;
        private string dayRequiredMessage;
        private string monthRequiredMessage;
        private string yearRequiredMessage;
        private string hourRequiredMessage;
        private string minuteRequiredMessage;
        private string hourMinuteMessage;
        private bool generateFieldset = true;
        private bool blankDate;
        private bool blankTime;

        private EsccCustomValidator vcDayReqMonthYear;
        private EsccCustomValidator vcMonthReqDayYear;
        private EsccCustomValidator vcYearReqDayMonth;
        private EsccRequiredFieldValidator validDay;
        private EsccRequiredFieldValidator validMonth;
        private EsccRequiredFieldValidator validYear;
        private EsccRequiredFieldValidator validHour;
        private EsccRequiredFieldValidator validMin;
        private EsccCustomValidator validHourMin1;
        private EsccCustomValidator validHourMin2;
        private string resourceFileName = typeof(EsccWebTeam_FormControls).Name;

        #endregion

        #region Constructors

        /// <summary>
        /// Specify a date and time using dropdown lists
        /// </summary>
        /// <remarks>Time has not yet been implemented - needs to remain optional</remarks>
        public DateControl()
            : base(HtmlTextWriterTag.Fieldset)
        {
            this.ShowBlankDateOption = true; // default to what happened before the option was added
            this.ShowBlankTimeOption = true; // default to what happened before the option was added
        }

        /// <summary>
        /// Specify a date and time using dropdown lists
        /// </summary>
        /// <param name="label">Text to be used for form control label</param>
        /// <remarks>Time has not yet been implemented - needs to remain optional</remarks>
        public DateControl(string label)
            : base(HtmlTextWriterTag.Fieldset)
        {
            this.label = label;

            this.ShowBlankDateOption = true; // default to what happened before the option was added
            this.ShowBlankTimeOption = true; // default to what happened before the option was added
        }

        /// <summary>
        /// OVERRIDDEN FOR EFORMS
        /// Specify a date and time using dropdown lists
        /// </summary>
        /// <param name="label">Text to be used for form control label</param>
        /// <param name="questionId"></param>
        /// <param name="questionRef"></param>
        /// <param name="required"></param>
        /// <param name="firstYear">Earliest year to offer in list of choices for the year</param>
        /// <param name="lastYear">Latest year to offer in list of choices for the year</param>
        public DateControl(string label, string questionId, string questionRef, string firstYear, string lastYear, bool required)
            : base(HtmlTextWriterTag.Fieldset)
        {
            this.firstYear = Convert.ToInt32(firstYear);
            this.lastYear = Convert.ToInt32(lastYear);
            this.label = label;

            this.required = required;
            this.questionId = questionId;
            this.questionRef = questionRef;

            this.ShowBlankDateOption = true; // default to what happened before the option was added
            this.ShowBlankTimeOption = true; // default to what happened before the option was added
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/>
        /// event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.EnsureChildControls();
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether the control should generate a fieldset around the three dropdown lists
        /// </summary>
        public bool GenerateFieldset
        {
            get
            {
                return this.generateFieldset;
            }
            set
            {
                this.generateFieldset = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DateControl"/> is required.
        /// </summary>
        /// <value><c>true</c> if required; otherwise, <c>false</c>.</value>
        public bool Required
        {
            get
            {
                return this.required;
            }
            set
            {
                this.required = value;

                this.ConfigureDateValidators();
                this.ConfigureTimeValidators();
            }
        }


        /// <summary>
        /// Gets or sets the error message to show if the year is missing when <see cref="Required"/> is set to <c>true</c>.
        /// </summary>
        /// <value>The error message.</value>
        public string YearRequiredMessage
        {
            get
            {
                return this.yearRequiredMessage;
            }
            set
            {
                this.yearRequiredMessage = value;
            }
        }


        /// <summary>
        /// Gets or sets the error message to show if the month is missing when <see cref="Required"/> is set to <c>true</c>.
        /// </summary>
        /// <value>The error message.</value>
        public string MonthRequiredMessage
        {
            get
            {
                return this.monthRequiredMessage;
            }
            set
            {
                this.monthRequiredMessage = value;
            }
        }


        /// <summary>
        /// Gets or sets the error message to show if the day is missing when <see cref="Required"/> is set to <c>true</c>.
        /// </summary>
        /// <value>The error message.</value>
        public string DayRequiredMessage
        {
            get
            {
                return this.dayRequiredMessage;
            }
            set
            {
                this.dayRequiredMessage = value;
            }
        }


        /// <summary>
        /// Gets or sets the error message to show if the hour is missing when <see cref="Required"/> is set to <c>true</c>.
        /// </summary>
        /// <value>The error message.</value>
        public string HourRequiredMessage
        {
            get
            {
                return this.hourRequiredMessage;
            }
            set
            {
                this.hourRequiredMessage = value;
            }
        }

        /// <summary>
        /// Gets or sets the error message to show if the minute is missing when <see cref="Required"/> is set to <c>true</c>.
        /// </summary>
        /// <value>The error message.</value>
        public string MinuteRequiredMessage
        {
            get
            {
                return this.minuteRequiredMessage;
            }
            set
            {
                this.minuteRequiredMessage = value;
            }
        }

        /// <summary>
        /// Gets or sets the error message to show if the hour is selected without the minute or vice versa.
        /// </summary>
        /// <value>The error message.</value>
        public string HourMinuteMessage
        {
            get
            {
                return this.hourMinuteMessage;
            }
            set
            {
                this.hourMinuteMessage = value;
            }
        }

        /// <summary>
        /// Gets or sets the interval between minutes to list as choices
        /// </summary>
        /// <value>The interval.</value>
        /// <example>Set to 5 minutes to allow choices of 0, 5, 10, 15 etc</example>
        public int MinuteInterval
        {
            get
            { return this.minInterval; }
            set
            {
                if (value <= 0)
                {
                    ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException(String.Empty, TextUtilities.ResourceString(resourceFileName, "ErrorMinuteInterval", EsccWebTeam_FormControls.ErrorMinuteInterval));
                    throw ex;
                }
                this.minInterval = value;
            }
        }

        ///TODO:added by RG to enable controls to be instantiated on the aspx page; is there another way to do this?
        /// <summary>
        /// gets or sets the control's label text which is used to set a unique ID for child controls
        /// </summary>
        /// <remarks>
        /// Code updated (lower down) by RM because labels can have spaces and punctuation in them, which makes ids invalid;
        /// </remarks>
        public String Label
        {
            get
            {
                return this.label;
            }
            set
            {
                this.label = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to show controls for setting the time
        /// </summary>
        public bool ShowDate
        {
            get { return this.showDate; }
            set { this.showDate = value; }
        }

        /// <summary>
        /// Gets or sets whether to show controls for setting the time
        /// </summary>
        public bool ShowTime
        {
            get { return this.showTime; }
            set { this.showTime = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show a blank option at the top of the day, month and year lists.
        /// </summary>
        /// <value><c>true</c> to show a blank option; otherwise, <c>false</c>.</value>
        public bool ShowBlankDateOption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show a blank option at the top of the hour and minute lists.
        /// </summary>
        /// <value><c>true</c> to show a blank option; otherwise, <c>false</c>.</value>
        public bool ShowBlankTimeOption { get; set; }

        /// <summary>
        /// Gets or sets the first year to show in the year DropDownList
        /// </summary>
        public int FirstYear
        {
            get { return this.firstYear; }
            set
            {
                this.firstYear = value;
                this.GenerateYearOptions();
            }
        }

        /// <summary>
        /// Gets or sets the last year to show in the year DropDownList
        /// </summary>
        public int LastYear
        {
            get { return this.lastYear; }
            set
            {
                this.lastYear = value;
                this.GenerateYearOptions();
            }
        }

        /// <summary>
        /// Gets or sets the date selected in the dropdown lists
        /// </summary>
        public DateTime SelectedDate
        {
            get
            {
                if (dateToSelect != null)
                {
                    return dateToSelect;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            set
            {
                this.dateToSelect = value;
                this.SelectDateTime();
            }
        }

        /// <summary>
        /// Gets or sets whether the dropdown lists have an partially or completely blank date/time selected
        /// </summary>
        public bool SelectedDateIsBlank
        {
            get
            {
                if (this.showDate && this.showTime)
                {
                    return (this.blankDate || this.blankTime);
                }
                else if (this.showDate && !this.showTime)
                {
                    return this.blankDate;
                }
                else if (!this.showDate && this.showTime)
                {
                    return this.blankTime;
                }
                else return true;
            }
            set
            {
                if (this.showDate) this.blankDate = value;
                if (this.showTime) this.blankTime = value;
                this.SelectDateTime();
            }
        }

        /// <summary>
        /// Public property so we can get and set the day dropdownlist
        /// </summary>
        public DropDownList DayList
        {
            get
            {
                return this.dayList;
            }
            set
            {
                this.dayList = value;
            }
        }

        /// <summary>
        /// Public property so we can get and set the month dropdownlist
        /// </summary>
        public DropDownList MonthList
        {
            get
            {
                return this.monthList;
            }
            set
            {
                this.monthList = value;
            }
        }

        /// <summary>
        /// Public property so we can get and set the year dropdownlist
        /// </summary>
        public DropDownList YearList
        {
            get
            {
                return this.yearList;
            }
            set
            {
                this.yearList = value;
            }
        }


        #endregion

        #region Create controls

        /// <summary>
        /// Notifies server controls that use composition-based implementation to create any child
        /// controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            if (this.label == null) this.label = "";

            // set up fieldset/legend
            if (this.generateFieldset)
            {
                // set up container for dropdown lists
                dateBox = new HtmlGenericControl("div");

                // set style
                this.CssClass = (this.CssClass + " " + this.partCss).TrimStart();
                dateBox.Attributes["class"] = this.controlCss + " dateControl";

                // set up legend 
                legend = new HtmlGenericControl("legend");
                legend.Attributes["class"] = this.labelCss;

                // TODO: should be innertext, but labels have been set using HTML for <span class="requiredField">
                legend.InnerHtml = this.label;
                if (this.required && this.label.IndexOf("<span") == -1)
                {
                    // This is a better way to do the asterisk
                    HtmlGenericControl asterisk = new HtmlGenericControl("span");
                    asterisk.Attributes["class"] = "requiredField";
                    asterisk.InnerText = "*";
                    legend.Controls.Add(asterisk);
                }

                // set up dropdown lists
                if (this.showDate) this.CreateDateControls(dateBox); // If date is to be selected
                if (this.showTime) this.CreateTimeControls(dateBox); // If time is to be selected 

                // add controls
                this.Controls.Add(legend);
                this.Controls.Add(dateBox);
            }
            else
            {
                // set up dropdown lists
                if (this.showDate) this.CreateDateControls(this); // If date is to be selected
                if (this.showTime) this.CreateTimeControls(this); // If time is to be selected 
            }

            // add validators
            if (this.showDate) this.CreateDateValidators();
            if (this.showTime) this.CreateTimeValidators();

            // select date
            GetDateFromForm();
            SelectDateTime();
        }

        /// <summary>
        /// Creates the controls to select the date.
        /// </summary>
        /// <param name="container">The container.</param>
        private void CreateDateControls(Control container)
        {
            // set up field labels
            dayLabel = new Label();
            monthLabel = new Label();
            yearLabel = new Label();

            // hide labels
            dayLabel.CssClass = "aural";
            monthLabel.CssClass = "aural";
            yearLabel.CssClass = "aural";

            // label used for ID, but it can contain all sorts of bad characters
            string tidyLabel = this.label.Replace(" ", "").Replace("(", "").Replace(")", "");

            // set up days
            dayList.Attributes["title"] = TextUtilities.ResourceString(resourceFileName, "DaySelectPopupText", EsccWebTeam_FormControls.DaySelectPopupText);
            if (this.ShowBlankDateOption) dayList.Items.Add(new ListItem());
            if (questionId != null && questionId.Length > 0)
            {
                dayList.ID = dayList.ID + questionId;
            }
            else
            {
                dayList.ID = tidyLabel + "day";
            }
            dayLabel.Text = TextUtilities.ResourceString(resourceFileName, "DatePartDayEntryPrompt", EsccWebTeam_FormControls.DatePartDayEntryPrompt);
            dayLabel.AssociatedControlID = dayList.ID;
            for (short i = 1; i <= 31; i++)
            {
                ListItem item = new ListItem(i.ToString(), i.ToString());
                dayList.Items.Add(item);
            }

            // set up months
            monthList.Attributes["title"] = TextUtilities.ResourceString(resourceFileName, "MonthSelectPopupText", EsccWebTeam_FormControls.MonthSelectPopupText);
            if (this.ShowBlankDateOption) monthList.Items.Add(new ListItem());
            if (questionId != null & questionId.Length > 0)
            {
                monthList.ID = monthList.ID + questionId;
            }
            else
            {
                monthList.ID = tidyLabel + "month";
            }
            monthLabel.Text = TextUtilities.ResourceString(resourceFileName, "DatePartMonthEntryPrompt", EsccWebTeam_FormControls.DatePartMonthEntryPrompt);
            monthLabel.AssociatedControlID = monthList.ID;
            string[] monthNames = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames;
            for (int i = 0; i < monthNames.Length; i++)
            {
                if (monthNames[i].Length > 0)
                {
                    int monthVal = i + 1;
                    ListItem item = new ListItem(monthNames[i], monthVal.ToString());
                    monthList.Items.Add(item);
                }
            }

            // set up year
            yearList.Attributes["title"] = TextUtilities.ResourceString(resourceFileName, "YearSelectPopupText", EsccWebTeam_FormControls.YearSelectPopupText);
            if (questionId != null & questionId.Length > 0)
            {
                yearList.ID = yearList.ID + questionId;
            }
            else
            {
                yearList.ID = tidyLabel + "year";
            }
            yearLabel.Text = TextUtilities.ResourceString(resourceFileName, "DatePartYearEntryPrompt", EsccWebTeam_FormControls.DatePartYearEntryPrompt);
            yearLabel.AssociatedControlID = yearList.ID;
            this.GenerateYearOptions();

            container.Controls.Add(dayLabel);
            container.Controls.Add(dayList);
            container.Controls.Add(monthLabel);
            container.Controls.Add(monthList);
            container.Controls.Add(yearLabel);
            container.Controls.Add(yearList);

        }

        /// <summary>
        /// Creates the controls to select the time.
        /// </summary>
        private void CreateTimeControls(Control container)
        {
            // label used for ID, but it can contain all sorts of bad characters
            string tidyLabel = this.label.Replace(" ", "").Replace("(", "").Replace(")", "");

            // set up hours
            if (this.ShowBlankTimeOption) this.hourList.Items.Add(new ListItem());
            if (questionId != null & questionId.Length > 0)
            {
                this.hourList.ID = this.hourList.ID + questionId;
            }
            else
            {
                this.hourList.ID = tidyLabel + "hour";
            }

            this.hourLabel = new Label();
            this.hourLabel.CssClass = "aural";
            this.hourLabel.Text = TextUtilities.ResourceString(resourceFileName, "DatePartHourEntryPrompt", EsccWebTeam_FormControls.DatePartHourEntryPrompt);
            this.hourLabel.AssociatedControlID = this.hourList.ID;
            for (int i = 1; i <= 12; i++)
            {
                ListItem item = new ListItem(i.ToString(CultureInfo.CurrentCulture), i.ToString(CultureInfo.CurrentCulture));
                this.hourList.Items.Add(item);
            }

            container.Controls.Add(this.hourLabel);
            container.Controls.Add(this.hourList);


            // set up minutes
            if (this.ShowBlankTimeOption) this.minList.Items.Add(new ListItem());
            if (questionId != null & questionId.Length > 0)
            {
                this.minList.ID = this.minList.ID + questionId;
            }
            else
            {
                this.minList.ID = tidyLabel + "min";
            }

            this.minLabel = new Label();
            this.minLabel.CssClass = "aural";
            this.minLabel.Text = TextUtilities.ResourceString(resourceFileName, "DatePartMinuteEntryPrompt", EsccWebTeam_FormControls.DatePartMinuteEntryPrompt);
            this.minLabel.AssociatedControlID = this.minList.ID;
            string minuteText;
            for (int i = 0; i <= 59; i = i + this.minInterval)
            {
                minuteText = i.ToString(CultureInfo.CurrentCulture);
                ListItem item = new ListItem((i < 10) ? "0" + minuteText : minuteText, minuteText);
                this.minList.Items.Add(item);
            }

            container.Controls.Add(this.minLabel);
            container.Controls.Add(this.minList);


            // AM or PM
            this.amPMList.Items.Add(new ListItem("am", "am"));
            this.amPMList.Items.Add(new ListItem("pm", "pm"));

            if (questionId != null & questionId.Length > 0)
            {
                this.amPMList.ID = this.amPMList.ID + questionId;
            }
            else
            {
                this.amPMList.ID = tidyLabel + "ampm";
            }

            this.amPMLabel = new Label();
            this.amPMLabel.CssClass = "aural";
            this.amPMLabel.Text = TextUtilities.ResourceString(resourceFileName, "DatePartAMPMEntryPrompt", EsccWebTeam_FormControls.DatePartAMPMEntryPrompt);
            this.amPMLabel.AssociatedControlID = this.amPMList.ID;


            container.Controls.Add(this.amPMLabel);
            container.Controls.Add(this.amPMList);

        }

        /// <summary>
        /// Prevent the opening tag from rendering if it's not wanted
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (this.generateFieldset)
            {
                base.RenderBeginTag(writer);
            }
        }

        /// <summary>
        /// Prevent the closing tag from rendering if it's not wanted
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            if (this.generateFieldset)
            {
                base.RenderEndTag(writer);
            }
        }



        #endregion

        #region Update selection in dropdown lists

        /// <summary>
        /// Select a date and time in the dropdown lists
        /// </summary>
        private void SelectDateTime()
        {
            if (dateToSelect != DateTime.MinValue)
            {
                this.EnsureChildControls();
                if (this.showDate) this.SelectDate();
                if (this.showTime) this.SelectTime();
            }
            this.OnDateSubmitted();
        }


        /// <summary>
        /// Selects the date in the dropdown lists. Helper method - use <seealso cref="SelectDateTime"/> instead.
        /// </summary>
        private void SelectDate()
        {
            this.dayList.ClearSelection();
            this.monthList.ClearSelection();
            this.yearList.ClearSelection();

            if (!this.blankDate)
            {
                string day = this.dateToSelect.Day.ToString(CultureInfo.CurrentCulture);
                ListItem dayItem = this.dayList.Items.FindByValue(day);
                if (dayItem != null) dayItem.Selected = true;

                string month = this.dateToSelect.Month.ToString(CultureInfo.CurrentCulture);
                ListItem monthItem = this.monthList.Items.FindByValue(month);
                if (monthItem != null) monthItem.Selected = true;

                string year = this.dateToSelect.Year.ToString(CultureInfo.CurrentCulture);
                ListItem yearItem = this.yearList.Items.FindByValue(year);
                if (yearItem != null) yearItem.Selected = true;
            }

        }

        /// <summary>
        /// Selects the time in the dropdown lists. Helper method - use <seealso cref="SelectDateTime"/> instead.
        /// </summary>
        private void SelectTime()
        {
            this.hourList.ClearSelection();
            this.minList.ClearSelection();
            this.amPMList.ClearSelection();

            if (!this.blankTime)
            {
                bool pm = false;
                string hour;

                // Convert 24 hour to 12 hour
                if (this.dateToSelect.Hour == 12)
                {
                    pm = true;
                    hour = "12";
                }
                else if (this.dateToSelect.Hour > 12)
                {
                    pm = true;
                    hour = (this.dateToSelect.Hour - 12).ToString(CultureInfo.CurrentCulture);
                }
                else if (this.dateToSelect.Hour == 0)
                {
                    hour = "12";
                }
                else
                {
                    hour = this.dateToSelect.Hour.ToString(CultureInfo.CurrentCulture);
                }

                ListItem hourItem = this.hourList.Items.FindByValue(hour);
                if (hourItem != null) hourItem.Selected = true;

                string min = this.dateToSelect.Minute.ToString(CultureInfo.CurrentCulture);
                ListItem minItem = this.minList.Items.FindByValue(min);
                if (minItem != null)
                {
                    minItem.Selected = true;
                }
                else if (this.minInterval > 1)
                {
                    // If the minute wasn't found it may be because, for example, we only have options for 
                    // every 15 minutes and the time to select is 10 past the hour.

                    // If so, reduce the minutes so that we select something slightly earlier 
                    // (earlier better than later because there'll always be a 0).
                    int exactMinutes = Int32.Parse(min);
                    int approxMinutes = exactMinutes - 1;
                    while (approxMinutes >= (exactMinutes - this.minInterval))
                    {
                        ListItem approxItem = this.minList.Items.FindByValue(approxMinutes.ToString(CultureInfo.CurrentCulture));
                        if (approxItem != null)
                        {
                            approxItem.Selected = true;
                            break;
                        }
                        else
                        {
                            approxMinutes--;
                        }

                    }


                }

                if (pm) this.amPMList.Items[1].Selected = true;
                else this.amPMList.Items[0].Selected = true;
            }
        }

        #endregion Update selection in dropdown lists

        #region Get selected date from dropdowns

        /// <summary>
        /// Gets the date from posted form values.
        /// </summary>
        public virtual void GetDateFromForm()
        {
            if (Context.Request.HttpMethod == "POST")
            {
                // This method won't work unless the list IDs have been set up
                this.EnsureChildControls();

                // Now set the selected date based on the posted values 
                var iNamingContainerPrefix = WorkOutNamingContainerPrefix();

                if (this.showDate && Context.Request.Form[iNamingContainerPrefix + this.dayList.ID] != null)
                {
                    string SelDay = Context.Request.Form[iNamingContainerPrefix + this.dayList.ID];
                    string SelMonth = Context.Request.Form[iNamingContainerPrefix + this.monthList.ID];
                    string SelYear = Context.Request.Form[iNamingContainerPrefix + this.yearList.ID];

                    if (SelDay != null && SelDay.Length > 0 && SelMonth != null && SelMonth.Length > 0 && SelYear != null && SelYear.Length > 0)
                    {
                        this.SelectedDate = AdjustIncorrectDaySelection(SelDay, SelMonth, SelYear, false);
                    }
                    else
                    {
                        this.SelectedDate = DateTime.MinValue;
                        this.blankDate = true;
                    }
                }

                // And modify it to show the selected time
                if (this.showTime && this.Context.Request.Form[iNamingContainerPrefix + this.hourList.ID] != null)
                {
                    string selectedHour = this.Context.Request.Form[iNamingContainerPrefix + this.hourList.ID];
                    string selectedMinute = this.Context.Request.Form[iNamingContainerPrefix + this.minList.ID];
                    string selectedAMPM = this.Context.Request.Form[iNamingContainerPrefix + this.amPMList.ID];

                    this.SelectedDate = this.AdjustTimeSelection(selectedHour, selectedMinute, selectedAMPM);

                    if (selectedHour == null || selectedHour.Length == 0 || selectedMinute == null || selectedMinute.Length == 0)
                    {
                        this.blankTime = true;
                    }
                }

            }
        }

        /// <summary>
        /// Works out the prefix that <see cref="INamingContainer"/> has added to our child controls
        /// </summary>
        /// <returns>INamingContainer prefix</returns>
        private string WorkOutNamingContainerPrefix()
        {
            var idToLookFor = (this.showDate) ? this.dayList.ID : this.hourList.ID;
            var iNamingContainerPrefix = this.UniqueID + this.IdSeparator + idToLookFor;

            foreach (string key in Context.Request.Form.Keys)
            {
                if (key.EndsWith(iNamingContainerPrefix))
                {
                    iNamingContainerPrefix = key.Substring(0, key.Length - idToLookFor.Length);
                    break;
                }
            }
            return iNamingContainerPrefix;
        }

        /// <summary>
        /// Updates the selected date with values for the time
        /// </summary>
        private DateTime AdjustTimeSelection(string selectedHour, string selectedMinute, string selectedAMPM)
        {
            int hour = (selectedHour.Length > 0) ? Int32.Parse(selectedHour) : 0;
            int min = (selectedMinute.Length > 0) ? Int32.Parse(selectedMinute) : 0;
            bool am = (String.Compare(selectedAMPM, "am", true, CultureInfo.CurrentCulture) == 0);
            return this.AdjustTimeSelection(hour, min, am);
        }
        /// <summary>
        /// Updates the selected date with values for the time
        /// </summary>
        private DateTime AdjustTimeSelection(int selectedHour, int selectedMinute, bool am)
        {
            DateTime dateToAdjust = this.SelectedDate.Date;

            // convert to 24-hour
            if (selectedHour == 12 && am) selectedHour = 0;
            if (selectedHour > 0 && selectedHour < 12 && !am) selectedHour = selectedHour + 12;

            // add time to date and return
            dateToAdjust = dateToAdjust.AddHours(selectedHour);
            dateToAdjust = dateToAdjust.AddMinutes(selectedMinute);

            return dateToAdjust;
        }
        /// <summary>
        /// Method to construct a dateTime object from integers for day, month and year.
        /// </summary>
        /// <param name="SelDay">integer representing the number of the day of the week</param>
        /// <param name="SelMonth">integer representing the number of the month of the year </param>
        /// <param name="SelYear">integer representing the year as a number</param>
        /// <param name="throwError">A boolean flag. When false incorrect day selections will be defaulted (e.g. a selection of day 31 in a 30 day month will be changed to 30; also deals with leap year errors). When true a simple custom error is thrown.</param>
        /// <returns>A valid DateTime object</returns>
        public virtual DateTime AdjustIncorrectDaySelection(int SelDay, int SelMonth, int SelYear, bool throwError)
        {
            DateTime date = DateTime.Now;
            // set boolean flags for thirty day months and leap years
            bool thirtyDayMonth = false;
            bool isLeapYear = DateTime.IsLeapYear(SelYear);
            // simple integer array holding 30 day months as integers
            int[] thirtyDayMonths = new int[4] { 4, 6, 9, 11 };
            // test if the user entered month is a thirty day month
            foreach (int m in thirtyDayMonths)
            {
                if (SelMonth == m) thirtyDayMonth = true;
            }
            // the incorrect day will be when the user chooses 31 in a 30 day month or 28th Feb in a leap year, set the day to 30 for the user unless the throwError parameter is true
            if (!throwError)
            {
                if (thirtyDayMonth & SelDay > 30) SelDay = 30;
                if (isLeapYear & SelMonth == 2 & SelDay > 29) SelDay = 29;
                if (!isLeapYear & SelMonth == 2 & SelDay > 28) SelDay = 28;
            }
            else
            {
                throw new FormatException("The selected day is incorrect");
            }
            //return a DateTime object	
            try
            {
                /*				CultureInfo ukCulture = new CultureInfo("en-GB");
                                date = DateTime.Parse(SelDay.ToString() +"/"+ SelMonth.ToString()  +"/"+ SelYear.ToString(), ukCulture.DateTimeFormat);	*/
                CultureInfo ukCulture = new CultureInfo("en-GB");
                date = DateTime.Parse(SelDay.ToString() + " " + ukCulture.DateTimeFormat.MonthNames[SelMonth - 1] + " " + SelYear.ToString(), ukCulture.DateTimeFormat);
            }
            catch (FormatException ex)
            {
                //throw ex;
                throw new FormatException("Could not recognise the date submitted.", ex);

            }
            return date;
        }

        /// <summary>
        /// Overload. Accepts numbers as string parameters.
        /// </summary>
        /// <param name="SelDay">string representing the number of the day of the week (e.g. "1" to "7")</param>
        /// <param name="SelMonth">string representing the number of the month of the year (e.g. "1" to "12")</param>
        /// <param name="SelYear">string representing the year as a number (e.g. "2005")</param>
        /// <param name="throwError">A boolean flag. When false incorrect day selections will be defaulted (e.g. a selection of day 31 in a 30 day month will be changed to 30; also deals with leap year errors). When true a simple custom error is thrown.</param>
        /// <returns>A valid DateTime object</returns>
        private DateTime AdjustIncorrectDaySelection(string SelDay, string SelMonth, string SelYear, bool throwError)
        {
            // convert string parameters to integer types
            int day = Convert.ToInt32(SelDay);
            int month = Convert.ToInt32(SelMonth);
            int year = Convert.ToInt32(SelYear);
            return AdjustIncorrectDaySelection(day, month, year, throwError);
        }

        /// <summary>
        /// Whenever range of years changes, we need to regenerate the options in the dropdown list
        /// </summary>
        private void GenerateYearOptions()
        {
            this.yearList.Items.Clear();
            if (this.ShowBlankDateOption) this.yearList.Items.Add(new ListItem());

            if (this.firstYear <= this.lastYear)
            {
                for (int i = firstYear; i <= this.lastYear; i++)
                {
                    ListItem item = new ListItem(i.ToString(), i.ToString());
                    this.yearList.Items.Add(item);
                }
            }
            else
            {
                for (int i = firstYear; i >= this.lastYear; i--)
                {
                    ListItem item = new ListItem(i.ToString(), i.ToString());
                    this.yearList.Items.Add(item);
                }
            }
        }

        #endregion Get selected date from dropdowns

        #region Validation

        /// <summary>
        /// When <see cref="Required" /> is set to <c>true</c>, use ASP.NET validators to check that a valid date has been entered
        /// </summary>
        public bool EnableValidation
        {
            get { return this.enableValidaton; }
            set { this.enableValidaton = value; }
        }

        /// <summary>
        /// Validator ensuring a complete date must be selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DayRequiresMonthAndYearValidator(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = (this.monthList.SelectedIndex > 0 && this.yearList.SelectedIndex > 0);
        }

        /// <summary>
        /// Validator ensuring a complete date must be selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthRequiresDayAndYearValidator(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = (this.dayList.SelectedIndex > 0 && this.yearList.SelectedIndex > 0);
        }

        /// <summary>
        /// Validator ensuring a complete date must be selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YearRequiresMonthAndDayValidator(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = (this.dayList.SelectedIndex > 0 && this.monthList.SelectedIndex > 0);
        }

        /// <summary>
        /// Handles the ServerValidate event of the vcHourMinute control to ensure one control isn't selected without the other.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.</param>
        private void vcHourMinute_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ((this.hourList.SelectedIndex > 0) == (this.minList.SelectedIndex > 0));
        }


        /// <summary>
        /// Creates validators for the fields used to select the time.
        /// </summary>
        private void CreateTimeValidators()
        {
            if (this.enableValidaton)
            {
                this.validHour = new EsccRequiredFieldValidator();
                this.validHour.ControlToValidate = this.hourList.ID;
                this.Controls.Add(this.validHour);

                this.validMin = new EsccRequiredFieldValidator();
                this.validMin.ControlToValidate = this.minList.ID;
                this.Controls.Add(this.validMin);

                // Add same validator to both controls, because it'll only fire on the one with something selected
                this.validHourMin1 = new EsccCustomValidator();
                this.validHourMin1.ControlToValidate = this.hourList.ID;
                this.validHourMin1.ServerValidate += new ServerValidateEventHandler(vcHourMinute_ServerValidate);
                this.Controls.Add(this.validHourMin1);

                this.validHourMin2 = new EsccCustomValidator();
                this.validHourMin2.ControlToValidate = this.minList.ID;
                this.validHourMin2.ServerValidate += new ServerValidateEventHandler(vcHourMinute_ServerValidate);
                this.Controls.Add(this.validHourMin2);

                this.ConfigureTimeValidators();
            }
        }


        /// <summary>
        /// Creates validators for the fields used to select the date.
        /// </summary>
        private void CreateDateValidators()
        {
            if (this.enableValidaton)
            {
                // Add required validators
                this.validDay = new EsccRequiredFieldValidator();
                this.validDay.ControlToValidate = dayList.ID;
                this.Controls.Add(this.validDay);

                this.validMonth = new EsccRequiredFieldValidator();
                this.validMonth.ControlToValidate = monthList.ID;
                this.Controls.Add(this.validMonth);

                this.validYear = new EsccRequiredFieldValidator();
                this.validYear.ControlToValidate = yearList.ID;
                this.Controls.Add(this.validYear);

                // Add non-required validators
                vcDayReqMonthYear = new EsccCustomValidator();
                vcDayReqMonthYear.ControlToValidate = dayList.ID;
                vcDayReqMonthYear.ServerValidate += new ServerValidateEventHandler(this.DayRequiresMonthAndYearValidator);
                this.Controls.Add(vcDayReqMonthYear);

                vcMonthReqDayYear = new EsccCustomValidator();
                vcMonthReqDayYear.ControlToValidate = monthList.ID;
                vcMonthReqDayYear.ServerValidate += new ServerValidateEventHandler(this.MonthRequiresDayAndYearValidator);
                this.Controls.Add(vcMonthReqDayYear);

                vcYearReqDayMonth = new EsccCustomValidator();
                vcYearReqDayMonth.ControlToValidate = yearList.ID;
                vcYearReqDayMonth.ServerValidate += new ServerValidateEventHandler(this.YearRequiresMonthAndDayValidator);
                this.Controls.Add(vcYearReqDayMonth);

                this.ConfigureDateValidators();
            }
        }

        /// <summary>
        /// Makes sure the date validators reflect the values of the <see cref="Required"/> property and various error message properties
        /// </summary>
        private void ConfigureDateValidators()
        {
            if (this.validDay != null)
            {
                // Reset message for required validators
                if (this.dayRequiredMessage != null && this.dayRequiredMessage.Length > 0)
                {
                    this.validDay.ErrorMessage = this.dayRequiredMessage;
                }
                else
                {
                    this.validDay.ErrorMessage = "question " + questionRef + "requires the day to be completed ";
                }

                if (this.monthRequiredMessage != null && this.monthRequiredMessage.Length > 0)
                {
                    this.validMonth.ErrorMessage = this.monthRequiredMessage;
                }
                else
                {
                    this.validMonth.ErrorMessage = "question " + questionRef + "requires the month to be completed ";
                }

                if (this.yearRequiredMessage != null && this.yearRequiredMessage.Length > 0)
                {
                    this.validYear.ErrorMessage = this.yearRequiredMessage;
                }
                else
                {
                    this.validYear.ErrorMessage = "question " + questionRef + "requires the year to be completed ";
                }

                // No message property defined yet for the partial date validators
                this.vcDayReqMonthYear.ErrorMessage = TextUtilities.ResourceString(resourceFileName, "DateRequiresMoreThanDayError", EsccWebTeam_FormControls.DateRequiresMoreThanDayError);
                this.vcMonthReqDayYear.ErrorMessage = TextUtilities.ResourceString(resourceFileName, "DateRequiresMoreThanMonthError", EsccWebTeam_FormControls.DateRequiresMoreThanMonthError);
                this.vcYearReqDayMonth.ErrorMessage = TextUtilities.ResourceString(resourceFileName, "DateRequiresMoreThanYearError", EsccWebTeam_FormControls.DateRequiresMoreThanYearError);

                // Enable/disable validators
                this.validDay.Enabled = this.required;
                this.validMonth.Enabled = this.required;
                this.validYear.Enabled = this.required;

                this.vcDayReqMonthYear.Enabled = !this.required;
                this.vcMonthReqDayYear.Enabled = !this.required;
                this.vcYearReqDayMonth.Enabled = !this.required;
            }
        }

        /// <summary>
        /// Makes sure the time validators reflect the values of the <see cref="Required"/> property and various error message properties
        /// </summary>
        private void ConfigureTimeValidators()
        {
            if (this.validHour != null)
            {
                // Error messages
                this.validHour.ErrorMessage = this.hourRequiredMessage;
                this.validMin.ErrorMessage = this.minuteRequiredMessage;
                this.validHourMin1.ErrorMessage = (this.hourMinuteMessage != null && this.hourMinuteMessage.Length > 0) ? this.hourMinuteMessage : TextUtilities.ResourceString(resourceFileName, "ErrorHourMinuteRequireEachOther", EsccWebTeam_FormControls.ErrorHourMinuteRequireEachOther);
                this.validHourMin2.ErrorMessage = this.validHourMin1.ErrorMessage;

                // Enable/disable validators
                this.validHour.Enabled = this.required;
                this.validMin.Enabled = this.required;

                this.validHourMin1.Enabled = !this.required;
                this.validHourMin2.Enabled = !this.required;
            }

        }

        #endregion Validation

        #region custom events
        /// <summary>
        /// Event indicating that a date been submitted
        /// </summary>
        public event DateSubmittedEventHandler DateSubmitted;

        /// <summary>
        /// Event handler delegate for <c>DateSubmitted</c> event.
        /// </summary>
        public delegate void DateSubmittedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Raise event indicating that a date has been submitted
        /// </summary>
        protected void OnDateSubmitted()
        {
            // check there are handlers for the event before raising
            if (this.DateSubmitted != null)
            {
                // raise event
                this.DateSubmitted(this, new EventArgs());
            }
        }
        #endregion

    }
}
