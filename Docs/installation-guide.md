# Installation & Project Setup {#usoui-installation}

[TOC]

## Package Requirements
- **Unity Version**: Unity 2021.3 LTS, 2022.3 LTS, 2023.2+, or Unity 6000+ (Unity 6).
- **UI Toolkit**: Built-in with Unity.
- **Assembly Definition**: `GWG.UsoUIElements.asmdef`.

---

## Installation Steps
1. Place the `USOUIElements` package directory inside your project's `Assets` or `Packages` folder (typically `Assets/GarageWareGames/USOUIElements`).
2. If your project uses custom assembly definitions (`.asmdef`), add `GWG.UsoUIElements` to your assembly's reference list:
```json
{
    "references": [
        "GWG.UsoUIElements"
    ]
}
```

---

## Directory Organization
- `Scripts/BaseElementOverrides/`: Turnkey replacements for Unity controls (`UsoButton`, `UsoLabel`, `UsoTextField`, `UsoToggle`, etc.).
- `Scripts/CustomElements/`: Compound and vector controls (`UsoFillBar`, `UsoSlideToggle`, `RadialProgress`, `UsoUiPieChart`).
- `Scripts/Interfaces/`: Core contracts (`IUsoUiElement`, `IUsoForm`).
- `Scripts/Templates/`: Boilerplate generators for custom elements and data binding.

Proceed to [Basic Usage & Element Overrides](@ref usoui-basic-use).
