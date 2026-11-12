#r "nuget: Belin.Which.FSharp"
open Belin.Which

// Finds all instances of an executable and returns them one at a time.
printn "The 'foobar' command is available at these locations:"
for path in which "foo" do printn $"- {path}"
