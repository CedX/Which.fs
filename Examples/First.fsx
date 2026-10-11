#r "../Binaries/Belin.Which.FSharp.dll"
open Belin.Which

// Finds the first instance of an executable.
let path = which("foobar").First
match path with
| None -> printfn "The 'foobar' command cannot be found."
| Some value -> printfn $"The 'foobar' command is located at: {value}"
