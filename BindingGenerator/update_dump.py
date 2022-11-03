import os

godot_path = "../godot/bin/godot.windows.editor.x86_64.exe"
target_path = "./BindingGenerator/dump/"

godot_path = os.getcwd() + "/" + godot_path
os.chdir(target_path)

os.system(godot_path + " --dump-gdextension-interface")
os.system(godot_path + " --dump-extension-api")
