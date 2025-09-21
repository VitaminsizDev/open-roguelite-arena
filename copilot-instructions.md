# Copilot Instructions for Open Roguelite Arena (Unity)

This repo is a Unity project. Use these notes to quickly align with conventions and be productive when proposing or editing code.

## Big picture
- Project code lives under `Assets/_Project`.
- Architecture layers:
  - Core: lightweight infra (`Code/Core`): `ServiceLocator`, `EventBus`, simple `Pool`.
  - Systems: cross-cutting features (`Code/Systems`): Input, Camera, Combat utilities.
  - Gameplay: domain logic (`Code/Gameplay`), e.g., Player finite state machine (FSM).
  - Data: ScriptableObject configs in `Assets/_Project/Data` (e.g., `PlayerConfig`, `AttackConfig`, `CameraConfig`).
- Scenes: main playable scene at `Assets/_Project/Scenes/Arena_MVP.unity`.

## Player movement FSM example
- Interface: `Gameplay/Player/FSM/IState.cs`.
  - Contract: `Priority`, `Enter/Exit`, `Tick(dt)`, `CanExit()`, and `Next(InputSnapshot)`.
- State machine: `Gameplay/Player/FSM/StateMachine.cs`.
  - `Change(next)` respects `Priority` and `CanExit()`.
- Movement states (in `Gameplay/Player/FSM/Movement`):
  - `Idle`(0), `Run`(1), `Sprint`(2). Each extends `MoveBase` and sets speed/accel via `MovementContext`.
  - `MoveBase.ApplyMotion(speed, accel, input, dt)` moves `CharacterController`, updates `Facing` and `Transform.forward`.
- Wiring: `PlayerBrain_MovementOnly` composes states, links transitions (`Idle.Run`, `Idle.Sprint`, etc.), and feeds input.
- Input snapshot: `InputSnapshot` carries `move` Vector2 and `sprint` bool; states compute `Next(...)` accordingly.

## Input system
- Uses Unity Input System (`com.unity.inputsystem`).
- `Systems/Input/PlayerInputDriver.cs` uses SendMessage-style callbacks (`OnMove`, `OnSprint`, `OnAttack`) to populate intents: `MoveIntent`, `AttackIntent`, `CameraIntent`.
- Player brain reads `input.Move.dir` and `input.Move.sprint` to drive FSM.
- Camera bridge: `Systems/Camera/OrbitalFollowInputBridge.cs` derives from `Unity.Cinemachine.CinemachineInputAxisController` and gates orbit input with RMB via `InputActionReference`.

## Combat helpers
- `Systems/Combat/HitboxArc.Query(...)` finds `Hurtbox` components in an arc using `Physics.OverlapSphereNonAlloc`, angle check against `Facing`.
- `Hurtbox` tracks last hit ID to prevent double-hits per swing.

## Data/configs
- ScriptableObjects in `Assets/_Project/Data` drive tuning: `PlayerConfig` (walkSpeed, accel, sprintMultiplier, decel), `AttackConfig` (damage, timings, cone, reach), `CameraConfig` (orbit, zoom, collision params).
- `MovementContext` holds `Transform`, `CharacterController`, `PlayerConfig`, `Velocity`, and `Facing`.

## Conventions and patterns
- Namespaces: `Game.<Area>...` reflect folder layout.
- Keep MonoBehaviours thin; game logic in plain C# classes (FSM states, utilities).
- Prefer data-driven tuning via ScriptableObjects; pass config through contexts instead of hard-coding.
- Use `ServiceLocator` and `EventBus` sparingly; they exist for small-scale decoupling.
- Input is polled via a `PlayerInputDriver` component; avoid direct `InputSystem` calls inside gameplay classes.
- When adding FSM states:
  - Implement `IState`, set a `Priority`, and ensure `Next(...)` returns self when no transition.
  - If a higher-priority state shouldn’t be pre-empted, override `CanExit()` accordingly.
  - Wire references among neighbor states in the composer (e.g., a PlayerBrain MonoBehaviour).

## Workflows (Editor-first)
- Open `UnityProject` in the Unity Editor. No custom CLI build scripts are present.
- Play the `Arena_MVP.unity` scene to test. Input requires Input System actions mapped to `OnMove`, `OnSprint`, `OnAttack`.
- Packages used: `com.unity.inputsystem`, `com.unity.cinemachine`, URP, AI Navigation, UGUI, Test Framework.

## When proposing changes
- Reference files by exact path under `Assets/_Project/...` and follow existing namespaces.
- Provide minimal, surgical edits. Don’t reformat unrelated code.
- Include example usage/wiring when adding new states/components.
- Prefer extending existing utilities (e.g., add helpers to `HitboxArc`) over creating parallel systems.

## Pointers to explore
- `Assets/_Project/Code/Core/*` for infra utilities.
- `Assets/_Project/Code/Systems/*` for shared systems (Input/Camera/Combat).
- `Assets/_Project/Code/Gameplay/Player/*` for player logic and FSM composition.
- `Assets/_Project/Data/*` for gameplay tuning.
- `Assets/_Project/Scenes/Arena_MVP.unity` for scene wiring.

