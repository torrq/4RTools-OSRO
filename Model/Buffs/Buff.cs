// Interface for logging
using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using static _ORTools.Utils.FormHelper;

public interface ILogger
{
    void Error(string message);
    void Error(Exception ex, string message);
}

// Interface for buff factory
public interface IBuffFactory
{
    Buff CreateBuff(string name, string effectStatusName, string iconName);
    List<Buff> CreateServerSpecificBuffList(Dictionary<int, List<Buff>> serverBuffs);
}

// Buff class (minimal, only holds data)
public class Buff
{
    public string Name { get; set; }
    public EffectStatusIDs EffectStatusID { get; set; }
    public Bitmap Icon { get; set; }

    public Buff(string name, EffectStatusIDs effectStatus, Bitmap icon)
    {
        Name = name;
        EffectStatusID = effectStatus;
        Icon = icon;
    }
}

// Factory for creating Buff instances
public class BuffFactory : IBuffFactory
{
    private readonly IResourceLoader _resourceLoader;
    private readonly ILogger _logger;

    public BuffFactory(IResourceLoader resourceLoader, ILogger logger)
    {
        _resourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Buff CreateBuff(string name, string effectStatusName, string iconName)
    {
        if (!Enum.TryParse(effectStatusName, out EffectStatusIDs effect))
        {
            _logger.Error($"Invalid EffectStatusID '{effectStatusName}' for buff '{name}'.");
        }

        Bitmap icon = null;
        try
        {
            icon = _resourceLoader.LoadIcon(iconName);
            if (icon == null)
            {
                _logger.Error($"Could not resolve icon '{iconName}' for buff '{name}'.");
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Exception loading icon '{iconName}' for buff '{name}': {ex.Message}");
        }

        return new Buff(name, effect, icon);
    }

    public List<Buff> CreateServerSpecificBuffList(Dictionary<int, List<Buff>> serverBuffs)
    {
        if (serverBuffs.TryGetValue(AppConfig.ServerMode, out var buffs))
        {
            return new List<Buff>(buffs);
        }
        // Fallback to Mid-rate for any unsupported modes
        if (serverBuffs.TryGetValue(0, out var fallbackBuffs))
        {
            return new List<Buff>(fallbackBuffs);
        }
        throw new InvalidOperationException($"Unsupported ServerMode value: {AppConfig.ServerMode}");
    }
}

// Buff definitions (data storage)
public static class BuffDefinitions
{
    // ARCHER buffs
    public static readonly Dictionary<int, List<Buff>> ArcherBuffs = new Dictionary<int, List<Buff>>();
    // SWORDMAN buffs
    public static readonly Dictionary<int, List<Buff>> SwordmanBuffs = new Dictionary<int, List<Buff>>();
    // MAGE buffs
    public static readonly Dictionary<int, List<Buff>> MageBuffs = new Dictionary<int, List<Buff>>();
    // MERCHANT buffs
    public static readonly Dictionary<int, List<Buff>> MerchantBuffs = new Dictionary<int, List<Buff>>();
    // THIEF buffs
    public static readonly Dictionary<int, List<Buff>> ThiefBuffs = new Dictionary<int, List<Buff>>();
    // ACOLYTE buffs
    public static readonly Dictionary<int, List<Buff>> AcolyteBuffs = new Dictionary<int, List<Buff>>();
    // NINJA buffs
    public static readonly Dictionary<int, List<Buff>> NinjaBuffs = new Dictionary<int, List<Buff>>();
    // TAEKWON buffs
    public static readonly Dictionary<int, List<Buff>> TaekwonBuffs = new Dictionary<int, List<Buff>>();
    // GUNSLINGER buffs
    public static readonly Dictionary<int, List<Buff>> GunslingerBuffs = new Dictionary<int, List<Buff>>();
    // PADAWAN buffs
    public static readonly Dictionary<int, List<Buff>> PadawanBuffs = new Dictionary<int, List<Buff>>();

    // ITEM buffs
    public static readonly List<Buff> PotionBuffs = new List<Buff>();
    public static readonly List<Buff> ElementBuffs = new List<Buff>();
    public static readonly List<Buff> FoodBuffs = new List<Buff>();
    public static readonly List<Buff> BoxBuffs = new List<Buff>();
    public static readonly List<Buff> ScrollBuffs = new List<Buff>();
    public static readonly List<Buff> EtcBuffs = new List<Buff>();

    // DEBUFFS
    public static readonly List<Buff> Debuffs = new List<Buff>();

    // Initialize buffs using the factory
    public static void Initialize(IBuffFactory factory)
    {
        InitializeSkillBuffs(factory);
        InitializeItemBuffs(factory);
        InitializeDebuffs(factory);
    }

    private static void InitializeSkillBuffs(IBuffFactory b)
    {
        ArcherBuffs[0] = new List<Buff>
        {
            b.CreateBuff("Improve Concentration", "CONCENTRATION", "ac_concentration"),
            b.CreateBuff("True Sight", "TRUESIGHT", "sn_sight"),
            b.CreateBuff("Wind Walk", "WINDWALK", "sn_windwalk")
        };

        SwordmanBuffs[0] = new List<Buff>
        {
            b.CreateBuff("Endure", "ENDURE", "sm_endure"),
            b.CreateBuff("Auto Berserk", "AUTOBERSERK", "sm_autoberserk"),
            b.CreateBuff("Auto Guard", "AUTOGUARD", "cr_autoguard"),
            b.CreateBuff("Reflect Shield", "REFLECTSHIELD", "cr_reflectshield"),
            b.CreateBuff("Spear Quicken", "SPEARQUICKEN", "cr_spearquicken"),
            b.CreateBuff("Defender", "DEFENDER", "cr_defender"),
            b.CreateBuff("Concentration", "LKCONCENTRATION", "lk_concentration"),
            b.CreateBuff("Berserk", "BERSERK", "lk_berserk"),
            b.CreateBuff("Two-Hand Quicken", "TWOHANDQUICKEN", "mer_quicken"),
            b.CreateBuff("Parry", "PARRYING", "ms_parrying"),
            b.CreateBuff("Aura Blade", "AURABLADE", "lk_aurablade"),
            b.CreateBuff("Shrink", "CR_SHRINK", "cr_shrink"),
            b.CreateBuff("Magnum Break", "MAGNUM", "magnum"),
            b.CreateBuff("One-Hand Quicken", "ONEHANDQUICKEN", "onehand"),
            b.CreateBuff("Provoke", "PROVOKE", "provoke"),
            b.CreateBuff("Providence", "PROVIDENCE", "providence")
        };
        SwordmanBuffs[1] = new List<Buff>
        {
            b.CreateBuff("Endure", "ENDURE", "sm_endure"),
            b.CreateBuff("Auto Berserk", "AUTOBERSERK", "sm_autoberserk"),
            b.CreateBuff("Auto Guard", "AUTOGUARD", "cr_autoguard"),
            b.CreateBuff("Reflect Shield", "REFLECTSHIELD", "cr_reflectshield"),
            b.CreateBuff("Spear Quicken", "SPEARQUICKEN", "cr_spearquicken"),
            b.CreateBuff("Defender", "DEFENDER", "cr_defender"),
            b.CreateBuff("Concentration", "LKCONCENTRATION", "lk_concentration"),
            b.CreateBuff("Berserk", "BERSERK", "lk_berserk"),
            b.CreateBuff("Two-Hand Quicken", "TWOHANDQUICKEN", "mer_quicken"),
            b.CreateBuff("Parry", "PARRYING", "ms_parrying"),
            b.CreateBuff("Aura Blade", "AURABLADE", "lk_aurablade"),
            b.CreateBuff("Shrink", "CR_SHRINK", "cr_shrink"),
            b.CreateBuff("Magnum Break", "MAGNUM", "magnum"),
            b.CreateBuff("One-Hand Quicken", "ONEHANDQUICKEN", "onehand"),
            b.CreateBuff("Provoke", "PROVOKE", "provoke"),
            b.CreateBuff("Providence", "PROVIDENCE", "providence"),
            b.CreateBuff("Mana Shield", "MANA_SHIELD", "manashield")
        };

        MageBuffs[0] = new List<Buff>
        {
            b.CreateBuff("Energy Coat", "ENERGYCOAT", "mg_energycoat"),
            b.CreateBuff("Sight Blaster", "SIGHTBLASTER", "wz_sightblaster"),
            b.CreateBuff("Autospell", "AUTOSPELL", "sa_autospell"),
            b.CreateBuff("Double Casting", "DOUBLECASTING", "pf_doublecasting"),
            b.CreateBuff("Memorize", "MEMORIZE", "pf_memorize"),
            b.CreateBuff("Amplify Magic Power / Mystical Amplification", "MYST_AMPLIFY", "amplify"),
            b.CreateBuff("Mind Breaker", "MINDBREAKER", "mindbreaker")
        };

        MerchantBuffs[0] = new List<Buff>
        {
            b.CreateBuff("Crazy Uproar", "CRAZY_UPROAR", "mc_loud"),
            b.CreateBuff("Overthrust", "OVERTHRUST", "bs_overthrust"),
            b.CreateBuff("Adrenaline Rush", "ADRENALINE", "bs_adrenaline"),
            b.CreateBuff("Full Adrenaline Rush", "ADRENALINE2", "bs_adrenaline2"),
            b.CreateBuff("Weapon Perfection", "WEAPONPERFECT", "bs_weaponperfect"),
            b.CreateBuff("Maximize Power", "MAXIMIZE", "bs_maximize"),
            b.CreateBuff("Cart Boost", "CARTBOOST", "ws_cartboost"),
            b.CreateBuff("Meltdown", "MELTDOWN", "ws_meltdown"),
            b.CreateBuff("Maximum Overthrust", "OVERTHRUSTMAX", "ws_overthrustmax"),
            b.CreateBuff("Greed Parry", "GREED_PARRY", "ws_greedparry")
        };
        MerchantBuffs[1] = new List<Buff>
        {
            b.CreateBuff("Crazy Uproar", "CRAZY_UPROAR", "mc_loud"),
            b.CreateBuff("Overthrust", "OVERTHRUST", "bs_overthrust"),
            b.CreateBuff("Adrenaline Rush", "ADRENALINE", "bs_adrenaline"),
            b.CreateBuff("Full Adrenaline Rush", "ADRENALINE2", "bs_adrenaline2"),
            b.CreateBuff("Weapon Perfection", "WEAPONPERFECT", "bs_weaponperfect"),
            b.CreateBuff("Maximize Power", "MAXIMIZE", "bs_maximize"),
            b.CreateBuff("Cart Boost", "CARTBOOST", "ws_cartboost"),
            b.CreateBuff("Meltdown", "MELTDOWN", "ws_meltdown"),
            b.CreateBuff("Maximum Overthrust", "OVERTHRUSTMAX", "ws_overthrustmax"),
            b.CreateBuff("Greed Parry", "RESIST_PROPERTY_FIRE", "ws_greedparry")
        };

        ThiefBuffs[0] = new List<Buff>
        {
            b.CreateBuff("Poison React", "POISONREACT", "as_poisonreact"),
            b.CreateBuff("Reject Sword", "SWORDREJECT", "st_rejectsword"),
            b.CreateBuff("Preserve", "PRESERVE", "st_preserve"),
            b.CreateBuff("Enchant Deadly Poison", "EDP", "asc_edp"),
            b.CreateBuff("Hiding", "HIDING", "hiding"),
            b.CreateBuff("Cloaking", "CLOAKING", "cloaking"),
            b.CreateBuff("Chase Walk", "CHASEWALK", "chase_walk")
        };
        ThiefBuffs[1] = new List<Buff>
        {
            b.CreateBuff("Poison React", "POISONREACT", "as_poisonreact"),
            b.CreateBuff("Reject Sword", "SWORDREJECT", "st_rejectsword"),
            b.CreateBuff("Preserve", "PRESERVE", "st_preserve"),
            b.CreateBuff("Enchant Deadly Poison", "EDP", "asc_edp"),
            b.CreateBuff("Hiding", "HIDING", "hiding"),
            b.CreateBuff("Cloaking", "CLOAKING", "cloaking"),
            b.CreateBuff("Chase Walk", "CHASEWALK", "chase_walk"),
            b.CreateBuff("Enchant Poison Armor", "ENCHANT_POISON_ARMOR", "enchantpoisonarmor")
        };

        AcolyteBuffs[0] = new List<Buff>
        {
            b.CreateBuff("Blessing", "BLESSING", "al_blessing1"),
            b.CreateBuff("Increase Agility", "INC_AGI", "al_incagi1"),
            b.CreateBuff("Gloria", "GLORIA", "pr_gloria"),
            b.CreateBuff("Magnificat", "MAGNIFICAT", "pr_magnificat"),
            b.CreateBuff("Angelus", "ANGELUS", "al_angelus"),
            b.CreateBuff("Fury", "FURY", "fury"),
            b.CreateBuff("Impositio Manus", "IMPOSITIO", "impositio_manus"),
            b.CreateBuff("Basilica", "BASILICA", "basilica")
        };
        AcolyteBuffs[1] = new List<Buff>
        {
            b.CreateBuff("Blessing", "BLESSING", "al_blessing1"),
            b.CreateBuff("Increase Agility", "INC_AGI", "al_incagi1"),
            b.CreateBuff("Gloria", "GLORIA", "pr_gloria"),
            b.CreateBuff("Magnificat", "MAGNIFICAT", "pr_magnificat"),
            b.CreateBuff("Angelus", "ANGELUS", "al_angelus"),
            b.CreateBuff("Fury", "FURY", "fury"),
            b.CreateBuff("Impositio Manus", "IMPOSITIO", "impositio_manus"),
            b.CreateBuff("Basilica", "BASILICA", "basilica"),
            b.CreateBuff("Refraction", "REFRACTION", "refraction"),
            b.CreateBuff("Shallow Grave", "KAIZEL", "shallowgrave")
        };

        NinjaBuffs[0] = new List<Buff>
        {
            b.CreateBuff("Ninja Aura", "NINJA_AURA", "nj_nen"),
            b.CreateBuff("Cast-off Cicada / Cicada SS", "CICADA_SKIN_SHED", "nj_utsusemi"),
            b.CreateBuff("Illusion Shadow / Mirror Image", "MIRROR_IMAGE", "bunsinjyutsu")
        };

        TaekwonBuffs[0] = new List<Buff>
        {
            b.CreateBuff("Mild Wind (Earth)", "PROPERTYGROUND", "tk_mild_earth"),
            b.CreateBuff("Mild Wind (Fire)", "PROPERTYFIRE", "tk_mild_fire"),
            b.CreateBuff("Mild Wind (Water)", "PROPERTYWATER", "tk_mild_water"),
            b.CreateBuff("Mild Wind (Wind)", "PROPERTYWIND", "tk_mild_wind"),
            b.CreateBuff("Mild Wind (Ghost)", "PROPERTYTELEKINESIS", "tk_mild_ghost"),
            b.CreateBuff("Mild Wind (Holy)", "ASPERSIO", "tk_mild_holy"),
            b.CreateBuff("Mild Wind (Shadow)", "PROPERTYDARK", "tk_mild_shadow"),
            b.CreateBuff("Tumbling", "DODGE_ON", "tumbling"),
            b.CreateBuff("Solar, Lunar, and Stellar Warmth", "WARM", "sun_warm"),
            b.CreateBuff("Comfort of the Sun", "SUN_COMFORT", "sun_comfort"),
            b.CreateBuff("Comfort of the Moon", "MOON_COMFORT", "moon_comfort"),
            b.CreateBuff("Comfort of the Stars", "STAR_COMFORT", "star_comfort"),
            b.CreateBuff("Kaupe", "KAUPE", "kaupe"),
            b.CreateBuff("Kaite", "KAITE", "kaite"),
            b.CreateBuff("Kaizel", "KAIZEL", "kaizel"),
            b.CreateBuff("Kaahi", "KAAHI", "kaahi")
        };

        GunslingerBuffs[0] = new List<Buff>
        {
            b.CreateBuff("Gatling Fever", "GATLINGFEVER", "gatling_fever"),
            b.CreateBuff("Last Stand", "MADNESSCANCEL", "madnesscancel"),
            b.CreateBuff("Adjustment", "ADJUSTMENT", "adjustment"),
            b.CreateBuff("Increased Accuracy", "ACCURACY", "increase_accuracy")
        };

        PadawanBuffs[1] = new List<Buff>
        {
            b.CreateBuff("Force Element (Earth)", "PROPERTYGROUND", "forceelement_earth"),
            b.CreateBuff("Force Element (Wind)", "PROPERTYWIND", "forceelement_wind"),
            b.CreateBuff("Force Element (Water)", "PROPERTYWATER", "forceelement_water"),
            b.CreateBuff("Force Element (Fire)", "PROPERTYFIRE", "forceelement_fire"),
            b.CreateBuff("Force Element (Ghost)", "PROPERTYTELEKINESIS", "forceelement_ghost"),
            b.CreateBuff("Force Element (Shadow)", "PROPERTYDARK", "forceelement_shadow"),
            b.CreateBuff("Force Element (Holy)", "ASPERSIO", "forceelement_holy"),
            b.CreateBuff("Force Projection", "HR_PROJECTION", "hr_forceprojection"),
            b.CreateBuff("Cold Skin", "RESIST_PROPERTY_WATER", "hr_coldskin"),
            b.CreateBuff("Saber Parry", "HR_SABERPARRY", "hr_saberparry"),
            b.CreateBuff("Force Concentration", "HR_FORCECONCENTRATE", "hr_forceconcentrate"),
            b.CreateBuff("Saber Thrust", "LKCONCENTRATION", "hr_saberthrust"),
            b.CreateBuff("Force Persuasion", "HR_FORCEPERSUASION", "hr_forcepersuasion"),
            b.CreateBuff("Force Haste", "HR_FORCEHASTE", "forcehaste"),
            b.CreateBuff("Force Sacrifice", "HR_FORCESACRIFICE", "hr_forcesacrifice"),
            b.CreateBuff("Jedi Frenzy", "HR_JEDIFRENZY", "hr_jedifrenzy")
        };
        PadawanBuffs[0] = new List<Buff>
        {
            b.CreateBuff("Force Element (Earth)", "ELEMENT_EARTH", "forceelement_earth"),
            b.CreateBuff("Force Element (Wind)", "ELEMENT_WIND", "forceelement_wind"),
            b.CreateBuff("Force Element (Water)", "ELEMENT_WATER", "forceelement_water"),
            b.CreateBuff("Force Element (Fire)", "ELEMENT_FIRE", "forceelement_fire"),
            b.CreateBuff("Force Element (Ghost)", "ELEMENT_GHOST", "forceelement_ghost"),
            b.CreateBuff("Force Element (Shadow)", "ELEMENT_SHADOW", "forceelement_shadow"),
            b.CreateBuff("Force Element (Holy)", "ELEMENT_HOLY", "forceelement_holy"),
            b.CreateBuff("Force Projection", "PROJECTION", "forceprojection"),
            b.CreateBuff("Cold Skin", "COLDSKIN", "coldskin"),
            b.CreateBuff("Saber Parry", "SABERPARRY", "saberparry"),
            b.CreateBuff("Force Concentration", "FORCECONCENTRATE", "forceconcentrate"),
            b.CreateBuff("Saber Thrust", "SABERTHRUST", "saberthrust"),
            b.CreateBuff("Force Persuasion", "FORCEPERSUASION", "forcepersuasion"),
            b.CreateBuff("Jedi Stealth", "JEDISTEALTH", "jedistealth"),
            b.CreateBuff("Force Levitate", "FORCELEVITATE", "forcelevitate"),
            b.CreateBuff("Jedi Frenzy", "JEDIFRENZY", "jedifrenzy"),
            b.CreateBuff("Force Sacrifice", "FORCESACRIFICE", "forcesacrifice")
        };
    }

    private static void InitializeItemBuffs(IBuffFactory b)
    {
        PotionBuffs.AddRange(new[]
        {
            b.CreateBuff("Concentration Potion", "CONCENTRATION_POTION", "concentration_potion"),
            b.CreateBuff("Awakening Potion", "AWAKENING_POTION", "awakening_potion"),
            b.CreateBuff("Berserk Potion", "BERSERK_POTION", "berserk_potion")
        });

        ElementBuffs.AddRange(new[]
        {
            b.CreateBuff("Fire Elemental Converter", "PROPERTYFIRE", "ele_fire_converter"),
            b.CreateBuff("Wind Elemental Converter", "PROPERTYWIND", "ele_wind_converter"),
            b.CreateBuff("Earth Elemental Converter", "PROPERTYGROUND", "ele_earth_converter"),
            b.CreateBuff("Water Elemental Converter", "PROPERTYWATER", "ele_water_converter"),
            b.CreateBuff("Box of Storms", "BOX_OF_STORMS", "boxofstorms"),
            b.CreateBuff("Cursed Water", "PROPERTYDARK", "cursed_water"),
            b.CreateBuff("Fireproof Potion", "RESIST_PROPERTY_FIRE", "fireproof"),
            b.CreateBuff("Coldproof Potion", "RESIST_PROPERTY_WATER", "coldproof"),
            b.CreateBuff("Thunderproof Potion", "RESIST_PROPERTY_WIND", "thunderproof"),
            b.CreateBuff("Earthproof Potion", "RESIST_PROPERTY_GROUND", "earthproof")
        });

        FoodBuffs.AddRange(new[]
        {
            b.CreateBuff("Steamed Tongue (STR+10)", "FOOD_STR", "strfood"),
            b.CreateBuff("Steamed Scorpion (AGI+10)", "FOOD_AGI", "agi_food"),
            b.CreateBuff("Stew of Immortality (VIT+10)", "FOOD_VIT", "vit_food"),
            b.CreateBuff("Dragon Breath Cocktail (INT+10)", "FOOD_INT", "int_food"),
            b.CreateBuff("Hwergelmir's Tonic (DEX+10)", "FOOD_DEX", "dex_food"),
            b.CreateBuff("Cooked Nine Tail's Tails (LUK+10)", "FOOD_LUK", "luk_food"),
            b.CreateBuff("Halo-Halo", "HALOHALO", "halohalo"),
            b.CreateBuff("Glass of Illusion", "GLASS_OF_ILLUSION", "Glass_Of_Illusion")
        });

        BoxBuffs.AddRange(new[]
        {
            b.CreateBuff("Box of Drowsiness / Tasty W. Ration", "DROWSINESS_BOX", "drowsiness"),
            b.CreateBuff("Box of Resentment / Tasty P. Ration / Chewy Ricecake", "RESENTMENT_BOX", "resentment"),
            b.CreateBuff("Box of Sunlight", "SUNLIGHT_BOX", "sunbox"),
            b.CreateBuff("Box of Gloom", "CONCENTRATION", "gloom"),
            b.CreateBuff("Box of Thunder", "BOX_OF_THUNDER", "boxofthunder"),
            b.CreateBuff("Speed Potion", "SPEED_POT", "speedpotion"),
            b.CreateBuff("Anodyne", "ENDURE", "anodyne"),
            b.CreateBuff("Aloe Vera", "PROVOKE", "aloevera"),
            b.CreateBuff("Abrasive", "CRITICALPERCENT", "abrasive"),
            b.CreateBuff("Combat Pill", "COMBAT_PILL", "combat_pill"),
            b.CreateBuff("Enriched Celermine Juice", "ENRICH_CELERMINE_JUICE", "celermine"),
            b.CreateBuff("ASPD Potion Infinity", "ASPDPOTIONINFINITY", "poison")
        });

        ScrollBuffs.AddRange(new[]
        {
            b.CreateBuff("Increase Agility Scroll", "INC_AGI", "al_incagi1"),
            b.CreateBuff("Bless Scroll", "BLESSING", "al_blessing1"),
            b.CreateBuff("Full Chemical Protection ", "PROTECTARMOR", "cr_fullprotection"),
            b.CreateBuff("Link Scroll", "SOULLINK", "sl_soullinker"),
            b.CreateBuff("Assumptio", "ASSUMPTIO", "assumptio"),
            b.CreateBuff("Flee Scroll / Spray of Flowers", "FLEE_SCROLL", "flee_scroll"),
            b.CreateBuff("Accuracy Scroll", "ACCURACY_SCROLL", "accuracy_scroll")
        });

        EtcBuffs.AddRange(new[]
        {
            b.CreateBuff("VIP Ticket", "VIP_BONUS", "vip_ticket"),
            b.CreateBuff("Field Manual 100% / 300%", "FIELD_MANUAL", "fieldmanual"),
            b.CreateBuff("Bubble Gum / HE Bubble Gum", "CASH_RECEIVEITEM", "he_bubble_gum")
        });
    }

    private static void InitializeDebuffs(IBuffFactory b)
    {
        Debuffs.AddRange(new[]
        {
            b.CreateBuff("Bleeding", "BLEEDING", "bleeding"),
            b.CreateBuff("Burning", "BURNING", "burning"),
            b.CreateBuff("Chaos / Confusion", "CONFUSION", "chaos"),
            b.CreateBuff("Critical Wound", "CRITICALWOUND", "critical_wound"),
            b.CreateBuff("Curse", "CURSE", "curse"),
            b.CreateBuff("Decrease AGI", "DECREASE_AGI", "decrease_agi"),
            b.CreateBuff("Freezing", "FREEZING", "freezing"),
            b.CreateBuff("Frozen", "FROZEN", "frozen"),
            b.CreateBuff("Hallucination", "HALLUCINATION_DEBUFF", "hallucination"),
            b.CreateBuff("Poison", "POISON", "poison_status"),
            b.CreateBuff("Silence", "SILENCE", "silence"),
            b.CreateBuff("Sit", "SIT", "sit"),
            b.CreateBuff("Deep Sleep", "DEEP_SLEEP", "deep_sleep"),
            b.CreateBuff("Sleep", "SLEEP", "sleep"),
            b.CreateBuff("Slow Cast", "SLOW_CAST", "slow_cast"),
            b.CreateBuff("Stone Curse (initial stage)", "STONE", "stonecurse1"),
            b.CreateBuff("Stone Curse (petrified)", "STONEWAIT", "stonecurse2"),
            b.CreateBuff("Stun", "STUN", "stun")
        });
    }
}

// Static facade to maintain existing public interface
public static class BuffService
{
    private static IBuffFactory _b;

    public static void Initialize(IResourceLoader resourceLoader, ILogger logger)
    {
        _b = new BuffFactory(resourceLoader, logger);
        BuffDefinitions.Initialize(_b);
    }

    // SKILL BUFFS
    public static List<Buff> GetArcherBuffs() => _b.CreateServerSpecificBuffList(BuffDefinitions.ArcherBuffs);
    public static List<Buff> GetSwordmanBuffs() => _b.CreateServerSpecificBuffList(BuffDefinitions.SwordmanBuffs);
    public static List<Buff> GetMageBuffs() => _b.CreateServerSpecificBuffList(BuffDefinitions.MageBuffs);
    public static List<Buff> GetMerchantBuffs() => _b.CreateServerSpecificBuffList(BuffDefinitions.MerchantBuffs);
    public static List<Buff> GetThiefBuffs() => _b.CreateServerSpecificBuffList(BuffDefinitions.ThiefBuffs);
    public static List<Buff> GetAcolyteBuffs() => _b.CreateServerSpecificBuffList(BuffDefinitions.AcolyteBuffs);
    public static List<Buff> GetNinjaBuffs() => _b.CreateServerSpecificBuffList(BuffDefinitions.NinjaBuffs);
    public static List<Buff> GetTaekwonBuffs() => _b.CreateServerSpecificBuffList(BuffDefinitions.TaekwonBuffs);
    public static List<Buff> GetGunslingerBuffs() => _b.CreateServerSpecificBuffList(BuffDefinitions.GunslingerBuffs);
    public static List<Buff> GetPadawanBuffs() => _b.CreateServerSpecificBuffList(BuffDefinitions.PadawanBuffs);

    // ITEM BUFFS
    public static List<Buff> GetPotionsBuffs() => new List<Buff>(BuffDefinitions.PotionBuffs);
    public static List<Buff> GetElementBuffs() => new List<Buff>(BuffDefinitions.ElementBuffs);
    public static List<Buff> GetFoodBuffs() => new List<Buff>(BuffDefinitions.FoodBuffs);
    public static List<Buff> GetBoxBuffs() => new List<Buff>(BuffDefinitions.BoxBuffs);
    public static List<Buff> GetScrollBuffs() => new List<Buff>(BuffDefinitions.ScrollBuffs);
    public static List<Buff> GetEtcBuffs() => new List<Buff>(BuffDefinitions.EtcBuffs);

    // DEBUFFS
    public static List<Buff> GetDebuffs() => new List<Buff>(BuffDefinitions.Debuffs);
}