# Sci-Fi Roguelite Tower Defense

[![Engine](https://img.shields.io/badge/Made%20with-Unity-black?style=flat-square&logo=unity)](https://unity.com/)
[![Focus](https://img.shields.io/badge/Focus-Systems%20Architecture-brightgreen?style=flat-square)](#)
[![Playable Build](https://img.shields.io/badge/Playable_Build-itch.io-FA5C5C?style=flat-square&logo=itchdotio)](https://cryptixthala.itch.io/tower-defense-game)

A system-heavy, 2D strategy prototype focusing on deep modular inheritance, abstract upgrade loops, and event-driven environmental hazards. Designed to showcase scalable OOP principles in game architecture.

## 🧠 Core Engineering Frameworks

### 1. Modular Inheritance Tree (Tower Architecture)
All towers derive from an abstract `BaseTower` class that handles universal logic (target sorting algorithms, cooldown timers, rotation mathematics).
*   **Extensibility:** Creating a new tower requires overriding only the `FireRoutine()` method.
*   **Custom Implementations:** Developed **Sentry**, **Artillery** (AoE calculation), **EMP Pulse** (Status effect application), and a **Stunner** featuring custom chain-lightning logic that utilizes recursive overlap spheres to find the next closest un-stunned target within a bounding radius.

### 2. Event-Driven "Satellite Freeze" Hazard
Instead of tightly coupling environmental hazards to the game manager, I utilized an Observer Pattern.
*   When the "Freeze" event is invoked, all active `BaseTower` instances listening to the event pause their internal firing coroutines and trigger an overload state, forcing the player to prioritize manual grid recovery loops.

### 3. Layer-Stripping Economy System
Enemies do not use standard integer health pools. 
*   **The System:** Enemies possess a Stack of "Armor Layers". Taking damage pops the top layer.
*   **The Economy:** A dynamic delegate observes layer destruction, awarding currency back to the player based on the specific tier of the armor destroyed, creating a highly volatile and rewarding economy loop.

## 📂 Key Scripts to Review
*   `BaseTower.cs` & `ChainLightningStunner.cs` - Inheritance and recursive targeting.
*   `EnemyHealthController.cs` - Layer-stripping stack logic.
*   `GameEventManager.cs` - Observer pattern handling the Satellite Freeze.
