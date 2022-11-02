var documentationPath = "../godot/doc/classes/"; //set to null if no godot repository is avaible
var configName = "float_64";
var apiPath = "./extension_api.json";
var dir = "./GDExtension/Generated";

//create or clean directory
if (Directory.Exists(dir)) {
	Directory.Delete(dir, true);
}
Directory.CreateDirectory(dir);

var api = Api.Create(apiPath);
var convert = new Convert(api, dir, documentationPath, configName);
convert.Emit();
