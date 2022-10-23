extends Node

func _ready():
	var n = load("res://source.gdextension") as NativeExtension
	print(n.get_minimum_library_initialization_level())
