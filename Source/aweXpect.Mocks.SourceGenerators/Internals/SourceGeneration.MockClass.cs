using System.Security.Claims;
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
		          using aweXpect.Mocks.Invocations;
		          using aweXpect.Mocks.Setup;

		          namespace aweXpect.Mocks;

		          #nullable enable

		          """);
		sb.Append("public static class For").Append(mockClass.ClassName).AppendLine();
		sb.AppendLine("{");
		sb.Append("\tpublic partial class MockObject(IMockSetup mock)")
			.Append(" : ").AppendLine(mockClass.ClassName);
		sb.AppendLine("\t{");

		foreach (Property property in mockClass.Properties)
		{
			sb.Append("\t\t/// <inheritdoc cref=\"").Append(mockClass.ClassName).Append('.').Append(property.Name).AppendLine("\" />");
			sb.Append("\t\t").Append(property.Accessibility.ToVisibilityString()).Append(' ');
			if (!mockClass.IsInterface && property.IsVirtual)
			{
				sb.Append("override ");
			}
			sb.Append(property.Type.GetMinimizedString(namespaces))
				.Append(" ").Append(property.Name).AppendLine();
			sb.AppendLine("\t\t{");
			if (property.Getter != null && property.Getter.Value.Accessibility != Microsoft.CodeAnalysis.Accessibility.Private)
			{
				sb.Append("\t\t\t");
				if (property.Getter.Value.Accessibility != property.Accessibility)
				{
					sb.Append(property.Getter.Value.Accessibility.ToVisibilityString()).Append(' ');
				}
				sb.AppendLine("get");
				sb.AppendLine("\t\t\t{");
				sb.Append("\t\t\t\treturn mock.Get<")
					.Append(property.Type.GetMinimizedString(namespaces))
					.Append(">(nameof(").Append(property.Name).AppendLine("));");
				sb.AppendLine("\t\t\t}");
			}
			if (property.Setter != null && property.Setter.Value.Accessibility != Microsoft.CodeAnalysis.Accessibility.Private)
			{
				sb.Append("\t\t\t");
				if (property.Setter.Value.Accessibility != property.Accessibility)
				{
					sb.Append(property.Setter.Value.Accessibility.ToVisibilityString()).Append(' ');
				}
				sb.AppendLine("set");
				sb.AppendLine("\t\t\t{");
				sb.Append("\t\t\t\tmock.Set(nameof(").Append(property.Name).AppendLine("), value);");
				sb.AppendLine("\t\t\t}");
			}

			sb.AppendLine("\t\t}");
			sb.AppendLine();
		}
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
		sb.AppendLine("\t\tpublic Mock(MockBehavior mockBehavior) : base(mockBehavior)");
		sb.AppendLine("\t\t{");
		sb.AppendLine("\t\t\tObject = new MockObject(this);");
		sb.AppendLine("\t\t}");
		sb.Append("\t\t/// <inheritdoc cref=\"Mock{").Append(mockClass.ClassName).AppendLine("}.Object\" />");
		sb.Append("\t\tpublic override ").Append(mockClass.ClassName).AppendLine(" Object { get; }");
		sb.AppendLine("\t}");

		sb.AppendLine();
		sb.Append("\textension(MockSetup<").Append(mockClass.ClassName).AppendLine("> mock)");
		sb.AppendLine("\t{");
		foreach (Property property in mockClass.Properties)
		{
			sb.Append("\t\tpublic PropertySetup<").Append(property.Type.GetMinimizedString(namespaces)).Append("> ")
				.Append(property.Name).AppendLine();

			sb.AppendLine("\t\t{");
			sb.AppendLine("\t\t\tget");
			sb.AppendLine("\t\t\t{");
			sb.Append("\t\t\t\tvar setup = new PropertySetup<").Append(property.Type.GetMinimizedString(namespaces)).Append(">();").AppendLine();
			sb.AppendLine("\t\t\t\tif (mock is IMockSetup mockSetup)");
			sb.AppendLine("\t\t\t\t{");
			sb.Append("\t\t\t\t\tmockSetup.RegisterProperty(\"").Append(property.Name).Append("\", setup);").AppendLine();
			sb.AppendLine("\t\t\t\t}");
			sb.AppendLine("\t\t\t\treturn setup;");
			sb.AppendLine("\t\t\t}");
			sb.AppendLine("\t\t}");
			sb.AppendLine();
		}
		
		foreach (Method method in mockClass.Methods)
		{
			if (method.ReturnType != Type.Void)
			{
				sb.Append("\t\tpublic MethodWithReturnValueSetup<")
					.Append(method.ReturnType.GetMinimizedString(namespaces));
				foreach (MethodParameter parameter in method.Parameters)
				{
					sb.Append(", ").Append(parameter.Type.GetMinimizedString(namespaces));
				}

				sb.Append("> ");
				sb.Append(method.Name).Append("(");
				int i = 0;
				foreach (MethodParameter parameter in method.Parameters)
				{
					if (i++ > 0)
					{
						sb.Append(", ");
					}
					sb.Append("With.Parameter<").Append(parameter.Type.GetMinimizedString(namespaces))
						.Append("> ").Append(parameter.Name);
				}

				sb.Append(")").AppendLine();
				sb.AppendLine("\t\t{");
				sb.Append("\t\t\tvar setup = new MethodWithReturnValueSetup<")
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
				sb.AppendLine("\t\t\tif (mock is IMockSetup mockSetup)");
				sb.AppendLine("\t\t\t{");
				sb.AppendLine("\t\t\t\tmockSetup.RegisterMethod(setup);");
				sb.AppendLine("\t\t\t}");
				sb.AppendLine("\t\t\treturn setup;");
				sb.AppendLine("\t\t}");
			}
			else
			{
				sb.Append("\t\tpublic MethodWithoutReturnValueSetup");
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

				sb.Append(' ').Append(method.Name).Append("(");
				int i = 0;
				foreach (MethodParameter parameter in method.Parameters)
				{
					if (i++ > 0)
					{
						sb.Append(", ");
					}
					sb.Append("With.Parameter<").Append(parameter.Type.GetMinimizedString(namespaces))
						.Append("> ").Append(parameter.Name);
				}

				sb.Append(")").AppendLine();
				sb.AppendLine("\t\t{");
				sb.Append("\t\t\tvar setup = new MethodWithoutReturnValueSetup");

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
				sb.AppendLine("\t\t\tif (mock is IMockSetup mockSetup)");
				sb.AppendLine("\t\t\t{");
				sb.AppendLine("\t\t\t\tmockSetup.RegisterMethod(setup);");
				sb.AppendLine("\t\t\t}");
				sb.AppendLine("\t\t\treturn setup;");
				sb.AppendLine("\t\t}");
			}

			sb.AppendLine();
		}
		sb.AppendLine("\t}");

		sb.AppendLine("}");

		sb.AppendLine("#nullable disable");

		return sb.ToString();
	}
}
