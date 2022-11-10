extends Node

func _ready() -> void:
	var s = Summator.new()
	s.add(10)
	s.add(20)
	print(s.get_total())
	s.reset()
