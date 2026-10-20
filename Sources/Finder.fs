namespace Belin.Which

open System
open System.IO
open System.Runtime.InteropServices
open System.Text.RegularExpressions

/// Finds the instances of an executable in the system path.
type Finder() =
  let mutable extensions: string list = []
  let mutable paths: string list = []

  /// Tests the accessibility of a file.
  [<DllImport("libc", EntryPoint = "access", CharSet = CharSet.Ansi)>]
  static extern int testFileAccess(string path, int mode)

  /// The list of default executable file extensions.
  static let defaultExtensions = [".exe"; ".cmd"; ".bat"; ".com"]

  /// The regular expression used to remove quotation marks from a path.
  static let quotePattern = Regex @"^""|""$"

  /// The split options used to process the environment variables.
  static let splitOptions = StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries

  // Creates a new finder.
  do
    let extensionList =
      match Environment.GetEnvironmentVariable "PATHEXT" with
      | null -> defaultExtensions
      | variable -> match variable.Split(';', splitOptions) with [||] -> defaultExtensions | items -> List.ofArray items

    let pathList =
      match Environment.GetEnvironmentVariable "PATH" with
      | null -> []
      | variable -> variable.Split(Path.PathSeparator, splitOptions) |> List.ofArray

    extensions <- extensionList |> List.map _.ToLowerInvariant() |> List.distinct
    paths <- pathList |> List.map (fun path -> quotePattern.Replace(path, "")) |> List.distinct

  /// The list of executable file extensions.
  member _.Extensions
    with get() = extensions
    and set(value: string list) = extensions <- value |> List.map _.ToLowerInvariant() |> List.distinct

  /// The list of system paths.
  member _.Paths
    with get() = paths
    and set(value: string list) = paths <- value |> List.map (fun path -> quotePattern.Replace(path, "")) |> List.distinct

  /// Finds the instances of an executable in the system path.
  /// Returns the paths of executables found.
  member this.Find(command: string): string seq =
    seq { for directory in this.Paths do yield! this.FindExecutables(directory, command) }

  /// Gets a value indicating whether the specified file is executable.
  member this.IsExecutable(file: string): bool =
    match File.Exists file with
    | true when OperatingSystem.IsWindows() -> Path.GetExtension file |> defaultIfNull "" |> _.ToLowerInvariant() |> List.contains <| this.Extensions
    | true -> testFileAccess(file, 1 (* X_OK *)) = 0
    | false -> false

  /// Finds the instances of the specified command in a given directory.
  /// Returns the paths of executables found.
  member private this.FindExecutables(directory: string, command: string): string seq =
    seq { "" }
    |> Seq.append (if OperatingSystem.IsWindows() then this.Extensions else [])
    |> Seq.map (fun extension -> Path.Join(directory, $"{command}{extension}") |> Path.GetFullPath)
    |> Seq.filter this.IsExecutable
