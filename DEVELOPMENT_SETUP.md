# Development Setup Guide

## Overview
This guide will help you set up the Rumhold development environment and understand the codebase structure.

## Requirements

### Software
- **Unity 2021.3.0f1 or higher** (LTS version recommended)
- **Visual Studio 2019/2022** or **Visual Studio Code** with C# extension
- **Git** for version control

### Hardware Recommendations
- **OS**: Windows 10/11, macOS 10.15+, or Linux
- **RAM**: 8GB minimum, 16GB recommended
- **GPU**: DirectX 11 compatible graphics card
- **Storage**: 5GB free space

## Initial Setup

### 1. Clone the Repository
```bash
git clone https://github.com/Mav24/first-person-shooter-maverick.git
cd first-person-shooter-maverick
```

### 2. Open in Unity
1. Launch Unity Hub
2. Click "Open" or "Add"
3. Navigate to the cloned repository folder
4. Select the folder and open

Unity will automatically import all assets and compile scripts.

### 3. First Run
1. Open the main scene (if available) in `Assets/Scenes/`
2. Press the Play button to test the game
3. Verify that basic controls work (WASD, mouse look)

## Project Structure

```
first-person-shooter-maverick/
├── Assets/
│   ├── Scripts/              # All C# game scripts
│   │   ├── PlayerController.cs       # Player movement and camera
│   │   ├── PlayerHealth.cs           # Player health management
│   │   ├── EnemyAI.cs               # Base enemy AI
│   │   ├── PirateEnemy.cs           # Pirate-specific behavior
│   │   ├── EmpireSoldier.cs         # Empire soldier behavior
│   │   ├── SeaCreature.cs           # Sea creature behavior
│   │   ├── EnemyHealth.cs           # Enemy health management
│   │   ├── DrunkennesSystem.cs      # Drunkenness mechanics
│   │   ├── WeaponSystem.cs          # Weapon firing and ammo
│   │   ├── TrapSystem.cs            # Trap placement and effects
│   │   ├── CannonSystem.cs          # Cannon interaction
│   │   ├── Cannonball.cs            # Cannonball projectile
│   │   ├── BarrelHealth.cs          # Barrel objectives
│   │   ├── WaveManager.cs           # Wave spawning system
│   │   └── GameManager.cs           # Central game state
│   ├── Prefabs/              # Reusable game objects
│   ├── Scenes/               # Game scenes/levels
│   └── Materials/            # Visual materials
├── ProjectSettings/          # Unity project configuration
├── Packages/                 # Unity package dependencies
├── .gitignore               # Git ignore rules
├── README.md                # Main project documentation
├── GAME_DESIGN.md           # Detailed game design
└── DEVELOPMENT_SETUP.md     # This file
```

## Key Components

### Player System
- **PlayerController**: Handles WASD movement, mouse look, jumping, sprinting
- **PlayerHealth**: Manages health, damage, death, and respawn

### Enemy System
- **EnemyAI**: Base class for all enemies with NavMesh pathfinding
- **EnemyHealth**: Handles enemy damage and death
- **DrunkennesSystem**: Controls enemy drunkenness states and effects
- **Enemy Types**: Specialized classes for Pirates, Empire Soldiers, and Sea Creatures

### Combat System
- **WeaponSystem**: Shooting, reloading, ammo management
- **TrapSystem**: Trap placement and triggering effects
- **CannonSystem**: Player-controlled cannons
- **Cannonball**: Cannonball projectile with explosion physics

### Game Management
- **GameManager**: Tracks score, barrels, enemies, game state
- **WaveManager**: Spawns enemy waves with progressive difficulty
- **BarrelHealth**: Manages defensive objectives

## Setting Up a Scene

### 1. Create a New Scene
1. Right-click in `Assets/Scenes/`
2. Create > Scene
3. Name it appropriately (e.g., "MainLevel")

### 2. Add Required GameObjects

#### Player Setup
1. Create Empty GameObject named "Player"
2. Add Components:
   - Character Controller
   - PlayerController script
   - PlayerHealth script
3. Add child object "Camera" with Camera component
4. Tag as "Player"

#### Ground/Environment
1. Create 3D Object > Plane for ground
2. Scale appropriately (10x scale for large areas)
3. Add collider if not present

#### NavMesh Setup (for Enemy AI)
1. Select all walkable surfaces
2. Window > AI > Navigation
3. Mark as "Walkable"
4. Click "Bake" to generate NavMesh

