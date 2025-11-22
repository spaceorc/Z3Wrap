# What Can You Do With Bitvector Constraints?

Bitvector theory in Z3 lets you **reason about fixed-width binary arithmetic** at the bit level. Think of it as "algebra for binary data" - perfect for hardware verification, cryptography, and low-level programming.

## Use Cases

### 1. **Find Integer Overflow Bugs**

**Problem**: Does this arithmetic operation overflow in 8-bit unsigned math?

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var x = context.BvConst<Size8>("x");
var y = context.BvConst<Size8>("y");

// Check if x + y overflows (wraps around)
var sum = x + y;

// For overflow: sum < x (in unsigned arithmetic)
solver.Assert(sum < x);  // Unsigned comparison
solver.Assert(x > 0);
solver.Assert(y > 0);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    var xVal = model.GetBitVec(x);
    var yVal = model.GetBitVec(y);
    Console.WriteLine($"Overflow: {xVal} + {yVal} = {model.GetBitVec(sum)}");
    // Example: 200 + 100 = 44 (300 wrapped to 8 bits)
}
```

**What Z3 does**: Finds concrete values that trigger overflow conditions.

---

### 2. **Reverse Engineering - Find XOR Key**

**Problem**: What XOR key decrypts this data?

```csharp
var key = context.BvConst<Size32>("key");
var encrypted = context.Bv<Size32>(0xDEADBEEF);
var plaintext = context.Bv<Size32>(0x48454C4C);  // "HELL" in ASCII

// XOR encryption: plaintext ^ key = encrypted
solver.Assert((plaintext ^ key) == encrypted);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    var keyValue = model.GetBitVec(key);
    Console.WriteLine($"Key: 0x{keyValue:X}");
    // Verifies: HELL ^ key = DEADBEEF
}
```

**What Z3 does**: Solves for unknown crypto keys given plaintext/ciphertext pairs.

---

### 3. **Hardware Verification - ALU Testing**

**Problem**: Does our ALU's subtract operation work correctly?

```csharp
var a = context.BvConst<Size16>("a");
var b = context.BvConst<Size16>("b");

// Hardware implementation (with potential bug)
var result = context.BvConst<Size16>("result");
var borrow = context.BvConst<Size1>("borrow");

// Model the circuit: result = a - b
solver.Assert(result == (a - b));

// Check: does result ever equal a when b != 0?
solver.Assert(result == a);
solver.Assert(b != 0);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Bug found: {model.GetBitVec(a)} - {model.GetBitVec(b)} = {model.GetBitVec(result)}");
    // Example: 0 - 1 = 0 in buggy implementation
}
```

**What Z3 does**: Finds counterexamples to hardware specifications.

---

### 4. **Bit Manipulation Puzzles**

**Problem**: Find number where flipping bit 3 makes it divisible by 8

```csharp
var x = context.BvConst<Size32>("x");

// Flip bit 3 (zero-indexed from right)
var bit3 = (x >> 3) & 1;
var flipped = x ^ (context.Bv<Size32>(1) << 3);

// Original not divisible by 8, but flipped version is
solver.Assert((x & 7) != 0);          // x % 8 != 0
solver.Assert((flipped & 7) == 0);    // flipped % 8 == 0

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    var xVal = model.GetBitVec(x);
    var flippedVal = model.GetBitVec(flipped);
    Console.WriteLine($"{xVal} -> {flippedVal} (bit 3 flipped)");
    // Example: 5 (0b101) -> 13 (0b1101) which is NOT divisible by 8
    // Better: 16 (0b10000) -> 24 (0b11000) which IS divisible by 8
}
```

**What Z3 does**: Explores bit-level transformations symbolically.

---

### 5. **Cryptographic Collision Finding**

**Problem**: Find two different inputs with same hash output (simplified hash)

```csharp
var input1 = context.BvConst<Size32>("input1");
var input2 = context.BvConst<Size32>("input2");

// Simple "hash" function: rotate left 7 bits and XOR with 0xABCD
var hash1 = (input1 << 7 | input1 >> 25) ^ context.Bv<Size32>(0xABCD);
var hash2 = (input2 << 7 | input2 >> 25) ^ context.Bv<Size32>(0xABCD);

// Collision: same hash, different inputs
solver.Assert(hash1 == hash2);
solver.Assert(input1 != input2);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Collision: {model.GetBitVec(input1)} and {model.GetBitVec(input2)}");
}
else
{
    Console.WriteLine("Hash is collision-resistant!");
}
```

**What Z3 does**: Analyzes cryptographic properties automatically.

---

### 6. **Signed vs Unsigned Arithmetic**

**Problem**: When does signed division differ from unsigned?

```csharp
var x = context.BvConst<Size8>("x");
var y = context.BvConst<Size8>("y");

// Signed division (interprets as two's complement)
var signedDiv = x.Div(y, signed: true);

// Unsigned division
var unsignedDiv = x.Div(y, signed: false);

// Find cases where they differ
solver.Assert(signedDiv != unsignedDiv);
solver.Assert(y != 0);  // Avoid division by zero

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    var xVal = model.GetBitVec(x);
    var yVal = model.GetBitVec(y);
    Console.WriteLine($"Signed: {xVal} / {yVal} = {model.GetBitVec(signedDiv)}");
    Console.WriteLine($"Unsigned: {xVal} / {yVal} = {model.GetBitVec(unsignedDiv)}");
    // Example: 0xFF (255 unsigned, -1 signed) / 2
    // Unsigned: 255/2 = 127
    // Signed: -1/2 = 0 (rounds toward zero)
}
```

**What Z3 does**: Reveals subtle differences in numeric interpretations.

---

### 7. **Bit Field Extraction**

**Problem**: Extract RGB values from packed 32-bit color (0xAARRGGBB)

```csharp
var color = context.BvConst<Size32>("color");

