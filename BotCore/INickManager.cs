namespace MikuBot
{
	public interface INickManager
	{
		IrcName Current { get; }

		bool CurrentIsPrimary { get; }

		IrcName Primary { get; }

		IrcName Next();

		void Set(IrcName nick);
	}
}
