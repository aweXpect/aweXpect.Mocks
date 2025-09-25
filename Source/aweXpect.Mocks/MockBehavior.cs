using System;
using System.Threading;
using System.Threading.Tasks;

namespace aweXpect.Mocks;

/// <summary>
/// The behavior of the mock.
/// </summary>
public record MockBehavior
{
	/// <summary>
	/// Specifies whether an exception is thrown when an operation is attempted without prior setup.
	/// </summary>
	public bool ThrowWhenNotSetup { get; init; } = true;

	/// <summary>
	/// Specifies the strategy used to determine how default values that are not prior setup are generated.
	/// </summary>
	public IDefaultValueGenerator DefaultValueGenerator { get; init; } = new ReturnDefaultDefaultValueGenerator();

	private static MockBehavior _globalDefault = new MockBehavior();

	/// <summary>
	///     The globally used default behavior settings.
	/// </summary>
	public static MockBehavior Default => _globalDefault;

	/// <summary>
	/// Defines a mechanism for generating default values of a specified type.
	/// </summary>
	public interface IDefaultValueGenerator
	{
		/// <summary>
		/// Generates a default value of the specified type.
		/// </summary>
		T Generate<T>();
	}

	private sealed class ReturnDefaultDefaultValueGenerator : IDefaultValueGenerator
	{
		private static readonly (Type Type, object Value)[] _defaultValues =
			[
				(typeof(Task), Task.CompletedTask),
				(typeof(CancellationToken), CancellationToken.None),
			];

		/// <inheritdoc cref="IDefaultValueGenerator.Generate{T}" />
		public T Generate<T>()
		{
			foreach (var defaultValue in _defaultValues)
			{
				if (defaultValue.Value is T value &&
					defaultValue.Type == typeof(T))
				{
					return value;
				}
			}

			return default!;
		}
	}
}
