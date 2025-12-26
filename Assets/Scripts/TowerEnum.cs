public enum TowerArchetype
{
    AntiVirus,   // Gunner: Rapid Fire, Standard DPS
    AntiSpyware, // Sniper: Long Range, High Single target damage
    Firewall,    // Tank:   High health, Absorbs damage, Possible AoE/blocking
}

public enum TowerEnhancer
{
    Swift,       // ↑ Rate of Fire, ↓ Damage
    Hardened,    // ↑ Defense, ↓ Speed, Immune to Status Effects
    Extended,    // ↑ Range, ↓ Rate of Fire
}

public enum TowerStratagem
{
    First,
    Last,
    Closest,
    Farthest,
    Lowest,
    Highest,
    Random
}

public enum StatType
{
    Health,
    Attack,
    Defense,
    Speed,
    Range
}