using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ExampleGame;

public class TestClass {
	public IntPtr @object = IntPtr.Zero;
	public GCHandle handle;

	public long test;

	public TestClass() {
		handle = GCHandle.Alloc(this, GCHandleType.Pinned);
	}

	public static implicit operator IntPtr(TestClass test) => GCHandle.ToIntPtr(test.handle);
	public static implicit operator TestClass(IntPtr test) => (TestClass)(GCHandle.FromIntPtr(test).Target!);

	public void Notification(int what) {
		//Console.WriteLine(what);
	}

	public static unsafe void Register() {

		var info = new ExtensionClassCreationInfo() {
			set_func = &SetFunc,
			get_func = &GetFunc,
			get_property_list_func = &GetPropertyList,
			free_property_list_func = &FreePropertyList,
			//property_can_revert_func = &PropertyCanConvert,
			//property_get_revert_func = &PropertyGetRevert,
			notification_func = &Notification,
			//to_string_func = &ToString,
			//reference_func = &Reference,
			//unreference_func = &Unreference,
			create_instance_func = &CreateObject,
			free_instance_func = &FreeObject,
			//get_virtual_func = &GetVirtual,
			//get_rid_func = &GetRid,
			//class_userdata = IntPtr.Zero,
		};

		fixed (byte* name = System.Text.Encoding.UTF8.GetBytes("TestClass")) {
			fixed (byte* baseClass = System.Text.Encoding.UTF8.GetBytes("Node2D")) {
				Initialization.inter.classdb_register_extension_class(Initialization.lib, name, baseClass, &info);
			}
		};

		var x = new Vector2i(6, 8);
		var y = x.abs();
		Console.WriteLine($"{y.x} {y.y}");
		var z = new Variant(x);
		GD.Print(z, z);
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe IntPtr CreateObject(IntPtr userdata) {
		var test = new TestClass();
		fixed (byte* name = System.Text.Encoding.UTF8.GetBytes("Node2D")) {
			test.@object = Initialization.inter.classdb_construct_object(name);
		}
		fixed (byte* name = System.Text.Encoding.UTF8.GetBytes("TestClass")) {
			Initialization.inter.object_set_instance(test.@object, name, test);
		}
		return test.@object;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void FreeObject(IntPtr userdata, IntPtr instance) {
		var test = (TestClass)instance;
		test.handle.Free();
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void Notification(IntPtr instance, int what) {
		var test = (TestClass)instance;
		test.Notification(what);
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe PropertyInfo* GetPropertyList(IntPtr instance, uint* count) {
		var ptr = (PropertyInfo*)Initialization.inter.mem_alloc(sizeof(PropertyInfo) * 1);
		*count = 1;

		ptr[0] = new PropertyInfo() {
			type = (uint)VariantType.Int,
			name = (byte*)Marshal.StringToHGlobalAnsi("test"),
			class_name = (byte*)Marshal.StringToHGlobalAnsi(""),
			hint = 0,
			hint_string = (byte*)Marshal.StringToHGlobalAnsi(""),
			usage = 7,
		};
		return ptr;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void FreePropertyList(IntPtr instance, PropertyInfo* infos) {
		Initialization.inter.mem_free(new IntPtr(infos));
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe bool SetFunc(IntPtr instance, StringName* name, IntPtr varPtr) {
		if (*name == new StringName(new GDExtension.String("test"))) {
			var test = (TestClass)instance;
			var variant = Variant.InteropFromPointer(varPtr);
			test.test = variant.AsInt();
			return true;
		}
		return false;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe bool GetFunc(IntPtr instance, StringName* name, IntPtr variant) {
		if (*name == new StringName(new GDExtension.String("test"))) {
			var test = (TestClass)instance;
			Variant.InteropSaveIntoPointer(test.test, variant);
			return true;
		}
		return false;
	}
}
