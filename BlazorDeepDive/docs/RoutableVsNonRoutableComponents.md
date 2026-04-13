# Routable vs Non-Routable Components in Blazor

## Overview

Blazor components fall into two categories based on whether they can be navigated to directly via a URL.

## Routable Components

A **routable component** has a `@page` directive that assigns it a route. Users can navigate to it directly through the browser address bar or via links.

```razor
@page "/routable-component"

<h3>RoutableComponent</h3>
```

### Key Characteristics

| Aspect | Detail |
|--------|--------|
| **Directive** | Requires `@page "/route-path"` |
| **Location** | Typically placed under `Components/Pages` |
| **Navigation** | Accessible directly via URL (e.g., `/routable-component`) |
| **Usage** | Acts as a full page rendered by the router |

### Example — `RoutableComponent.razor`

```razor
@page "/routable-component"
@using StaticServerRendering.Components.SubComponents;

<h3>RoutableComponent</h3>

<Non_RoutableComponents></Non_RoutableComponents>
```

---

## Non-Routable Components

A **non-routable component** has **no** `@page` directive. It cannot be reached through a URL and is instead embedded inside other components using its tag name.

```razor
<h3>Non Routable Components</h3>

@code {
}
```

### Key Characteristics

| Aspect | Detail |
|--------|--------|
| **Directive** | No `@page` directive |
| **Location** | Typically placed under a subfolder such as `Components/SubComponents` or `Components/Shared` |
| **Navigation** | Not accessible via URL |
| **Usage** | Embedded inside routable (or other non-routable) components as a child tag |

---

## Using a Non-Routable Component Inside a Routable Component

There are **three ways** to reference a non-routable component:

### 1. Fully Qualified Namespace

Use the complete namespace as the tag name. No `@using` directive is needed.

```razor
<StaticServerRendering.Components.SubComponents.Non_RoutableComponents />
```

### 2. `@using` Directive at File Level

Add a `@using` directive at the top of the consuming component and then use the short tag name.

```razor
@using StaticServerRendering.Components.SubComponents;

<Non_RoutableComponents />
```

### 3. Global `@using` in `_Imports.razor`

Add the namespace in the `_Imports.razor` file so every component in the project can use the short tag name without a per-file `@using`.

```razor
@* In _Imports.razor *@
@using StaticServerRendering.Components.SubComponents
```

Then directly use:

```razor
<Non_RoutableComponents />
```

---

## Quick Comparison

| Feature | Routable Component | Non-Routable Component |
|---|---|---|
| `@page` directive | ? Yes | ? No |
| Directly accessible via URL | ? Yes | ? No |
| Can contain child components | ? Yes | ? Yes |
| Can be embedded in other components | ? Yes (though uncommon) | ? Yes (primary use case) |
| Typical folder | `Components/Pages` | `Components/SubComponents` or `Components/Shared` |

---

## References

- [Blazor Routing Documentation](https://learn.microsoft.com/aspnet/core/blazor/fundamentals/routing)
- [Blazor Components Overview](https://learn.microsoft.com/aspnet/core/blazor/components/)
