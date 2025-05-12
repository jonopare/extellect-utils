# Extellect Utilities
A free library of utility types that (at the time of writing) I felt were missing from the major libraries such as Microsoft's .NET Framework, Spring.NET and log4net.
As time went on, the library grew. These types are intended to increase your productivity by letting you reuse something that somebody else has already written and hopefully unit tested.

---

# Utilities vs Core

## ⚠️ Utilities: Legacy

`Extellect.Utilities` targets the now-legacy .NET Framework 4. As Microsoft has shifted its focus to the modern cross-platform .NET Core ecosystem, this project will mainly receive security updates.

## ➡️ Core: Active

Active development has moved to the `Extellect.Core` solution, which is being updated to support modern .NET versions (such as .NET 8+). Users are encouraged to migrate to `Extellect.Core` for future-proofing, performance, and cross-platform compatibility. Where a type exists in `Extellect.Utilities` and its dependencies don't preclude it from being migrated .NET 8, it may be copied into the similarly namespaced class inside `Extellect.Core`.

