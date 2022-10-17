using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ExampleGame;

public unsafe class TestClass : Node2D {
	public GCHandle handle;

	public long test;

	public TestClass() {
		handle = GCHandle.Alloc(this);
		Native.gdInterface.object_set_instance.Call(_internal_pointer, "TestClass", this);
	}

	public static implicit operator Native.GDExtensionClassInstancePtr(TestClass test) => new(GCHandle.ToIntPtr(test.handle));
	public static implicit operator TestClass(Native.GDExtensionClassInstancePtr test) => (TestClass)(GCHandle.FromIntPtr(test.data).Target!);

	//attribute to create notification function
	//maybe [Notify(NOTIFICATION_PROCESS, new [] { "GetProcessDeltaTime()" })]
	//or short "alias" [Process]
	public void Process(double delta) {
		position += Vector2.RIGHT * delta * 10f;
	}

	//automate
	public void Notification(int what) {
		switch (what) {
		case NOTIFICATION_PROCESS:
			Process(GetProcessDeltaTime());
			break;
		}
	}

	public static unsafe void Register() {

		var info = new Native.ExtensionClassCreationInfo() {
			set_func = new(SetFunc),
			get_func = new(GetFunc),
			get_property_list_func = new(GetPropertyList),
			free_property_list_func = new(FreePropertyList),
			//property_can_revert_func = &PropertyCanConvert,
			//property_get_revert_func = &PropertyGetRevert,
			notification_func = new(Notification),
			//to_string_func = &ToString,
			//reference_func = &Reference,
			//unreference_func = &Unreference,
			create_instance_func = new(CreateObject),
			free_instance_func = new(FreeObject),
			//get_virtual_func = &GetVirtual,
			//get_rid_func = &GetRid,
			//class_userdata = IntPtr.Zero,
		};

		Native.gdInterface.classdb_register_extension_class.Call(Native.gdLibrary, "TestClass", "Node2D", &info);
	}

	public static unsafe Native.ObjectPtr CreateObject(IntPtr userdata) {
		var test = new TestClass();
		test.SetProcess(true);
		return test._internal_pointer;
	}

	public static unsafe void FreeObject(IntPtr userdata, Native.GDExtensionClassInstancePtr instance) {
		var test = (TestClass)instance;
		test.handle.Free();
	}

	public static unsafe void Notification(Native.GDExtensionClassInstancePtr instance, int what) {
		var test = (TestClass)instance;
		test.Notification(what);
	}

	public static unsafe Native.PropertyInfo* GetPropertyList(Native.GDExtensionClassInstancePtr instance, uint* count) {
		var ptr = (Native.PropertyInfo*)Native.gdInterface.mem_alloc.Call((nuint)sizeof(Native.PropertyInfo) * 1);
		*count = 1;

		ptr[0] = new Native.PropertyInfo() {
			type = (uint)Native.VariantType.Int,
			name = (byte*)Marshal.StringToHGlobalAnsi("test"),
			class_name = null,
			hint = (uint)PropertyHint.PROPERTY_HINT_NONE,
			hint_string = null,
			usage = (uint)PropertyUsageFlags.PROPERTY_USAGE_DEFAULT,
		};
		return ptr;
	}

	public static unsafe void FreePropertyList(Native.GDExtensionClassInstancePtr instance, Native.PropertyInfo* infos) {
		for (var i = 0; i < 1; i++) {
			var info = infos[i];
			Marshal.FreeHGlobal(new IntPtr(info.name));
		}
		Native.gdInterface.mem_free.Call(new IntPtr(infos));
	}

	public static unsafe Native.Bool SetFunc(Native.GDExtensionClassInstancePtr instance, StringName* name, Native.VariantPtr varPtr) {
		var y = (string)*name;
		Console.WriteLine($"Set: {y}");
		if (*name == "test") {
			var test = (TestClass)instance;
			var variant = new Variant(varPtr.data);
			test.test = variant.AsInt();
			return true;
		}
		return false;
	}

	public static unsafe Native.Bool GetFunc(Native.GDExtensionClassInstancePtr instance, StringName* name, Native.VariantPtr variant) {
		if (*name == "test") {
			var test = (TestClass)instance;
			Variant.InteropSaveIntoPointer(test.test, variant.data);
			return true;
		}
		return false;
	}
}
