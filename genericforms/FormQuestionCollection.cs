using System;
using System.Collections;

namespace eastsussexgovuk.webservices.FormControls.genericforms
{
	/// <summary>
	/// Summary description for FormQuestionCollection.
	/// </summary>
	public class FormQuestionCollection: CollectionBase
	{
		#region constructor
		/// <summary>
		/// Class constructor
		/// </summary>
		public FormQuestionCollection()
		{
		}
		#endregion
		#region Interface implementations

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Add(FormQuestion item)
		{
			return List.Add(item);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, FormQuestion item)
		{
			List.Insert(index, item);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		public void Remove(FormQuestion item)
		{
			List.Remove(item);
		} 
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(FormQuestion item)
		{
			return List.Contains(item);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(FormQuestion item)
		{
			return List.IndexOf(item);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(FormQuestion[] array, int index)
		{
			List.CopyTo(array, index);
		}
		#endregion
		/// <summary>
		/// 
		/// </summary>
		public FormQuestion this[int index]
		{
			get { return (FormQuestion)List[index]; }
			set { List[index] = value; }
		}
	}
}
