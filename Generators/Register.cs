using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators {

	public static class Register {

		public enum Level {
			Scene,
			Editor,
		}

		public struct Data {
			public string name;
			public string godotName;
			public string @base;
			public string @namespace;
			public Level level;
		}

		public static Data Generate(GeneratorExecutionContext context, INamedTypeSymbol c, bool notification, SpecialBase sBase) {
			var gdName = (string)(c.GetAttributes().
				SingleOrDefault(x => x.AttributeClass.Name == "RegisterAttribute").NamedArguments.
				SingleOrDefault(x => x.Key == "name").Value.Value ?? c.Name
			);
			var isRefCounted = sBase switch {
				SpecialBase.Resource => true,
				SpecialBase.RefCounted => true,
				_ => false,
			};

			var source = $$"""
			using System.Runtime.CompilerServices;
			using System.Runtime.InteropServices;
			using GDExtension.Native;

			namespace {{c.ContainingNamespace}};

			public unsafe partial class {{c.Name}} : {{c.BaseType.Name}} {

			#pragma warning disable CS8618
				public static new StringName __godot_name;
			#pragma warning restore CS8618

				private GCHandle handle;

			#pragma warning disable CS8618
				public {{c.Name}}() {
					handle = GCHandle.Alloc(this {{(isRefCounted ? ", GCHandleType.Weak" : "")}});
					GDExtensionInterface.gdInterface.object_set_instance(_internal_pointer, __godot_name._internal_pointer, (void*)this);
				}
			#pragma warning restore CS8618

				public static explicit operator void*({{c.Name}} instance) => (void*)GCHandle.ToIntPtr(instance.handle);
				public static explicit operator {{c.Name}}(void* ptr) => ({{c.Name}})(GCHandle.FromIntPtr(new IntPtr(ptr)).Target!);

				public static {{c.Name}} Construct(void* ptr) {
					ptr = (void*)*(((IntPtr*)ptr) + 2); //Did I miss the inverse function to 'object_set_instance'?, this only works if Godot.Object does not change
					return ({{c.Name}})ptr;
				}

				public static unsafe new void Register() {
					__godot_name = new StringName("{{gdName}}");

					var info = new GDExtensionClassCreationInfo() {
						is_virtual = System.Convert.ToByte(false),
						is_abstract = System.Convert.ToByte(false),
						//set_func = &SetFunc,
						//get_func = &GetFunc,
						//get_property_list_func = &GetPropertyList,
						//free_property_list_func = &FreePropertyList,
						//property_can_revert_func = &PropertyCanConvert,
						//property_get_revert_func = &PropertyGetRevert,
						notification_func = {{(notification ? $"Engine.IsEditorHint()? null : &__Notification" : "null")}},
						//to_string_func = &ToString,
						//reference_func = &Reference,
						//unreference_func = &Unreference,
						create_instance_func = &CreateObject,
						free_instance_func = &FreeObject,
						//get_virtual_func = &GetVirtual,
						//get_rid_func = &GetRid,
					};
					GDExtensionInterface.gdInterface.classdb_register_extension_class(GDExtensionInterface.gdLibrary, __godot_name._internal_pointer, {{c.BaseType.Name}}.__godot_name._internal_pointer, &info);
					RegisterMethods();
					RegisterExports();
					RegisterSignals();

					GDExtension.Object.RegisterConstructor("{{c.Name}}", Construct);
				}

				[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
				static unsafe void* CreateObject(void* userdata) {
					var instance = new {{c.Name}}();
					return instance._internal_pointer;
				}

				[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
				static unsafe void FreeObject(void* userdata, void* instancePtr) {
					var instance = ({{c.Name}})instancePtr;
					instance.handle.Free();
				}
			}
			""";

			context.AddSource($"{c.Name}.gen.cs", source);

			var editorOnly = (bool)(c.GetAttributes().
				SingleOrDefault(x => x.AttributeClass.Name == "RegisterAttribute").NamedArguments.
				SingleOrDefault(x => x.Key == "editorOnly").Value.Value ?? false
			);
			var level = editorOnly switch {
				true => Level.Editor,
				false => Level.Scene,
			};
			return new Data() {
				name = c.Name,
				godotName = gdName,
				@base = c.BaseType.Name,
				@namespace = c.ContainingNamespace.ToString(),
				level = level,
			};
		}
	}
}
