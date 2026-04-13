# Blazor Project Types — Comparison & Guide

## Overview

Blazor in .NET 10 supports multiple **render modes** that determine where and how your component code executes. This solution contains four project types, each demonstrating a different render mode. All projects use the `Microsoft.NET.Sdk.Web` SDK (server host) and, where needed, a companion `Microsoft.NET.Sdk.BlazorWebAssembly` client project.

---

## 1. Static Server Rendering (SSR)

**Project:** `StaticServerRendering`

### What Is It?

Components are rendered entirely on the server into static HTML and sent to the browser. There is **no** persistent SignalR connection and **no** WebAssembly runtime. Every user interaction that changes state requires a full or enhanced page navigation.

### Program.cs Setup

```csharp
builder.Services.AddRazorComponents();          // No interactive mode registered

app.MapRazorComponents<App>();                   // No interactive render mode mapped
```

### When to Use

| ? Good Fit | ? Avoid When |
|---|---|
| Content-heavy / informational pages | You need real-time UI updates (e.g., live dashboards) |
| SEO is a top priority | Frequent user interactions are expected (button clicks, forms with instant feedback) |
| You want the smallest client payload (no JS interop runtime) | You need client-side state that survives between navigations |
| Simple read-only pages with minimal interactivity | |

---

## 2. Interactive Server (Blazor Server)

**Project:** `ServerInteractivity`

### What Is It?

Components render on the server and a **SignalR (WebSocket) connection** is maintained between the browser and the server. UI events travel to the server, the component re-renders, and the DOM diff is pushed back to the client in real time.

### Program.cs Setup

```csharp
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();            // Register server interactivity services

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();            // Enable interactive server render mode
```

### Key Characteristics

- A `<ReconnectModal />` component is included in `App.razor` to handle connection drops gracefully.
- All component code stays on the server — no .NET assemblies are downloaded to the browser.
- Requires a stable network connection; latency affects responsiveness.

### When to Use

| ? Good Fit | ? Avoid When |
|---|---|
| Rich, interactive UI that needs access to server resources (databases, file system) | Users are on high-latency or unreliable networks |
| You want fast initial load — no large download | You need offline support |
| Keeping application logic on the server is a security requirement | Scaling to a very large number of concurrent users (each holds a SignalR connection) |
| Internal / enterprise apps on a reliable intranet | |

---

## 3. Interactive WebAssembly (Blazor WASM)

**Projects:** `WebAssemblyInteractivity` (server host) + `WebAssemblyInteractivity.Client` (client)

### What Is It?

The .NET runtime and your application assemblies are downloaded to the browser and executed client-side via **WebAssembly**. After the initial download, the app runs entirely in the browser with no ongoing server connection for UI rendering.

### Program.cs Setup — Server Host

```csharp
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();       // Register WASM interactivity services

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()        // Enable WASM render mode
    .AddAdditionalAssemblies(typeof(WebAssemblyInteractivity.Client._Imports).Assembly);
```

### Program.cs Setup — Client

```csharp
var builder = WebAssemblyHostBuilder.CreateDefault(args);
await builder.Build().RunAsync();
```

### Project Structure

| Project | SDK | Role |
|---|---|---|
| `WebAssemblyInteractivity` | `Microsoft.NET.Sdk.Web` | Server host — serves the app, handles pre-rendering |
| `WebAssemblyInteractivity.Client` | `Microsoft.NET.Sdk.BlazorWebAssembly` | Client — contains interactive components that run in the browser |

### When to Use

| ? Good Fit | ? Avoid When |
|---|---|
| Offline or near-offline scenarios (PWA) | You need access to server-side resources directly from components |
| You want to reduce server load — UI runs in the browser | First-load time is critical (WASM runtime + assemblies must be downloaded) |
| Client-heavy apps such as editors, drawing tools, or games | Targeting very old browsers that lack WebAssembly support |
| Public-facing apps where scaling SignalR connections is a concern | Application logic must remain on the server for security |

---

## 4. Auto (Interactive Auto)

**Projects:** `AutoInteractivity` (server host) + `AutoInteractivity.Client` (client)

### What Is It?

Auto mode combines **Interactive Server** and **Interactive WebAssembly**. On the first visit, the component renders interactively on the server via SignalR (fast startup). In the background, the WebAssembly runtime and assemblies are downloaded. On subsequent visits, the component switches to running entirely in the browser via WebAssembly.

### Program.cs Setup — Server Host

