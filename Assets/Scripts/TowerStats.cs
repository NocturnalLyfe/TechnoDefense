using System;
using System.Collections.Generic;
using UnityEngine;
public class TowerStats : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField] private string towerName = "New Tower";
    [SerializeField] private TowerArchetype archetype;

    [Header("Level & Experience")]
    [SerializeField] private int level = 1;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int xpToNextLevel = 100;
    [SerializeField] private int availableStatPoints = 0;

    [Header("Base Stats (From Archetype)")]
    [SerializeField] private int baseMaxHealth;
    [SerializeField] private int baseAttack;
    [SerializeField] private int baseDefense;
    [SerializeField] private float baseAttackSpeed;
    [SerializeField] private float baseRange;

    [Header("Attribute Points")]
    [SerializeField] private int allocatedHealth = 0;
    [SerializeField] private int allocatedAttack = 0;
    [SerializeField] private int allocatedDefense = 0;
    [SerializeField] private int allocatedSpeed = 0;
    [SerializeField] private int allocatedRange = 0;

    [Header("Current Stats (Calculated)")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float range;

    [Header("Modifiers & Upgrades")]
    [SerializeField] private List<TowerEnhancer> enhancements = new List<TowerEnhancer>();

    // Properties
    public string TowerName => towerName;
    public TowerArchetype Archetype => archetype;
    public int Level => level;
    public int CurrentXP => currentXP;
    public int XPToNextLevel => xpToNextLevel;
    public int AvailableStatPoints => availableStatPoints;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public int Attack => attack;
    public int Defense => defense;
    public float AttackSpeed => attackSpeed;
    public float Range => range;

    // Initialize tower with chosen element and archetype
    public void Initialize(TowerArchetype chosenArchetype, int startLevel = 1)
    {
        archetype = chosenArchetype;
        level = startLevel;

        // Set base stats based on archetype
        SetArchetypeStats();

        // Give stat points for starting level
        availableStatPoints = (startLevel - 1) * 3; // 3 points per level

        // Calculate all stats
        RecalculateStats();

        // Full health on creation
        currentHealth = maxHealth;

        // Generate name based on element and archetype
        towerName = $"Tower: {archetype}";

        Debug.Log($"Tower Created: {towerName} | Lvl {level} | {availableStatPoints} points available");
    }

    private void SetArchetypeStats()
    {
        switch (archetype)
        {
            case TowerArchetype.AntiVirus:
                baseMaxHealth = 50;
                baseAttack = 25;
                baseDefense = 5;
                baseAttackSpeed = 1.0f;
                baseRange = 10.0f;
                break;

            case TowerArchetype.AntiSpyware:
                baseMaxHealth = 40;
                baseAttack = 10;
                baseDefense = 8;
                baseAttackSpeed = 0.5f;
                baseRange = 6.0f;
                break;

            case TowerArchetype.Firewall:
                baseMaxHealth = 60;
                baseAttack = 20;
                baseDefense = 30;
                baseAttackSpeed = 3.0f;
                baseRange = 12.0f;
                break;
        }
    }

    // Allocate stat points (called from UI)
    public bool AllocateStatPoint(StatType statType, int points = 1)
    {
        if (availableStatPoints < points) return false;

        switch (statType)
        {
            case StatType.Health:
                allocatedHealth += points;
                break;
            case StatType.Attack:
                allocatedAttack += points;
                break;
            case StatType.Defense:
                allocatedDefense += points;
                break;
            case StatType.Speed:
                allocatedSpeed += points;
                break;
            case StatType.Range:
                allocatedRange += points;
                break;
        }

        availableStatPoints -= points;
        RecalculateStats();
        return true;
    }

    // Reset allocated points (costs something or limited uses)
    public void ResetStatPoints()
    {
        availableStatPoints += allocatedHealth + allocatedAttack + allocatedDefense + allocatedSpeed + allocatedRange;
        allocatedHealth = 0;
        allocatedAttack = 0;
        allocatedDefense = 0;
        allocatedSpeed = 0;
        allocatedRange = 0;
        RecalculateStats();
    }

    private void RecalculateStats()
    {
        // Base + Allocated + Level scaling
        maxHealth = baseMaxHealth + (allocatedHealth * 5) + (level * 3);
        attack = baseAttack + (allocatedAttack * 2) + (level * 1);
        defense = baseDefense + (allocatedDefense * 2) + (level * 1);
        attackSpeed = baseAttackSpeed + (allocatedSpeed * 0.05f);
        range = baseRange + (allocatedRange * 0.2f);

        // Update XP requirement
        xpToNextLevel = 100 + (level * 50);

        // Heal to max if health increased
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    // Gain experience
    public void GainXP(int amount)
    {
        currentXP += amount;

        while (currentXP >= xpToNextLevel && level < 100)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentXP -= xpToNextLevel;
        level++;
        availableStatPoints += 3; // Gain 3 stat points per level

        Debug.Log($"{towerName} leveled up to {level}! +3 stat points");

        RecalculateStats();
        currentHealth = maxHealth; // Full heal on level up
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(1, damage - defense);
        currentHealth -= actualDamage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{towerName} has been destroyed!");
        // Add death logic
    }

    public string GetStatsSummary()
    {
        return $"{towerName}\n" +
               $"Level: {level} | XP: {currentXP}/{xpToNextLevel}\n" +
               $"Archetype: {archetype}\n" +
               $"Available Points: {availableStatPoints}\n" +
               $"HP: {currentHealth}/{maxHealth} (+{allocatedHealth})\n" +
               $"ATK: {attack} (+{allocatedAttack}) | DEF: {defense} (+{allocatedDefense})\n" +
               $"Speed: {attackSpeed:F2} (+{allocatedSpeed}) | Range: {range:F1} (+{allocatedRange})\n" +
               $"Modifiers: {enhancements.Count}";
    }
}
