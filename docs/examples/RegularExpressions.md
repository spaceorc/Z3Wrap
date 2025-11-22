# What Can You Do With Regular Expression Constraints?

Regular expressions in Z3 let you **solve puzzles and verify properties about strings that match patterns** without knowing the strings in advance. Think of it as "constraint solving with pattern matching".

## Use Cases

### 1. **Input Validation - Generate Valid Inputs**

**Problem**: What string satisfies this email-like pattern?

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var email = context.StringConst("email");

// Pattern: lowercase letters+ @ lowercase letters+ . 2-3 lowercase letters
var letter = context.RegexRange('a', 'z');
var localPart = letter.Plus();
var at = context.Regex("@");
var domain = letter.Plus();
var dot = context.Regex(".");
var tld = letter.Loop(2, 3);

var emailPattern = localPart + at + domain + dot + tld;

solver.Assert(email.Matches(emailPattern));
solver.Assert(email.Length() <= 20);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine(model.GetStringValue(email));  // e.g., "abc@def.gh"
}
```

**What Z3 does**: Generates ANY string that matches the regex pattern and length constraint.

---

### 2. **Pattern Intersection - Find Strings Matching Multiple Patterns**

**Problem**: Find a string that's both a valid identifier AND contains a digit.

```csharp
var identifier = context.StringConst("id");

// Pattern 1: Valid identifier (letter followed by letters/digits)
var letter = context.RegexRange('a', 'z') | context.RegexRange('A', 'Z');
var digit = context.RegexRange('0', '9');
var alphaNum = letter | digit;
var identifierPattern = letter + alphaNum.Star();

// Pattern 2: Must contain at least one digit
var anyChar = context.RegexAllChar();
var hasDigit = anyChar.Star() + digit + anyChar.Star();

// Intersection: Both patterns must match
var combinedPattern = identifierPattern & hasDigit;

solver.Assert(identifier.Matches(combinedPattern));
solver.Assert(identifier.Length() <= 8);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Valid ID: {model.GetStringValue(identifier)}");
    // e.g., "abc123", "x1y2z3"
}
```

**What Z3 does**: Finds strings satisfying the intersection of multiple regex patterns.

---

### 3. **Security - Bypass Detection**

**Problem**: Can we find input that matches a "safe" pattern but still contains dangerous content?

```csharp
var userInput = context.StringConst("input");

// "Safe" pattern: alphanumeric only
var alphaNum = context.RegexRange('a', 'z')
             | context.RegexRange('A', 'Z')
             | context.RegexRange('0', '9');
var safePattern = alphaNum.Plus();

// But we want to check: can input match safe pattern AND contain SQL keywords?
var containsSelect = userInput.Contains(context.String("SELECT"));

solver.Assert(userInput.Matches(safePattern));
solver.Assert(containsSelect);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Found bypass: {model.GetStringValue(userInput)}");
    // Will find "SELECT" matches both patterns!
}
else
{
    Console.WriteLine("Pattern correctly blocks SQL injection!");
}
```

**What Z3 does**: Finds vulnerabilities where patterns might overlap unexpectedly.

---

### 4. **Format Verification - Phone Numbers**

**Problem**: Generate valid phone numbers in different formats.

```csharp
var phone = context.StringConst("phone");

// Pattern: 3 digits, dash, 3 digits, dash, 4 digits
var digit = context.RegexRange('0', '9');
var dash = context.Regex("-");
var phonePattern = digit.Power(3) + dash + digit.Power(3) + dash + digit.Power(4);

solver.Assert(phone.Matches(phonePattern));

// Additional constraint: area code must start with 5 or higher
var areaCode = context.StringConst("area");
solver.Assert(phone == areaCode + context.String("-") + context.StringConst("rest"));
solver.Assert(areaCode.Length() == 3);
solver.Assert(areaCode[0].ToInt() >= 5);  // First digit >= 5

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Phone: {model.GetStringValue(phone)}");
    // e.g., "555-123-4567", "678-901-2345"
}
```

**What Z3 does**: Combines regex patterns with additional string constraints.

---

### 5. **Pattern Complement - Find Non-Matching Strings**

**Problem**: What strings do NOT match this pattern?

```csharp
var text = context.StringConst("text");

// Pattern: anything containing "admin"
var anyChar = context.RegexAllChar();
var containsAdmin = anyChar.Star() + context.Regex("admin") + anyChar.Star();

// Complement: everything EXCEPT strings with "admin"
var noAdmin = ~containsAdmin;

solver.Assert(text.Matches(noAdmin));
solver.Assert(text.Length() == 5);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Safe string: {model.GetStringValue(text)}");
    // e.g., "hello", "world", "12345" - anything without "admin"
}
```

**What Z3 does**: Uses complement to find strings that explicitly DON'T match a pattern.

---

### 6. **Password Strength - Complex Requirements**

**Problem**: Generate passwords meeting complex regex-based rules.

```csharp
var password = context.StringConst("pwd");

