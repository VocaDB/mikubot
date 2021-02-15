using System;

namespace VocaDb.Model.DataContracts.Albums
{
	[Flags]
	public enum AlbumOptionalFields
	{
		None = 0,
		AdditionalNames = 1,
		Artists = 2,
		Description = 4,
		Discs = 8,
		Identifiers = 16,
		MainPicture = 32,
		Names = 64,
		PVs = 128,
		ReleaseEvent = 256,
		Tags = 512,
		Tracks = 1024,
		WebLinks = 2048
	}
}
