namespace VocaDb.Model.Domain.PVs
{
	/// <summary>
	/// PV service identifier.
	/// </summary>
	/// <remarks>
	/// These values are supposed to be serialized as strings.
	/// </remarks>
	public enum PVService
	{
		NicoNicoDouga = 1,

		Youtube = 2,

		SoundCloud = 4,

		Vimeo = 8,

		Piapro = 16,

		Bilibili = 32,

		File = 64,

		LocalFile = 128,

		Creofuga = 256,

		Bandcamp = 512
	}
}
