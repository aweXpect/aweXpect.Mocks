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

		          namespace aweXpect.Mocks.Implementations;

		          #nullable enable

		          """);
		sb.Append("public partial class MockFor").Append(mockClass.ClassName)
			.Append(" : Mock<").Append(mockClass.ClassName).Append(">, ").AppendLine(mockClass.ClassName);
		sb.AppendLine("{");

		foreach (Method method in mockClass.Methods)
		{
			sb.Append("\t");
			method.AppendSignatureTo(sb, mockClass.GetNamespaces());
			sb.AppendLine();
			sb.AppendLine("\t{");
			if (method.ReturnType != Type.Void)
			{
				sb.Append("\t\treturn default(").Append(method.ReturnType.GetMinimizedString(mockClass.GetNamespaces())).AppendLine(");");
			}

			sb.AppendLine("\t}");
			sb.AppendLine();
		}

		sb.Append("\tpublic override ").Append(mockClass.ClassName).AppendLine(" Object");
		sb.AppendLine("\t\t=> this;");
		sb.AppendLine("}");
		sb.AppendLine("#nullable disable");

		return sb.ToString();
	}
}
