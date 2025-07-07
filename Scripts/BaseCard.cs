using UnityEngine;
using NaughtyAttributes;

public enum RaceType { Elf, Dwarf, Naga, Rotkin, Neutral }
public enum CardRarity { Common, Rare, Epic }
public enum CardType { Building, Unit, Skill }

[CreateAssetMenu(menuName = "Cards/Card")]
public class BaseCard : ScriptableObject
{
    [Header("🔹 General Info")]
    public string cardName;
    public RaceType race;
    public CardRarity rarity;
    public CardType cardType;
    public Sprite artwork;
    public int level;
    public int cost;
    public GameObject cardModelPrefab;
    [TextArea]
    public string description;

    // ========== Building Card Properties ==========
    [Space(10)]
    [Header("🏗️ Building Properties")]

    [ShowIf("IsBuilding"), Foldout("Building")]
    public bool isDefensive;

    [ShowIf("IsBuilding"), Foldout("Building")]
    public bool isProductionBuilding;

    [ShowIf("IsBuilding"), Foldout("Building")]
    public bool explodesOnDeath;

    [ShowIf("IsBuilding"), Foldout("Building")]
    public int productionAmount;

    [ShowIf("IsBuilding"), Foldout("Building")]
    public float passiveBuffPercent;

    [ShowIf("IsBuilding"), Foldout("Building")]
    public float destroyEffectValue;

    [ShowIf("IsBuilding"), Foldout("Building")]
    public int turnsToGrow;

    [ShowIf("IsBuilding"), Foldout("Building")]
    public float surroundingEffectRadius;

    [ShowIf("IsBuilding"), Foldout("Building")]
    public float levelBuffPercent;

    // ========== Unit Card Properties ==========
    [Space(10)]
    [Header("⚔️ Unit Properties")]

    [ShowIf("IsUnit"), Foldout("Unit")]
    public int attack;

    [ShowIf("IsUnit"), Foldout("Unit")]
    public int health;

    [ShowIf("IsUnit"), Foldout("Unit")]
    public bool isRanged;

    [ShowIf("IsUnit"), Foldout("Unit")]
    public bool isInvisible;

    [ShowIf("IsUnit"), Foldout("Unit")]
    public bool copiesNearbyUnits;

    [ShowIf("IsUnit"), Foldout("Unit")]
    public bool attacksWeakest;

    [ShowIf("IsUnit"), Foldout("Unit")]
    public float lifestealPercent;

    [ShowIf("IsUnit"), Foldout("Unit")]
    public float reflectDamagePercent;

    [ShowIf("IsUnit"), Foldout("Unit")]
    public float summonChancePercent;

    [ShowIf("IsUnit"), Foldout("Unit")]
    public float poisonChancePercent;

    [ShowIf("IsUnit"), Foldout("Unit")]
    public bool canOneShot;

    [ShowIf("IsUnit"), Foldout("Unit")]
    public float resourceScalingFactor;

    // ========== Skill Card Properties ==========
    [Space(10)]
    [Header("🌟 Skill Properties")]

    [ShowIf("IsSkill"), Foldout("Skill")]
    public bool isPassive;

    [ShowIf("IsSkill"), Foldout("Skill")]
    public int cooldown;

    [ShowIf("IsSkill"), Foldout("Skill")]
    public bool targetsEnemy;

    [ShowIf("IsSkill"), Foldout("Skill")]
    public bool isOneTimeUse;

    [ShowIf("IsSkill"), Foldout("Skill")]
    public bool disablesEnemyAttack;

    [ShowIf("IsSkill"), Foldout("Skill")]
    public bool revealsEnemy;

    [ShowIf("IsSkill"), Foldout("Skill")]
    public float damageBoostPercent;

    [ShowIf("IsSkill"), Foldout("Skill")]
    public float defenseBoostPercent;

    [ShowIf("IsSkill"), Foldout("Skill")]
    public float resourceStealAmount;

    [ShowIf("IsSkill"), Foldout("Skill")]
    public float evasionChance;

    [ShowIf("IsSkill"), Foldout("Skill")]
    public bool altersDayNightCycle;

    // ========= Utility Conditions =========
    private bool IsBuilding() => cardType == CardType.Building;
    private bool IsUnit() => cardType == CardType.Unit;
    private bool IsSkill() => cardType == CardType.Skill;
}
