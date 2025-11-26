# Lambda Expressions and Higher-Order Functions

This guide demonstrates how to use lambda expressions and higher-order functions in Z3Wrap for functional programming over sequences and arrays.

## Table of Contents

- [Lambda Expression Basics](#lambda-expression-basics)
- [Sequence Map Operations](#sequence-map-operations)
- [Sequence Fold Operations](#sequence-fold-operations)
- [Practical Examples](#practical-examples)
- [Advanced Patterns](#advanced-patterns)

## Lambda Expression Basics

Lambda expressions in Z3 are anonymous functions that can be used with higher-order operations. Z3Wrap supports lambdas with 1-3 parameters.

### Creating Lambda Expressions

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();

// Single parameter: x => x + 1
var x = context.IntConst("x");
var increment = context.Lambda(x, x + context.Int(1));

// Two parameters: (a, b) => a + b
var a = context.IntConst("a");
var b = context.IntConst("b");
var add = context.Lambda(a, b, a + b);

// Three parameters: (i, acc, elem) => acc + elem + i
var i = context.IntConst("i");
var acc = context.IntConst("acc");
var elem = context.IntConst("elem");
var foldWithIndex = context.Lambda(i, acc, elem, acc + elem + i);
```

### Applying Lambda Expressions

Lambdas can be applied directly or used with higher-order sequence functions:

```csharp
// Direct application
var doubler = context.Lambda(x, x * context.Int(2));
var result = doubler.Apply(context.Int(5));  // Returns 10

// With sequences (shown below)
var seq = context.Seq(context.Int(1), context.Int(2), context.Int(3));
var doubled = seq.Map(doubler);  // [2, 4, 6]
```

## Sequence Map Operations

Map transforms each element of a sequence using a lambda function.

### Basic Map

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// Create sequence [1, 2, 3, 4, 5]
var numbers = context.Seq<IntExpr>(1, 2, 3, 4, 5);

// Double each element
var x = context.IntConst("x");
var doubler = context.Lambda(x, x * 2);
var doubled = numbers.Map(doubler);

// Verify: [2, 4, 6, 8, 10]
var expected = context.Seq<IntExpr>(2, 4, 6, 8, 10);

solver.Assert(doubled == expected);
Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
```

### Map with Index (Mapi)

Map with index tracking allows transformations that depend on element position:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// Create sequence [100, 200, 300]
var values = context.Seq(context.Int(100), context.Int(200), context.Int(300));

// Add index to each element: (index, value) => value + index
var i = context.IntConst("i");
var v = context.IntConst("v");
var addIndex = context.Lambda(i, v, v + i);

// Map with index starting at 0
var result = values.Mapi(addIndex, context.Int(0));

// Result: [100+0, 200+1, 300+2] = [100, 201, 302]
var expected = context.Seq(context.Int(100), context.Int(201), context.Int(302));

solver.Assert(result == expected);
Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
```

### Chaining Map Operations

Multiple map operations can be chained for complex transformations:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var numbers = context.Seq(context.Int(1), context.Int(2), context.Int(3));

// First: square each element
var x = context.IntConst("x");
var square = context.Lambda(x, x * x);
var squared = numbers.Map(square);  // [1, 4, 9]

// Then: add 10 to each
var y = context.IntConst("y");
var addTen = context.Lambda(y, y + context.Int(10));
var result = squared.Map(addTen);  // [11, 14, 19]

var expected = context.Seq(context.Int(11), context.Int(14), context.Int(19));
solver.Assert(result == expected);
Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
```

## Sequence Fold Operations

Fold (reduce) operations accumulate sequence elements into a single value.

### Sum with Foldl

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// Create sequence [1, 2, 3, 4, 5]
var numbers = context.Seq<IntExpr>(1, 2, 3, 4, 5);

// Sum: (accumulator, element) => accumulator + element
var acc = context.IntConst("acc");
var elem = context.IntConst("elem");
var sum = context.Lambda(acc, elem, acc + elem);

// Fold with initial accumulator 0
var total = numbers.Foldl(sum, 0);

// Verify sum is 15
solver.Assert(total == 15);
Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
```

### Product with Foldl

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var numbers = context.Seq(context.Int(2), context.Int(3), context.Int(4));

// Product: (accumulator, element) => accumulator * element
var acc = context.IntConst("acc");
var elem = context.IntConst("elem");
var product = context.Lambda(acc, elem, acc * elem);

// Fold with initial accumulator 1
var result = numbers.Foldl(product, context.Int(1));

// Verify product is 24 (2 * 3 * 4)
solver.Assert(result == context.Int(24));
Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
```

### Fold with Index (Foldli)

Foldli tracks the index during folding:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var values = context.Seq(context.Int(10), context.Int(20), context.Int(30));

// Sum indices: (index, accumulator, element) => accumulator + index
var i = context.IntConst("i");
var acc = context.IntConst("acc");
var elem = context.IntConst("elem");
var sumIndices = context.Lambda(i, acc, elem, acc + i);

// Fold with index starting at 0, accumulator 0
var result = values.Foldli(sumIndices, context.Int(0), context.Int(0));

// Result: 0 + 0 + 1 + 2 = 3
solver.Assert(result == context.Int(3));
Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
```

## Practical Examples

### Computing Average

Use map and fold together to compute sequence statistics:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var numbers = context.Seq(
    context.Int(10), context.Int(20), context.Int(30), context.Int(40)
);

// Sum all elements
var acc = context.IntConst("acc");
var elem = context.IntConst("elem");
var sum = context.Lambda(acc, elem, acc + elem);
var total = numbers.Foldl(sum, context.Int(0));  // 100

// Verify average: total / length = 100 / 4 = 25
var length = numbers.Length();
var average = total / length;

solver.Assert(average == context.Int(25));
Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
```

### Finding Maximum Value

Implement max using fold with conditional logic:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var numbers = context.Seq(
    context.Int(5), context.Int(12), context.Int(3),
    context.Int(18), context.Int(7)
);

// Max: (acc, elem) => if (elem > acc) elem else acc
var acc = context.IntConst("acc");
var elem = context.IntConst("elem");
var max = context.Lambda(acc, elem, context.Ite(elem > acc, elem, acc));

// Fold with initial value (first element conceptually)
var result = numbers.Foldl(max, context.Int(-1000));

// Verify maximum is 18
solver.Assert(result == context.Int(18));
Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
```

### Transform and Aggregate Pattern

Common pattern: transform elements then aggregate:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// Compute sum of squares
var numbers = context.Seq(context.Int(1), context.Int(2), context.Int(3), context.Int(4));

// Step 1: Map to squares
var x = context.IntConst("x");
var square = context.Lambda(x, x * x);
var squares = numbers.Map(square);  // [1, 4, 9, 16]

// Step 2: Fold to sum
var acc = context.IntConst("acc");
var elem = context.IntConst("elem");
var sum = context.Lambda(acc, elem, acc + elem);
var sumOfSquares = squares.Foldl(sum, context.Int(0));  // 30

solver.Assert(sumOfSquares == context.Int(30));
Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
```

## Advanced Patterns

### Boolean Predicate Mapping

Use map to apply predicates to sequences:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var numbers = context.Seq(context.Int(1), context.Int(2), context.Int(3), context.Int(4));

// Map to "is even" predicates
var x = context.IntConst("x");
var isEven = context.Lambda(x, (x % context.Int(2)) == context.Int(0));
var predicates = numbers.Map(isEven);

// Result should be [false, true, false, true]
var expected = context.Seq(
    false, true,
    false, true
);

solver.Assert(predicates == expected);
Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
```

### Custom Accumulator Types

Fold can accumulate into any expression type:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// Sequence of integers
var numbers = context.Seq(context.Int(1), context.Int(2), context.Int(3));

// Fold into a string by concatenating string representations
// Note: This would require string conversion operations
// Shown here conceptually - actual implementation may vary

// More practical: Count elements using integer accumulator
var acc = context.IntConst("acc");
var elem = context.IntConst("elem");
var count = context.Lambda(acc, elem, acc + context.Int(1));
var length = numbers.Foldl(count, context.Int(0));

solver.Assert(length == context.Int(3));
Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
```

### Index-Based Transformations

Use Mapi for position-dependent transformations:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// Create sequence of equal values
var values = context.Seq(context.Int(10), context.Int(10), context.Int(10));

// Multiply each by its index
var i = context.IntConst("i");
var v = context.IntConst("v");
var multiplyByIndex = context.Lambda(i, v, v * i);

var result = values.Mapi(multiplyByIndex, context.Int(1));

// Result: [10*1, 10*2, 10*3] = [10, 20, 30]
var expected = context.Seq(context.Int(10), context.Int(20), context.Int(30));

solver.Assert(result == expected);
Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
```

## Performance Considerations

1. **Lambda Reuse**: Create lambdas once and reuse them across multiple operations
2. **Minimize Chaining**: While functional, deeply chained operations may increase solving time
3. **Simplification**: Z3 will simplify lambda applications, but complex bodies may impact performance
4. **Type Inference**: Explicit type parameters help Z3 optimize constraint generation

## Best Practices

1. **Clear Lambda Bodies**: Keep lambda bodies simple and focused
2. **Meaningful Names**: Use descriptive variable names even for bound variables
3. **Test Incrementally**: Test map and fold operations separately before combining
4. **Consider Alternatives**: For simple operations, direct sequence manipulation may be clearer
5. **Document Intent**: Comment complex lambda expressions explaining the transformation logic

---

This guide covers the fundamentals of lambda expressions and higher-order functions in Z3Wrap. For more advanced use cases, consult the Z3 documentation on lambda expressions and array theory.