#### Spawn Points
1. Create Empty GameObjects for enemy spawns
2. Position around the map perimeter
3. Tag as "SpawnPoint"

#### Barrel Objectives
1. Create Cylinder or import barrel model
2. Add Collider component
3. Add BarrelHealth script
4. Tag as "Barrel"
5. Duplicate and place around map

#### Game Managers
1. Create Empty GameObject "GameManager"
2. Add GameManager script
3. Create Empty GameObject "WaveManager"
4. Add WaveManager script

### 3. Configure Enemy Prefabs
1. Create Empty GameObject for enemy
2. Add:
   - Capsule Collider
   - NavMesh Agent
   - Rigidbody
   - EnemyAI or specific enemy script (PirateEnemy, etc.)
   - EnemyHealth
   - DrunkennesSystem
3. Tag appropriately ("Enemy", "Pirate", etc.)
4. Save as Prefab in Assets/Prefabs/

### 4. Assign References
- In WaveManager, assign spawn points and enemy prefabs
- In EnemyAI, assign player and barrel targets (or leave auto-detect)
- In GameManager, verify it can find all barrels

## Testing

### Manual Testing
1. **Player Movement**: Test WASD, mouse look, jump, sprint
2. **Combat**: Test weapon firing, reloading, hit detection
3. **Enemy AI**: Verify enemies navigate, attack, respond to drunkenness
4. **Traps**: Place traps and verify they trigger correctly
5. **Cannons**: Interact with cannons, aim, and fire
6. **Barrels**: Damage barrels and verify game over condition
7. **Waves**: Test wave progression and enemy spawning

### Debug Tools
- Enable Gizmos in Scene view to see ranges and debug visualizations
- Use Debug.Log statements (already in code) to track behavior
- Unity Profiler for performance testing

## Common Issues

### Enemy AI Not Moving
- Ensure NavMesh is baked
- Check NavMeshAgent is enabled
- Verify walkable surfaces are marked correctly

### Player Can't Move
- Ensure CharacterController is attached
- Check PlayerController script is enabled
- Verify input settings in Project Settings > Input Manager

### Scripts Not Compiling
- Check for missing using statements
- Ensure all script files are in Assets/Scripts/
- Check Console for specific errors

### References Not Found
- Some scripts auto-detect references (Player, Barrels)
- Check tags are correctly assigned
- Verify scene has required GameObjects

## Building the Game

### Build Settings
1. File > Build Settings
2. Add scenes to build list
3. Select target platform
4. Configure player settings
5. Click Build

### Optimization Tips
- Use object pooling for projectiles
- Limit active enemies
- Use LOD for distant objects
- Optimize particle effects

## Coding Standards

### Naming Conventions
- Classes: PascalCase (e.g., `PlayerController`)
- Methods: PascalCase (e.g., `TakeDamage()`)
- Variables: camelCase (e.g., `currentHealth`)
- Constants: UPPER_CASE (e.g., `MAX_AMMO`)

### Documentation
- Add XML comments to public methods
- Include header comments on each script
- Document complex logic inline

### Best Practices
- Keep scripts focused and single-purpose
- Use [SerializeField] for private variables in Inspector
- Cache component references in Start/Awake
- Use tags and layers efficiently

## Version Control

### Git Workflow
```bash
# Create feature branch
git checkout -b feature/new-enemy-type

# Make changes and commit
git add .
git commit -m "Add new enemy type"

# Push to remote
git push origin feature/new-enemy-type

# Create pull request
```

### Commit Messages
- Use descriptive messages
- Reference issues when applicable
- Keep commits focused

## Resources

### Unity Documentation
- [Unity Manual](https://docs.unity3d.com/Manual/index.html)
- [Scripting Reference](https://docs.unity3d.com/ScriptReference/)
- [FPS Microgame Tutorial](https://learn.unity.com/project/fps-template)

### External Resources
- Unity Asset Store for models and effects
- Mixamo for character animations
- Freesound for audio effects

## Getting Help

### Debugging
1. Check Unity Console for errors
2. Use Debug.Log() to trace execution
3. Set breakpoints in Visual Studio
4. Review script documentation

### Community
- Unity Forums
- Unity Discord
- Stack Overflow
- Game Dev subreddit

## Next Steps

1. Review [GAME_DESIGN.md](GAME_DESIGN.md) for feature details
2. Explore existing scripts in Assets/Scripts/
3. Test each system individually
4. Create your first custom level
5. Experiment with new enemy types or weapons

Happy developing! 🎮
