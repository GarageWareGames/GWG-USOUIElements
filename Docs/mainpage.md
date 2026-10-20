# UsoUIElements: Data-Centric UI Toolkit Controls {#mainpage}

[TOC]

## Overview
**UsoUIElements** is the Garage-Ware Games foundational UI Toolkit expansion package for Unity 2022, 2023, and Unity 6+. It extends and wraps Unity's native `VisualElement` controls to eliminate boilerplate data-binding, automate visual field validation, and provide a standardized constructor pipeline.

Designed as an instant drop-in replacement, you can convert any standard UI Toolkit control by simply prepending the `Uso` prefix:
- `Button` $\rightarrow$ `UsoButton`
- `Label` $\rightarrow$ `UsoLabel`
- `TextField` $\rightarrow$ `UsoTextField`
- `Toggle` $\rightarrow$ `UsoToggle`
- `ListView` $\rightarrow$ `UsoListView`

---

## User Manual Navigation
Explore the technical guides and architectural specifications:

1. [Installation & Project Setup](@ref usoui-installation)
   Adding the package, dependencies, and assembly definition configuration.
2. [Basic Usage & Element Overrides](@ref usoui-basic-use)
   Using base control overrides, standardized constructors, and inline creation workflows.
3. [Simplified Data Binding](@ref usoui-data-binding)
   Binding data sources, binding modes, and zero-boilerplate model-to-view synchronization.
4. [Field Validation Indicators](@ref usoui-validation)
   Configuring visual feedback indicators (`Error`, `Warning`, `Success`, `Info`) on form fields.
5. [Custom & Compound Components](@ref usoui-custom-components)
   Radial progress indicators, fill bars, pie charts, slide toggles, and multi-source display sections.

---

## Key Capabilities
- **Standardized Constructors**: Create and configure elements, assign labels, apply tooltips, wire click callbacks, and configure data bindings all in a single constructor call.
- **Built-In Validation Badging**: Immediate visual status badges without writing custom CSS or overlay elements.
- **Form & DisplaySection Containers**: Group fields by sub-object data models, allowing multiple disparate data sources inside a single compound form.
- **High-Performance Vector Graphics**: Smooth vector-based radial progress and pie charts rendered via UI Toolkit's vector painter API.
- **Seamless WindowFramework Integration**: Fully compatible with `GWG.WindowFramework` for desktop/MMO-style draggable and resizable windows.

---

## Reference Links
- [Garage-Ware Games Webpage](https://www.garage-ware.com)
- [Unity UI Toolkit Documentation](https://docs.unity3d.com/Manual/UIElements.html)
- [Doxygen Manual Specification](https://www.doxygen.nl/manual/markdown.html)
