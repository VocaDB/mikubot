using System;

namespace VocaDb.Model.DataContracts.Songs
{
	[Flags]
	public enum SongOptionalFields
	{
		None = 0,
		AdditionalNames = 1,
		Albums = 2,
		Artists = 4,
		Lyrics = 8,
		MainPicture = 16,
		Names = 32,
		PVs = 64,
		ReleaseEvent = 128,
		Tags = 256,
		ThumbUrl = 512,
		WebLinks = 1024
	}
}
