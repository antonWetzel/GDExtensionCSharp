using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators {
	[Generator]
	public class Generators : ISourceGenerator {

		public void Execute(GeneratorExecutionContext context) {
			try {

				var rec = (SyntaxReciever)context.SyntaxReceiver!;

				var classes = new List<INamedTypeSymbol>();

				foreach (var c in rec.names) {

					var s = (INamedTypeSymbol)context.Compilation.GetSemanticModel(c.SyntaxTree).GetDeclaredSymbol(c);

					var gdName = s.GetAttributes().SingleOrDefault(x => x.AttributeClass.Name == "RegisterAttribute").NamedArguments.SingleOrDefault(x => x.Key == "name").Value.Value ?? s.Name;

					var methods = new Methods();

					var notification = Notification.Generate(context, s);
					Export.Generate(context, s, methods);
					Signal.Generate(context, s);

					methods.Generate(context, s);

					var source = $$"""
					using System.Runtime.CompilerServices;
					using System.Runtime.InteropServices;

					namespace {{s.ContainingNamespace}};

					public unsafe partial class {{s.Name}} : {{s.BaseType.Name}} {
						private GCHandle handle;

					#pragma warning disable CS8618
						public {{s.Name}}() {
							handle = GCHandle.Alloc(this);
							var name = Marshal.StringToHGlobalAnsi("{{gdName}}");
							Native.gdInterface.object_set_instance(_internal_pointer, (sbyte*)name, (IntPtr)this);
							Marshal.FreeHGlobal(name);
						}
					#pragma warning restore CS8618

						public static explicit operator IntPtr({{s.Name}} instance) => GCHandle.ToIntPtr(instance.handle);
						public static explicit operator {{s.Name}}(IntPtr ptr) => ({{s.Name}})(GCHandle.FromIntPtr(ptr).Target!);

						public static {{s.Name}} Construct(IntPtr ptr) {
							ptr = *(IntPtr*)(void*)(ptr + 16); //Did I miss the inverse function to 'object_set_instance'?, this only works if Godot.Object does not change
							return ({{s.Name}})ptr;
						}

						public static unsafe new void Register() {
							var info = new Native.ExtensionClassCreationInfo() {
								is_virtual = false,
								is_abstract = false,
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
								//class_userdata = IntPtr.Zero,
							};
							var ptrName = Marshal.StringToHGlobalAnsi("{{gdName}}");
							var ptrBase = Marshal.StringToHGlobalAnsi("{{s.BaseType.Name}}");
							Native.gdInterface.classdb_register_extension_class(Native.gdLibrary, (sbyte*)ptrName, (sbyte*)ptrBase, &info);
							Marshal.FreeHGlobal(ptrName);
							Marshal.FreeHGlobal(ptrBase);
							RegisterMethods();
							RegisterExports();
							RegisterSignals();

							GDExtension.Object.RegisterConstructor("{{s.Name}}", Construct);
						}

						[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
						static unsafe IntPtr CreateObject(IntPtr userdata) {
							var test = new {{s.Name}}();
							return test._internal_pointer;
						}

						[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
						static unsafe void FreeObject(IntPtr userdata, IntPtr instance) {
							var test = ({{s.Name}})instance;
							test.handle.Free();
						}
					}
					""";
					context.AddSource($"{s.Name}.gen.cs", source);

					classes.Add(s);
				}
				Entry.Execute(context, classes);
			} catch (System.Exception e) {
				context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
					"godot",
					"godotError",
					e.Message,
					"location",
					DiagnosticSeverity.Error,
					true
				), null));
			}
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
