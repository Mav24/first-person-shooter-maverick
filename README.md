# Rumhold: Caribbean Distillery Defense

A fast-paced first-person shooter where you defend a Caribbean distillery under siege by pirates, rival empires, and mythical sea creatures. Protect barrels of priceless rum, set traps, man cannons, and use improvised weapons fueled by booze and black powder.

## 🎮 Game Overview

The drunker the enemy gets, the more chaotic—and dangerous—the fight becomes. Strategic use of rum-based weapons and traps creates unpredictable, humorous gameplay as enemies stumble, miss shots, and become increasingly erratic.

## ✨ Key Features

### 🛡️ Defense Mechanics
- **Barrel Protection**: Defend valuable rum barrels from destruction or theft
- **Strategic Placement**: Position yourself to protect multiple barrels
- **Dynamic Objectives**: Lose the game if too many barrels are destroyed

### 🔫 Weapons & Combat
- **Improvised Arsenal**: Pistols, muskets, cutlasses, and blunderbusses
- **Booze-Fueled Power**: Rum bottle grenades that explode and intoxicate enemies
- **Black Powder Weapons**: Authentic period firearms with reload mechanics
- **Molotov Cocktails**: Crafted from rum and cloth for fire damage

### 🪤 Trap System
- **Rum Puddles**: Causes enemies to slip and become intoxicated
- **Barrel Bombs**: Explosive traps with area damage
- **Spike Traps**: Hidden floor hazards
- **Net Traps**: Temporarily immobilize enemies
- **Fire Barrels**: Create burning zones for damage over time

### 🎯 Cannon Mechanics
- **Manual Aiming**: Take control of fixed cannon positions
- **Multiple Ammunition**: Chain shot, grape shot, explosive rounds
- **Area Damage**: Devastating against groups of enemies
- **Strategic Positioning**: Defend key chokepoints

### 🏴‍☠️ Enemy Types

#### Pirates
- Aggressive melee and ranged attackers
- Can steal rum barrels if they reach them
- Highly affected by drunkenness

#### Rival Empire Soldiers
- Tactical and organized formations
- Use muskets and coordinated attacks
- Maintain some discipline even when drunk

#### Mythical Sea Creatures
- **Kraken**: Tentacle attacks with area damage
- **Merfolk**: Fast melee attackers with sonic abilities
- **Cursed Sailors**: Undead warriors with special powers

### 🍺 Drunkenness System
The core mechanic that makes Rumhold unique:

- **Sober**: Enemies are fast, accurate, and coordinated
- **Tipsy**: Slightly slower with reduced accuracy
- **Drunk**: Erratic movement, poor aim, more aggressive
- **Wasted**: Stumbling, nearly harmless but causes chaos

Enemies become drunk through:
- Rum puddle traps
- Spilled barrel contents
- Direct hits from rum bottle weapons
- Environmental hazards

### 🌊 Wave System
- **Progressive Difficulty**: Each wave brings more and tougher enemies
- **Multiple Spawn Points**: Enemies attack from land and sea
- **Preparation Time**: Brief periods between waves to set up defenses
- **Boss Waves**: Special challenging waves with unique enemies

## 🎨 Caribbean Setting

Experience an authentic 18th-century Caribbean distillery:
- Tropical island compound with wooden buildings
- Stone walls and defensive positions
- Docks where sea creatures emerge
- Beaches and jungle perimeters
- Interactive rum production facilities

## 🛠️ Technical Stack

- **Engine**: Unity 2021.3+
- **Language**: C#
- **AI**: NavMesh-based pathfinding
- **Physics**: Ragdoll effects and projectile physics
- **Scripting**: Modular component-based architecture

## 📁 Project Structure

```
Assets/
├── Scripts/           # Core game scripts
│   ├── PlayerController.cs
│   ├── EnemyAI.cs
│   ├── DrunkennesSystem.cs
│   ├── WeaponSystem.cs
│   ├── TrapSystem.cs
│   ├── CannonSystem.cs
│   └── ...
├── Prefabs/          # Reusable game objects
├── Scenes/           # Game levels
└── Materials/        # Textures and materials
```

## 🎯 Game Modes

- **Campaign**: Story-driven defense missions
- **Endless Mode**: Survive as many waves as possible
- **Challenge Mode**: Special objectives and constraints
- **Multiplayer Co-op**: Team defense (future feature)

## 🚀 Getting Started

### Prerequisites
- Unity 2021.3 or higher
- Basic understanding of Unity FPS mechanics

### Installation
1. Clone this repository
2. Open the project in Unity
3. Open the main scene in `Assets/Scenes/`
4. Press Play to start

### Controls
- **WASD**: Move
- **Mouse**: Look around
- **Left Click**: Shoot/Fire
- **R**: Reload
- **E**: Interact (use cannons, place traps)
- **Space**: Jump
- **Left Shift**: Sprint
- **1-5**: Switch weapons

## 📖 Documentation

- [GAME_DESIGN.md](GAME_DESIGN.md) - Comprehensive game design document
- Scripts include detailed inline documentation

## 🎮 Development Roadmap

- [x] Core FPS movement and controls
- [x] Basic enemy AI with NavMesh pathfinding
- [x] Drunkenness state system
- [x] Weapon system with multiple types
- [x] Trap placement and mechanics
- [x] Cannon interaction system
- [x] Wave spawning and management
- [x] Barrel objective system
- [x] Specialized enemy types (Pirates, Soldiers, Sea Creatures)
- [ ] Full level design with 3D models
- [ ] Advanced visual effects and particles
- [ ] Audio implementation (music, SFX, voice)
- [ ] UI/HUD system
- [ ] Save/load system
- [ ] Multiplayer support

## 🤝 Contributing

Contributions are welcome! Please feel free to submit pull requests or open issues.

## 📝 License

This project is for educational and portfolio purposes.

## 🎨 Credits

Created as part of the first-person-shooter-maverick project.

---

**Rumhold** - Where rum, gunpowder, and chaos collide! 🏴‍☠️🍺💥
