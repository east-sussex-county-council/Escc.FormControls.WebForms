using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;

namespace eastsussexgovuk.webservices.FormControls.CustomControls
{
	/// <summary>
	/// A date control which allows a date to be typed into a TextBox or selected from a popup calendar
	/// </summary>
	public class DatePicker:Control, INamingContainer
	{
		#region Private Properties
		private string m_closeGif = "close.gif";
		private string m_dividerGif = "divider.gif";
		private string m_drop1Gif = "drop1.gif";
		private string m_drop2Gif = "drop2.gif";
		private string m_left1Gif = "left1.gif";
		private string m_left2Gif = "left2.gif";
		private string m_right1Gif = "right1.gif";
		private string m_right2Gif = "right2.gif";
		private string m_imgDirectory = "/img/";
		private string m_ControlCssClass = "";
		private string m_text = "";
		private string m_Css = "";
		private string m_DateType = "dd mmm yyyy";
		private bool isRequired = false;
		private string questionRef;
		private Label label = new Label();
		
		#endregion

		#region Public Properties
		
		/// <summary>
		/// Gets or sets the number of the question being asked, eg question 1, question 2, question 3
		/// </summary>
        public string QuestionRef
		{
			get
			{
				return this.questionRef;
			}
			set
			{
				this.questionRef = value;
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

        /// <summary>
        /// Gets or sets the label for the date control
        /// </summary>
        /// <value>The label.</value>
		public Label Label
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
		/// Gets or sets the text value to be parsed as a date
		/// </summary>
        public string Text
		{
			get 
			{
				if (this.Controls.Count == 0)
					return "";
				return ((System.Web.UI.WebControls.TextBox)Controls[1]).Text;
			}
			set
			{
				if (this.Controls.Count != 0)
				{
					((System.Web.UI.WebControls.TextBox)Controls[1]).Text = value;
					m_text = "";
				}
				else
					m_text = value;
			}
		}
        /// <summary>
        /// Gets or sets the CSS class of the TextBox
        /// </summary>
        /// <value>The CSS class.</value>
		public string CssClass
		{
			get 
			{
				if (this.Controls.Count == 0)
					return "";
				return ((System.Web.UI.WebControls.TextBox)Controls[1]).CssClass;
			}
			set 
			{
				if (this.Controls.Count != 0)
				{
					((System.Web.UI.WebControls.TextBox)Controls[1]).CssClass = value;
					m_Css = value;
				}
				else
					m_Css = value;
			}
		}
		/// <summary>
		/// Gets or sets the close image
		/// </summary>
		public string imgClose
		{
			get {return m_closeGif;}
			set {m_closeGif = value;}
		}
		/// <summary>
        /// Gets or sets the divider image
		/// </summary>
		public string imgDivider
		{
			get {return m_dividerGif;}
			set {m_dividerGif = value;}
		}
		/// <summary>
        /// Gets or sets the drop image
		/// </summary>
		public string imgDrop1
		{
			get {return m_drop1Gif;}
			set {m_drop1Gif = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string imgDrop2
		{
			get {return m_drop2Gif;}
			set {m_drop2Gif = value;}
		}
		public string imgLeft1
		{
			get {return m_left1Gif;}
			set {m_left1Gif = value;}
		}
		public string imgLeft2
		{
			get {return m_left2Gif;}
			set {m_left2Gif = value;}
		}
		public string imgRight1
		{
			get {return m_right1Gif;}
			set {m_right1Gif = value;}
		}
		public string imgDirectory
		{
			get {return m_imgDirectory;}
			set {m_imgDirectory = value;}
		}
		public string ControlCssClass
		{
			get {return m_ControlCssClass;}
			set {m_ControlCssClass = value;}
		}
		public string DateType
		{
			get {return m_DateType;}
			set {m_DateType = value;}
		}
		#endregion

		#region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DatePicker"/> class.
        /// </summary>
		public DatePicker()
		{
			
		}
		#endregion

		#region Private Methods/Properties
		private void placeJavascript()
		{
			string strBuildUp = "<script type=\"text/JavaScript\">";
			strBuildUp += "var	fixedX = -1	// x position (-1 if to appear below control)\n";
            strBuildUp += "var	fixedY = -1			// y position (-1 if to appear below control)\n";
            strBuildUp += "var startAt = 1			// 0 - sunday ; 1 - monday\n";
            strBuildUp += "var showWeekNumber = 1	// 0 - don't show; 1 - show\n";
            strBuildUp += "var showToday = 1		// 0 - don't show; 1 - show\n";
            strBuildUp += @"var imgDir = """ + m_imgDirectory + @"""" + "\n";
			strBuildUp += @"var gotoString = ""Go To Current Month""" + "\n";
            strBuildUp += @"var todayString = ""Today is""" + "\n";
            strBuildUp += @"var weekString = ""Wk""" + "\n";
            strBuildUp += @"var scrollLeftMessage = ""Click to scroll to previous month. Hold mouse button to scroll automatically.""" + "\n";
            strBuildUp += @"var scrollRightMessage = ""Click to scroll to next month. Hold mouse button to scroll automatically.""" + "\n";
            strBuildUp += @"var selectMonthMessage = ""Click to select a month."""  + "\n";
            strBuildUp += @"var selectYearMessage = ""Click to select a year.""" + "\n";
            strBuildUp += @"var selectDateMessage = ""Select [date] as date."" // do not replace [date], it will be replaced by date." + "\n";
			strBuildUp += "var	crossobj, crossMonthObj, crossYearObj, monthSelected, yearSelected, dateSelected, omonthSelected, oyearSelected, odateSelected, monthConstructed, yearConstructed, intervalID1, intervalID2, timeoutID1, timeoutID2, ctlToPlaceValue, ctlNow, dateFormat, nStartingYear\n\n";
            strBuildUp += "var	bPageLoaded=false\n";
            strBuildUp += "var	ie=document.all\n";
            strBuildUp += "var	dom=document.getElementById\n\n";
            strBuildUp += "var	ns4=document.layers\n";
            strBuildUp += "var	today =	new	Date()\n" ;
            strBuildUp += "var	dateNow	 = today.getDate()\n";
            strBuildUp += "var	monthNow = today.getMonth()\n";
            strBuildUp += "var	yearNow	 = today.getYear()\n";
            strBuildUp += @"var	imgsrc = new Array(""" + m_drop1Gif + @""",""" + m_drop2Gif + @""",""" + m_left1Gif + @""",""" + m_left2Gif + @""",""" + m_right1Gif + @""",""" + m_right2Gif + @""")" + "\n";
            strBuildUp += "var	img	= new Array()\n\n";
            strBuildUp += "var bShow = false;\n\n";
            strBuildUp += "/* hides <select> and <applet> objects (for IE only) */\n";
            strBuildUp += "function hideElement( elmID, overDiv )\n";
			strBuildUp += "{\n";
			strBuildUp += "if( ie )\n";
            strBuildUp += "{\n";
            strBuildUp += "for( i = 0; i < document.all.tags( elmID ).length; i++ )\n";
            strBuildUp += "{\n";
            strBuildUp += "obj = document.all.tags( elmID )[i];\n";
            strBuildUp += "if( !obj || !obj.offsetParent )\n";
            strBuildUp += "{\n";
            strBuildUp += "continue;\n";
            strBuildUp += "}\n\n";
            strBuildUp += "// Find the element's offsetTop and offsetLeft relative to the BODY tag.\n";
            strBuildUp += "objLeft   = obj.offsetLeft;\n";
            strBuildUp += "objTop    = obj.offsetTop;\n";
            strBuildUp += "objParent = obj.offsetParent;\n\n";
            strBuildUp += @"while( objParent.tagName.toUpperCase() != ""BODY"" )" + "\n";
            strBuildUp += "{\n";
            strBuildUp += "objLeft  += objParent.offsetLeft;\n";
            strBuildUp += "objTop   += objParent.offsetTop;\n";
            strBuildUp += "objParent = objParent.offsetParent;\n";
            strBuildUp += "}\n\n";
            strBuildUp += "objHeight = obj.offsetHeight;\n";
            strBuildUp += "objWidth = obj.offsetWidth;\n\n";
            strBuildUp += "if(( overDiv.offsetLeft + overDiv.offsetWidth ) <= objLeft );\n";
            strBuildUp += "else if(( overDiv.offsetTop + overDiv.offsetHeight ) <= objTop );\n";
            strBuildUp += "else if( overDiv.offsetTop >= ( objTop + objHeight ));\n";
            strBuildUp += "else if( overDiv.offsetLeft >= ( objLeft + objWidth ));\n";
            strBuildUp += "else\n";
            strBuildUp += "{\n";
            strBuildUp += @"obj.style.visibility = ""hidden"";" + "\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n\n";
			strBuildUp += "/*\n";
            strBuildUp += "* unhides <select> and <applet> objects (for IE only)\n";
            strBuildUp += "*/\n";
            strBuildUp += "function showElement( elmID )\n";
            strBuildUp += "{\n";
            strBuildUp += "if( ie )\n";
            strBuildUp += "{\n";
            strBuildUp += "for( i = 0; i < document.all.tags( elmID ).length; i++ )\n";
            strBuildUp += "{\n";
            strBuildUp += "obj = document.all.tags( elmID )[i];\n\n";
            strBuildUp += "if( !obj || !obj.offsetParent )\n";
            strBuildUp += "{\n";
            strBuildUp += "continue;\n";
            strBuildUp += "}\n\n";
            strBuildUp += @"obj.style.visibility = """";" + "\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function HolidayRec (d, m, y, desc)\n";
            strBuildUp += "{\n";
            strBuildUp += "this.d = d\n";
            strBuildUp += "this.m = m\n";
            strBuildUp += "this.y = y\n";
            strBuildUp += "this.desc = desc\n";
            strBuildUp += "}\n\n";
            strBuildUp += "var HolidaysCounter = 0\n";
            strBuildUp += "var Holidays = new Array()\n\n";
            strBuildUp += "function addHoliday (d, m, y, desc)\n";
            strBuildUp += "{\n";
            strBuildUp += "Holidays[HolidaysCounter++] = new HolidayRec ( d, m, y, desc )\n";
            strBuildUp += "}\n\n";
			strBuildUp += "if (dom)\n";
            strBuildUp += "{\n";
            strBuildUp += "for	(i=0;i<imgsrc.length;i++)\n";
            strBuildUp += "{\n";
            strBuildUp += "img[i] = new Image\n";
            strBuildUp += "img[i].src = imgDir + imgsrc[i]\n";
            strBuildUp += "}\n";
            strBuildUp += @"document.write (""<div onclick='bShow=true' id='calendar'	style='z-index:+999;position:absolute;visibility:hidden;'><table	width=""+((showWeekNumber==1)?250:220)+"" style='font-family:arial;font-size:11px;border-width:1;border-style:solid;border-color:#a0a0a0;font-family:arial; font-size:11px}' bgcolor='#ffffff'><tr bgcolor='#0000aa'><td><table width='""+((showWeekNumber==1)?248:218)+""'><tr><td style='padding:2px;font-family:arial; font-size:11px;'><font color='#ffffff'><strong><span id='caption'></span></strong></font></td><td align=right><a href='javascript:hideCalendar()'><img src='""+imgDir+""" + m_closeGif + @"' width='15' height='13' border='0' alt='Close the Calendar'></a></td></tr></table></td></tr><tr><td style='padding:5px' bgcolor=#ffffff><span id='content'></span></td></tr>"")" + "\n\n";
            strBuildUp += "if (showToday==1)\n";
            strBuildUp += "{\n";
            strBuildUp += @"document.write (""<tr bgcolor=#f0f0f0><td style='padding:5px' align=center><span id='lblToday'></span></td></tr>"")" + "\n";
            strBuildUp += "}\n\n";
            strBuildUp += @"document.write (""</table></div><div id='selectMonth' style='z-index:+999;position:absolute;visibility:hidden;'></div><div id='selectYear' style='z-index:+999;position:absolute;visibility:hidden;'></div>"");" + "\n";
            strBuildUp += "}\n\n";
            strBuildUp += @"var	monthName =	new	Array(""January"",""February"",""March"",""April"",""May"",""June"",""July"",""August"",""September"",""October"",""November"",""December"")" + "\n";
            strBuildUp += "if (startAt==0)\n";
            strBuildUp += "{\n";
            strBuildUp += @"dayName = new Array	(""Sun"",""Mon"",""Tue"",""Wed"",""Thu"",""Fri"",""Sat"")" + "\n";
            strBuildUp += "}\n";
            strBuildUp += "else\n";
            strBuildUp += "{\n";
            strBuildUp += @"dayName = new Array	(""Mon"",""Tue"",""Wed"",""Thu"",""Fri"",""Sat"",""Sun"")" + "\n";
            strBuildUp += "}\n";
            strBuildUp += @"var	styleAnchor=""text-decoration:none;color:black;""" + "\n";
            strBuildUp += @"var	styleLightBorder=""border-style:solid;border-width:1px;border-color:#a0a0a0;""" + "\n\n";
            strBuildUp += "function swapImage(srcImg, destImg){\n";
            strBuildUp += @"if (ie)	{ document.getElementById(srcImg).setAttribute(""src"",imgDir + destImg) }" + "\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function init()	{\n";
            strBuildUp += "if (!ns4)\n";
            strBuildUp += "{\n";
			strBuildUp += "if (!ie) { yearNow += 1900	}\n\n";
            strBuildUp += @"crossobj=(dom)?document.getElementById(""calendar"").style : ie? document.all.calendar : document.calendar" + "\n";
            strBuildUp += "hideCalendar()\n\n";
            strBuildUp += @"crossMonthObj=(dom)?document.getElementById(""selectMonth"").style : ie? document.all.selectMonth	: document.selectMonth" + "\n\n";
            strBuildUp += @"crossYearObj=(dom)?document.getElementById(""selectYear"").style : ie? document.all.selectYear : document.selectYear" + "\n\n";
            strBuildUp += "monthConstructed=false;\n";
            strBuildUp += "yearConstructed=false;\n\n";
            strBuildUp += "if (showToday==1)\n";
            strBuildUp += "{\n";
            strBuildUp += @"document.getElementById(""lblToday"").innerHTML =	todayString + "" <a onmousemove='window.status=\""""+gotoString+""\""' onmouseout='window.status=\""\""' title='""+gotoString+""' style='""+styleAnchor+""' href='javascript:monthSelected=monthNow;yearSelected=yearNow;constructCalendar();'>""+dayName[(today.getDay()-startAt==-1)?6:(today.getDay()-startAt)]+"", "" + dateNow + "" "" + monthName[monthNow].substring(0,3)	+ ""	"" +	yearNow	+ ""</a>""" + "\n";
            strBuildUp += "}\n\n";
            strBuildUp += @"sHTML1=""<span id='spanLeft'	style='border-style:solid;border-width:1;border-color:#3366FF;cursor:pointer' onmouseover='swapImage(\""changeLeft\"",\""left2.gif\"");this.style.borderColor=\""#88AAFF\"";window.status=\""""+scrollLeftMessage+""\""' onclick='javascript:decMonth()' onmouseout='clearInterval(intervalID1);swapImage(\""changeLeft\"",\""left1.gif\"");this.style.borderColor=\""#3366FF\"";window.status=\""\""' onmousedown='clearTimeout(timeoutID1);timeoutID1=setTimeout(\""StartDecMonth()\"",500)'	onmouseup='clearTimeout(timeoutID1);clearInterval(intervalID1)'>&nbsp<img id='changeLeft' src='""+imgDir+""left1.gif' width=10 height=11 border=0>&nbsp</span>&nbsp;""" + "\n";
            strBuildUp += @"sHTML1+=""<span id='spanRight' style='border-style:solid;border-width:1;border-color:#3366FF;cursor:pointer'	onmouseover='swapImage(\""changeRight\"",\""right2.gif\"");this.style.borderColor=\""#88AAFF\"";window.status=\""""+scrollRightMessage+""\""' onmouseout='clearInterval(intervalID1);swapImage(\""changeRight\"",\""right1.gif\"");this.style.borderColor=\""#3366FF\"";window.status=\""\""' onclick='incMonth()' onmousedown='clearTimeout(timeoutID1);timeoutID1=setTimeout(\""StartIncMonth()\"",500)'	onmouseup='clearTimeout(timeoutID1);clearInterval(intervalID1)'>&nbsp<img id='changeRight' src='""+imgDir+""right1.gif'	width=10 height=11 border=0>&nbsp</span>&nbsp""" + "\n";
            strBuildUp += @"sHTML1+=""<span id='spanMonth' style='border-style:solid;border-width:1;border-color:#3366FF;cursor:pointer'	onmouseover='swapImage(\""changeMonth\"",\""drop2.gif\"");this.style.borderColor=\""#88AAFF\"";window.status=\""""+selectMonthMessage+""\""' onmouseout='swapImage(\""changeMonth\"",\""drop1.gif\"");this.style.borderColor=\""#3366FF\"";window.status=\""\""' onclick='popUpMonth()'></span>&nbsp;""" + "\n";
            strBuildUp += @"sHTML1+=""<span id='spanYear' style='border-style:solid;border-width:1;border-color:#3366FF;cursor:pointer' onmouseover='swapImage(\""changeYear\"",\""drop2.gif\"");this.style.borderColor=\""#88AAFF\"";window.status=\""""+selectYearMessage+""\""'	onmouseout='swapImage(\""changeYear\"",\""drop1.gif\"");this.style.borderColor=\""#3366FF\"";window.status=\""\""'	onclick='popUpYear()'></span>&nbsp;""" + "\n\n";
            strBuildUp += @"document.getElementById(""caption"").innerHTML  =	sHTML1" + "\n\n";
            strBuildUp += "bPageLoaded=true\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function hideCalendar()	{\n";
            strBuildUp += @"crossobj.visibility=""hidden""" + "\n";
            strBuildUp += @"if (crossMonthObj != null){crossMonthObj.visibility=""hidden""}" + "\n";
            strBuildUp += @"if (crossYearObj !=	null){crossYearObj.visibility=""hidden""}" + "\n\n";
            strBuildUp += "showElement( 'SELECT' );\n";
            strBuildUp += "showElement( 'APPLET' );\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function padZero(num) {\n";
			strBuildUp += "return (num	< 10)? '0' + num : num ;\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function constructDate(d,m,y)\n";
            strBuildUp += "{\n";
            strBuildUp += "sTmp = dateFormat\n";
            strBuildUp += @"sTmp = sTmp.replace	(""dd"",""<e>"")" + "\n";
            strBuildUp += @"sTmp = sTmp.replace	(""d"",""<d>"")" + "\n";
            strBuildUp += @"sTmp = sTmp.replace	(""<e>"",padZero(d))"  + "\n";
            strBuildUp += @"sTmp = sTmp.replace	(""<d>"",d)" + "\n";
            strBuildUp += @"sTmp = sTmp.replace	(""mmm"",""<o>"")" + "\n";
            strBuildUp += @"sTmp = sTmp.replace	(""mm"",""<n>"")" + "\n";
            strBuildUp += @"sTmp = sTmp.replace	(""m"",""<m>"")" + "\n";
            strBuildUp += @"sTmp = sTmp.replace	(""<m>"",m+1)" + "\n";
            strBuildUp += @"sTmp = sTmp.replace	(""<n>"",padZero(m+1))" + "\n";
            strBuildUp += @"sTmp = sTmp.replace	(""<o>"",monthName[m])" + "\n";
            strBuildUp += @"return sTmp.replace (""yyyy"",y)"  + "\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function closeCalendar() {\n";
            strBuildUp += "var	sTmp\n\n";
            strBuildUp += "hideCalendar();\n";
            strBuildUp += "ctlToPlaceValue.value =	constructDate(dateSelected,monthSelected,yearSelected)\n";
            strBuildUp += "}\n\n";
            strBuildUp += "/*** Month Pulldown	***/\n\n";
            strBuildUp += "function StartDecMonth()\n";
            strBuildUp += "{\n";
            strBuildUp += @"intervalID1=setInterval(""decMonth()"",80)" + "\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function StartIncMonth()\n";
            strBuildUp += "{\n";
            strBuildUp += @"intervalID1=setInterval(""incMonth()"",80)" + "\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function incMonth () {\n";
			strBuildUp += "monthSelected++\n";
            strBuildUp += "if (monthSelected>11) {\n";
            strBuildUp += "monthSelected=0\n";
            strBuildUp += "yearSelected++\n";
            strBuildUp += "}\n";
            strBuildUp += "constructCalendar()\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function decMonth () {\n";
            strBuildUp += "monthSelected--\n";
            strBuildUp += "if (monthSelected<0) {\n";
            strBuildUp += "monthSelected=11\n";
            strBuildUp += "yearSelected--\n";
            strBuildUp += "}\n";
            strBuildUp += "constructCalendar()\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function constructMonth() {\n";
            strBuildUp += "popDownYear()\n";
            strBuildUp += "if (!monthConstructed) {\n";
            strBuildUp += @"sHTML =	""""" + "\n";
            strBuildUp += "for	(i=0; i<12;	i++) {\n";
            strBuildUp += "sName =	monthName[i];\n";
            strBuildUp += "if (i==monthSelected){\n";
            strBuildUp += @"sName =	""<strong>"" +	sName +	""</strong>""" + "\n";
            strBuildUp += "}\n";
            strBuildUp += @"sHTML += ""<tr><td id='m"" + i + ""' onmouseover='this.style.backgroundColor=\""#FFCC99\""' onmouseout='this.style.backgroundColor=\""\""' style='cursor:pointer' onclick='monthConstructed=false;monthSelected="" + i + "";constructCalendar();popDownMonth();event.cancelBubble=true'>&nbsp;"" + sName + ""&nbsp;</td></tr>""" + "\n";
            strBuildUp += "}\n\n";
            strBuildUp += @"document.getElementById(""selectMonth"").innerHTML = ""<table width=70	style='font-family:arial; font-size:11px; border-width:1; border-style:solid; border-color:#a0a0a0;' bgcolor='#FFFFDD' cellspacing=0 onmouseover='clearTimeout(timeoutID1)'	onmouseout='clearTimeout(timeoutID1);timeoutID1=setTimeout(\""popDownMonth()\"",100);event.cancelBubble=true'>"" +	sHTML +	""</table>""" + "\n\n";
            strBuildUp += "monthConstructed=true\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function popUpMonth() {\n";
            strBuildUp += "constructMonth()\n";
            strBuildUp += @"crossMonthObj.visibility = (dom||ie)? ""visible""	: ""show""" + "\n";
			strBuildUp += "crossMonthObj.left = parseInt(crossobj.left) + 50\n";
            strBuildUp += "crossMonthObj.top =	parseInt(crossobj.top) + 26\n\n";
            strBuildUp += @"hideElement( 'SELECT', document.getElementById(""selectMonth"") );" + "\n";
            strBuildUp += @"hideElement( 'APPLET', document.getElementById(""selectMonth"") );" + "\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function popDownMonth()	{\n";
            strBuildUp += @"crossMonthObj.visibility= ""hidden""" + "\n";
            strBuildUp += "}\n\n";
            strBuildUp += "/*** Year Pulldown ***/\n\n";
            strBuildUp += "function incYear() {\n";
            strBuildUp += "for	(i=0; i<7; i++){\n";
            strBuildUp += "newYear	= (i+nStartingYear)+1\n";
            strBuildUp += "if (newYear==yearSelected)\n";
            strBuildUp += @"{ txtYear =	""&nbsp;<strong>""	+ newYear +	""</strong>&nbsp;"" }" + "\n";
            strBuildUp += "else\n";
            strBuildUp += @"{ txtYear =	""&nbsp;"" + newYear + ""&nbsp;"" }" + "\n";
            strBuildUp += @"document.getElementById(""y""+i).innerHTML = txtYear" + "\n";
            strBuildUp += "}\n";
            strBuildUp += "nStartingYear ++;\n";
            strBuildUp += "bShow=true\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function decYear() {\n";
            strBuildUp += "for	(i=0; i<7; i++){\n";
            strBuildUp += "newYear	= (i+nStartingYear)-1\n";
            strBuildUp += "if (newYear==yearSelected)\n";
            strBuildUp += @"{ txtYear =	""&nbsp;<strong>""	+ newYear +	""</strong>&nbsp;"" }" + "\n";
            strBuildUp += "else\n";
            strBuildUp += @"{ txtYear =	""&nbsp;"" + newYear + ""&nbsp;"" }" + "\n";
            strBuildUp += @"document.getElementById(""y""+i).innerHTML = txtYear" + "\n";
            strBuildUp += "}\n";
            strBuildUp += "nStartingYear --;\n";
            strBuildUp += "bShow=true\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function selectYear(nYear) {\n";
            strBuildUp += "yearSelected=parseInt(nYear+nStartingYear);\n";
            strBuildUp += "yearConstructed=false;\n";
            strBuildUp += "constructCalendar();\n";
            strBuildUp += "popDownYear();\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function constructYear() {\n";
            strBuildUp += "popDownMonth()\n";
			strBuildUp += @"sHTML =	""""" + "\n";
            strBuildUp += "if (!yearConstructed) {\n\n";
            strBuildUp += @"sHTML =	""<tr><td align='center'	onmouseover='this.style.backgroundColor=\""#FFCC99\""' onmouseout='clearInterval(intervalID1);this.style.backgroundColor=\""\""' style='cursor:pointer'	onmousedown='clearInterval(intervalID1);intervalID1=setInterval(\""decYear()\"",30)' onmouseup='clearInterval(intervalID1)'>-</td></tr>""" + "\n\n";
            strBuildUp += "j =	0\n";
            strBuildUp += "nStartingYear =	yearSelected-3\n";
            strBuildUp += "for	(i=(yearSelected-3); i<=(yearSelected+3); i++) {\n";
            strBuildUp += "sName =	i;\n";
            strBuildUp += "if (i==yearSelected){\n";
            strBuildUp += @"sName =	""<strong>"" +	sName +	""</strong>""" + "\n";
            strBuildUp += "}\n\n";
            strBuildUp += @"sHTML += ""<tr><td id='y"" + j + ""' onmouseover='this.style.backgroundColor=\""#FFCC99\""' onmouseout='this.style.backgroundColor=\""\""' style='cursor:pointer' onclick='selectYear(""+j+"");event.cancelBubble=true'>&nbsp;"" + sName + ""&nbsp;</td></tr>""" + "\n";
            strBuildUp += "j ++;\n";
            strBuildUp += "}\n\n";
            strBuildUp += @"sHTML += ""<tr><td align='center' onmouseover='this.style.backgroundColor=\""#FFCC99\""' onmouseout='clearInterval(intervalID2);this.style.backgroundColor=\""\""' style='cursor:pointer' onmousedown='clearInterval(intervalID2);intervalID2=setInterval(\""incYear()\"",30)'	onmouseup='clearInterval(intervalID2)'>+</td></tr>""" + "\n\n";
            strBuildUp += @"document.getElementById(""selectYear"").innerHTML	= ""<table width=44 style='font-family:arial; font-size:11px; border-width:1; border-style:solid; border-color:#a0a0a0;'	bgcolor='#FFFFDD' onmouseover='clearTimeout(timeoutID2)' onmouseout='clearTimeout(timeoutID2);timeoutID2=setTimeout(\""popDownYear()\"",100)' cellspacing=0>""	+ sHTML	+ ""</table>""" + "\n\n";
            strBuildUp += "yearConstructed	= true\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n";
            strBuildUp += "function popDownYear() {\n";
            strBuildUp += "clearInterval(intervalID1)\n";
            strBuildUp += "clearTimeout(timeoutID1)\n";
            strBuildUp += "clearInterval(intervalID2)\n";
            strBuildUp += "clearTimeout(timeoutID2)\n";
            strBuildUp += @"crossYearObj.visibility= ""hidden""" + "\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function popUpYear() {\n";
            strBuildUp += "var	leftOffset\n\n";
            strBuildUp += "constructYear()\n";
            strBuildUp += @"crossYearObj.visibility	= (dom||ie)? ""visible"" : ""show""" + "\n";
            strBuildUp += @"leftOffset = parseInt(crossobj.left) + document.getElementById(""spanYear"").offsetLeft" + "\n";
            strBuildUp += "if (ie)\n";
            strBuildUp += "{\n";
            strBuildUp += "leftOffset += 6\n";
            strBuildUp += "}\n";
            strBuildUp += "crossYearObj.left =	leftOffset\n";
            strBuildUp += "crossYearObj.top = parseInt(crossobj.top) +	26\n";
            strBuildUp += "}\n\n";
            strBuildUp += "/*** calendar ***/\n";
			strBuildUp += "function WeekNbr(n) {\n";
            strBuildUp += "// Algorithm used:\n";
            strBuildUp += "// From Klaus Tondering's Calendar document (The Authority/Guru)\n";
            strBuildUp += "// http://www.tondering.dk/claus/calendar.html\n";
            strBuildUp += "// a = (14-month) / 12\n";
            strBuildUp += "// y = year + 4800 - a\n";
            strBuildUp += "// m = month + 12a - 3\n";
            strBuildUp += "// J = day + (153m + 2) / 5 + 365y + y / 4 - y / 100 + y / 400 - 32045\n";
            strBuildUp += "// d4 = (J + 31741 - (J mod 7)) mod 146097 mod 36524 mod 1461\n";
            strBuildUp += "// L = d4 / 1460\n";
            strBuildUp += "// d1 = ((d4 - L) mod 365) + L\n";
            strBuildUp += "// WeekNumber = d1 / 7 + 1\n\n";
            strBuildUp += "year = n.getFullYear();\n";
            strBuildUp += "month = n.getMonth() + 1;\n";
            strBuildUp += "if (startAt == 0) {\n";
            strBuildUp += "day = n.getDate() + 1;\n";
            strBuildUp += "}\n";
            strBuildUp += "else {\n";
            strBuildUp += "day = n.getDate();\n";
            strBuildUp += "}\n\n";
            strBuildUp += "a = Math.floor((14-month) / 12);\n";
            strBuildUp += "y = year + 4800 - a;\n";
            strBuildUp += "m = month + 12 * a - 3;\n";
            strBuildUp += "b = Math.floor(y/4) - Math.floor(y/100) + Math.floor(y/400);\n";
            strBuildUp += "J = day + Math.floor((153 * m + 2) / 5) + 365 * y + b - 32045;\n";
            strBuildUp += "d4 = (((J + 31741 - (J % 7)) % 146097) % 36524) % 1461;\n";
            strBuildUp += "L = Math.floor(d4 / 1460);\n";
            strBuildUp += "d1 = ((d4 - L) % 365) + L;\n";
            strBuildUp += "week = Math.floor(d1/7) + 1;\n\n";
            strBuildUp += "return week;\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function constructCalendar () {\n";
            strBuildUp += "var aNumDays = Array (31,0,31,30,31,30,31,31,30,31,30,31)\n\n";
            strBuildUp += "var dateMessage\n";
            strBuildUp += "var	startDate =	new	Date (yearSelected,monthSelected,1)\n";
            strBuildUp += "var endDate\n\n";
            strBuildUp += "if (monthSelected==1)\n";
            strBuildUp += "{\n";
            strBuildUp += "endDate	= new Date (yearSelected,monthSelected+1,1);\n";
            strBuildUp += "endDate	= new Date (endDate	- (24*60*60*1000));\n";
            strBuildUp += "numDaysInMonth = endDate.getDate()\n";
            strBuildUp += "}\n";
            strBuildUp += "else\n";
            strBuildUp += "{\n";
            strBuildUp += "numDaysInMonth = aNumDays[monthSelected];\n";
            strBuildUp += "}\n\n";
			strBuildUp += "datePointer	= 0\n";
            strBuildUp += "dayPointer = startDate.getDay() - startAt\n\n";
            strBuildUp += "if (dayPointer<0)\n";
            strBuildUp += "{\n";
            strBuildUp += "dayPointer = 6\n";
            strBuildUp += "}\n\n";
            strBuildUp += @"sHTML =	""<table	 border=0 style='font-family:verdana;font-size:10px;'><tr>""" + "\n\n";
            strBuildUp += "if (showWeekNumber==1)\n";
            strBuildUp += "{\n";
            strBuildUp += @"sHTML += ""<td width=27><strong>"" + weekString + ""</strong></td><td width=1 rowspan=7 bgcolor='#d0d0d0' style='padding:0px'><img src='""+imgDir+""" + m_dividerGif + @"' width=1></td>""" + "\n";
            strBuildUp += "}\n\n";
            strBuildUp += "for	(i=0; i<7; i++)	{\n";
            strBuildUp += @"sHTML += ""<td width='27' align='right'><strong>""+ dayName[i]+""</strong></td>""" + "\n";
            strBuildUp += "}\n";
            strBuildUp += @"sHTML +=""</tr><tr>""" + "\n\n";
            strBuildUp += "if (showWeekNumber==1)\n";
            strBuildUp += "{\n";
            strBuildUp += @"sHTML += ""<td align=right>"" + WeekNbr(startDate) + ""&nbsp;</td>""" + "\n";
            strBuildUp += "}\n\n";
            strBuildUp += "for	( var i=1; i<=dayPointer;i++ )\n";
            strBuildUp += "{\n";
            strBuildUp += @"sHTML += ""<td>&nbsp;</td>""" + "\n";
            strBuildUp += "}\n\n";
            strBuildUp += "for	( datePointer=1; datePointer<=numDaysInMonth; datePointer++ )\n";
            strBuildUp += "{\n";
            strBuildUp += "dayPointer++;\n";
            strBuildUp += @"sHTML += ""<td align=right>""" + "\n";
            strBuildUp += "sStyle=styleAnchor\n";
            strBuildUp += "if ((datePointer==odateSelected) &&	(monthSelected==omonthSelected)	&& (yearSelected==oyearSelected))\n";
            strBuildUp += "{ sStyle+=styleLightBorder }\n\n";
            strBuildUp += @"sHint = """"" + "\n";
            strBuildUp += "for (k=0;k<HolidaysCounter;k++)\n";
            strBuildUp += "{\n";
            strBuildUp += "if ((parseInt(Holidays[k].d)==datePointer)&&(parseInt(Holidays[k].m)==(monthSelected+1)))\n";
            strBuildUp += "{\n";
            strBuildUp += "if ((parseInt(Holidays[k].y)==0)||((parseInt(Holidays[k].y)==yearSelected)&&(parseInt(Holidays[k].y)!=0)))\n";
            strBuildUp += "{\n";
            strBuildUp += @"sStyle+=""background-color:#FFDDDD;""" + "\n";
            strBuildUp += @"sHint+=sHint==""""?Holidays[k].desc:""\n""+Holidays[k].desc" + "\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n\n";
            strBuildUp += @"var regexp= /\""/g" + "\n";
			strBuildUp += @"sHint=sHint.replace(regexp,""&quot;"")" + "\n\n";
            strBuildUp += @"dateMessage = ""onmousemove='window.status=\""""+selectDateMessage.replace(""[date]"",constructDate(datePointer,monthSelected,yearSelected))+""\""' onmouseout='window.status=\""\""' """ + "\n\n";
            strBuildUp += "if ((datePointer==dateNow)&&(monthSelected==monthNow)&&(yearSelected==yearNow))\n";
            strBuildUp += @"{ sHTML += ""<strong><a ""+dateMessage+"" title=\"""" + sHint + ""\"" style='""+sStyle+""' href='javascript:dateSelected=""+datePointer+"";closeCalendar();'><font color=#ff0000>&nbsp;"" + datePointer + ""</font>&nbsp;</a></strong>""}" + "\n";
            strBuildUp += "else if	(dayPointer % 7 == (startAt * -1)+1)\n";
            strBuildUp += @"{ sHTML += ""<a ""+dateMessage+"" title=\"""" + sHint + ""\"" style='""+sStyle+""' href='javascript:dateSelected=""+datePointer + "";closeCalendar();'>&nbsp;<font color=#909090>"" + datePointer + ""</font>&nbsp;</a>"" }" + "\n";
            strBuildUp += "else\n";
            strBuildUp += @"{ sHTML += ""<a ""+dateMessage+"" title=\"""" + sHint + ""\"" style='""+sStyle+""' href='javascript:dateSelected=""+datePointer + "";closeCalendar();'>&nbsp;"" + datePointer + ""&nbsp;</a>"" }" + "\n\n";
            strBuildUp += @"sHTML += """"" + "\n";
            strBuildUp += "if ((dayPointer+startAt) % 7 == startAt) {\n";
            strBuildUp += @"sHTML += ""</tr><tr>""" + "\n";
            strBuildUp += "if ((showWeekNumber==1)&&(datePointer<numDaysInMonth))\n";
            strBuildUp += "{\n";
            strBuildUp += @"sHTML += ""<td align=right>"" + (WeekNbr(new Date(yearSelected,monthSelected,datePointer+1))) + ""&nbsp;</td>""" + "\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n\n";
            strBuildUp += @"document.getElementById(""content"").innerHTML   = sHTML" + "\n";
            strBuildUp += @"document.getElementById(""spanMonth"").innerHTML = ""&nbsp;"" +	monthName[monthSelected] + ""&nbsp;<IMG id='changeMonth' SRC='""+imgDir+""drop1.gif' WIDTH='12' HEIGHT='10' BORDER=0>""" + "\n";
            strBuildUp += @"document.getElementById(""spanYear"").innerHTML =	""&nbsp;"" + yearSelected	+ ""&nbsp;<IMG id='changeYear' SRC='""+imgDir+""drop1.gif' WIDTH='12' HEIGHT='10' BORDER=0>""" + "\n";
            strBuildUp += "}\n\n";
            strBuildUp += "function popUpCalendar(ctl,	ctl2, format) {\n";
            strBuildUp += "var	leftpos=0\n";
            strBuildUp += "var	toppos=0\n\n";
            strBuildUp += "if (bPageLoaded)\n";
            strBuildUp += "{\n";
            strBuildUp += @"if ( crossobj.visibility ==	""hidden"" ) {" + "\n";
            strBuildUp += "ctlToPlaceValue	= ctl2\n";
            strBuildUp += "dateFormat=format;\n\n";
            strBuildUp += @"formatChar = "" """ + "\n";
            strBuildUp += "aFormat	= dateFormat.split(formatChar)\n";
            strBuildUp += "if (aFormat.length<3)\n";
            strBuildUp += "{\n";
            strBuildUp += @"formatChar = ""/""" + "\n";
            strBuildUp += "aFormat	= dateFormat.split(formatChar)\n";
            strBuildUp += "if (aFormat.length<3)\n";
            strBuildUp += "{\n";
            strBuildUp += @"formatChar = "".""" + "\n";
            strBuildUp += "aFormat	= dateFormat.split(formatChar)\n";
            strBuildUp += "if (aFormat.length<3)\n";
            strBuildUp += "{\n";
            strBuildUp += @"formatChar = ""-""" + "\n";
            strBuildUp += "aFormat	= dateFormat.split(formatChar)\n";
            strBuildUp += "if (aFormat.length<3)\n";
            strBuildUp += "{\n";
            strBuildUp += "// invalid date	format\n";
            strBuildUp += @"formatChar=""""" + "\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n\n";
            strBuildUp += "tokensChanged =	0\n";
			strBuildUp += @"if ( formatChar	!= """" )" + "\n";
            strBuildUp += "{\n";
            strBuildUp += "// use user's date\n";
            strBuildUp += "aData =	ctl2.value.split(formatChar)\n\n";
            strBuildUp += "for	(i=0;i<3;i++)\n";
            strBuildUp += "{\n";
            strBuildUp += @"if ((aFormat[i]==""d"") || (aFormat[i]==""dd""))" + "\n";
            strBuildUp += "{\n";
            strBuildUp += "dateSelected = parseInt(aData[i], 10)\n";
            strBuildUp += "tokensChanged ++\n";
            strBuildUp += "}\n";
            strBuildUp += @"else if	((aFormat[i]==""m"") || (aFormat[i]==""mm""))" + "\n";
            strBuildUp += "{\n";
            strBuildUp += "monthSelected =	parseInt(aData[i], 10) - 1\n";
            strBuildUp += "tokensChanged ++\n";
            strBuildUp += "}\n";
            strBuildUp += @"else if	(aFormat[i]==""yyyy"")" + "\n";
            strBuildUp += "{\n";
            strBuildUp += "yearSelected = parseInt(aData[i], 10)\n";
            strBuildUp += "tokensChanged ++\n";
            strBuildUp += "}\n";
            strBuildUp += @"else if	(aFormat[i]==""mmm"")" + "\n";
            strBuildUp += "{\n";
            strBuildUp += "for	(j=0; j<12;	j++)\n";
            strBuildUp += "{\n";
            strBuildUp += "if (aData[i]==monthName[j])\n";
            strBuildUp += "{\n";
            strBuildUp += "monthSelected=j\n";
            strBuildUp += "tokensChanged ++\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n\n";
            strBuildUp += "if ((tokensChanged!=3)||isNaN(dateSelected)||isNaN(monthSelected)||isNaN(yearSelected))\n";
            strBuildUp += "{\n";
            strBuildUp += "dateSelected = dateNow\n";
            strBuildUp += "monthSelected =	monthNow\n";
            strBuildUp += "yearSelected = yearNow\n";
            strBuildUp += "}\n\n";
            strBuildUp += "odateSelected=dateSelected\n";
            strBuildUp += "omonthSelected=monthSelected\n";
            strBuildUp += "oyearSelected=yearSelected\n\n";
            strBuildUp += "aTag = ctl\n";
            strBuildUp += "do {\n";
            strBuildUp += "aTag = aTag.offsetParent;\n";
            strBuildUp += "leftpos	+= aTag.offsetLeft;\n";
            strBuildUp += "toppos += aTag.offsetTop;\n";
            strBuildUp += @"} while(aTag.tagName!=""BODY"");" + "\n\n";
            strBuildUp += "crossobj.left =	fixedX==-1 ? ctl.offsetLeft	+ leftpos :	fixedX\n";
            strBuildUp += "crossobj.top = fixedY==-1 ?	ctl.offsetTop +	toppos + ctl.offsetHeight +	2 :	fixedY\n";
            strBuildUp += "constructCalendar (1, monthSelected, yearSelected);\n";
            strBuildUp += @"crossobj.visibility=(dom||ie)? ""visible"" : ""show""" + "\n\n";
            strBuildUp += @"hideElement( 'SELECT', document.getElementById(""calendar"") );" + "\n";
            strBuildUp += @"hideElement( 'APPLET', document.getElementById(""calendar"") );" + "\n\n";
            strBuildUp += "bShow = true;\n";
            strBuildUp += "}\n";
            strBuildUp += "else\n";
            strBuildUp += "{\n";
            strBuildUp += "hideCalendar()\n";
            strBuildUp += "if (ctlNow!=ctl) {popUpCalendar(ctl, ctl2, format)}\n";
            strBuildUp += "}\n";
            strBuildUp += "ctlNow = ctl\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n\n";
            strBuildUp += "document.onkeypress = function hidecal1 () {\n";
            strBuildUp += "if (event.keyCode==27)\n";
            strBuildUp += "{\n";
            strBuildUp += "hideCalendar()\n";
            strBuildUp += "}\n";
            strBuildUp += "}\n";
            strBuildUp += "document.onclick = function hidecal2 () {\n";
            strBuildUp += "if (!bShow)\n";
            strBuildUp += "{\n";
            strBuildUp += "hideCalendar()\n";
            strBuildUp += "}\n";
            strBuildUp += "bShow = false\n";
            strBuildUp += "}\n\n";
            strBuildUp += "if(ie)\n";
            strBuildUp += "{\n";
            strBuildUp += "init()\n";
            strBuildUp += "}\n";
            strBuildUp += "else\n";
            strBuildUp += "{\n";
            strBuildUp += "window.onload=init;\n";
            strBuildUp += "}\n";
            strBuildUp += "<";
            strBuildUp += "/";
            strBuildUp += "script>";
            Page.Response.Write(strBuildUp);
		}

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
		protected override void CreateChildControls()
		{
			base.CreateChildControls ();
			
			this.Controls.Add(label);
			
				placeJavascript();
	
		
			System.Web.UI.WebControls.TextBox txtTextBox = new TextBox();
			if (m_ControlCssClass.Length > 0)
				txtTextBox.CssClass = m_ControlCssClass;
			txtTextBox.ReadOnly = false;
			if (m_text != "")
				txtTextBox.Text = m_text;
			if (m_Css == "")
				txtTextBox.CssClass = m_Css;
			txtTextBox.ID = "foo";
		this.label.AssociatedControlID = txtTextBox.ID.ToString();
			txtTextBox.Attributes.Add("onfocus", "popUpCalendar(document.all." + this.ClientID + "_foo, document.all." + this.ClientID + "_foo, '" + m_DateType + "');");
		//		txtTextBox.Attributes.Add("onfocus", "popUpCalendar(this, this, '" + m_DateType + "');");
			this.Controls.Add(txtTextBox);
			
			if (this.isRequired)
			{
				RequiredFieldValidator rfvDatePicker = new RequiredFieldValidator();
				rfvDatePicker.ID = "val";
				rfvDatePicker.ControlToValidate = "foo";
				rfvDatePicker.ErrorMessage = "question " + questionRef + " must be completed";
				rfvDatePicker.Display = ValidatorDisplay.None;
				rfvDatePicker.EnableClientScript = false;
				this.Controls.Add(rfvDatePicker);
			}



		}

		#endregion
	}
}
