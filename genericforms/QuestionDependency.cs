using System;

namespace eastsussexgovuk.webservices.FormControls.genericforms
{
	/// <summary>
	/// Summary description for QuestionDependency.
	/// </summary>
	public class QuestionDependency
	{

		#region Private fields
			
		private int dependencyId;
		private int mainQuestionId;
		private int dependentQuestionId;
		private bool enableDependentQuestion;
		private string comparisonOperator;
		private string comparisonVarchar;
		private int comparisonInteger = -1;
		private DateTime comparisonDate;
		private bool comparisonBool;
		private double comparisonDecimal = -1;
		private string answerType;


		private string dependencyRef;
		
	
		
		
		
	
		

		#endregion

		#region Public properties

		/// <summary>
		/// 
		/// </summary>
		public string AnswerType
		{
		
			get 
			{
				return answerType;
			}
			set
			{
				answerType = value;
			}
		}

		/// <summary>
		/// Property DependencyRef (string)
		/// </summary>
		public string DependencyRef
		{
			get
			{
				return this.dependencyRef;
			}
			set
			{
				this.dependencyRef = value;
			}
		}
		/// <summary>
		/// Property DependencyId (int)
		/// </summary>
		public int DependencyId
		{
			get
			{
				return this.dependencyId;
			}
			set
			{
				this.dependencyId = value;
			}
		}

		
		/// <summary>
		/// Property MainQuestionId (int)
		/// </summary>
		public int MainQuestionId
		{
			get
			{
				return this.mainQuestionId;
			}
			set
			{
				this.mainQuestionId = value;
			}
		}

		/// <summary>
		/// Property DependentQuestionId (int)
		/// </summary>
		public int DependentQuestionId
		{
			get
			{
				return this.dependentQuestionId;
			}
			set
			{
				this.dependentQuestionId = value;
			}
		}

		/// <summary>
		/// Property EnableDependentQuestion (bool)
		/// </summary>
		public bool EnableDependentQuestion
		{
			get
			{
				return this.enableDependentQuestion;
			}
			set
			{
				this.enableDependentQuestion = value;
			}
		}

		/// <summary>
		/// Property ComparisonOperator (string)
		/// </summary>
		public string ComparisonOperator
		{
			get
			{
				return this.comparisonOperator;
			}
			set
			{
				this.comparisonOperator = value;
			}
		}

		/// <summary>
		/// Property ComparisonVarchar (string)
		/// </summary>
		public string ComparisonVarchar
		{
			get
			{
				return this.comparisonVarchar;
			}
			set
			{
				this.comparisonVarchar = value;
			}
		}

		/// <summary>
		/// Property ComparisonInteger (int)
		/// </summary>
		public int ComparisonInteger
		{
			get
			{
				return this.comparisonInteger;
			}
			set
			{
				this.comparisonInteger = value;
			}
		}
		

		/// <summary>
		/// Property ComparisonDate (DateTime)
		/// </summary>
		public DateTime ComparisonDate
		{
			get
			{
				return this.comparisonDate;
			}
			set
			{
				this.comparisonDate = value;
			}
		}

		/// <summary>
		/// Property ComparisonBool (bool)
		/// </summary>
		public bool ComparisonBool
		{
			get
			{
				return this.comparisonBool;
			}
			set
			{
				this.comparisonBool = value;
			}
		}

		/// <summary>
		/// Property ComparisonDecimal (double)
		/// </summary>
		public double ComparisonDecimal
		{
			get
			{
				return this.comparisonDecimal;
			}
			set
			{
				this.comparisonDecimal = value;
			}
		}
		
		#endregion
		



		/// <summary>
		/// Constructor
		/// </summary>
		public QuestionDependency()
		{
			
		}
	}
}
