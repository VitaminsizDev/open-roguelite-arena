# Game Design Document — Core Systems

## Overview
**Title:** Open Roguelite Arena (working title)  
**Genre:** Action Roguelite (top‑down, single‑arena, wave survival)  
**Pillars:** easy onboarding; addictive grind; risk & reward.

## Movement
- WASD (KBM) / Left Stick (pad). Hold to sprint.
- Metin2‑inspired run; no dash/teleport.
- Facing follows movement.

## Combat
- Melee only. Two weapon classes: **1H** (fast) and **2H** (slow, heavy).
- `Space` (KBM) / `X` (pad) to attack. Hold to chain.
- Attack cone: 1H ~100°, 2H ~130°. Animation‑driven hitboxes.
- Passive block chance from Defense. No parry.

## Camera
- 3rd‑person orbit, limited top‑down angle range.
- Orbit via mouse/R‑stick. Zoom in/out. Collision against walls.
- Smooth follow with tunable lag; stored in `CameraConfig`.

## Configs
ScriptableObjects: `PlayerConfig`, `AttackConfig`, `CameraConfig` for all tunables.

## Acceptance
- Input‑to‑pose ≤ 100 ms.
- ≥95% hit accuracy inside cone.
- Camera never occludes player >2 frames.
