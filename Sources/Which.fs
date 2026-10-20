namespace Belin.Which

/// Contains operations for finding executables in the system path.
[<AutoOpen>]
module Which =

  /// Finds the instances of the specified command in the system path.
  let which (command: string) = ResultSet(command, Finder())

  /// Finds the instances of the specified command in the given set of paths.
  let whichInPaths (command: string) (paths: string seq) = ResultSet(command, Finder(Paths = Seq.toList paths))
