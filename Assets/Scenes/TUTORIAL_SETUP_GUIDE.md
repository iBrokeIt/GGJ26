# Tutorial Scene Setup Guide

## Step 1: Create the Scene

1. In Unity: **File → New Scene → Basic 2D**
2. Save as `Assets/Scenes/Tutorial.unity`
3. Add it to Build Settings: **File → Build Settings → Add Open Scenes**

## Step 2: Add Singleton Prefabs

Drag these into the scene hierarchy (if they aren't already persistent from another scene):
- **GameManager** prefab
- **DimensionSwitcher** prefab (with camera reference)
- **AudioManager** prefab

> These are DontDestroyOnLoad singletons. If you're testing the Tutorial scene directly (not coming from Menu), you need them in the scene. If coming from Menu, they already exist.

## Step 3: Add the Player

1. Drag the **Player** prefab into the scene
2. Position at `(-20, 2, 0)` — left side of the level
3. Ensure it has the `Player` tag and is on the `Player` layer

## Step 4: Camera Setup

1. Select the **Main Camera**
2. Add **CinemachineCamera** component (if not already present)
3. Set it to follow the Player
4. Set the DimensionSwitcher's `mainCam` reference to this camera
5. Add camera bounds (BoxCollider2D trigger) to constrain the camera to the level area

## Step 5: Create the TutorialManager

1. Create an empty GameObject: `TutorialManager`
2. Add the `TutorialManager` script component

## Step 6: Build the Level Sections

### General Level Structure
- The level runs **left to right**, approximately 100-120 units wide
- All **common floor** tiles should be tagged `Floor` and on the `Default` layer
- Place a **FallDeathCollider** spanning the entire level below (y = -10), with `KillPlayer` script and a BoxCollider2D (isTrigger)

---

### Section 1 — Movement (x: -20 to -5)

1. **Floor**: Create a long platform (sprite/tilemap), tag `Floor`, layer `Default`
   - Position: `(-12, 0, 0)`, Width: ~16 units
2. **Sign — "Use A/D or Arrow Keys to move"**:
   - Create empty GameObject `Sign_Movement` at `(-15, 3, 0)`
   - Add `BoxCollider2D` (isTrigger, size ~6x4)
   - Add `TutorialSign` script
   - Create child TextMeshPro (3D): set text to `"Use A/D or Arrow Keys to move"`, font size ~4, center aligned
   - Assign the TextMeshPro to the `signText` field

---

### Section 2 — Jumping (x: -5 to 10)

1. **Floor before gap**: platform from `(-5, 0)` to `(-1, 0)`, tag `Floor`, layer `Default`
2. **Gap**: 4-unit gap (no floor from x=-1 to x=3)
3. **Floor after gap**: platform from `(3, 0)` to `(10, 0)`, tag `Floor`, layer `Default`
4. **Sign — "Press Space to jump"**:
   - `Sign_Jump` at `(-3, 3, 0)`
   - Same setup as Section 1 sign
   - Text: `"Press Space to jump"`
5. **Checkpoint**: Place at `(5, 1, 0)` with `Checkpoints` script and flame child object

---

### Section 3 — Rere / K key (x: 10 to 30)

1. **Floor**: Common layer floor from `(10, 0)` to `(30, 0)`, tag `Floor`, layer `Default`
2. **Wall (blocking)**:
   - Create a tall sprite/box at `(18, 2, 0)`, size ~1x6
   - **Layer: `World_A`** (Physical World) — visible and solid at start
   - Tag: `Floor` (so it has collision)
   - Add `BoxCollider2D` (NOT trigger — solid wall)
3. **Sign — "Press K to enter the Rere"**:
   - `Sign_Rere` at `(13, 3, 0)`
   - Text: `"Press K to enter the Rere"`
   - Optionally place the `keyboard_key_k.png` sprite next to the text
4. **Checkpoint**: Place at `(25, 1, 0)`

**How it works**: Player walks right, hits the wall. Reads sign, presses K. Camera switches to magenta, World_A layer is hidden → wall disappears. Player walks through.

---

### Section 4 — Appadi / L key (x: 30 to 55)

1. **Floor before gap**: `(30, 0)` to `(38, 0)`, tag `Floor`, layer `Default`
2. **Gap**: 6-unit gap (x=38 to x=44) — no common floor
3. **Bridge platform (Appadi only)**:
   - Create platform at `(41, 0, 0)`, width ~6
   - **Layer: `World_C`** (Appadi) — invisible until player switches to Appadi
   - Tag: `Floor`
4. **Floor after gap**: `(44, 0)` to `(55, 0)`, tag `Floor`, layer `Default`
5. **Sign — "Press L to enter the Appadi"**:
   - `Sign_Appadi` at `(33, 3, 0)`
   - Text: `"Press L to enter the Appadi"`
   - Optionally place `keyboard_key_l.png` sprite nearby
6. **Checkpoint**: Place at `(48, 1, 0)`

**How it works**: Player (now in Rere from Section 3) reaches a gap. Reads sign, presses L. Camera switches to yellow, World_C platform appears. Player jumps across.

---

### Section 5 — Combined Puzzle (x: 55 to 85)

**Part A — Appadi wall + Physical World switch**:
1. **Floor**: `(55, 0)` to `(70, 0)`, tag `Floor`, layer `Default`
2. **Wall (Appadi layer)**:
   - Position: `(62, 2, 0)`, size ~1x6
   - **Layer: `World_C`** — solid because player is in Appadi from Section 4
   - Tag: `Floor`, BoxCollider2D (solid)
3. **Sign — "Press J to return to the Physical World"**:
   - `Sign_Physical` at `(57, 3, 0)`
   - Text: `"Press J to return to the Physical World"`

**Part B — Rere platform**:
4. **Floor before gap**: `(70, 0)` to `(74, 0)`, layer `Default`
5. **Gap**: 6-unit gap (x=74 to x=80)
6. **Bridge platform (Rere only)**:
   - Position: `(77, 0, 0)`, width ~6
   - **Layer: `World_B`** (Rere)
   - Tag: `Floor`
7. **Floor after gap**: `(80, 0)` to `(90, 0)`, layer `Default`
8. **Sign — "Press K to enter the Rere"**:
   - `Sign_Rere2` at `(71, 3, 0)`
   - Text: `"Press K to enter the Rere"`

**Ending**:
9. **Final Sign — "You're ready! Good luck!"**:
   - `Sign_End` at `(84, 3, 0)`
   - Text: `"You're ready! Good luck!"`
10. **End Zone Trigger**:
    - Empty GameObject `TutorialEndZone` at `(88, 2, 0)`
    - Add `BoxCollider2D` (isTrigger, size ~2x6)
    - Add `TutorialEndZone` script
11. **Checkpoint**: Place at `(82, 1, 0)`

---

## Step 7: Menu Integration

In your **Menu scene**, add a "Tutorial" button:
1. Create a UI Button
2. Set OnClick → `GameManager.Instance.StartTutorial()`

## Step 8: Build Settings

Ensure all scenes are in Build Settings in this order:
1. Menu
2. Tutorial
3. Main
4. HotKeys

## Layer Quick Reference

| Layer | Name | Used For |
|-------|------|----------|
| 0 | Default | Common floor (always visible) |
| 6 | World_A | Physical World objects (cyan) |
| 7 | World_B | Rere objects (magenta) |
| 8 | World_C | Appadi objects (yellow) |
| 20 | Player | Player character |

## Tips
- Test each section individually by moving the player spawn point
- Use the Z key (dev mode) to toggle all layers visible for debugging
- Make sure all floor tiles have the `Floor` tag for GroundCheck to work
- Keyboard key sprites are in `Assets/Prefabs/Common/`
- Use the `Arcade Quest SDF` font for TextMeshPro signs to match the game's style
