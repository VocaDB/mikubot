using System;

namespace VocaDb.Model.DataContracts.Api
{
	[Flags]
	public enum EntryOptionalFields
	{
		None = 0,
		AdditionalNames = 1,
		Description = 2,
		MainPicture = 4,
		Names = 8,

		/// <summary>
		/// List of PVs, for songs and albums
		/// </summary>
		PVs = 16,

		Tags = 32,
		WebLinks = 64
	}
}
