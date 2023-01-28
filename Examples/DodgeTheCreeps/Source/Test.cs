namespace DodgeTheCreeps;

[Register]
partial class CSharpScriptLanguage : ScriptLanguageExtension {
	[Method] public override void _Init() { }
	[Method] public override void _Frame() { }
	[Method] public override string _GetName() => "CSharpScript";
	[Method] public override TypedArray<Dictionary> _GetPublicAnnotations() => new TypedArray<Dictionary>(Variant.Type.Dictionary);
	[Method] public override Dictionary _GetPublicConstants() => new Dictionary();
	[Method] public override TypedArray<Dictionary> _GetPublicFunctions() => new TypedArray<Dictionary>(Variant.Type.Dictionary);
	[Method] public override string _GetType() => "CSharpScript";
	[Method] public override bool _HandlesGlobalClassType(string type) => false;
	[Method] public override void _ThreadEnter() { }
	[Method] public override void _ThreadExit() { }
	[Method] public override PackedStringArray _GetRecognizedExtensions() => new PackedStringArray() { "cs" };
	[Method] public override bool _HasNamedClasses() => false;
	[Method] public override bool _CanInheritFromFile() => false;
	[Method] public override bool _SupportsBuiltinMode() => false;
	[Method] public override string _GetExtension() => "cs";
	[Method] public override bool _IsUsingTemplates() => false;
}

[Register]
public unsafe partial class CSharpScript : Script {

}

[Register]
partial class CSharpScriptLoader : ResourceFormatLoader {
	[Method]
	public override PackedStringArray _GetRecognizedExtensions() {
		return new PackedStringArray() { "cs" };
	}

	[Method]
	public override string _GetResourceScriptClass(string path) {
		//todo?
		return "";
	}

	[Method]
	public override string _GetResourceType(string path) {
		Prints("GetType for", path);
		if (!path.EndsWith(".cs")) {
			return "";
		}
		return nameof(CSharpScript);
	}

	[Method]
	public override bool _HandlesType(StringName type) {
		return type == nameof(CSharpScript);
	}

	[Method]
	public override Variant _Load(string path, string original_path, bool use_sub_threads, long cache_mode) {
		Prints("load", original_path);
		return new CSharpScript();
	}
}

[Register]
partial class CSharpScriptSaver : ResourceFormatSaver {

	[Method] public override PackedStringArray _GetRecognizedExtensions(Resource resource) => new PackedStringArray() { "cs" };

	[Method]
	public override bool _Recognize(Resource resource) {
		Prints("recognise", resource);
		return resource is CSharpScript;
	}

	[Method]
	public override long _Save(Resource resource, string path, long flags) {
		Prints("save", path);
		return (long)Error.Ok;
	}
}

static unsafe class Test {
	public static void Register() {
		ResourceLoader.AddResourceFormatLoader(new CSharpScriptLoader());
		ResourceSaver.AddResourceFormatSaver(new CSharpScriptSaver());
		var language = new CSharpScriptLanguage();
		Engine.RegisterScriptLanguage(language);
	}
}