```csharp
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()             // Register server interactivity
    .AddInteractiveWebAssemblyComponents();       // Register WASM interactivity

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()             // Enable server render mode
    .AddInteractiveWebAssemblyRenderMode()        // Enable WASM render mode
    .AddAdditionalAssemblies(typeof(AutoInteractivity.Client._Imports).Assembly);
```

### Program.cs Setup — Client

```csharp
var builder = WebAssemblyHostBuilder.CreateDefault(args);
await builder.Build().RunAsync();
```

### Project Structure

| Project | SDK | Role |
|---|---|---|
| `AutoInteractivity` | `Microsoft.NET.Sdk.Web` | Server host — serves the app, handles server-side rendering & SignalR |
| `AutoInteractivity.Client` | `Microsoft.NET.Sdk.BlazorWebAssembly` | Client — contains interactive components that eventually run in the browser |

### Key Characteristics

- A `<ReconnectModal />` is included in `App.razor` (needed for the server-rendered phase).
- First visit = server-rendered interactivity (fast). Subsequent visits = WebAssembly (no server dependency for UI).
- Components placed in the `.Client` project must be compatible with **both** server and WebAssembly execution.

### When to Use

| ? Good Fit | ? Avoid When |
|---|---|
| You want the best of both worlds — fast first load *and* client-side execution | Simplicity is a priority (two-project setup adds complexity) |
| Public-facing apps that need a snappy first impression and offline potential | Components need server-only APIs that cannot be abstracted behind services |
| Progressive enhancement scenarios | The extra build & deployment complexity is not justified |

---

## Quick Comparison

| Feature | Static Server Rendering | Interactive Server | Interactive WebAssembly | Auto |
|---|---|---|---|---|
| **Render location** | Server (static HTML) | Server (SignalR) | Browser (WebAssembly) | Server first ? WebAssembly later |
| **Interactivity** | ? None (page navigations only) | ? Full | ? Full | ? Full |
| **SignalR connection** | ? No | ? Yes (persistent) | ? No | ? Initially, then drops |
| **.NET in browser** | ? No | ? No | ? Yes | ? After first visit |
| **Initial load speed** | ? Fastest | ? Fast | ?? Slower (download required) | ? Fast (server first) |
| **Offline support** | ? No | ? No | ? Possible (PWA) | ? After WASM cached |
| **Server resources access** | ? Direct | ? Direct | ? Via HTTP API only | ? Direct on server / API on WASM |
| **Scalability** | ? Excellent | ?? Limited by connections | ? Excellent | ? Good (connections are temporary) |
| **Project count** | 1 | 1 | 2 (host + client) | 2 (host + client) |
| **Best for** | Static / content sites | Internal / enterprise apps | Client-heavy / offline apps | Public apps needing fast start + rich UX |

---

## Solution Structure at a Glance

```
Blazor-Demos/
??? StaticServerRendering/              ? Static SSR (single project)
?   ??? Components/
?   ??? Program.cs
?
??? ServerInteractivity/                ? Interactive Server (single project)
?   ??? Components/
?   ??? Program.cs
?
??? WebAssemblyInteractivity/           ? Interactive WebAssembly (two projects)
?   ??? WebAssemblyInteractivity/       ? Server host
?   ?   ??? Components/
?   ?   ??? Program.cs
?   ??? WebAssemblyInteractivity.Client/ ? Client (runs in browser)
?       ??? Program.cs
?
??? AutoInteractivity/                  ? Auto mode (two projects)
?   ??? AutoInteractivity/              ? Server host
?   ?   ??? Components/
?   ?   ??? Program.cs
?   ??? AutoInteractivity.Client/       ? Client (runs in browser)
?       ??? Program.cs
?
??? docs/                               ? Documentation
```

---

## How to Choose — Decision Flowchart

1. **Do you need interactivity?**
   - **No** ? Use **Static Server Rendering**
   - **Yes** ? Continue ?

2. **Must the app work offline or reduce server load?**
   - **Yes** ? Continue ?
   - **No** ? Use **Interactive Server**

3. **Is fast first-load important?**
   - **Yes** ? Use **Auto**
   - **No** ? Use **Interactive WebAssembly**

---

## References

- [Blazor Render Modes](https://learn.microsoft.com/aspnet/core/blazor/components/render-modes)
- [Blazor Server vs WebAssembly](https://learn.microsoft.com/aspnet/core/blazor/hosting-models)
- [Blazor Project Structure](https://learn.microsoft.com/aspnet/core/blazor/project-structure)
