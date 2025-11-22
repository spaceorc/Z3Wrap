# What Can You Do With String Constraints?

String theory in Z3 lets you **solve puzzles and verify properties about strings** without knowing the strings in advance. Think of it as "algebra for text".

## Use Cases

### 1. **Input Validation - Find Valid Inputs**

**Problem**: What password satisfies our complex rules?

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var password = context.StringConst("password");

// Business rules
solver.Assert(password.Length() >= 8);
solver.Assert(password.Length() <= 20);
solver.Assert(password.Contains(context.String("@")));  // Must have special char
solver.Assert(!password.Contains(context.String(" "))); // No spaces

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine(model.GetStringValue(password));  // e.g., "abcdefgh@"
}
```

**What Z3 does**: Finds ANY string that satisfies all constraints simultaneously.

---

### 2. **String Puzzles - Find Unknown Values**

**Problem**: Find strings where `x + y = "hello" + x`

```csharp
var x = context.StringConst("x");
var y = context.StringConst("y");

solver.Assert(x + y == context.String("hello") + x);
solver.Assert(x.Length() <= 10);
solver.Assert(y.Length() > 0);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"x = {model.GetStringValue(x)}");  // x = "" (empty)
    Console.WriteLine($"y = {model.GetStringValue(y)}");  // y = "hello"
}
```

**What Z3 does**: Solves string equations like algebra.

---

### 3. **Security - Find Exploit Strings**

**Problem**: Can we find an input that bypasses validation?

```csharp
var userInput = context.StringConst("input");
var sanitized = context.StringConst("sanitized");

// Supposed sanitization: replace "<script>" with ""
solver.Assert(sanitized == userInput.Replace(context.String("<script>"), context.String("")));

// But check: can we still get <script> in output?
solver.Assert(sanitized.Contains(context.String("<script>")));

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Attack: {model.GetStringValue(userInput)}");
    // Might find: "<<script>script>" which becomes "<script>" after replacing!
}
else
{
    Console.WriteLine("Sanitization is safe!");
}
```

**What Z3 does**: Automatically finds edge cases and vulnerabilities.

---

### 4. **Data Format Validation**

**Problem**: Does this string match format "XXX-YYY" where X, Y are digits?

```csharp
var phoneCode = context.StringConst("code");

// Format: XXX-YYY (3 digits, dash, 3 digits)
var part1 = context.StringConst("part1");
var part2 = context.StringConst("part2");

solver.Assert(phoneCode == part1 + context.String("-") + part2);
solver.Assert(part1.Length() == 3);
solver.Assert(part2.Length() == 3);

// Both parts must be valid numbers
var num1 = part1.ToInt();
var num2 = part2.ToInt();
solver.Assert(num1 >= 0);
solver.Assert(num1 <= 999);
solver.Assert(num2 >= 0);
solver.Assert(num2 <= 999);

// Additional constraint: area code must be >= 200
solver.Assert(num1 >= 200);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine(model.GetStringValue(phoneCode));  // e.g., "200-000"
}
```

**What Z3 does**: Generates test cases that match complex formats.

---

### 5. **Reverse Engineering - What Input Produces This Output?**

**Problem**: What input to a function produces "HELLO"?

```csharp
var input = context.StringConst("input");
var output = context.StringConst("output");

// Simulated function: uppercase and add exclamation
// (In reality, you'd model the actual transformation)
solver.Assert(output == input + context.String("!"));
solver.Assert(output.Length() == 6);
solver.Assert(output == context.String("HELLO!"));

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Input was: {model.GetStringValue(input)}");  // "HELLO"
}
```

**What Z3 does**: Works backwards from desired output to required input.

---

### 6. **Configuration Validation**

**Problem**: Are these configuration strings consistent?

```csharp
var dbUrl = context.StringConst("db_url");
var dbHost = context.StringConst("db_host");
var dbPort = context.StringConst("db_port");

// URL format: "protocol://host:port"
solver.Assert(dbUrl == context.String("postgres://") + dbHost + context.String(":") + dbPort);

// Constraints
solver.Assert(dbHost.Length() > 0);
solver.Assert(dbPort.Length() > 0);
solver.Assert(dbPort.ToInt() >= 1024);  // Non-privileged ports
solver.Assert(dbPort.ToInt() <= 65535);

// Check: can we have a valid URL with host "localhost"?
solver.Assert(dbHost == context.String("localhost"));

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine(model.GetStringValue(dbUrl));  // e.g., "postgres://localhost:5432"
}
```

**What Z3 does**: Verifies configuration consistency and generates valid configs.

---

### 7. **Palindrome Checker (Symbolic)**

**Problem**: Find a 5-letter palindrome that contains "a"

```csharp
var word = context.StringConst("word");

solver.Assert(word.Length() == 5);
solver.Assert(word.Contains(context.String("a")));

// Palindrome constraint: word[i] == word[4-i] for all i
// (Requires character indexing)
var c0 = word.CharAt(0);
var c1 = word.CharAt(1);
var c2 = word.CharAt(2);
var c3 = word.CharAt(3);
var c4 = word.CharAt(4);

solver.Assert(c0 == c4);
solver.Assert(c1 == c3);
// c2 is center, no constraint

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine(model.GetStringValue(word));  // e.g., "aabaa" or "racecar" truncated
}
```

**What Z3 does**: Finds examples satisfying complex string properties.

---

### 8. **Test Case Generation**

**Problem**: Generate diverse test inputs for a parser

```csharp
// Generate 10 different valid JSON-like strings
for (int i = 0; i < 10; i++)
{
    var json = context.StringConst($"json{i}");

    // JSON object format: {"key":"value"}
    solver.Assert(json.StartsWith(context.String("{")));
    solver.Assert(json.EndsWith(context.String("}")));
    solver.Assert(json.Contains(context.String(":")));
    solver.Assert(json.Length() >= 10);
    solver.Assert(json.Length() <= 50);

    if (solver.Check() == Z3Status.Satisfiable)
    {
        var model = solver.GetModel();
        var testCase = model.GetStringValue(json);
        Console.WriteLine($"Test {i}: {testCase}");

        // Exclude this solution for next iteration
        solver.Assert(json != context.String(testCase));
    }
}
```

**What Z3 does**: Automatically generates diverse test cases.

---

## Key Insight

**You don't write strings and check them - you write CONSTRAINTS and Z3 finds strings for you!**

It's like:
- ❌ **Testing**: "Does password 'abc123' satisfy rules?" (you know the string)
- ✅ **Solving**: "Find me ANY password that satisfies these 10 rules!" (Z3 finds it)

## Real-World Applications

1. **Security Auditing**: Find inputs that bypass sanitization
2. **Test Generation**: Auto-generate valid/invalid test cases
3. **Configuration Validation**: Ensure config files are consistent
4. **Protocol Verification**: Verify message formats
5. **Compiler Testing**: Generate edge-case programs
6. **Smart Contract Analysis**: Find inputs that cause issues
7. **API Fuzzing**: Generate API requests that satisfy complex rules

## Comparison to Regex

| Regex | Z3 String Theory |
|-------|-----------------|
| "Does this string match pattern?" | "Find me a string matching ALL these constraints!" |
| Pattern matching | Constraint solving |
| Forward checking | Bidirectional reasoning |
| One pattern at a time | Many constraints simultaneously |

---

**The power**: Z3 reasons about **unknown strings symbolically**, finding concrete values that satisfy complex combinations of constraints you couldn't easily enumerate by hand.
