/// <summary>
/// Finds the instances of an executable in the system path.
/// </summary>
public partial class Finder {

	/// <summary>
	/// Finds the instances of the specified command in the system path.
	/// </summary>
	/// <param name="command">The command to be resolved.</param>
	/// <param name="paths">The system path. Defaults to the <c>PATH</c> environment variable.</param>
	/// <param name="extensions">The executable file extensions. Defaults to the <c>PATHEXT</c> environment variable.</param>
	/// <returns>The search results.</returns>
	public static ResultSet Which(string command, string[]? paths = null, string[]? extensions = null) =>
		new(command, new Finder(paths, extensions));
