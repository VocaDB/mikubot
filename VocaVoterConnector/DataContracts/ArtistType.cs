namespace VocaDb.Model.Domain.Artists
{
	/// <summary>
	/// Artist type (category).
	/// </summary>
	/// <remarks>
	/// Do not persist the numeric values anywhere - they may change.
	/// Names must match with <see cref="ArtistTypes"/>.
	/// </remarks>
	public enum ArtistType
	{
		Unknown,

		/// <summary>
		/// Doujin circle. A group of doujin producers that also releases music (acts as a label).
		/// </summary>
		Circle,

		/// <summary>
		/// Commercial music label. Does not produce music by itself.
		/// </summary>
		Label,

		/// <summary>
		/// Producer is the maker or the song (usually an individual, for example doriko)
		/// </summary>
		Producer,

		Animator,

		Illustrator,

		Lyricist,

		Vocaloid,

		UTAU,

		CeVIO,

		OtherVoiceSynthesizer,

		OtherVocalist,

		OtherGroup,

		OtherIndividual,

		Utaite,

		Band,

		Vocalist,

		Character,

		SynthesizerV,
	}
}
