public record struct Api {

	public static Api Create(string path) {
		var file = System.IO.File.OpenRead(path);
		var api = JsonSerializer.Deserialize<Api>(file);
		file.Close();
		return api;
	}

	public record struct Argument {
		public string name { get; set; }
		public string type { get; set; }
		[JsonPropertyName("default_value")] public string? defaultValue { get; set; }
		public string? meta { get; set; }
	}

	public record struct ReturnValue {
		public string type { get; set; }
		public string? meta { get; set; }
	}

	public record struct Constant {
		public string name { get; set; }
		public string type { get; set; }
		public string value { get; set; }
	}

	public record struct Size {
		public string name { get; set; }
		public int size { get; set; }
	}

	public record struct Signal {
		public string name { get; set; }
		public Argument[]? arguments { get; set; }
	}

	public record struct Property {
		public string type { get; set; }
		public string name { get; set; }
		public string setter { get; set; }
		public string getter { get; set; }
		public int index { get; set; }
	}

	public record struct Singleton {
		public string name { get; set; }
		public string type { get; set; }
	}

	public record struct NativeStructure {
		public string name { get; set; }
		public string format { get; set; }
	}

	public record struct Header {
		[JsonPropertyName("version_major")] public int versionMajor { get; set; }
		[JsonPropertyName("version_minor")] public int versionMinor { get; set; }
		[JsonPropertyName("version_patch")] public int versionPatch { get; set; }
		[JsonPropertyName("version_status")] public string versionStatus { get; set; }
		[JsonPropertyName("version_build")] public string versionBuild { get; set; }
		[JsonPropertyName("version_full_name")] public string versionFullName { get; set; }
	}

	public record struct BuiltinClassSizes {
		[JsonPropertyName("build_configuration")] public string buildConfiguration { get; set; }

		public Size[] sizes { get; set; }
	}

	public record struct MemberOffset {
		public string member { get; set; }
		public int offset { get; set; }
	}

	public record struct Member {
		public string name { get; set; }
		public string type { get; set; }
	}

	public record struct ClassOffsets {
		public string name { get; set; }
		public MemberOffset[] members { get; set; }
	}

	public record struct BuiltinClassMemberOffsets {
		[JsonPropertyName("build_configuration")] public string buildConfiguration { get; set; }
		public ClassOffsets[] classes { get; set; }
	}

	public record struct Value {
		public string name { get; set; }
		public int value { get; set; }
	}

	public record struct Enum {
		public string name { get; set; }
		[JsonPropertyName("is_bitfield")] public bool? isBitfield { get; set; }
		public Value[] values { get; set; }
	}

	public record struct Method {
		public string name { get; set; }
		[JsonPropertyName("return_type")] public string returnType { get; set; }
		[JsonPropertyName("is_vararg")] public bool isVararg { get; set; }
		[JsonPropertyName("is_const")] public bool isConst { get; set; }
		[JsonPropertyName("is_static")] public bool isStatic { get; set; }
		[JsonPropertyName("is_virtual")] public bool isVirtual { get; set; }
		public uint hash { get; set; }
		[JsonPropertyName("return_value")] public ReturnValue? returnValue { get; set; }
		public Argument[]? arguments { get; set; }

		public string? category { get; set; }
	}

	public record struct Operator {
		public string name { get; set; }
		[JsonPropertyName("right_type")] public string? rightType { get; set; }
		[JsonPropertyName("return_type")] public string returnType { get; set; }
	}

	public record Constructor {
		public int index { get; set; }
		public Argument[]? arguments { get; set; }
	}

	public record struct BuiltinClass {
		public string name { get; set; }
		[JsonPropertyName("is_keyed")] public bool isKeyed { get; set; }
		[JsonPropertyName("indexing_return_type")] public string? indexingReturnType { get; set; }
		public Member[]? members { get; set; }
		public Constant[]? constants { get; set; }
		public Enum[]? enums { get; set; }
		public Operator[]? operators { get; set; }
		public Method[]? methods { get; set; }
		public Constructor[]? constructors { get; set; }
		[JsonPropertyName("has_destructor")] public bool hasDestructor { get; set; }
	}

	public record struct Class {
		public string name { get; set; }
		[JsonPropertyName("is_refcounted")] public bool isRefcounted { get; set; }
		[JsonPropertyName("is_instantiable")] public bool isInstantiable { get; set; }
		public string? inherits { get; set; }
		[JsonPropertyName("api_type")] public string apiType { get; set; }
		public Value[]? constants { get; set; }
		public Enum[]? enums { get; set; }
		public Method[]? methods { get; set; }
		public Signal[]? signals { get; set; }
		public Property[]? properties { get; set; }
	}



	public Header header { get; set; }
	[JsonPropertyName("builtin_class_sizes")] public BuiltinClassSizes[] builtinClassSizes { get; set; }
	[JsonPropertyName("builtin_class_member_offsets")] public BuiltinClassMemberOffsets[] builtinClassMemberOffsets { get; set; }
	[JsonPropertyName("global_constants")] public object[] globalConstants { get; set; }
	[JsonPropertyName("global_enums")] public Enum[] globalEnums { get; set; }
	[JsonPropertyName("utility_functions")] public Method[] untilityFunction { get; set; }
	[JsonPropertyName("builtin_classes")] public BuiltinClass[] builtinClasses { get; set; }
	public Class[] classes { get; set; }
	public Singleton[] singletons { get; set; }
	[JsonPropertyName("native_structures")] public NativeStructure[] nativeStructures { get; set; }
}
