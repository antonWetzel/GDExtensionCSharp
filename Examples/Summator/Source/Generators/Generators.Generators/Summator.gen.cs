using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GDExtension.Native;

namespace Summator;

public unsafe partial class Summator : RefCounted {

#pragma warning disable CS8618
	public static new StringName __godot_name;
#pragma warning restore CS8618

	private GCHandle handle;

#pragma warning disable CS8618
	public Summator() {
		handle = GCHandle.Alloc(this , GCHandleType.Weak);
		GDExtensionInterface.gdInterface.object_set_instance(_internal_pointer, __godot_name._internal_pointer, (void*)this);
	}
#pragma warning restore CS8618

	public static explicit operator void*(Summator instance) => (void*)GCHandle.ToIntPtr(instance.handle);
	public static explicit operator Summator(void* ptr) => (Summator)(GCHandle.FromIntPtr(new IntPtr(ptr)).Target!);

	public static Summator Construct(void* ptr) {
		ptr = (void*)*(((IntPtr*)ptr) + 2); //Did I miss the inverse function to 'object_set_instance'?, this only works if Godot.Object does not change
		return (Summator)ptr;
	}

	public static unsafe new void Register() {
		__godot_name = new StringName("Summator");

		var info = new GDExtensionClassCreationInfo() {
			is_virtual = System.Convert.ToByte(false),
			is_abstract = System.Convert.ToByte(false),
			//set_func = &SetFunc,
			//get_func = &GetFunc,
			//get_property_list_func = &GetPropertyList,
			//free_property_list_func = &FreePropertyList,
			//property_can_revert_func = &PropertyCanConvert,
			//property_get_revert_func = &PropertyGetRevert,
			notification_func = null,
			//to_string_func = &ToString,
			//reference_func = &Reference,
			//unreference_func = &Unreference,
			create_instance_func = &CreateObject,
			free_instance_func = &FreeObject,
			//get_virtual_func = &GetVirtual,
			//get_rid_func = &GetRid,
		};
		GDExtensionInterface.gdInterface.classdb_register_extension_class(GDExtensionInterface.gdLibrary, __godot_name._internal_pointer, RefCounted.__godot_name._internal_pointer, &info);
		RegisterMethods();
		RegisterExports();
		RegisterSignals();

		GDExtension.Object.RegisterConstructor("Summator", Construct);
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	static unsafe void* CreateObject(void* userdata) {
		var instance = new Summator();
		return instance._internal_pointer;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	static unsafe void FreeObject(void* userdata, void* instancePtr) {
		var instance = (Summator)instancePtr;
		instance.handle.Free();
	}
}