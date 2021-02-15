namespace VocaDb.Model.Service
{
	public enum AlbumSortRule
	{
		None,

		Name,

		/// <summary>
		/// By release date in descending order, excluding entries without a full release date.
		/// </summary>
		ReleaseDate,

		/// <summary>
		/// By release date in descending order, including entries without a release date.
		/// Null release dates will be shown LAST (in descending order - in ascending order they'd be shown first).
		/// </summary>
		ReleaseDateWithNulls,

		AdditionDate,

		RatingAverage,

		RatingTotal,

		NameThenReleaseDate,

		CollectionCount
	}
}
