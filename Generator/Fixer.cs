public static class Fixer {

	public static string Type(string name) {
		return name switch {
			"int" => "long",
			"float" => "double",
			_ => name,
		};
	}

	public static string Name(string name) {
		return name switch {
			"object" => "@object",
			"base" => "@base",
			"interface" => "@interface",
			"class" => "@class",
			"default" => "@default",
			"char" => "@char",
			_ => name,
		};
	}

	public static string VariantOperator(string type) {
		return type switch {
			"==" => "Equal",
			"!=" => "NotEqual",
			"<" => "Less",
			"<=" => "LessEqual",
			">" => "Greater",
			">=" => "GreaterEqual",
			/* mathematic */
			"+" => "Add",
			"-" => "Subtract",
			"*" => "Multiply",
			"/" => "Divide",
			"unary-" => "Negate",
			"unary+" => "Positive",
			"%" => "Module",
			"**" => "Power",
			/* bitwise */
			"<<" => "ShiftLeft",
			">>" => "ShiftRight",
			"&" => "BitAnd",
			"|" => "BitOr",
			"^" => "BitXor",
			"!" => "BitNegate",
			/* logic */
			"and" => "And",
			"or" => "Or",
			"xor" => "Xor",
			"not" => "Not",
			/* containment */
			"in" => "In",
			_ => type,
		};
	}

	public static string VariantName(string name) {
		return name switch {
			"int" => "Int",
			"float" => "Float",
			"bool" => "Bool",
			_ => name,
		};
	}

	public static string Value(string value) {
		if (value.Contains("(")) {
			value = "new " + value;
		};
		value = value.Replace("inf", "double.PositiveInfinity");
		return value;
	}
}
