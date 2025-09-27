﻿using Microsoft.CodeAnalysis;

namespace aweXpect.Mocks.SourceGenerators.Entities;

internal readonly record struct Event
{
	public Event(IEventSymbol eventSymbol, IMethodSymbol delegateInvokeMethod)
	{
		Accessibility = eventSymbol.DeclaredAccessibility;
		IsVirtual = eventSymbol.IsVirtual;
		Name = eventSymbol.Name;
		Type = new Type(eventSymbol.Type);
		Delegate = new Method(delegateInvokeMethod);
	}

	public Method Delegate { get; }

	public Type Type { get; }

	public bool IsVirtual { get; }

	public Accessibility Accessibility { get; }
	public string Name { get; }
}
