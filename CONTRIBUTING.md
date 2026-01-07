# Contributing to Rumhold

Thank you for your interest in contributing to Rumhold! This document provides guidelines and instructions for contributing to the project.

## Code of Conduct

- Be respectful and inclusive
- Focus on constructive feedback
- Help create a welcoming environment for all contributors

## How to Contribute

### Reporting Bugs

1. Check existing issues to avoid duplicates
2. Create a new issue with:
   - Clear title
   - Detailed description
   - Steps to reproduce
   - Expected vs actual behavior
   - Unity version and platform
   - Screenshots or videos if applicable

### Suggesting Features

1. Check if the feature has been suggested before
2. Create a new issue with:
   - Clear description of the feature
   - Use cases and benefits
   - Possible implementation approach

### Pull Requests

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Make your changes following our coding standards
4. Test your changes thoroughly
5. Commit with clear messages
6. Push to your fork
7. Create a pull request

## Development Setup

See [DEVELOPMENT_SETUP.md](DEVELOPMENT_SETUP.md) for detailed setup instructions.

## Coding Standards

### C# Style Guide

#### Naming Conventions
```csharp
// Classes: PascalCase
public class PlayerController { }

// Methods: PascalCase
public void TakeDamage() { }

// Public fields: camelCase with [Header] and [SerializeField]
[Header("Combat Settings")]
public float attackDamage = 10f;

// Private fields: camelCase
private float currentHealth;

// Constants: UPPER_CASE
private const int MAX_ENEMIES = 50;

// Properties: PascalCase
public float CurrentHealth { get; private set; }
```

#### File Organization
```csharp
// 1. Using statements
using UnityEngine;
using System.Collections;

// 2. Class summary comment
/// <summary>
/// Brief description of the class
/// </summary>

// 3. Class declaration
public class ClassName : MonoBehaviour
{
    // 4. Public serialized fields (with headers)
    [Header("Settings")]
    public float speed = 5f;
    
    // 5. Private fields
    private Rigidbody rb;
    
    // 6. Unity lifecycle methods
    private void Awake() { }
    private void Start() { }
    private void Update() { }
    
    // 7. Public methods
    public void PublicMethod() { }
    
    // 8. Private methods
    private void PrivateMethod() { }
}
```

#### Comments
- Add XML comments to public methods
- Use inline comments for complex logic
- Keep comments up-to-date with code changes

```csharp
/// <summary>
/// Applies damage to the enemy and checks for death
/// </summary>
/// <param name="amount">Amount of damage to apply</param>
public void TakeDamage(float amount)
{
    currentHealth -= amount;
    
    // Check if health drops to zero or below
    if (currentHealth <= 0)
    {
        Die();
    }
}
```

### Unity Best Practices

#### Component References
- Cache component references in Awake() or Start()
- Use [SerializeField] for inspector-visible private fields
- Avoid GetComponent() in Update()

```csharp
[SerializeField] private Rigidbody rb;

private void Awake()
{
    rb = GetComponent<Rigidbody>();
}
```

#### Performance
- Use object pooling for frequently instantiated objects
- Avoid Camera.main in frequently called methods
- Use CompareTag() instead of tag ==
- Cache transform and gameObject references

```csharp
// Good
if (hitCollider.CompareTag("Enemy"))

// Avoid
if (hitCollider.tag == "Enemy")
```

## Areas for Contribution

### High Priority
- [ ] Additional enemy AI behaviors
- [ ] More weapon types
- [ ] Additional trap variations
- [ ] Level design and environment art
- [ ] Audio implementation
- [ ] UI/HUD improvements

### Medium Priority
- [ ] Power-ups and buffs
- [ ] Achievement system
- [ ] Save/load functionality
- [ ] Settings menu
- [ ] Tutorial system

### Low Priority
- [ ] Multiplayer support
- [ ] Additional game modes
- [ ] Localization
- [ ] Mod support

## Testing

### Before Submitting
- Test in Unity Editor Play mode
- Verify no console errors or warnings
- Test on target platforms if possible
- Ensure no performance regressions
- Check that existing features still work

### Test Checklist
- [ ] Player movement works correctly
- [ ] Weapons fire and deal damage
- [ ] Enemies spawn and navigate properly
- [ ] Drunkenness system affects enemies
- [ ] Traps trigger and function correctly
- [ ] Barrels can be destroyed
- [ ] Game over conditions work
- [ ] Wave system progresses properly

## Documentation

### Code Documentation
- Add XML comments to all public APIs
- Include parameter descriptions
- Document return values
- Explain complex algorithms

### Project Documentation
- Update README.md if adding major features
- Update GAME_DESIGN.md for design changes
- Add tutorials for new systems
- Keep DEVELOPMENT_SETUP.md current

## Git Workflow

### Branch Naming
- `feature/` - New features
- `fix/` - Bug fixes
- `docs/` - Documentation only
- `refactor/` - Code refactoring
- `test/` - Adding tests

Examples:
- `feature/new-weapon-type`
- `fix/enemy-pathfinding-bug`
- `docs/update-api-reference`

### Commit Messages
Format: `<type>: <subject>`

Types:
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation
- `style`: Formatting, missing semicolons, etc.
- `refactor`: Code restructuring
- `test`: Adding tests
- `chore`: Maintenance

Examples:
```
feat: Add rum bottle grenade weapon
fix: Resolve enemy spawning issue
docs: Update quick start guide
refactor: Improve drunkenness system performance
```

### Pull Request Template
```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Tested in Unity Editor
- [ ] No console errors
- [ ] Existing tests pass

## Screenshots
If applicable, add screenshots

## Checklist
- [ ] Code follows style guidelines
- [ ] Comments added for complex code
- [ ] Documentation updated
- [ ] No warnings in console
```

## Questions?

- Check [DEVELOPMENT_SETUP.md](DEVELOPMENT_SETUP.md)
- Review [GAME_DESIGN.md](GAME_DESIGN.md)
- Open an issue for questions
- Join discussions in pull requests

## Recognition

Contributors will be recognized in:
- Project README
- In-game credits (if applicable)
- Release notes

Thank you for contributing to Rumhold! 🏴‍☠️🍺
