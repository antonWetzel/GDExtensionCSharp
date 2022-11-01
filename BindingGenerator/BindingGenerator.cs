var documentationPath = "../godot/doc/classes/";
var apiPath = "./extension_api.json";
var serializer = new XmlSerializer(typeof(Documentation.Class));

var api = Api.Create(apiPath);


var dir = "./GDExtension/Generated";

var configName = "float_64";

if (Directory.Exists(dir)) {
	Directory.Delete(dir, true);
}
Directory.CreateDirectory(dir);

var convert = new Convert(api, dir, documentationPath, configName);
convert.Emit();
