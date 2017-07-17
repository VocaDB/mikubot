using System;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;

namespace MikuBot.ExtraPlugins.Helpers {
	public class ScriptContext {

		private readonly StringCollection referencedAssemblies;
		private readonly StringCollection usings;

		/// <summary>
		/// Initializes a script context with default usings and referenced assemblies.
		/// System namespace and system.dll assembly will be referenced.
		/// </summary>
		public ScriptContext() {
			referencedAssemblies = new StringCollection();
			usings = new StringCollection();
			AddReferencedAssembly("system.dll");
			AddUsingDirective("System");
		}

		/// <summary>
		/// Initializes a script context with custom usings and referenced assemblies.
		/// No namespaces or assemblies will be referenced automatically, you must specify system.dll
		/// assembly and System namespace yourself.
		/// </summary>
		/// <param name="referencedAssemblies">List of referenced assemblies. Cannot be null.</param>
		/// <param name="usings">List of using directives. Cannot be null.</param>
		public ScriptContext(string[] referencedAssemblies, string[] usings)
			: this() {

			this.referencedAssemblies.AddRange(referencedAssemblies);
			this.usings.AddRange(usings);

		}

		/// <summary>
		/// Initializes script context, copying all assembly references and using directives from
		/// another context.
		/// </summary>
		/// <param name="another">Source context. Cannot be null.</param>
		public ScriptContext(ScriptContext another)
			: this() {

			CopyFrom(another);

		}

		public void AddExecutingAssembly() {
			AddReferencedAssembly(Assembly.GetCallingAssembly().Location);
		}

		public void AddReferencedAssembly(string path) {

			if (string.IsNullOrEmpty(path))
				throw new ArgumentException("Path cannot be null or empty", "path");

			if (!referencedAssemblies.Contains(path))
				referencedAssemblies.Add(path);

		}

		public void AddUsingDirective(string usingDirective) {

			if (string.IsNullOrEmpty(usingDirective))
				throw new ArgumentException("Using-directive cannot be null or empty", "usingDirective");

			if (!usings.Contains(usingDirective))
				usings.Add(usingDirective);

		}

		public void CopyFrom(ScriptContext another) {

			foreach (string assembly in another.referencedAssemblies)
				AddReferencedAssembly(assembly);

			foreach (string usingDirective in another.usings)
				AddUsingDirective(usingDirective);

		}

		public CompilerParameters CreateCompilerParameters(string outputAssembly) {

			var compilerParameters = new CompilerParameters {
				GenerateExecutable = false,
				GenerateInMemory = true,
				OutputAssembly = outputAssembly
			};

			foreach (string assembly in referencedAssemblies)
				compilerParameters.ReferencedAssemblies.Add(assembly);

#if DEBUG
			compilerParameters.IncludeDebugInformation = true;
#endif

			return compilerParameters;

		}

		public void AppendUsings(StringBuilder code) {

			foreach (string usingDirective in usings) {
				code.AppendLine("using " + usingDirective + ";");
			}

		}

	}

}
