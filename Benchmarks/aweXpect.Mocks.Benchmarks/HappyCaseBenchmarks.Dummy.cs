using BenchmarkDotNet.Attributes;

namespace aweXpect.Mocks.Benchmarks;

/// <summary>
///     This is a dummy benchmark in the Mocks template.
/// </summary>
public partial class HappyCaseBenchmarks
{
	[Benchmark]
	public TimeSpan Dummy_aweXpect()
		=> TimeSpan.FromSeconds(10);
}
