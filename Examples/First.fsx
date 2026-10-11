#r "nuget: Belin.Which.FSharp"
open Belin.Which

// Finds the first instance of an executable.
match which("foobar").First with
| None ->
    eprintfn "The 'foobar' command cannot be found."
| Some path ->
    printfn $"The 'foobar' command is located at: {path}"
