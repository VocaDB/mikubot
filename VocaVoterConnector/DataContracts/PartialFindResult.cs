using System.Runtime.Serialization;

namespace VocaDb.Model.Service
{
	public class PartialFindResult<T>
	{
		public PartialFindResult()
		{
			Items = new T[] { };
		}

		public PartialFindResult(T[] items, int totalCount)
		{
			Items = items;
			TotalCount = totalCount;
		}

		public PartialFindResult(T[] items, int totalCount, string term)
			: this(items, totalCount)
		{
			Term = term;
		}

		[DataMember]
		public T[] Items { get; set; }

		[DataMember]
		public string Term { get; set; }

		[DataMember]
		public int TotalCount { get; set; }
	}
}
