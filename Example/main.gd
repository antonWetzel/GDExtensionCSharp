extends Node

@onready var test: Test = $"Test"

func _ready():
	print(test)
	var n = load("res://source.gdextension") as NativeExtension
	print(n.get_minimum_library_initialization_level())
