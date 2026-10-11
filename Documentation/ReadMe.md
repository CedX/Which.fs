# Which for F#
Find the instances of an executable in the system path,
in [F#](https://learn.microsoft.com/en-us/dotnet/fsharp).

## Quick start
Install the latest version of **Which for F#** with [NuGet](https://www.nuget.org) package manager:

```powershell
dotnet package add Belin.Which.FSharp
```

For detailed instructions, see the [installation guide](Installation.md).

## Usage
This package provides the `which (command: string)` function, allowing to locate a command in the system path.  
This function takes the name of the command to locate, and returns a `ResultSet` instance.

The `ResultSet` class implements the `string seq` interface.  
It is therefore possible to iterate over the results using a `for...in` loop:

```fsharp
open Belin.Which

// Finds all instances of an executable and returns them one at a time.
printfn "The 'foobar' command is available at these locations:"
for path in which "foo" do printfn $"- {path}"
```

The `ResultSet` class also provides two convenient properties:

- `All` : get all instances of the searched command.
- `First` : get the first instance of the searched command.

### **All**: string array
The `ResultSet.All` property returns an array of the absolute paths of all instances of an executable found in the system path.
If the executable could not be located, it returns an empty array.

```fsharp
open Belin.Which

match which("foobar").All with
| [||] ->
    eprintfn "The 'foobar' command cannot be found."
| paths ->
    printfn "The 'foobar' command is available at these locations:"
    for path in paths do printfn $"- {path}"
```

### **First**: string option
The `ResultSet.First` property returns the absolute path of the first instance of an executable found in the system path.
If the executable could not be located, it returns a `None` value.

```fsharp
open Belin.Which

match which("foobar").First with
| None ->
    eprintfn "The 'foobar' command cannot be found."
| Some path ->
    printfn $"The 'foobar' command is located at: {path}"
```

## Options
The `which()` function is defined as:

```fsharp
let which (command: string) = ResultSet(command, Finder())
```

You can configure the search behavior by using a customized `Finder` instance.  
The `Finder` class has the following properties.

### **Extensions**: string list
A list of strings specifying the executable file extensions.
On Windows, defaults to the list of extensions provided by the `PATHEXT` environment variable.

```fsharp
let finder = Finder(Extensions = [".foo"; ".exe"; ".cmd"])
ResultSet("foobar", finder).All |> Array.iter (printfn "%s")
```

> [!NOTE]
> The `Extensions` property is only meaningful on the Windows platform,
> where the executability of a file is determined from its extension.

### **Paths**: string list
A list of strings specifying the system paths from which the given command will be searched.
Defaults to the list of directories provided by the `PATH` environment variable.

```fsharp
let finder = Finder(Paths = ["/usr/local/bin", "/usr/bin"])
ResultSet("foobar", finder).All |> Array.iter (printfn "%s")
```
