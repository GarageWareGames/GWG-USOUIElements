# Simplified Data Binding {#usoui-data-binding}

[TOC]

## The Problem with Native Data Binding
In standard UI Toolkit, binding a UI element to a data property requires:
1. Instantiating the element.
2. Setting its data source reference (if not inherited from hierarchy).
3. Setting the `bindingPath` string.
4. Setting the `bindingMode` enum.
5. Calling `Bind()` or waiting for the panel binding pass.

Repeating this for dozens of form fields creates massive amounts of repetitive boilerplate.

---

## The UsoUIElements Solution
All `Uso` controls support immediate data binding directly inside their constructors:

```csharp
// Standard constructor signature across all Uso elements:
newElement(string elementName, string bindingPath, BindingMode bindingMode);
```

### Example: Binding a Character Stats Form
```csharp
var healthField = new UsoTextField("HealthField", "currentHealth", BindingMode.TwoWay);
var manaField = new UsoTextField("ManaField", "currentMana", BindingMode.TwoWay);

// Add to container and bind root data source once
var formContainer = new UsoForm("CharacterStatsForm");
formContainer.Add(healthField);
formContainer.Add(manaField);
formContainer.dataSource = myCharacterDataObject;
```

When `dataSource` is assigned, all child fields bind automatically to their respective property paths.

---

## Dynamic Binding After Creation
If an element was constructed without a binding path, you can bind it later using `ApplyBinding`:
```csharp
myTextField.ApplyBinding("playerName", BindingMode.TwoWay);
```

Proceed to [Field Validation Indicators](@ref usoui-validation).
