# Field Validation Indicators {#usoui-validation}

[TOC]

## Automatic Status Badging
Form fields such as `UsoTextField`, `UsoLabel`, and custom input controls include integrated validation status indicators. This eliminates the need to create extra error icons or custom CSS classes when validating user input.

---

## Configuring Field Status
You can set an element's validation state programmatically:

```csharp
// Set status using the FieldStatus enum
myTextField.SetFieldStatus = FieldStatus.Error;
myTextField.tooltip = "Character name must be between 3 and 16 characters.";

// Check current status
if (myTextField.FieldStatus == FieldStatus.Error)
{
    Debug.LogWarning("Please resolve errors before submitting.");
}
```

### Available Status States:
- `FieldStatus.None`: Clean default state without indicators.
- `FieldStatus.Info`: Blue informational badge.
- `FieldStatus.Warning`: Amber warning badge.
- `FieldStatus.Error`: Red error badge.
- `FieldStatus.Success`: Green check / confirmation badge.

---

## Toggling Indicator Visibility
You can enable or disable indicators globally or per field:
```csharp
myTextField.ShowFieldStatus = true; // or false
```

Proceed to [Custom & Compound Components](@ref usoui-custom-components).
