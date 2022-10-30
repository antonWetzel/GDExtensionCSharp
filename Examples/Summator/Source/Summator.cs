namespace Summator;

[Register]
public unsafe partial class Summator : RefCounted {

	long count;

	[Method] void Add(long value = 1) => count += value;
	[Method] void Reset() => count = 0;
	[Method] long GetTotal() => count;
}
