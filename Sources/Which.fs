namespace Belin.Which

/// Contains operations for finding executables in the system path.
[<AutoOpen>]
module Which =

  /// Finds the instances of the specified command in the system path.
  [<CompiledName("Which")>]
  let which (command: string) = ResultSet(command, Finder())
