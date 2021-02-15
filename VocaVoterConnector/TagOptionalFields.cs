using System;

namespace VocaDb.Model.DataContracts.Tags
{
	[Flags]
	public enum TagOptionalFields
	{
		None = 0,
		AdditionalNames = 1,
		[Obsolete("Tag aliases are now just names")]
		AliasedTo = 2,
		Description = 4,
		MainPicture = 8,
		Names = 16,
		Parent = 32,
		RelatedTags = 64,
		TranslatedDescription = 128,
		WebLinks = 256
	}
}
