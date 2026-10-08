namespace Belin.Which

open Mono.Unix.Native
open System
open System.IO
open System.Runtime.Versioning
open System.Text.RegularExpressions

/// Finds the instances of an executable in the system path.
type Finder() as this =

  /// The list of default executable file extensions.
  static let defaultExtensions = [".exe"; ".cmd"; ".bat"; ".com"]

  /// The regular expression used to remove quotation marks from a path.
  static let quotePattern = Regex @"^""|""$"

  // Creates a new finder.
  do
    let extensions =
      match Environment.GetEnvironmentVariable "PATHEXT" with
      | null -> defaultExtensions
      | variable ->
        let splitOptions = StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries
        match variable.Split(';', splitOptions) with
        | [||] -> defaultExtensions
        | items -> items |> List.ofArray

    let paths =
      match Environment.GetEnvironmentVariable "PATH" with
      | null -> []
      | variable ->
        let splitOptions = StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries
        variable.Split(Path.PathSeparator, splitOptions) |> List.ofArray

    this.Extensions <- extensions |> List.map (fun extension -> extension.ToLowerInvariant()) |> List.distinct
    this.Paths <- paths |> List.map (fun path -> quotePattern.Replace(path, "")) |> List.distinct

  /// The list of executable file extensions.
  member val Extensions: string list = [] with get, set

  /// The list of system paths.
  member val Paths: string list = [] with get, set

  /// Finds the instances of an executable in the system path.
  /// Returns the paths of executables found.
  member this.Find (command: string): string seq =
    seq { for directory in this.Paths do yield! this.FindExecutables directory command }

  /// Gets a value indicating whether the specified file is executable.
  member this.IsExecutable (file: string): bool =
    match File.Exists file with
    | true when OperatingSystem.IsWindows() -> this.CheckFileExtension file
    | true -> this.CheckFilePermissions file
    | _ -> false

  /// Checks that the specified file is executable according to the executable file extensions.
  member private _.CheckFileExtension (file: string): bool =
    let extension = file |> Path.GetExtension |> defaultIfNull ""
    this.Extensions |> List.contains (extension.ToLowerInvariant())

  /// Checks that the specified file is executable according to its permissions.
  [<UnsupportedOSPlatform("windows")>]
  member private _.CheckFilePermissions (file: string): bool =
    let _, stat = Syscall.stat file
    match stat.st_mode with
    | mode when mode.HasFlag FilePermissions.S_IXOTH -> true
    | mode when mode.HasFlag FilePermissions.S_IXGRP -> stat.st_gid = Syscall.getgid()
    | mode when mode.HasFlag FilePermissions.S_IXUSR -> stat.st_uid = Syscall.getuid()
    | mode when mode.HasFlag FilePermissions.S_IXGRP || mode.HasFlag FilePermissions.S_IXUSR -> Syscall.getuid() = 0u
    | _ -> false

  /// Finds the instances of the specified command in a given directory.
  /// Returns the paths of executables found.
  member private this.FindExecutables (directory: string) (command: string): string seq =
    seq { "" }
    |> Seq.append (if OperatingSystem.IsWindows() then this.Extensions else [])
    |> Seq.map (fun extension -> Path.Join(directory, $"{command}{extension}") |> Path.GetFullPath)
    |> Seq.filter this.IsExecutable
