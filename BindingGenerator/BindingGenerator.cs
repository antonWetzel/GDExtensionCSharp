var api = Api.Create("./godot-headers/extension_api.json");

var dir = "./GDExtension/Generated";

var configName = "float_64";

if (Directory.Exists(dir)) {
	Directory.Delete(dir, true);
}
Directory.CreateDirectory(dir);

Convert.Api(api, dir, configName);
