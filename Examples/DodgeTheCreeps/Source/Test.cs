namespace DodgeTheCreeps;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GDExtension.Native;

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

static unsafe class Test {
	public static void Register() {
		var language = new CSharpScriptLanguage();
		Engine.RegisterScriptLanguage(language);
	}
}
