# Custom & Compound Components {#usoui-custom-components}

[TOC]

## Overview
Beyond wrapping standard controls, **UsoUIElements** includes high-performance compound controls and vector graphic elements.

---

## 1. Radial Progress & Pie Charts
Rendered using UI Toolkit's vector painter API for crisp, high-DPI scaling with zero pixelation or sprite slicing artifacts:

### RadialProgress
```csharp
var progress = new RadialProgress
{
    progress = 75f, // 0 to 100%
    style = { width = 64, height = 64 }
};
root.Add(progress);
```

### UsoUiPieChart
Renders proportional statistical breakdowns (e.g. damage types, resource allocations, or quest progress).

---

## 2. UsoFillBar
A smooth, clamped progress bar with animated transitions, text overlay, and custom fill colors:
```csharp
var healthBar = new UsoFillBar("HealthBar")
{
    value = 0.85f, // Clamped between 0.0 and 1.0
    text = "850 / 1000 HP"
};
root.Add(healthBar);
```

---

## 3. UsoSlideToggle
A modern mobile/iOS-style animated toggle switch replacing the legacy checkbox look:
```csharp
var toggle = new UsoSlideToggle("AudioToggle", isOn =>
{
    Debug.Log($"Master Audio Enabled: {isOn}");
});
```

---

## 4. Multi-Source DisplaySections (`UsoUiDisplaySection`)
Complex character sheets often draw data from multiple independent objects (e.g. Account Data, Character Vitals, and Guild Info).

`UsoUiDisplaySection` isolates its child bindings to a specific sub-object, enabling multiple disparate data sources within a single compound `UsoForm`.
