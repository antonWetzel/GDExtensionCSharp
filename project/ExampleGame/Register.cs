using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GDExtension;

public class Test {
	public IntPtr @object = IntPtr.Zero;
	public GCHandle handle;

	public long test;

	public Test() {
		handle = GCHandle.Alloc(this, GCHandleType.Pinned);
	}

	public static implicit operator IntPtr(Test test) => GCHandle.ToIntPtr(test.handle);
	public static implicit operator Test(IntPtr test) => (Test)(GCHandle.FromIntPtr(test).Target!);

	public void Notification(int what) {
		Console.WriteLine(what);
	}
}

public static class Register {

	public static unsafe void Test() {

		var info = new ExtensionClassCreationInfo() {
			set_func = &SetFunc,
			get_func = &GetFunc,
			get_property_list_func = &GetPropertyList,
			free_property_list_func = &FreePropertyList,
			//property_can_revert_func = &PropertyCanConvert,
			//property_get_revert_func = &PropertyGetRevert,
			notification_func = &Notification,
			to_string_func = &ToString,
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
				Entry.@interface.classdb_register_extension_class(Entry.library, name, baseClass, &info);
			}
		};
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe IntPtr CreateObject(IntPtr userdata) {
		var test = new Test();
		fixed (void* ptr = &test.@object) {
			Console.WriteLine(new IntPtr(ptr));
		}
		fixed (byte* name = System.Text.Encoding.UTF8.GetBytes("Node2D")) {
			test.@object = Entry.@interface.classdb_construct_object(name);
		}
		fixed (byte* name = System.Text.Encoding.UTF8.GetBytes("TestClass")) {
			Entry.@interface.object_set_instance(test.@object, name, test);
		}
		Console.WriteLine($"create: {(IntPtr)test}");
		Console.WriteLine($"create: {test.@object}");
		return test.@object;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void FreeObject(IntPtr userdata, IntPtr instance) {
		var test = (Test)instance;
		Console.WriteLine($"free: {(IntPtr)test}");
		Console.WriteLine($"free: {test.@object}");
		test.handle.Free();
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void Notification(IntPtr instance, int what) {
		var test = (Test)instance;
		test.Notification(what);
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void ToString(IntPtr instance, InternalString* what) {
		var test = (Test)instance;
		*what = *InternalString.Create($"wowow{test.ToString()}");
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe PropertyInfo* GetPropertyList(IntPtr instance, uint* count) {
		var ptr = (PropertyInfo*)Entry.@interface.mem_alloc(sizeof(PropertyInfo) * 1);
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
		Entry.@interface.mem_free(new IntPtr(infos));
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe bool SetFunc(IntPtr instance, IntPtr name, IntPtr variant) {
		//var test = (Test)instance;
		//var val = (Variant)variant;
		//test.test = val.AsInt();
		return false;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe bool GetFunc(IntPtr instance, IntPtr name, IntPtr variant) {
		var test = (Test)instance;
		variant = new Variant(test.test);
		return false;
	}
}
