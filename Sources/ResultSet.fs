namespace Belin.Which

open System.Collections
open System.Collections.Generic

/// Provides convenient access to the stream of search results.
type ResultSet(command: string, finder: Finder) =

  interface IEnumerable<string> with
    /// Returns a new enumerator that allows iterating the results of this set.
    member _.GetEnumerator() : IEnumerator<string> =
      (items :> IEnumerable<string>).GetEnumerator()

  interface IEnumerable with
    /// Returns a new enumerator that allows iterating the results of this set.
    member _.GetEnumerator() : IEnumerator =
      (items :> IEnumerable).GetEnumerator()

/// Contains operations for finding executables in the system path.
module ResultSet = ()
