namespace Belin.Which

open Microsoft.VisualStudio.TestTools.UnitTesting

/// Contains assertions for working with unit tests.
[<AutoOpen>]
module Assertions =

  /// Tests whether the specified values are equal.
  let inline shouldBe (expected: 'T) (actual: 'T) =
    Assert.AreEqual<'T>(expected, actual)

  /// Tests whether the specified collection is empty.
  let inline shouldBeEmpty (collection: 'T seq) =
    Assert.IsEmpty collection

  /// Tests whether the specified string is empty.
  let inline shouldBeEmptyString (actual: string) =
    Assert.AreEqual(0, actual.Length)

  /// Tests whether the specified condition is false.
  let inline shouldBeFalse (condition: bool) =
    Assert.IsFalse condition

  /// Tests whether the specified condition is true.
  let inline shouldBeTrue (condition: bool) =
    Assert.IsTrue condition

  /// Tests whether the specified collection has the expected count/length.
  let inline shouldHaveCount (expected: int) (collection: 'T seq) =
    Assert.HasCount(expected, collection)

  /// Tests whether the specified values are unequal.
  let inline shouldNotBe (notExpected: 'T) (actual: 'T) =
    Assert.AreNotEqual<'T>(notExpected, actual)
