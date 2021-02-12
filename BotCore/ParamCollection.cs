using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MikuBot
{
	public class ParamCollection : IEnumerable<string>
	{
		private readonly string[] paramList;
		private readonly string trailing;

		public ParamCollection()
		{
			paramList = new string[] { };
			trailing = string.Empty;
		}

		public ParamCollection(CmdReader cmdReader)
		{
			var list = new List<string>();
			trailing = string.Empty;

			while (cmdReader.InBounds)
			{
				if (cmdReader.Peek == ':')
				{
					trailing = cmdReader.ReadToEnd().Substring(1);
				}
				else
				{
					list.Add(cmdReader.ReadNext());
				}
			}

			paramList = list.ToArray();

			//paramList = paramString.Split(' ').Where(s => s != " ").Select(s => s.StartsWith(":") ? s.Substring(1) : s).ToArray();
		}

		//public string ParamString { get; private set; }

		public string this[int index]
		{
			get { return ParamOrEmpty(index); }
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<string> GetEnumerator()
		{
			return (paramList.Concat(new[] { trailing }).GetEnumerator());
		}

		public bool HasParam(int index)
		{
			return (index < paramList.Length);
		}

		public int IntParamOrDefault(int index, int def)
		{
			if (!HasParam(index))
				return def;

			int val;
			if (int.TryParse(this[index], out val))
				return val;
			else
				return def;
		}

		/// <summary>
		/// Returns a parameter or empty string.
		/// </summary>
		/// <param name="index">Parameter index (0-based)</param>
		/// <returns>Parameter string. Cannot be null.</returns>
		public string ParamOrEmpty(int index)
		{
			return (index < paramList.Length ? paramList[index] : string.Empty);
		}

		/// <summary>
		/// Returns a parameter or null.
		/// </summary>
		/// <param name="index">Parameter index (0-based)</param>
		/// <returns>Parameter string. Can be null.</returns>
		public string ParamOrNull(int index)
		{
			return (index < paramList.Length ? paramList[index] : null);
		}

		public int Count
		{
			get
			{
				return paramList.Length;
			}
		}

		/// <summary>
		/// Trailing string. Can be empty. Cannot be null.
		/// </summary>
		public string Trailing
		{
			get { return trailing; }
		}
	}
}
