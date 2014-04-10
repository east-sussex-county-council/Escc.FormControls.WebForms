using System;
using System.Collections;

namespace eastsussexgovuk.webservices.FormControls.genericforms
{
	/// <summary>
	/// Summary description for QuestionDependencyCollection.
	/// </summary>
	public class QuestionDependencyCollection: CollectionBase
	{
		#region constructor
		/// <summary>
		/// Class constructor
		/// </summary>
		public QuestionDependencyCollection()
		{
		}
		#endregion
		#region Interface implementations
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Add(QuestionDependency item)
		{
			return List.Add(item);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, QuestionDependency item)
		{
			List.Insert(index, item);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		public void Remove(QuestionDependency item)
		{
			List.Remove(item);
		} 
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(QuestionDependency item)
		{
			return List.Contains(item);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(QuestionDependency item)
		{
			return List.IndexOf(item);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(QuestionDependency[] array, int index)
		{
			List.CopyTo(array, index);
		}
		#endregion
		/// <summary>
		/// 
		/// </summary>
		public QuestionDependency this[int index]
		{
			get { return (QuestionDependency)List[index]; }
			set { List[index] = value; }
		}
	}
}
