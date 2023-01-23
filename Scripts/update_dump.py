import os

godot_path = "../godot/bin/godot.windows.editor.dev.x86_64.exe"
target_path = "./BindingGenerator/dump/"
current_path = os.getcwd()

godot_path = os.getcwd() + "/" + godot_path
os.chdir(target_path)

os.system(godot_path + " --dump-gdextension-interface")
os.system(godot_path + " --dump-extension-api")

# nuget version has problems with the genration
os.system(current_path + "/../ClangSharp/artifacts/bin/sources/ClangSharpPInvokeGenerator/Debug/net6.0/ClangSharpPInvokeGenerator.exe @gdextension_interface.rsp")
