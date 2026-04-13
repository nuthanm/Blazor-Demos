# Implicit vs Explicit Razor Expressions in Blazor

## Overview

Razor syntax uses the `@` symbol to transition from HTML into C#. There are two ways to embed C# values inside your markup: **implicit expressions** and **explicit expressions**. Understanding both is essential for writing clean, error-free Blazor components.

---

## 1. Implicit Razor Expressions

An **implicit expression** starts with `@` followed directly by a C# identifier. The Razor engine automatically detects where the C# expression ends and HTML begins.

### Syntax

```razor
@<variable-or-property>
```

### Example (from `Employee.razor`)

```razor
@employee.Id - @employee.Name - @employee.Position
```

### Rules

| Rule | Detail |
|------|--------|
| **No spaces** | The expression ends at the first space — `@employee.Name is great` renders the value of `employee.Name` followed by literal text ` is great` |
| **Dot access is allowed** | Property chains like `@employee.Name` work out of the box |
| **No C# keywords** | You cannot use `await`, ternary operators, generics (`<T>`), or casts directly |
| **No parentheses or operators** | Arithmetic (`@a + b`), ternary (`@a ? b : c`), and string concatenation do not work as expected |

### When to Use

- Rendering a simple variable: `@message`
- Accessing a property: `@employee.Name`
- Accessing a nested property: `@employee.Address.City`

---

## 2. Explicit Razor Expressions

An **explicit expression** wraps the C# code inside `@(...)`. This tells the Razor engine exactly where the expression starts and ends, allowing complex C# to be evaluated inline.

### Syntax

```razor
@(<any-valid-csharp-expression>)
```

### Example (from `Employee.razor`)

```razor
Employee Full Name : @(employee.Name == "John Doe" ? "Johnathan Doe" : employee.Name)
```

### Rules

| Rule | Detail |
|------|--------|
| **Supports any C# expression** | Ternary operators, method calls, arithmetic, string interpolation, casts, etc. |
| **Parentheses define the boundary** | Everything between `@(` and `)` is evaluated as C# |
| **Whitespace is safe** | Spaces inside the parentheses do not break the expression |

### When to Use

- Ternary / conditional: `@(isAdmin ? "Admin" : "User")`
- Arithmetic: `@(price * quantity)`
- Method calls with arguments: `@(string.Format("{0:C}", price))`
- String concatenation: `@(firstName + " " + lastName)`
- Casting: `@((int)rating)`
- Generic method calls: `@(List<string>.Empty)` _(angle brackets would confuse implicit expressions)_
- Disambiguation from surrounding HTML: `@(employee.Name)@company.com` renders the variable followed by literal `@company.com`

---

## Side-by-Side Comparison

| Aspect | Implicit (`@`) | Explicit (`@()`) |
|--------|----------------|-------------------|
| **Syntax** | `@expression` | `@(expression)` |
| **Complexity** | Simple identifiers & property access only | Any valid C# expression |
| **Ternary operator** | ? Not supported | ? `@(a ? b : c)` |
| **Arithmetic** | ? `@a + b` treats `+ b` as HTML | ? `@(a + b)` |
| **Method calls** | ? `@DateTime.Now` (parameterless) | ? `@(myMethod(arg1, arg2))` |
| **String concatenation** | ? | ? `@(first + " " + last)` |
| **Generics** | ? `<T>` breaks HTML parsing | ? `@(MyMethod<string>())` |
| **Readability** | Cleaner for simple values | More verbose but unambiguous |

---

## 3. Other Places You Write C# in Razor

Beyond inline expressions, Razor provides several blocks and directives where C# code lives.

### `@code { }` Block

The primary location for component state, fields, properties, methods, and lifecycle overrides.

```razor
@code {
    private string greeting = "Hello, Blazor!";
}
```

### `@{ }` Statement Block

Executes C# statements (loops, conditionals, variable declarations) inline within the markup. Does **not** render output by itself — use `@variable` or `@()` inside to emit values.

```razor
@{
    var fullName = employee.Name + " (ID: " + employee.Id + ")";
}
<p>@fullName</p>
```

### Control-Flow Directives

| Directive | Example |
|-----------|---------|
| `@if / @else` | `@if (employee.IsActive) { <span>Active</span> }` |
| `@foreach` | `@foreach (var emp in employees) { <li>@emp.Name</li> }` |
| `@for` | `@for (int i = 0; i < 5; i++) { <p>@i</p> }` |
| `@switch` | `@switch (status) { case "Active": <span>?</span> break; }` |
| `@while` | `@while (condition) { ... }` |

### Attribute Expressions

You can bind C# values to HTML attributes using both implicit and explicit expressions.

```razor
<!-- Implicit -->
<input value="@employee.Name" />

<!-- Explicit (useful when concatenating) -->
<img src="@(baseUrl)/images/@(employee.Id).png" />
```

### Directive Attributes (Blazor-Specific)

| Attribute | Purpose | Example |
|-----------|---------|---------|
| `@bind` | Two-way data binding | `<input @bind="name" />` |
| `@onclick` | Event handling | `<button @onclick="HandleClick">Click</button>` |
| `@ref` | Element/component reference | `<input @ref="inputRef" />` |
| `@key` | Diffing hint for loops | `<li @key="emp.Id">@emp.Name</li>` |
| `@typeparam` | Generic component parameter | `@typeparam TItem` |
| `@attribute` | Arbitrary HTML attribute | `@attribute [Authorize]` |

---

## 4. Quick Reference — `Employee.razor` Annotated

```razor
@page "/employee"                    @* ? Razor directive (route) *@
<h3>Employee</h3>

Employee Details are:
@employee.Id - @employee.Name - @employee.Position          @* ? Implicit expressions *@
<br />
Employee Full Name : @(employee.Name == "John Doe"          @* ? Explicit expression *@
    ? "Johnathan Doe"
    : employee.Name)

@code {                                                      @* ? Code block *@
    private StaticServerRendering.Models.Employee employee = new StaticServerRendering.Models.Employee
    {
        Id = 1,
        Name = "John Doe",
        Position = "Software Engineer"
    };
}
```

---

## Summary

| Expression Type | Syntax | Best For |
|-----------------|--------|----------|
| **Implicit** | `@variable` | Simple property access and identifiers |
| **Explicit** | `@(expression)` | Ternary operators, arithmetic, method calls, generics, disambiguation |
| **Statement block** | `@{ ... }` | Multi-line C# logic that doesn't directly render output |
| **Code block** | `@code { ... }` | Component fields, properties, methods, and lifecycle hooks |
| **Control flow** | `@if`, `@foreach`, etc. | Conditional and iterative rendering |
| **Directive attributes** | `@bind`, `@onclick`, etc. | Blazor interactivity and data binding |