// Extract components (8 bits each)
var alpha = color.Extract<Size8>(24);  // Bits 31-24
var red   = color.Extract<Size8>(16);  // Bits 23-16
var green = color.Extract<Size8>(8);   // Bits 15-8
var blue  = color.Extract<Size8>(0);   // Bits 7-0

// Constraint: find semi-transparent red (alpha=128, red=255, green=0, blue=0)
solver.Assert(alpha == 128);
solver.Assert(red == 255);
solver.Assert(green == 0);
solver.Assert(blue == 0);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    var colorVal = model.GetBitVec(color);
    Console.WriteLine($"Color: 0x{colorVal:X8}");
    // Output: 0x80FF0000
}
```

**What Z3 does**: Reasons about bit packing and unpacking operations.

---

### 8. **Bounded Model Checking - Loop Unwinding**

**Problem**: Can counter overflow in this bounded loop?

```csharp
// Model 3 iterations of: counter = (counter + 1) & 0xFF
var counter0 = context.BvConst<Size8>("counter0");
var counter1 = (counter0 + 1) & 0xFF;
var counter2 = (counter1 + 1) & 0xFF;
var counter3 = (counter2 + 1) & 0xFF;

// Check if we can start at non-overflow and reach overflow
solver.Assert(counter0 < 253);  // Start below overflow
solver.Assert(counter3 < counter0);  // Wrapped around

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Overflow path: {model.GetBitVec(counter0)} -> " +
                     $"{model.GetBitVec(counter1)} -> " +
                     $"{model.GetBitVec(counter2)} -> " +
                     $"{model.GetBitVec(counter3)}");
    // Example: 253 -> 254 -> 255 -> 0
}
```

**What Z3 does**: Verifies properties over bounded execution traces.

---

### 9. **Bitwise Tricks Validation**

**Problem**: Verify "x & (x-1) clears lowest set bit"

```csharp
var x = context.BvConst<Size32>("x");
var result = x & (x - 1);

// Property: result has one fewer bit set than x (when x != 0)
var popcount_x = context.BvConst<Size32>("popcount_x");
var popcount_result = context.BvConst<Size32>("popcount_result");

// Constraint: x has at least one bit set
solver.Assert(x != 0);

// Model popcounts (simplified: just check one case)
solver.Assert(x == 12);  // Binary: 1100

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"x = {model.GetBitVec(x):B}");
    Console.WriteLine($"x & (x-1) = {model.GetBitVec(result):B}");
    // Output: 1100 & 1011 = 1000 (cleared lowest set bit)
}
```

**What Z3 does**: Verifies bit-manipulation algorithms formally.

---

### 10. **Generate Test Vectors for Hardware**

**Problem**: Generate diverse 16-bit test patterns

```csharp
for (int i = 0; i < 10; i++)
{
    var testVector = context.BvConst<Size16>($"test{i}");

    // Constraints: cover interesting cases
    solver.Assert(testVector.PopCount() >= 4);  // At least 4 bits set
    solver.Assert(testVector.PopCount() <= 12); // At most 12 bits set

    if (solver.Check() == Z3Status.Satisfiable)
    {
        var model = solver.GetModel();
        var vector = model.GetBitVec(testVector);
        Console.WriteLine($"Test {i}: 0x{vector:X4} ({vector:B16})");

        // Exclude this pattern for diversity
        solver.Assert(testVector != vector);
    }
}
```

**What Z3 does**: Auto-generates corner-case test patterns.

---

## Key Insight

**You don't test bits manually - you write CONSTRAINTS and Z3 finds bit patterns for you!**

It's like:
- ❌ **Manual Testing**: "Does 0x1234 overflow when doubled?" (you choose the value)
- ✅ **SMT Solving**: "Find me ANY value that overflows when doubled!" (Z3 finds it)

## Real-World Applications

1. **Hardware Verification**: CPU, ALU, memory controller correctness
2. **Cryptanalysis**: Find weaknesses in crypto implementations
3. **Compiler Optimization**: Verify bitwise optimizations preserve semantics
4. **Exploit Development**: Find integer overflow vulnerabilities
5. **Reverse Engineering**: Understand bit-level transformations
6. **Protocol Analysis**: Verify bit-field encodings/decodings
7. **FPGA Testing**: Generate comprehensive test vectors

## Comparison to Random Testing

| Random Testing | Z3 Bitvector Solving |
|----------------|----------------------|
| Generate random values, check properties | Generate values that VIOLATE properties |
| May miss corner cases | Systematically finds counterexamples |
| Coverage unknown | Proves properties or finds bugs |
| Fast but incomplete | Slower but exhaustive (within bounds) |

## Bitvector Sizes

Z3Wrap provides compile-time sized types:
- `BvExpr<Size1>` - Single bit (boolean as bitvector)
- `BvExpr<Size8>` - Byte (common for low-level operations)
- `BvExpr<Size16>` - Short (hardware registers, opcodes)
- `BvExpr<Size32>` - Int (most common width)
- `BvExpr<Size64>` - Long (64-bit arithmetic)
- `BvExpr<Size128>` - Cryptographic operations
- Custom sizes available via `ISize` interface

---

**The power**: Z3 reasons about **bit-level behavior symbolically**, finding edge cases in arithmetic, bitwise operations, and hardware designs that traditional testing would miss.
