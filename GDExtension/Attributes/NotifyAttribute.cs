namespace GDExtension;

[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
public class NotifyAttribute : System.Attribute {
	public int notification;
	public string? arguments;

	public NotifyAttribute(int notification) {
		this.notification = notification;
	}
}
