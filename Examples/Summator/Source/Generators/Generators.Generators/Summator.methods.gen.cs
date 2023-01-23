using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GDExtension.Native;

namespace Summator;

public unsafe partial class Summator : RefCounted {

	static unsafe void RegisterMethods() {
		{
			var args = stackalloc GDExtensionPropertyInfo[1];
			var args_meta = stackalloc GDExtensionClassMethodArgumentMetadata[1];
			args[0] = new GDExtensionPropertyInfo() {
				type = (GDExtensionVariantType)Variant.Type.Int,
				name = new StringName("value")._internal_pointer,
				class_name = __godot_name._internal_pointer,
				hint = (uint)PropertyHint.None,
				hint_string = StringMarshall.ToNative(""),
				usage = (uint)PropertyUsageFlags.Default,
			};
			args_meta[0] = GDExtensionClassMethodArgumentMetadata.GDEXTENSION_METHOD_ARGUMENT_METADATA_NONE;

			var info = new GDExtensionClassMethodInfo() {
				name = new StringName("add")._internal_pointer,
				method_userdata = (void*)(new IntPtr(0)),
				call_func = &CallFunc,
				ptrcall_func = &CallFuncPtr,
				method_flags = (uint)GDExtensionClassMethodFlags.GDEXTENSION_METHOD_FLAGS_DEFAULT,
				has_return_value = System.Convert.ToByte(false),
				return_value_info = null,
				return_value_metadata = GDExtensionClassMethodArgumentMetadata.GDEXTENSION_METHOD_ARGUMENT_METADATA_NONE,

				argument_count = 1,
				arguments_info = args,
				arguments_metadata = args_meta,

				default_argument_count = 0,
				default_arguments = null,
			};
			GDExtensionInterface.gdInterface.classdb_register_extension_class_method(GDExtensionInterface.gdLibrary, __godot_name._internal_pointer, &info);
		}
		{

			var info = new GDExtensionClassMethodInfo() {
				name = new StringName("reset")._internal_pointer,
				method_userdata = (void*)(new IntPtr(1)),
				call_func = &CallFunc,
				ptrcall_func = &CallFuncPtr,
				method_flags = (uint)GDExtensionClassMethodFlags.GDEXTENSION_METHOD_FLAGS_DEFAULT,
				has_return_value = System.Convert.ToByte(false),
				return_value_info = null,
				return_value_metadata = GDExtensionClassMethodArgumentMetadata.GDEXTENSION_METHOD_ARGUMENT_METADATA_NONE,

				argument_count = 0,
				arguments_info = null,
				arguments_metadata = null,

				default_argument_count = 0,
				default_arguments = null,
			};
			GDExtensionInterface.gdInterface.classdb_register_extension_class_method(GDExtensionInterface.gdLibrary, __godot_name._internal_pointer, &info);
		}
		{
			var ret = new GDExtensionPropertyInfo() {
				type = (GDExtensionVariantType)Variant.Type.Int,
				name = new StringName("return")._internal_pointer,
				class_name = __godot_name._internal_pointer,
				hint = (uint)PropertyHint.None,
				hint_string = StringMarshall.ToNative(""),
				usage = (uint)PropertyUsageFlags.Default,
			};

			var info = new GDExtensionClassMethodInfo() {
				name = new StringName("get_total")._internal_pointer,
				method_userdata = (void*)(new IntPtr(2)),
				call_func = &CallFunc,
				ptrcall_func = &CallFuncPtr,
				method_flags = (uint)GDExtensionClassMethodFlags.GDEXTENSION_METHOD_FLAGS_DEFAULT,
				has_return_value = System.Convert.ToByte(true),
				return_value_info = &ret,
				return_value_metadata = GDExtensionClassMethodArgumentMetadata.GDEXTENSION_METHOD_ARGUMENT_METADATA_NONE,

				argument_count = 0,
				arguments_info = null,
				arguments_metadata = null,

				default_argument_count = 0,
				default_arguments = null,
			};
			GDExtensionInterface.gdInterface.classdb_register_extension_class_method(GDExtensionInterface.gdLibrary, __godot_name._internal_pointer, &info);
		}
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	static void CallFuncPtr(void* method_userdata, void* p_instance, void** p_args, void* r_ret) {
		var instance = (Summator)p_instance;
		switch ((int)method_userdata) {
		case 0:
			instance.Add(*(long*)p_args[0]);
			break;
		case 1:
			instance.Reset();
			break;
		case 2:
			*(long*)r_ret = instance.GetTotal();
			break;
		}
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	static void CallFunc(
		void* method_userdata,
		void* p_instance,
		void** p_args,
		long p_argument_count,
		void* r_return,
		GDExtensionCallError* r_error
	) {
		GDExtensionInterface.gdInterface.variant_new_nil(r_return); //no clue why this is needed
		var instance = (Summator)p_instance;
		switch ((int)method_userdata) {
		case 0: {
			instance.Add((long)Variant.GetIntFromPointer(p_args[0]));
			break;
			}
		case 1: {
			instance.Reset();
			break;
			}
		case 2: {
			var res = instance.GetTotal();
			Variant.SaveIntoPointer(res, r_return);
			break;
			}
		}
	}
}