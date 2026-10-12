#r "nuget: Belin.Which.FSharp, 1.0.0-rc.1"
open Belin.Which

// Finds all instances of an executable.
match which("foobar").All with
| [||] ->
  eprintfn "The 'foobar' command cannot be found."
| paths ->
  printfn "The 'foobar' command is available at these locations:"
  for path in paths do printfn $"- {path}"
