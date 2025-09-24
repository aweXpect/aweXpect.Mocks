using System.Text;
using aweXpect.Mocks.SourceGenerators.Entities;
using Type = aweXpect.Mocks.SourceGenerators.Entities.Type;

namespace aweXpect.Mocks.SourceGenerators.Internals;

internal static partial class SourceGeneration
{
	public static string GetMockClass(MockClass mockClass)
	{
		string[] namespaces = mockClass.GetNamespaces();
		StringBuilder sb = new();
		foreach (string @namespace in namespaces)
		{
			sb.Append("using ").Append(@namespace).AppendLine(";");
		}

		sb.Append("""
		          using aweXpect.Mocks.Setup;

		          namespace aweXpect.Mocks.Implementations;

		          #nullable enable

		          """);
		sb.Append("public static class For").Append(mockClass.ClassName).AppendLine();
		sb.AppendLine("{");
		sb.Append("\tpublic partial class MockObject(IMockSetup mock)")
			.Append(" : ").AppendLine(mockClass.ClassName);
		sb.AppendLine("\t{");

		foreach (Method method in mockClass.Methods)
		{
			sb.Append("\t\t/// <inheritdoc cref=\"").Append(mockClass.ClassName).Append('.').Append(method.Name)
				.Append('(').Append(string.Join(",",
					method.Parameters.Select(p => p.Type.GetMinimizedString(namespaces))))
				.AppendLine(")\" />");
			sb.Append("\t\t");
			method.AppendSignatureTo(sb, mockClass, namespaces);
			sb.AppendLine();
			sb.AppendLine("\t\t{");
			if (method.ReturnType != Type.Void)
			{
				sb.Append("\t\t\treturn mock.Execute<")
					.Append(method.ReturnType.GetMinimizedString(namespaces))
					.Append(">(nameof(").Append(method.Name).Append(")");
				foreach (MethodParameter p in method.Parameters)
				{
					sb.Append(", ").Append(p.Name);
				}

				sb.AppendLine(");");
			}
			else
			{
				sb.Append("\t\t\tmock.Execute(nameof(").Append(method.Name).Append(")");
				foreach (MethodParameter p in method.Parameters)
				{
					sb.Append(", ").Append(p.Name);
				}

				sb.AppendLine(");");
			}

			sb.AppendLine("\t\t}");
			sb.AppendLine();
		}
		sb.AppendLine("\t}");
		sb.Append("\tpublic class Mock : Mock<").Append(mockClass.ClassName).AppendLine(">");
		sb.AppendLine("\t{");
		sb.AppendLine("\t\tpublic Mock()");
		sb.AppendLine("\t\t{");
		sb.AppendLine("\t\t\tObject = new MockObject(this);");
		sb.AppendLine("\t\t}");
		sb.Append("\t\t/// <inheritdoc cref=\"Mock{").Append(mockClass.ClassName).AppendLine("}.Object\" />");
		sb.Append("\t\tpublic override ").Append(mockClass.ClassName).AppendLine(" Object { get; }");
		sb.AppendLine("\t}");

		sb.AppendLine();
		foreach (Method method in mockClass.Methods)
		{
			if (method.ReturnType != Type.Void)
			{
				sb.Append("\tpublic static SetupMethodWithReturnValue<")
					.Append(method.ReturnType.GetMinimizedString(namespaces));
				foreach (MethodParameter parameter in method.Parameters)
				{
					sb.Append(", ").Append(parameter.Type.GetMinimizedString(namespaces));
				}

				sb.Append("> ");
				sb.Append(method.Name).Append("(this MockSetup<").Append(mockClass.ClassName).Append("> mock");
				foreach (MethodParameter parameter in method.Parameters)
				{
					sb.Append(", MatchParameter<").Append(parameter.Type.GetMinimizedString(namespaces))
						.Append("> ").Append(parameter.Name);
				}

				sb.Append(")").AppendLine();
				sb.AppendLine("\t{");
				sb.Append("\t\tvar setup = new SetupMethodWithReturnValue<")
					.Append(method.ReturnType.GetMinimizedString(namespaces));
				foreach (MethodParameter parameter in method.Parameters)
				{
					sb.Append(", ").Append(parameter.Type.GetMinimizedString(namespaces));
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

						sb.Append(parameter.Type.GetMinimizedString(namespaces));
					}

					sb.Append('>');
				}

				sb.Append(' ').Append(method.Name).Append("(this MockSetup<").Append(mockClass.ClassName)
					.Append("> mock");
				foreach (MethodParameter parameter in method.Parameters)
				{
					sb.Append(", MatchParameter<").Append(parameter.Type.GetMinimizedString(namespaces))
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

						sb.Append(parameter.Type.GetMinimizedString(namespaces));
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
