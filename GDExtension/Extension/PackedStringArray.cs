namespace GDExtension;

using System.Collections;

public unsafe partial class PackedStringArray : IEnumerable {

	public string this[int index] {
		get => this[(long)index];
	}

	public string this[long index] {
		get {
			var res = gdInterface.packed_string_array_operator_index(_internal_pointer, index);
			return StringMarshall.ToManaged(res);
		}
	}
	public IEnumerator GetEnumerator() => new PackedStringArrayEnumerator(this);

	public void Add(string value) => Append(value);
}

public class PackedStringArrayEnumerator : IEnumerator {
	PackedStringArray array;
	int current;

	public PackedStringArrayEnumerator(PackedStringArray array) {
		this.array = array;
		current = 0;
	}

	public bool MoveNext() {
		current += 1;
		return current < array.Size();
	}

	public void Reset() {
		current = 0;
	}
	public object Current => array[current];
}
