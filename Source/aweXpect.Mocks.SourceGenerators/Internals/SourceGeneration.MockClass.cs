using System.Text;
using aweXpect.Mocks.SourceGenerators.Entities;
using Type = aweXpect.Mocks.SourceGenerators.Entities.Type;

namespace aweXpect.Mocks.SourceGenerators.Internals;

internal static partial class SourceGeneration
{
	public static string GetMockClass(MockClass mockClass)
	{
		StringBuilder sb = new();
		foreach (string @namespace in mockClass.GetNamespaces())
		{
			sb.Append("using ").Append(@namespace).AppendLine(";");
		}

		sb.Append("""
		          using aweXpect.Mocks.Setup;

		          namespace aweXpect.Mocks.Implementations;

		          #nullable enable

		          """);
		sb.Append("public partial class MockFor").Append(mockClass.ClassName)
			.Append(" : Mock<").Append(mockClass.ClassName).Append(">, ").AppendLine(mockClass.ClassName);
		sb.AppendLine("{");

		foreach (Method method in mockClass.Methods)
		{
			sb.Append("\t/// <inheritdoc cref=\"").Append(mockClass.ClassName).Append('.').Append(method.Name)
				.Append('(').Append(string.Join(",",
					method.Parameters.Select(p => p.Type.GetMinimizedString(mockClass.GetNamespaces()))))
				.AppendLine(")\" />");
			sb.Append("\t");
			method.AppendSignatureTo(sb, mockClass.GetNamespaces());
			sb.AppendLine();
			sb.AppendLine("\t{");
			if (method.ReturnType != Type.Void)
			{
				sb.Append("\t\treturn Execute<")
					.Append(method.ReturnType.GetMinimizedString(mockClass.GetNamespaces()))
					.Append(">(nameof(").Append(method.Name).Append(")");
				foreach (MethodParameter p in method.Parameters)
				{
					sb.Append(", ").Append(p.Name);
				}

				sb.AppendLine(");");
			}
			else
			{
				sb.Append("\t\tExecute(nameof(").Append(method.Name).Append(")");
				foreach (MethodParameter p in method.Parameters)
				{
					sb.Append(", ").Append(p.Name);
				}

				sb.AppendLine(");");
			}

			sb.AppendLine("\t}");
			sb.AppendLine();
		}

		sb.Append("\t/// <inheritdoc cref=\"Mock{").Append(mockClass.ClassName).AppendLine("}.Object\" />");
		sb.Append("\tpublic override ").Append(mockClass.ClassName).AppendLine(" Object");
		sb.AppendLine("\t\t=> this;");
		sb.AppendLine("}");

		sb.AppendLine();
		sb.Append("public static class MockFor").Append(mockClass.ClassName).AppendLine("Extensions");
		sb.AppendLine("{");

		foreach (Method method in mockClass.Methods)
		{
			if (method.ReturnType != Type.Void)
			{
				sb.Append("\tpublic static SetupMethodWithReturnValue<")
					.Append(method.ReturnType.GetMinimizedString(mockClass.GetNamespaces()));
				foreach (MethodParameter parameter in method.Parameters)
				{
					sb.Append(", ").Append(parameter.Type.GetMinimizedString(mockClass.GetNamespaces()));
				}

				sb.Append("> ");
				sb.Append(method.Name).Append("(this MockSetup<").Append(mockClass.ClassName).Append("> mock");
				foreach (MethodParameter parameter in method.Parameters)
				{
					sb.Append(", MatchParameter<").Append(parameter.Type.GetMinimizedString(mockClass.GetNamespaces()))
						.Append("> ").Append(parameter.Name);
				}

				sb.Append(")").AppendLine();
				sb.AppendLine("\t{");
				sb.Append("\t\tvar setup = new SetupMethodWithReturnValue<")
					.Append(method.ReturnType.GetMinimizedString(mockClass.GetNamespaces()));
				foreach (MethodParameter parameter in method.Parameters)
				{
					sb.Append(", ").Append(parameter.Type.GetMinimizedString(mockClass.GetNamespaces()));
				}

				sb.Append(">").Append("(nameof(").Append(mockClass.ClassName).Append(".").Append(method.Name)
					.Append(")");
				foreach (MethodParameter p in method.Parameters)
				{
					sb.Append(", ").Append(p.Name);
				}

				sb.Append(");").AppendLine();
				sb.AppendLine("\t\tif (mock is IMockSetup mockSetup)");
				sb.AppendLine("\t\t{");
				sb.AppendLine("\t\t\tmockSetup.RegisterSetup(setup);");
				sb.AppendLine("\t\t}");
				sb.AppendLine("\t\treturn setup;");
				sb.AppendLine("\t}");
			}
			else
			{
				sb.Append("\tpublic static SetupMethodWithoutReturnValue");
				if (method.Parameters.Count > 0)
				{
					sb.Append('<');
					int index = 0;
					foreach (MethodParameter parameter in method.Parameters)
					{
						if (index++ > 0)
						{
							sb.Append(", ");
						}

						sb.Append(parameter.Type.GetMinimizedString(mockClass.GetNamespaces()));
					}

					sb.Append('>');
				}

				sb.Append(' ').Append(method.Name).Append("(this MockSetup<").Append(mockClass.ClassName)
					.Append("> mock");
				foreach (MethodParameter parameter in method.Parameters)
				{
					sb.Append(", MatchParameter<").Append(parameter.Type.GetMinimizedString(mockClass.GetNamespaces()))
						.Append("> ").Append(parameter.Name);
				}

				sb.Append(")").AppendLine();
				sb.AppendLine("\t{");
				sb.Append("\t\tvar setup = new SetupMethodWithoutReturnValue");

				if (method.Parameters.Count > 0)
				{
					sb.Append('<');
					int index = 0;
					foreach (MethodParameter parameter in method.Parameters)
					{
						if (index++ > 0)
						{
							sb.Append(", ");
						}

						sb.Append(parameter.Type.GetMinimizedString(mockClass.GetNamespaces()));
					}

					sb.Append('>');
				}

				sb.Append("(nameof(").Append(mockClass.ClassName).Append(".").Append(method.Name).Append(")");
				foreach (MethodParameter p in method.Parameters)
				{
					sb.Append(", ").Append(p.Name);
				}

				sb.Append(");").AppendLine();
				sb.AppendLine("\t\tif (mock is IMockSetup mockSetup)");
				sb.AppendLine("\t\t{");
				sb.AppendLine("\t\t\tmockSetup.RegisterSetup(setup);");
				sb.AppendLine("\t\t}");
				sb.AppendLine("\t\treturn setup;");
				sb.AppendLine("\t}");
			}

			sb.AppendLine();
		}

		sb.AppendLine("}");

		sb.AppendLine("#nullable disable");

		return sb.ToString();
	}
}
