using System.Linq;
using System.Text.RegularExpressions;
using VocaDb.Model.Domain.PVs;

namespace MikuBot.VocaDBConnector.Helpers
{
	public class VideoService
	{
		public static readonly VideoService[] Services = new[] {
			new VideoService(PVService.Youtube, new[] {
				new Regex(@"youtube.com/watch?\S*v=(\S{11})"),
				new Regex(@"youtu.be/(\S{11})"),
			}),
			new VideoService(PVService.NicoNicoDouga, new[] {
				new Regex(@"nicovideo.jp/watch/([a-z]{2}\d{4,10})"),
				new Regex(@"nicovideo.jp/watch/(\d{6,12})"),
				new Regex(@"nico.ms/([a-z]{2}\d{4,10})"),
				new Regex(@"nico.ms/(\d{6,12})")
			}),
			new VideoService(PVService.SoundCloud, new[] {
				new Regex(@"soundcloud.com/(\S+)")
			})
		};

		private readonly Regex[] linkMatchers;

		public VideoService(PVService service, Regex[] linkMatchers)
		{
			Service = service;
			this.linkMatchers = linkMatchers;
		}

		public PVService Service { get; set; }

		public string GetId(string url)
		{
			var matcher = linkMatchers.FirstOrDefault(m => m.IsMatch(url));
			var match = matcher.Match(url);

			return match.Groups[1].Value;
		}

		public bool IsMatch(string url)
		{
			return linkMatchers.Any(m => m.IsMatch(url));
		}
	}
}
