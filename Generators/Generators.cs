using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators {
	[Generator]
	public class Generators : ISourceGenerator {

		public void Execute(GeneratorExecutionContext context) {
			var rec = (SyntaxReciever)context.SyntaxReceiver!;

			var classes = new List<(string, string)>();

			foreach (var c in rec.names) {

				var s = (INamedTypeSymbol)context.Compilation.GetSemanticModel(c.SyntaxTree).GetDeclaredSymbol(c);

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
							notification_func = new(__Notification),
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

					static unsafe Native.ObjectPtr CreateObject(IntPtr userdata) {
						var test = new {{s.Name}}();
						test.SetProcess(true);
						return test._internal_pointer;
					}

					static unsafe void FreeObject(IntPtr userdata, Native.GDExtensionClassInstancePtr instance) {
						var test = ({{s.Name}})instance;
						test.handle.Free();
					}
				}
				""";
				context.AddSource($"{s.Name}.gen.cs", source);

				Notification.Generate(context, s);
				Export.Generate(context, s);

				classes.Add((s.Name, s.BaseType.Name));
			}
			Entry.Execute(context, classes);
		}

		public void Initialize(GeneratorInitializationContext context) {
			context.RegisterForSyntaxNotifications(() => new SyntaxReciever());
		}
	}

	class SyntaxReciever : ISyntaxReceiver {

		public HashSet<TypeDeclarationSyntax> names;

		public SyntaxReciever() {
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
