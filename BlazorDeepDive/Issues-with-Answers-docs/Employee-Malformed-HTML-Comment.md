# Employee Component - Malformed HTML Comment Hides Content

## Issue

Adding an HTML comment above the `<ul>` element causes the entire employee list to disappear. No errors are shown, but the `@foreach` loop output is not rendered.

**Working code:**

```razor
@page "/employee"
@using StaticServerRendering.Models

<h3>Employee</h3>

Employee Details are:

<ul>

@foreach (var emp in EmployeeRepository.GetEmployees())
{
    <li>@emp.Id - @emp.Name - @emp.Position</li>
}
</ul>
```

**Broken code (no data displayed):**

```razor
@page "/employee"
@using StaticServerRendering.Models

<h3>Employee</h3>

Employee Details are:

<!-- Using Control Structures - Looping ->
<ul>

@foreach (var emp in EmployeeRepository.GetEmployees())
{
    <li>@emp.Id - @emp.Name - @emp.Position</li>
}
</ul>
```

## Reason for the Issue

The HTML comment is **not properly closed**. It starts with `<!--` but ends with `->` (single hyphen) instead of `-->` (double hyphen). Since the comment is never closed, the browser treats **everything after it** — the `<ul>`, the `@foreach` loop, and all the `<li>` elements — as part of the comment. This is why no data is displayed.

```
<!-- Using Control Structures - Looping ->   ? single hyphen, comment never closes
                                         ^^
                                         Should be -->
```

## Fix

Close the HTML comment correctly with `-->` (two hyphens followed by `>`):

```razor
<!-- Using Control Structures - Looping -->
```

**Tip:** HTML comments must always follow the syntax `<!-- comment text -->`. A missing `-` in the closing sequence silently swallows all subsequent markup without any compiler or runtime error.
