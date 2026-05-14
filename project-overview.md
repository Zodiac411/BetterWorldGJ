# Astral Defense

## What This Project Is
Astral Defense is a Unity game-jam tower-defense project built for the theme "A Better World." The player builds defenses and generators around a base while enemies attack in waves.

The main scenes are:
- `Assets/Scenes/MainMenu.unity`
- `Assets/Scenes/SampleScene.unity`

### Recent cleanup
`EnemyBehaviour` now splits target selection into smaller helper methods, which makes the AI flow easier to follow while keeping the gameplay logic the same.
Build placement now also uses a small factory layer, and the build flow sits behind a mediator so selection, preview, validation, and placement stay separated.
`BaseScript` now exposes credits as observable state, so the economy is easier to keep in sync with the UI.

## How The Code Works
The codebase revolves around building, economy, combat, and spawning.

### UI and start flow
`UIManager` appears to manage the opening menu and any tutorial or panel navigation. It is the top-level entry point before the game starts.

### Building and economy
The build system is split across several scripts:
- `buildManager` likely handles selection and placement mode
- `TowerLogic` defines tower behavior
- `Hologram` provides placement preview feedback
- `BaseScript` stores base-side state, including credits
- `GeneratorLogic` generates credits and increases the build radius over time

That means the player can grow their economy while adding offensive defenses.

### Combat
`Attacker` looks like an automated defense unit that scans for enemies and fires bullets. `bullet`, `Damagable`, `gun`, and `EnemyHealth` support the damage loop.

### Enemy pressure
`WaveManager` and `EnemySpawning` drive enemy waves. `EnemyBehaviour` likely chooses targets and handles aggression against the player, base, or generators.

### Player control
`PlayerMovement` and `mouseLook` provide a first-person controller layer so the player can move around the base and place defenses.

## Main Design Traits
- The project mixes tower-defense, base-building, and first-person control.
- The code is organized by gameplay role, but several systems appear to overlap in responsibility.
- The economy, build radius, and defense placement loop are central to progression.

## Evidence Used
- `Assets/Scripts/UI/UIManager.cs`
- `Assets/buildManager.cs`
- `Assets/BaseScript.cs`
- `Assets/TowerLogic.cs`
- `Assets/Hologram.cs`
- `Assets/GeneratorLogic.cs`
- `Assets/Attacker.cs`
- `Assets/Scripts/Enemy/WaveManager.cs`
- `Assets/Scripts/Enemy/EnemySpawning.cs`
- `Assets/Scripts/EnemyBehaviour.cs`
