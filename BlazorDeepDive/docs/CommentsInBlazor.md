# Comments in Blazor

## Overview

Blazor components (`.razor` files) support multiple comment styles because they mix HTML markup and C# code. Choosing the correct syntax — and closing it properly — is essential to avoid silently hiding rendered content.

---

## 1. HTML Comments (`<!-- -->`)

Standard HTML comments can be placed anywhere in the **markup** section of a `.razor` file. They are sent to the browser but are **not visible** on the page (they appear only in the page source / DevTools).

### Syntax

```html
<!-- This is an HTML comment -->
```

### Example

```razor
<!-- Employee list rendered using a foreach loop -->
<ul>
    @foreach (var emp in employees)
    {
        <li>@emp.Name</li>
    }
</ul>
```

### Rules

| Rule | Detail |
|------|--------|
| **Must open with `<!--`** | Exactly `<`, `!`, `-`, `-` |
| **Must close with `-->`** | Exactly `-`, `-`, `>` — a single hyphen (`->`) does **not** close the comment |
| **Visible in page source** | The comment text is sent to the client; do not put secrets or sensitive data here |
| **Cannot be nested** | `<!-- outer <!-- inner --> -->` will break |

### Common Pitfall — Missing Hyphen

```razor
<!-- This comment is never closed ->
<ul>
    @foreach (var emp in employees)
    {
        <li>@emp.Name</li>   @* ? This entire block is swallowed by the unclosed comment *@
    }
</ul>
```

Because `->` is **not** a valid comment terminator, the browser treats everything after `<!--` as comment content. The `<ul>` and `@foreach` output are completely hidden — with **no compiler or runtime error**.

**Fix:** Always close with `-->` (two hyphens):

```razor
<!-- This comment is properly closed -->
```

---

## 2. Razor Comments (`@* *@`)

Razor comments are **server-side only**. They are stripped during compilation and are **never sent to the browser**. Use them for developer notes that should not appear in the page source.

### Syntax

```razor
@* This is a Razor comment *@
```

### Example

```razor
@* Loop through all employees and render each as a list item *@
<ul>
    @foreach (var emp in employees)
    {
        <li>@emp.Name</li>
    }
</ul>
```

### Multi-line Razor Comments

```razor
@*
    This is a multi-line Razor comment.
    It can span as many lines as needed.
    None of this text reaches the browser.
*@
```

### Rules

| Rule | Detail |
|------|--------|
| **Must open with `@*`** | At-sign followed by asterisk |
| **Must close with `*@`** | Asterisk followed by at-sign |
| **Not sent to browser** | Safe for internal notes, TODOs, or temporarily disabling markup |
| **Can wrap HTML and C#** | Anything between `@*` and `*@` is ignored by the compiler |

---

## 3. C# Comments (inside `@code` and `@{ }` blocks)

Inside C# blocks, standard C# comment syntax applies.

### Single-line

```razor
@code {
    // This is a single-line C# comment
    private string name = "Blazor";
}
```

### Multi-line

```razor
@code {
    /*
        This is a multi-line C# comment.
        It works just like in any .cs file.
    */
    private int count = 0;
}
```

---

## Side-by-Side Comparison

| Aspect | HTML (`<!-- -->`) | Razor (`@* *@`) | C# (`//`, `/* */`) |
|--------|-------------------|------------------|---------------------|
| **Where to use** | Markup sections | Anywhere in `.razor` file | Inside `@code { }` or `@{ }` blocks |
| **Sent to browser** | ? Yes (visible in page source) | ? No | ? No |
| **Can wrap HTML** | ? Yes | ? Yes | ? No |
| **Can wrap C#** | ? No (markup only) | ? Yes | ? Yes (within code blocks) |
| **Nesting** | ? Cannot nest | ? Cannot nest | ? Single-line inside multi-line |
| **Risk if malformed** | Silently hides all subsequent content | Compiler error | Compiler error |

---

## Best Practices

1. **Prefer Razor comments (`@* *@`)** for developer notes — they are never sent to the client, reducing page size and avoiding information leaks.
2. **Use HTML comments (`<!-- -->`)** only when you intentionally want the comment to appear in the browser's page source (e.g., for debugging or SEO hints).
3. **Always double-check the closing sequence** of HTML comments — `-->` not `->`. A missing hyphen silently swallows all markup that follows.
4. **Use C# comments (`//`, `/* */`)** for logic documentation inside `@code` and `@{ }` blocks, following standard C# conventions.
