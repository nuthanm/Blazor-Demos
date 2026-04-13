# Employee Component Naming Conflict

## Issue

`CS0117: 'Employee' does not contain a definition for 'Id'`

When using the `Employee` model class inside `Employee.razor`, the compiler throws an error saying `Employee` does not contain a definition for `Id`, even though the `Employee` model class in `StaticServerRendering.Models` clearly has an `Id` property.

```razor
@code {
    private Employee employee = new Employee
    {
        Id = 1,
        Name = "John Doe",
        Position = "Software Engineer"
    };
}
```

## Reason for the Issue

In Blazor, each `.razor` file generates a C# class with the same name as the file. So `Employee.razor` generates a class also named `Employee`. This **shadows** the `StaticServerRendering.Models.Employee` model class. When `Employee` is referenced inside the `@code` block, the compiler resolves it to the **component class itself** (which has no `Id` property), not the model class.

## Fix

Use the **fully qualified name** for the model class to disambiguate:

```razor
@code {
    private StaticServerRendering.Models.Employee employee = new StaticServerRendering.Models.Employee
    {
        Id = 1,
        Name = "John Doe",
        Position = "Software Engineer"
    };
}
```

**Alternative:** Rename the Razor component (e.g., `EmployeePage.razor`) to avoid the naming conflict entirely, which would allow using `Employee` directly without qualification.
