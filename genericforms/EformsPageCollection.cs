using System;
using System.Collections;
using System.Data;
namespace eastsussexgovuk.webservices.FormControls.genericforms
{
	/// <summary>
	/// formsPageCollection. Strongly typed collection of EformsDataTableCollections representing thye pages of a form.
	/// </summary>
	public class EformsPageCollection : CollectionBase
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		public EformsPageCollection()
		{
		}
		/// <summary>
		/// Interface implementation
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Add(EformsDataTableCollection item)
		{
			return List.Add(item);
		}
		/// <summary>
		/// Interface implementation
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, EformsDataTableCollection item)
		{
			List.Add(item);
		}
		/// <summary>
		/// Interface implementation
		/// </summary>
		/// <param name="item"></param>
		public void Remove(EformsDataTableCollection item)
		{
			List.Remove(item);
		}
		/// <summary>
		/// Interface implementation
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(EformsDataTableCollection item)
		{
			return List.Contains(item);
		}
		/// <summary>
		/// Interface implementation
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(EformsDataTableCollection item)
		{
			return List.IndexOf(item);
		}
		/// <summary>
		/// Interface implementation
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(EformsDataTableCollection[] array, int index)
		{
			List.CopyTo(array, index);
		}
		/// <summary>
		/// Interface implementation
		/// </summary>
		public EformsDataTableCollection this[int index]
		{
			get { return (EformsDataTableCollection)List[index]; }
			set { List[index] = value; }
		}
	}
}
