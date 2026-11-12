#r "nuget: Belin.Which.FSharp"
open Belin.Which

// Finds all instances of an executable.
match which("foobar").All with
| [||] ->
  eprintfn "The 'foobar' command cannot be found."
| paths ->
  printn "The 'foobar' command is available at these locations:"
  for path in paths do printn $"- {path}"
