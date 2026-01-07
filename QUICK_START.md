# Quick Start Guide - Creating Your First Playable Scene

This guide will walk you through creating a basic playable scene in Rumhold from scratch.

## Prerequisites
- Unity 2021.3+ installed
- Project opened in Unity
- Basic familiarity with Unity Editor

## Step-by-Step Setup

### 1. Create a New Scene

1. In Unity, go to `File > New Scene`
2. Choose "3D" template
3. Save the scene as "TestLevel" in `Assets/Scenes/`

### 2. Setup the Ground

1. Right-click in Hierarchy
2. Select `3D Object > Plane`
3. Rename to "Ground"
4. In Inspector, set:
   - Position: (0, 0, 0)
   - Scale: (5, 1, 5) - creates a 50x50 unit area

### 3. Setup NavMesh (Required for Enemy AI)

1. Select the Ground object
2. Go to `Window > AI > Navigation`
3. In the Navigation window:
   - Click "Object" tab
   - Check "Navigation Static"
   - Set "Navigation Area" to "Walkable"
4. Click "Bake" tab
5. Click "Bake" button
6. You should see a blue overlay on the ground

### 4. Create the Player

#### A. Create Player GameObject
1. Right-click in Hierarchy
2. Select `Create Empty`
3. Rename to "Player"
4. Set Position: (0, 2, 0)
5. Add Tag "Player":
   - Click "Tag" dropdown in Inspector
   - Select "Add Tag"
   - Create new tag "Player"
   - Go back to Player object and set tag

#### B. Add Player Components
1. Select Player object
2. Click "Add Component"
3. Add these components in order:
   - **Character Controller**
     - Set Height: 2
     - Set Radius: 0.5
   - **Player Controller** (script)
   - **Player Health** (script)

#### C. Add Camera
1. Right-click on Player object
2. Select `Camera`
3. Rename to "PlayerCamera"
4. Set Position: (0, 0.6, 0)
5. In Player Controller script:
   - Drag PlayerCamera into "Player Camera" slot

### 5. Add Light

1. Right-click in Hierarchy
2. Select `Light > Directional Light`
3. Adjust rotation for better lighting (e.g., X: 50, Y: -30)

### 6. Create Game Managers

#### A. GameManager
1. Right-click in Hierarchy > Create Empty
2. Rename to "GameManager"
3. Add Component: **Game Manager** script

#### B. WaveManager
1. Right-click in Hierarchy > Create Empty
2. Rename to "WaveManager"
3. Add Component: **Wave Manager** script

### 7. Add Spawn Points

1. Right-click in Hierarchy > Create Empty
2. Rename to "SpawnPoint1"
3. Set Position: (15, 0, 0)
4. Create tag "SpawnPoint" and assign
5. Duplicate (Ctrl+D) and place at:
   - SpawnPoint2: (-15, 0, 0)
   - SpawnPoint3: (0, 0, 15)
   - SpawnPoint4: (0, 0, -15)
6. In WaveManager:
   - Expand "Spawn Points" array
   - Drag all spawn points into the array

### 8. Create a Simple Enemy Prefab

#### A. Create Enemy GameObject
1. Right-click in Hierarchy > 3D Object > Capsule
2. Rename to "BasicPirate"
3. Set Position: (0, 1, 0)

#### B. Add Enemy Components
1. Select BasicPirate
2. Add Components:
   - **Nav Mesh Agent**
     - Speed: 3.5
     - Angular Speed: 120
     - Acceleration: 8
   - **Rigidbody**
     - Uncheck "Use Gravity"
     - Check "Is Kinematic"
   - **Enemy AI** script
   - **Enemy Health** script
   - **Drunkenness System** script
3. Add Tag "Enemy" or "Pirate"

#### C. Create Prefab
1. Create folder `Assets/Prefabs` if not exists
2. Drag BasicPirate from Hierarchy to Prefabs folder
3. Delete BasicPirate from Hierarchy

### 9. Setup Wave Manager

1. Select WaveManager in Hierarchy
2. In Inspector, expand "Waves"
3. Set Size to 1
4. Expand "Wave 0":
   - Wave Name: "Wave 1"
   - Expand "Enemies" array, set Size to 1
   - Element 0:
     - Drag BasicPirate prefab into "Enemy Prefab"
     - Count: 3
   - Time Between Spawns: 1
   - Preparation Time: 5

### 10. Add Barrels to Defend

#### A. Create Barrel
1. Right-click in Hierarchy > 3D Object > Cylinder
2. Rename to "RumBarrel"
3. Scale: (0.5, 0.7, 0.5)
4. Position: (0, 0.7, 5)

#### B. Add Barrel Components
1. Select RumBarrel
2. Ensure it has a Collider
3. Add Component: **Barrel Health** script
4. Add Tag "Barrel"

#### C. Duplicate Barrels
1. Duplicate RumBarrel (Ctrl+D)
2. Place at strategic locations:
   - Barrel2: (5, 0.7, 0)
   - Barrel3: (-5, 0.7, 0)

### 11. Add a Weapon to Player (Optional)

1. Select Player
2. Add Component: **Weapon System** script
3. In Inspector, set:
   - Weapon Name: "Pistol"
   - Damage: 25
   - Range: 100
   - Fire Rate: 0.5
   - Max Ammo: 12

### 12. Test the Scene

1. Click Play button
2. You should be able to:
   - Move with WASD
   - Look around with mouse
   - See enemies spawn after 5 seconds
   - Shoot with left mouse button (if weapon added)
   - Watch enemies navigate toward barrels

### 13. Common Issues

**Player Falls Through Ground:**
- Ensure Player has Character Controller
- Check Ground has a collider

**Enemies Don't Move:**
- Make sure NavMesh is baked (blue overlay visible)
- Check Nav Mesh Agent is on enemy
- Verify spawn points exist

**Can't Look Around:**
- Ensure cursor is locked (should happen automatically)
- Check Player Controller script is enabled
- Verify PlayerCamera is assigned

**Enemies Don't Spawn:**
- Check WaveManager has spawn points assigned
- Verify enemy prefab is assigned in wave
- Check spawn point positions are on NavMesh

### 14. Next Steps

Now that you have a basic working scene:
- Add more enemies to waves
- Place traps with **Trap Placer** script
- Add cannons with **Cannon System**
- Create different enemy types (PirateEnemy, EmpireSoldier, SeaCreature)
- Add UI with **Game HUD** script
- Experiment with drunkenness effects

### 15. Adding UI (Optional)

1. Right-click in Hierarchy > UI > Canvas
2. Right-click on Canvas > UI > Text
3. Rename to "HealthText"
4. Position in top-left corner
5. Create empty GameObject "HUD"
6. Add **Game HUD** script
7. Assign references in Inspector

### Tips
- Save frequently (Ctrl+S)
- Test often by clicking Play
- Use Console window to see debug messages
- Check Gizmos are enabled in Scene view to see ranges

## Summary
You now have:
- ✅ Playable FPS character
- ✅ Working enemy AI that spawns in waves
- ✅ Barrels to defend
- ✅ Basic combat mechanics
- ✅ NavMesh pathfinding

Build on this foundation by adding the unique Rumhold features like traps, cannons, and the drunkenness system!
