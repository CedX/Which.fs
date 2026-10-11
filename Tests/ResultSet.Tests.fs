namespace Belin.Which

open Microsoft.VisualStudio.TestTools.UnitTesting
open System
open System.IO

/// Tests the features of the `ResultSet` class.
[<TestClass>]
type ResultSetTests() =

  /// The path to the test fixtures.
  let fixtures = Path.Join(AppContext.BaseDirectory, "../Resources")

  /// Finds the instances of the specified command in the system path.
  let which (command: string) = ResultSet(command, Finder(Paths = [fixtures]))

  [<TestMethod>]
  member _.All() =
    // It should return the path of the `Executable.cmd` file on Windows.
    let mutable executables = which("Executable").All
    if not (OperatingSystem.IsWindows()) then Should.beEmpty executables
    else
      executables.Length |> Should.be 1
      executables[0] |> Should.endWith @"\Resources\Executable.cmd"

    // It should return the path of the `Executable.sh` file on POSIX.
    executables <- which("Executable.sh").All
    if OperatingSystem.IsWindows() then Should.beEmpty executables
    else
      executables.Length |> Should.be 1
      executables[0] |> Should.endWith "/Resources/Executable.sh"

    // It should return an empty array if the searched command is not executable or not found.
    Should.beEmpty (which("NotExecutable.sh").All)
    Should.beEmpty (which("foo").All)

  [<TestMethod>]
  member _.First() =
    // It should return the path of the `Executable.cmd` file on Windows.
    let mutable executable = which("Executable").First
    match OperatingSystem.IsWindows() with
    | false -> executable |> Should.be None
    | true -> executable |> Option.toObj |> defaultIfNull "" |> Should.endWith @"\Resources\Executable.cmd"

    // It should return the path of the `Executable.sh` file on POSIX.
    executable <- which("Executable.sh").First
    match OperatingSystem.IsWindows() with
    | false -> executable |> Option.toObj |> defaultIfNull "" |> Should.endWith "/Resources/Executable.sh"
    | true -> executable |> Should.be None

    // It should return `null` if the searched command is not executable or not found.
    which("NotExecutable.sh").First |> Should.be None
    which("foo").First |> Should.be None

  // [<TestMethod>]
  member _.GetEnumerator() =
    // It should return the path of the `Executable.cmd` file on Windows.
    let mutable found = false
    for executable in which "Executable" do
      executable |> Should.endWith @"\Resources\Executable.cmd"
      found <- true

    found |> Should.be (OperatingSystem.IsWindows())

    // It should return the path of the `Executable.sh` file on POSIX.
    found <- false
    for executable in which "Executable.sh" do
      executable |> Should.endWith "/Resources/Executable.sh"
      found <- true

    found |> Should.notBe (OperatingSystem.IsWindows())

    // It should not return any result if the searched command is not executable or not found.
    found <- false
    for _ in which "NotExecutable.sh" do found <- true
    Should.beFalse found

    found <- false
    for _ in which "foo" do found <- true
    Should.beFalse found
