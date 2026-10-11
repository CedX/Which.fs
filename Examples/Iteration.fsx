#r "../Binaries/Belin.Which.FSharp.dll"
open Belin.Which

// Finds all instances of an executable and returns them one at a time.
printfn "The 'foobar' command is available at these locations:"
for path in which "foo" do printfn $"- {path}"