var anyChar = context.RegexAllChar();
var digit = context.RegexRange('0', '9');
var lower = context.RegexRange('a', 'z');
var upper = context.RegexRange('A', 'Z');
var special = context.Regex("!") | context.Regex("@") | context.Regex("#");

// Must have at least one of each
var hasDigit = anyChar.Star() + digit + anyChar.Star();
var hasLower = anyChar.Star() + lower + anyChar.Star();
var hasUpper = anyChar.Star() + upper + anyChar.Star();
var hasSpecial = anyChar.Star() + special + anyChar.Star();

// Length constraint: exactly 8-12 characters
var validChars = digit | lower | upper | special;
var lengthPattern = validChars.Loop(8, 12);

// All requirements combined
var strongPassword = lengthPattern & hasDigit & hasLower & hasUpper & hasSpecial;

solver.Assert(password.Matches(strongPassword));

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Strong password: {model.GetStringValue(password)}");
    // e.g., "aB3!xYz9"
}
```

**What Z3 does**: Combines multiple regex constraints to enforce complex validation rules.

---

### 7. **URL Validation - Find Valid URLs**

**Problem**: Generate URLs matching a specific format.

```csharp
var url = context.StringConst("url");

var letter = context.RegexRange('a', 'z');
var digit = context.RegexRange('0', '9');

// Simplified URL: http:// + domain + .com
var protocol = context.Regex("http://");
var domain = letter.Plus();
var tld = context.Regex(".com");

var urlPattern = protocol + domain + tld;

solver.Assert(url.Matches(urlPattern));
solver.Assert(url.Length() <= 25);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"URL: {model.GetStringValue(url)}");
    // e.g., "http://example.com"
}
```

**What Z3 does**: Validates and generates strings matching URL-like patterns.

---

### 8. **Pattern Difference - Exclude Specific Cases**

**Problem**: Find strings that match one pattern but NOT another.

```csharp
var filename = context.StringConst("file");

var letter = context.RegexRange('a', 'z');
var digit = context.RegexRange('0', '9');
var alphaNum = letter | digit;

// Pattern: any alphanumeric filename
var anyFilename = alphaNum.Plus();

// But NOT ones starting with "test"
var startsWithTest = context.Regex("test") + alphaNum.Star();

// Difference: match anyFilename but exclude startsWithTest
var goodFilename = anyFilename - startsWithTest;

solver.Assert(filename.Matches(goodFilename));
solver.Assert(filename.Length() == 5);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Filename: {model.GetStringValue(filename)}");
    // e.g., "file1", "data2", "hello" - anything except "test*"
}
```

**What Z3 does**: Uses pattern difference to exclude specific subcases.

---

### 9. **Variable-Length Patterns - License Plates**

**Problem**: Generate license plates with flexible format.

```csharp
var plate = context.StringConst("plate");

var letter = context.RegexRange('A', 'Z');
var digit = context.RegexRange('0', '9');

// Format: 2-3 letters followed by 3-4 digits
var letterPart = letter.Loop(2, 3);
var digitPart = digit.Loop(3, 4);
var platePattern = letterPart + digitPart;

solver.Assert(plate.Matches(platePattern));

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"License: {model.GetStringValue(plate)}");
    // e.g., "AB123", "XYZ9876"
}
```

**What Z3 does**: Handles variable-length patterns with loop constraints.

---

### 10. **Equivalence Checking - Are Two Patterns the Same?**

**Problem**: Do these two patterns accept the same strings?

```csharp
var testString = context.StringConst("s");

// Pattern 1: a* (zero or more 'a's)
var pattern1 = context.Regex("a").Star();

// Pattern 2: (a+)? (optional one or more 'a's)
var pattern2 = context.Regex("a").Plus().Option();

// Are they different? Find a string that matches one but not the other
solver.Assert(testString.Matches(pattern1));
solver.Assert(!testString.Matches(pattern2));

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Difference: '{model.GetStringValue(testString)}'");
    // Will find: "" (empty string matches a* but not (a+)?)
}
else
{
    Console.WriteLine("Patterns are equivalent!");
}
```

**What Z3 does**: Proves or disproves regex equivalence by finding counterexamples.

---

## Key Takeaways

**What makes regex constraints powerful:**

1. **Pattern Matching**: Express complex format requirements concisely
2. **Set Operations**: Combine patterns with union (`|`), intersection (`&`), complement (`~`), difference (`-`)
3. **Quantifiers**: Precise repetition control with `Star()`, `Plus()`, `Loop()`, `Power()`
4. **Composability**: Build complex patterns from simple building blocks
5. **Symbolic Solving**: Find strings satisfying multiple constraints simultaneously

**Common patterns:**
- **Validation**: Generate test inputs matching format requirements
- **Security**: Find inputs that bypass or exploit validation
- **Equivalence**: Prove two patterns accept the same language
- **Intersection**: Find strings matching multiple patterns at once
- **Complement**: Find strings that explicitly avoid a pattern

**When to use regex constraints:**
- Input format validation (emails, phones, URLs)
- Password strength requirements
- Data format specifications
- Security vulnerability testing
- Pattern equivalence checking
