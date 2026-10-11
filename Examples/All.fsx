#r "../Binaries/Belin.Which.FSharp.dll"
open Belin.Which

// Finds all instances of an executable.
let paths = which("foobar").All
if paths.Length = 0 then
  printfn "The 'foobar' command cannot be found."
else
  printfn "The 'foobar' command is available at these locations:"
  for path in paths do printfn $"- {path}"
