using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GDExtension.Native;

class Test {
	public ObjectPtr @object;
}

public static class Register {

	public static unsafe void Test() {

		var info = new ExtensionClassCreationInfo() {
			set_func = &SetFunc,
			get_func = &GetFunc,
			get_property_list_func = &GetPropertyList,
			free_property_list_func = &FreePropertyList,
			property_can_revert_func = &PropertyCanConvert,
			property_get_revert_func = &PropertyGetRevert,
			notification_func = &Notification,
			to_string_func = &ToString,
			reference_func = &Reference,
			unreference_func = &Unreference,
			create_instance_func = &CreateObject,
			free_instance_func = &FreeObject,
			get_virtual_func = &GetVirtual,
			get_rid_func = &GetRid,
			class_userdata = null,
		};
		fixed (byte* name = System.Text.Encoding.UTF8.GetBytes("TestClass")) {
			fixed (byte* baseClass = System.Text.Encoding.UTF8.GetBytes("Node2D")) {
				Entry.@interface.classdb_register_extension_class(Entry.library, name, baseClass, &info);
			}
		};
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe ObjectPtr CreateObject(void* userdata) {
		Console.WriteLine("create");
		var x = new Test();
		return x.@object;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void FreeObject(void* userdata, ExtensionClassInstancePtr @object) {
		Console.WriteLine("free");
		Entry.@interface.mem_free(@object);
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe bool SetFunc(ExtensionClassInstancePtr instance, StringNamePtr name, VariantPtr variant) {
		Console.WriteLine("set");
		return true;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe bool GetFunc(ExtensionClassInstancePtr instance, StringNamePtr name, VariantPtr variant) {
		Console.WriteLine("get");
		return false;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe PropertyInfo* GetPropertyList(ExtensionClassInstancePtr instance, uint* count) {
		Console.WriteLine("get properties");
		*count = 0;
		return null;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void FreePropertyList(ExtensionClassInstancePtr instance, PropertyInfo* infos) {
		Console.WriteLine("free properties");
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe bool PropertyCanConvert(ExtensionClassInstancePtr instance, StringNamePtr name) {
		Console.WriteLine("property can convert");
		return false;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe bool PropertyGetRevert(ExtensionClassInstancePtr instance, StringNamePtr name, VariantPtr ret) {
		Console.WriteLine("property can convert");
		return false;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void Notification(ExtensionClassInstancePtr instance, int what) {
		Console.WriteLine("notification");
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void ToString(ExtensionClassInstancePtr instance, StringPtr result) {
		Console.WriteLine("ToString");
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void Reference(ExtensionClassInstancePtr instance) {
		Console.WriteLine("reference");
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void Unreference(ExtensionClassInstancePtr instance) {
		Console.WriteLine("unreference");
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe delegate* unmanaged[Cdecl]<ExtensionClassInstancePtr, TypePtr*, TypePtr, void> GetVirtual(void* userdata, byte* name) {
		Console.WriteLine("get virtual");
		return null;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe long GetRid(ExtensionClassInstancePtr instance) {
		Console.WriteLine("get rid");
		return 1234;
	}
}
