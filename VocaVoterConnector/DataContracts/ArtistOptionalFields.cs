using System;

namespace VocaDb.Model.DataContracts.Artists
{
	[Flags]
	public enum ArtistOptionalFields
	{
		None = 0,
		AdditionalNames = 1,
		ArtistLinks = 2,
		ArtistLinksReverse = 4,
		BaseVoicebank = 8,
		Description = 16,
		MainPicture = 32,
		Names = 64,
		Tags = 128,
		WebLinks = 256
	}
}
