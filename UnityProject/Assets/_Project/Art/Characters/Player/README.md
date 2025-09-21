# Player Character Asset Staging

Drop all player-facing meshes, rigs, and animation assets into the sub-folders here.

- `Models/` – FBX/GLB meshes + materials for the player avatar.
- `Animations/` – Imported animation clips (idle, locomotion, combat, etc.).
- `Animators/` – Animator Controllers, override controllers, and blend trees used by `PlayerAnimatorDriver`.

Keep source files in version control; Unity will generate `.meta` files automatically.
