namespace Belin.Which

open System.Collections
open System.Collections.Generic

/// Provides convenient access to the stream of search results.
type ResultSet(command: string, finder: Finder) =

  /// All instances of the searched command.
  member this.All: string list = this |> Seq.distinct |> Seq.toList

  /// The first instance of the searched command. Returns `None` if not found.
  member this.First: string option = Seq.tryHead this

  interface IEnumerable<string> with
    /// Returns a new enumerator that allows iterating the results of this set.
    member _.GetEnumerator() = finder.Find(command).GetEnumerator()

  interface IEnumerable with
    /// Returns a new enumerator that allows iterating the results of this set.
    member this.GetEnumerator() = (this :> string seq).GetEnumerator()

/// Contains operations for finding executables in the system path.
module ResultSet = ()
