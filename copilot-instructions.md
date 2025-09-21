# Copilot Instructions for Open Roguelite Arena (Unity)

This repo is a Unity project. Use these notes to quickly align with conventions and be productive when proposing or editing code.

## Big picture
- Project code lives under `Assets/_Project`.
- Architecture layers:
  - Core: lightweight infra (`Code/Core`): `ServiceLocator`, `EventBus`, simple `Pool`.
  - Systems: cross-cutting features (`Code/Systems`): Input, Camera, Combat utilities.
  - Gameplay: domain logic (`Code/Gameplay`), e.g., the player controller and future combat logic.
  - Data: ScriptableObject configs in `Assets/_Project/Data` (e.g., `PlayerConfig`, `AttackConfig`, `CameraConfig`).
- Scenes: main playable scene at `Assets/_Project/Scenes/Arena_MVP.unity`.

## Player movement controller
- `Gameplay/Player/PlayerBrain.cs` owns a `CharacterController`, reads `PlayerInputDriver`, and converts move input into camera-relative strafing (8-way) without FSM layers.
- `PlayerConfig.walkSpeed` defines the baseline; buffs/sprint just scale the target speed before motion.
- `NormalizedSpeed = currentSpeed / baseSpeed` (no clamping) so animation speed can track movement buffs 1:1.
- Gravity is a simple constant push; no jump/dash logic. The character always faces the camera-forward vector for shooter-style movement.
- `PlayerAnimatorDriver` writes `MoveSpeed`, `MoveForward`, `MoveRight` parameters each frame with light damping for blend trees.

## Input system
- Uses Unity Input System (`com.unity.inputsystem`).
- `Systems/Input/PlayerInputDriver.cs` uses SendMessage-style callbacks (`OnMove`, `OnSprint`, `OnAttack`) to populate intents: `MoveIntent`, `AttackIntent`, `CameraIntent`.
- Player brain reads `input.Move.dir` and `input.Move.sprint` to drive the strafing controller directly.
- Camera bridge: `Systems/Camera/OrbitalFollowInputBridge.cs` derives from `Unity.Cinemachine.CinemachineInputAxisController` and gates orbit input with RMB via `InputActionReference`.

## Combat helpers
- `Systems/Combat/HitboxArc.Query(...)` finds `Hurtbox` components in an arc using `Physics.OverlapSphereNonAlloc`, angle check against `Facing`.
- `Hurtbox` tracks last hit ID to prevent double-hits per swing.

## Data/configs
- ScriptableObjects in `Assets/_Project/Data` drive tuning: `PlayerConfig` (walkSpeed, sprintMultiplier, rotationSharpness, gravity values, baseDefense), `AttackConfig` (damage, timings, cone, reach), `CameraConfig` (orbit, zoom, collision params).

## Conventions and patterns
- Namespaces: `Game.<Area>...` reflect folder layout.
- Keep MonoBehaviours thin; lean on plain C# helpers where possible.
- Prefer data-driven tuning via ScriptableObjects; pass config through contexts instead of hard-coding.
- Use `ServiceLocator` and `EventBus` sparingly; they exist for small-scale decoupling.
- Input is polled via a `PlayerInputDriver` component; avoid direct `InputSystem` calls inside gameplay classes.
- When adjusting movement, keep the camera-relative projection and normalized speed contract intact so animation sync remains stable.

## Workflows (Editor-first)
- Open `UnityProject` in the Unity Editor. No custom CLI build scripts are present.
- Play the `Arena_MVP.unity` scene to test. Input requires Input System actions mapped to `OnMove`, `OnSprint`, `OnAttack`.
- Packages used: `com.unity.inputsystem`, `com.unity.cinemachine`, URP, AI Navigation, UGUI, Test Framework.

## When proposing changes
- Reference files by exact path under `Assets/_Project/...` and follow existing namespaces.
- Provide minimal, surgical edits. Don’t reformat unrelated code.
- Include example usage/wiring when adding new gameplay components.
- Prefer extending existing utilities (e.g., add helpers to `HitboxArc`) over creating parallel systems.

## Pointers to explore
- `Assets/_Project/Code/Core/*` for infra utilities.
- `Assets/_Project/Code/Systems/*` for shared systems (Input/Camera/Combat).
- `Assets/_Project/Code/Gameplay/Player/*` for the player controller and animator driver.
- `Assets/_Project/Data/*` for gameplay tuning.
- `Assets/_Project/Scenes/Arena_MVP.unity` for scene wiring.
