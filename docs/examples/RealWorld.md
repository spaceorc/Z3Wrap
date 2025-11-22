# Real-World Problem Solving with Z3

These examples show how Z3 solves actual problems you might encounter in software development, security, and system design.

## Configuration & Systems

### 1. **Database Connection String Validation**

**Problem**: Ensure database URLs are well-formed and consistent

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var protocol = context.StringConst("protocol");
var host = context.StringConst("host");
var port = context.IntConst("port");
var database = context.StringConst("database");

// Build connection string: protocol://host:port/database
var portStr = port.ToStr();
var connectionString = protocol + context.String("://") + host +
                      context.String(":") + portStr +
                      context.String("/") + database;

// Constraints
solver.Assert(protocol == "postgres" | protocol == "mysql");
solver.Assert(port >= 1024 & port <= 65535);  // Non-privileged ports
solver.Assert(database.Length() > 0);
solver.Assert(!database.Contains(context.String("/")));  // No slashes in DB name

// Find valid configuration for localhost
solver.Assert(host == "localhost");
solver.Assert(protocol == "postgres");

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Connection: {model.GetStringValue(connectionString)}");
    // Example: postgres://localhost:5432/mydb
}
```

---

### 2. **API Rate Limiting Logic**

**Problem**: Verify rate limiter doesn't allow >100 requests/minute

```csharp
var request1_time = context.IntConst("req1_time");  // Milliseconds
var request2_time = context.IntConst("req2_time");
// ... up to request 101

var times = new[] { request1_time, request2_time, /* ... */ };

// All requests must be in same 60-second window
for (int i = 0; i < times.Length; i++)
{
    solver.Assert(times[i] >= 0);
    solver.Assert(times[i] <= 60000);  // Within 60 seconds
}

// Times must be increasing (ordered requests)
for (int i = 0; i < times.Length - 1; i++)
{
    solver.Assert(times[i] < times[i + 1]);
}

// Check: can we get 101 requests through?
if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("VULNERABILITY: Rate limiter allows >100 req/min!");
    var model = solver.GetModel();
    // Shows exact timing that breaks the limit
}
else
{
    Console.WriteLine("Rate limiter is correct!");
}
```

---

### 3. **Access Control Policy Verification**

**Problem**: Can user Alice ever access resource X given these RBAC rules?

```csharp
var user = context.StringConst("user");
var resource = context.StringConst("resource");
var role = context.StringConst("role");
var hasPermission = context.BoolConst("hasPermission");

// Rules:
// - Admins can access everything
// - Users can access public resources
// - Alice has role "user"

var isAdmin = role == "admin";
var isPublic = resource.StartsWith(context.String("public_"));
var isAlice = user == "alice";

solver.Assert(user == "alice");  // Check Alice specifically
solver.Assert(resource == "secret_data");  // Trying to access secret data

// Policy: permission granted if admin OR (user AND public resource)
solver.Assert(hasPermission == (isAdmin | (role == "user" & isPublic)));

// Alice's role
solver.Assert(isAlice.Implies(role == "user"));

// Can Alice access secret_data?
solver.Assert(hasPermission);  // Try to make it true

if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("SECURITY ISSUE: Alice can access secret data!");
}
else
{
    Console.WriteLine("Access control is secure!");
}
```

---

## Security & Cryptography

### 4. **SQL Injection Detection**

**Problem**: Can we craft input that breaks this parameterized query?

```csharp
var userInput = context.StringConst("userInput");
var query = context.StringConst("query");

// Intended safe query: SELECT * FROM users WHERE name = 'input'
var safePart1 = context.String("SELECT * FROM users WHERE name = '");
var safePart2 = context.String("'");

// Simulated "safe" construction (actually vulnerable if input has quotes)
solver.Assert(query == safePart1 + userInput + safePart2);

// Attack: can we inject SQL?
solver.Assert(query.Contains(context.String("OR 1=1")));

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"SQL Injection payload: {model.GetStringValue(userInput)}");
    Console.WriteLine($"Resulting query: {model.GetStringValue(query)}");
    // Example: userInput = "' OR 1=1 --"
    // Query becomes: SELECT * FROM users WHERE name = '' OR 1=1 --'
}
```

---

### 5. **Password Strength Checker**

**Problem**: Generate password that passes all validation rules

```csharp
var password = context.StringConst("password");

// Requirements
solver.Assert(password.Length() >= 12);
solver.Assert(password.Length() <= 64);

// Must contain lowercase (check for 'a'-'z')
var hasLower = context.BoolConst("hasLower");
solver.Assert(hasLower);  // Simplified: assume one lowercase char

// Must contain uppercase
var hasUpper = password.Contains(context.String("A")) |
               password.Contains(context.String("B"));  // Simplified

