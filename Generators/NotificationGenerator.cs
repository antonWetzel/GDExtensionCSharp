using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators {
	[Generator]
	public class NotificationGenerator : ISourceGenerator {

		public void Execute(GeneratorExecutionContext context) {
			var rec = (ActorSyntaxReciever)context.SyntaxReceiver!;

			var classes = new List<string>();

			foreach (var c in rec.names) {

				var s = (ITypeSymbol)context.Compilation.GetSemanticModel(c.SyntaxTree).GetDeclaredSymbol(c);

				var gdName = s.GetAttributes().SingleOrDefault(x => x.AttributeClass.Name == "RegisterAttribute").NamedArguments.SingleOrDefault(x => x.Key == "name").Value.Value ?? s.Name;

				var source = $$"""
				using System.Runtime.CompilerServices;
				using System.Runtime.InteropServices;

				namespace {{s.ContainingNamespace}};

				public unsafe partial class {{s.Name}} : {{s.BaseType.Name}} {
					private GCHandle handle;

					public {{s.Name}}() {
						handle = GCHandle.Alloc(this);
						Native.gdInterface.object_set_instance.Call(_internal_pointer, "{{gdName}}", this);
					}

					public static implicit operator Native.GDExtensionClassInstancePtr({{s.Name}} instance) => new(GCHandle.ToIntPtr(instance.handle));
					public static implicit operator {{s.Name}}(Native.GDExtensionClassInstancePtr ptr) => ({{s.Name}})(GCHandle.FromIntPtr(ptr.data).Target!);

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
						Native.gdInterface.classdb_register_extension_class.Call(Native.gdLibrary, "{{gdName}}", "{{s.BaseType.Name}}", &info);
					}

					public static unsafe Native.ObjectPtr CreateObject(IntPtr userdata) {
						var test = new {{s.Name}}();
						test.SetProcess(true);
						return test._internal_pointer;
					}

					public static unsafe void FreeObject(IntPtr userdata, Native.GDExtensionClassInstancePtr instance) {
						var test = ({{s.Name}})instance;
						test.handle.Free();
					}

					public static unsafe void Notification(Native.GDExtensionClassInstancePtr instance, int what) {
						var test = ({{s.Name}})instance;
						test.Notification(what);
					}

					public static unsafe Native.PropertyInfo* GetPropertyList(Native.GDExtensionClassInstancePtr instance, uint* count) {
						var ptr = (Native.PropertyInfo*)Native.gdInterface.mem_alloc.Call((nuint)sizeof(Native.PropertyInfo) * 1);
						*count = 1;

						ptr[0] = new Native.PropertyInfo() {
							type = (uint)Native.VariantType.Float,
							name = (byte*)Marshal.StringToHGlobalAnsi("speed"),
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
						switch ((string)*name) {
						case "speed":
							var test = ({{s.Name}})instance;
							var variant = new Variant(varPtr.data);
							test.speed = variant.AsFloat();
							return true;
						default:
							return false;
						}
					}

					public static unsafe Native.Bool GetFunc(Native.GDExtensionClassInstancePtr instance, StringName* name, Native.VariantPtr variant) {
						switch ((string)*name) {
						case "speed":
							var test = ({{s.Name}})instance;
							Variant.InteropSaveIntoPointer(test.speed, variant.data);
							return true;
						default:
							return false;
						}
					}
				}
				""";
				context.AddSource($"{s.Name}.gen.cs", source);

				classes.Add(s.Name);
			}
			Entry.Execute(context, classes);
		}

		public void Initialize(GeneratorInitializationContext context) {
			context.RegisterForSyntaxNotifications(() => new ActorSyntaxReciever());
		}
	}

	//rename
	class ActorSyntaxReciever : ISyntaxReceiver {

		public HashSet<TypeDeclarationSyntax> names;

		public ActorSyntaxReciever() {
			names = new();
		}

		public void OnVisitSyntaxNode(SyntaxNode node) {
			if (node is ClassDeclarationSyntax c) {
				var att = GetAttribute(c, "Register");
				if (att != null) {
					names.Add(c);
				}
			}
		}

		AttributeSyntax GetAttribute(ClassDeclarationSyntax c, string name) {
			return c.AttributeLists.SelectMany(x => x.Attributes).SingleOrDefault(x => x.Name.ToString() == name);
		}
	}
}
