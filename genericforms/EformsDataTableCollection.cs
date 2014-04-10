using System;
using System.Collections;
using System.Data;
namespace eastsussexgovuk.webservices.FormControls.genericforms
{
	/// <summary>
	/// EformsDataTableCollection. Strongly typed collection of DataTables representing individual form questions.
	/// </summary>
	public class EformsDataTableCollection : CollectionBase
	{
		/// <summary>
		/// Class constructor
		/// </summary>
		public EformsDataTableCollection()
		{
		}
		/// <summary>
		/// Interface implementation
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Add(DataTable item)
		{
			return List.Add(item);
		}
		/// <summary>
		/// Interface implementation
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, DataTable item)
		{
			List.Add(item);
		}
		/// <summary>
		/// Interface implementation
		/// </summary>
		/// <param name="item"></param>
		public void Remove(DataTable item)
		{
			List.Remove(item);
		}
		/// <summary>
		/// Interface implementation
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(DataTable item)
		{
			return List.Contains(item);
		}
		/// <summary>
		/// Interface implementation
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(DataTable item)
		{
			return List.IndexOf(item);
		}
		/// <summary>
		/// Interface implementation
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(DataTable[] array, int index)
		{
			List.CopyTo(array, index);
		}
		/// <summary>
		/// Interface implementation
		/// </summary>
		public DataTable this[int index]
		{
			get { return (DataTable)List[index]; }
			set { List[index] = value; }
		}
	}
}
