namespace Belin.Which

open Microsoft.VisualStudio.TestTools.UnitTesting
open System
open System.IO

/// Tests the features of the `Finder` class.
[<TestClass>]
type FinderTests() =

  /// The test fixtures.
  let fixtures = Path.Join(AppContext.BaseDirectory, "../Resources")

  // [<TestMethod>]
  // member _.Constructor() =
  //   let splitOptions = StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries

  //   // It should set the `Paths` property to the value of the `PATH` environment variable by default.
  //   let pathEnv = Environment.GetEnvironmentVariable("PATH") ?? ""
  //   List<string> paths = pathEnv.Length > 0 ? [.. pathEnv.Split(Path.PathSeparator, splitOptions).Distinct()] : []
  //   Assert.AreSequenceEqual(paths, new Finder().Paths)

  //   // It should set the `Extensions` property to the value of the `PATHEXT` environment variable by default.
  //   let pathExt = Environment.GetEnvironmentVariable("PATHEXT") ?? ""
  //   List<string> extensions = pathExt.Length > 0 ? [.. pathExt.Split('', splitOptions).Select(item => item.ToLowerInvariant()).Distinct()] : [".exe", ".cmd", ".bat", ".com"]
  //   Assert.AreSequenceEqual(extensions, new Finder().Extensions)

  //   // It should put in lower case the list of file extensions.
  //   Assert.AreSequenceEqual([".exe", ".js", ".ps1"], new Finder(extensions: [".EXE", ".JS", ".PS1"]).Extensions)

  // [<TestMethod>]
  // member _.Find() =
  //   let finder = new Finder(paths: [fixtures])

  //   // It should return the path of the `Executable.cmd` file on Windows.
  //   List<string> executables = [.. finder.Find("Executable")]
  //   Assert.HasCount(OperatingSystem.IsWindows() ? 1 : 0, executables)
  //   if (OperatingSystem.IsWindows()) Assert.EndsWith(@"Resources\Executable.cmd", executables.First())

  //   // It should return the path of the `Executable.sh` file on POSIX.
  //   executables = [.. finder.Find("Executable.sh")]
  //   Assert.HasCount(OperatingSystem.IsWindows() ? 0 : 1, executables)
  //   if (!OperatingSystem.IsWindows()) Assert.EndsWith("Resources/Executable.sh", executables.First())

  //   // It should return an empty array if the searched command is not executable or not found.
  //   Assert.IsEmpty(finder.Find("NotExecutable.sh"))
  //   Assert.IsEmpty(finder.Find("foo"))

  [<TestMethod>]
  member _.IsExecutable() =
    let finder = Finder()

    // It should return `false` if the searched command is not executable or not found.
    finder.IsExecutable "foo/bar/baz.qux" |> shouldBeFalse
    finder.IsExecutable "Resources/NotExecutable.sh" |> shouldBeFalse

    // It should return `false` for a POSIX executable, when test is run on Windows.
    Path.Join(fixtures, "Executable.sh") |> finder.IsExecutable |> shouldNotBe (OperatingSystem.IsWindows())

    // It should return `false` for a Windows executable, when test is run on POSIX.
    Path.Join(fixtures, "Executable.cmd") |> finder.IsExecutable |> shouldBe (OperatingSystem.IsWindows())