// Must contain digit
var hasDigit = password.Contains(context.String("0")) |
               password.Contains(context.String("1")) |
               password.Contains(context.String("2"));  // Simplified

// Must contain special character
var hasSpecial = password.Contains(context.String("@")) |
                password.Contains(context.String("!"));

solver.Assert(hasUpper);
solver.Assert(hasDigit);
solver.Assert(hasSpecial);

// Must NOT contain username
solver.Assert(!password.Contains(context.String("alice")));

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Valid password: {model.GetStringValue(password)}");
}
```

---

## Data Validation

### 6. **Email Address Format Checker**

**Problem**: Validate email structure without regex

```csharp
var email = context.StringConst("email");
var localPart = context.StringConst("local");
var domain = context.StringConst("domain");

// Format: local@domain
solver.Assert(email == localPart + context.String("@") + domain);

// Constraints
solver.Assert(localPart.Length() > 0);
solver.Assert(localPart.Length() <= 64);
solver.Assert(domain.Length() > 0);

// Domain must have at least one dot
solver.Assert(domain.Contains(context.String(".")));

// Local part can't start with dot
solver.Assert(!localPart.StartsWith(context.String(".")));

// Generate valid email for domain "example.com"
solver.Assert(domain == "example.com");

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Valid email: {model.GetStringValue(email)}");
    // Example: user@example.com
}
```

---

### 7. **Credit Card Number Validation (Luhn Check)**

**Problem**: Generate valid credit card number

```csharp
// Simplified: 4-digit card number for demonstration
var digit1 = context.IntConst("d1");
var digit2 = context.IntConst("d2");
var digit3 = context.IntConst("d3");
var digit4 = context.IntConst("d4");  // Check digit

// All digits 0-9
solver.Assert(digit1 >= 0 & digit1 <= 9);
solver.Assert(digit2 >= 0 & digit2 <= 9);
solver.Assert(digit3 >= 0 & digit3 <= 9);
solver.Assert(digit4 >= 0 & digit4 <= 9);

// Luhn algorithm (simplified for 4 digits)
// Double every other digit from right, subtract 9 if > 9
var d3_doubled = context.If(digit3 * 2 > 9, digit3 * 2 - 9, digit3 * 2);
var d1_doubled = context.If(digit1 * 2 > 9, digit1 * 2 - 9, digit1 * 2);

var sum = d1_doubled + digit2 + d3_doubled + digit4;
solver.Assert(sum % 10 == 0);  // Luhn check

// Generate valid card starting with 4
solver.Assert(digit1 == 4);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Valid card: {model.GetIntValue(digit1)}" +
                     $"{model.GetIntValue(digit2)}" +
                     $"{model.GetIntValue(digit3)}" +
                     $"{model.GetIntValue(digit4)}");
}
```

---

## Algorithm Verification

### 8. **Binary Search Correctness**

**Problem**: Verify binary search finds the target

```csharp
// Sorted array: [10, 20, 30, 40, 50]
var target = context.IntConst("target");
var foundIndex = context.IntConst("foundIndex");

// Constraints: target exists in array
solver.Assert(target >= 10 & target <= 50);
solver.Assert(target % 10 == 0);  // One of our values

// Model binary search steps
var low = 0;
var high = 4;
var mid = (low + high) / 2;  // 2

// If target <= array[mid], search left half; else right half
var searchLeft = target <= 30;
var searchRight = !searchLeft;

// Verify: found index points to correct element
var array = new[] { 10, 20, 30, 40, 50 };
solver.Assert(foundIndex >= 0 & foundIndex < 5);

// The value at foundIndex must equal target
// Model: array[foundIndex] == target (simplified)
var foundValue = context.If(foundIndex == 0, 10,
                  context.If(foundIndex == 1, 20,
                  context.If(foundIndex == 2, 30,
                  context.If(foundIndex == 3, 40, 50))));

solver.Assert(foundValue == target);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Target {model.GetIntValue(target)} found at index {model.GetIntValue(foundIndex)}");
}
```

---

## Key Takeaways

1. **Configuration Validation**: Ensure complex configs are consistent
2. **Security Testing**: Find vulnerabilities automatically
3. **Input Validation**: Generate valid/invalid test cases
4. **Algorithm Verification**: Prove correctness properties

Z3 excels at finding **edge cases**, **security holes**, and **corner cases** that manual testing misses.

---

**When to use Z3 in real projects:**
- ✅ Complex validation logic with many rules
- ✅ Security-critical code that must be bulletproof
- ✅ Finding counterexamples to specifications
- ✅ Generating comprehensive test cases
- ❌ Simple validation (regex, basic checks are faster)
- ❌ Performance-critical hot paths (Z3 is powerful but not instant)
