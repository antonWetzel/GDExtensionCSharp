extends Node

@onready var test: TestClass = $"TestClass"

func _ready():
	print(test)
	var n = load("res://ein.gdextension") as NativeExtension
	print(n.get_minimum_library_initialization_level())
