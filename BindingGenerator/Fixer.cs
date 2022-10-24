public static class Fixer {

	public static string Type(string name) {
		if (name.StartsWith("enum::")) {
			name = name.Substring(6);
		}
		name = name.Replace("const ", "");
		if (name.Contains("typedarray::")) {
			return "Array";
		}
		name = name.Replace("::", ".");
		if (name.Contains("VariantType.")) {
			return "Variant";
		}
		if (name.StartsWith("bitfield.")) { name = name.Replace("bitfield.", ""); }
		if (name.StartsWith("uint64_t")) { name = name.Replace("uint64_t", "UInt64"); }
		if (name.StartsWith("uint16_t")) { name = name.Replace("uint16_t", "UInt16"); }
		if (name.StartsWith("uint8_t")) { name = name.Replace("uint8_t", "byte"); }
		if (name.StartsWith("int32_t")) { name = name.Replace("int32_t", "int"); }
		if (name.StartsWith("real_t")) { name = name.Replace("real_t", "float"); }
		if (name.StartsWith("float")) { name = name.Replace("float", "double"); }
		if (name.StartsWith("int")) { name = name.Replace("int", "long"); }
		if (name.StartsWith("String")) { name = name.Replace("int", "string"); }
		if (name.StartsWith("VariantType")) { name = name.Replace("VariantType", "Variant.Type"); }

		return name;
	}

	public static string Name(string name) {
		return name switch {
			"object" => "@object",
			"base" => "@base",
			"interface" => "@interface",
			"class" => "@class",
			"default" => "@default",
			"char" => "@char",
			"string" => "@string",
			"event" => "@event",
			"lock" => "@lock",
			"operator" => "@operator",
			"enum" => "@enum",
			"in" => "@in",
			"out" => "@out",
			"checked" => "@checked",
			"override" => "@override",
			"new" => "@new",
			"params" => "@params",
			"internal" => "@internal",
			"bool" => "@bool",
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

	public static string SnakeToPascal(string name) {
		var res = "";
		foreach (var w in name.Split('_')) {
			if (w.Length == 0) {
				res += "_";
			} else {
				res += w[0].ToString().ToUpper() + w.Substring(1);
			}
		}
		return res;
	}
}
