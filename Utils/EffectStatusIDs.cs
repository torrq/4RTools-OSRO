using System;
using System.ComponentModel;

namespace BruteGamingMacros.Core.Utils
{
    [Flags]
    public enum EffectStatusIDs : uint
    {
        [Description("Provoke")]
        PROVOKE = 0,

        [Description("Endure")]
        ENDURE = 1,

        [Description("Two Hand Quicken")]
        TWOHANDQUICKEN = 2,

        [Description("Concentration")]
        CONCENTRATION = 3,

        [Description("Hiding")]
        HIDING = 4,

        [Description("Cloaking")]
        CLOAKING = 5,

        [Description("Poison React")]
        POISONREACT = 7,

        [Description("Quagmire")]
        QUAGMIRE = 8,

        [Description("Angelus")]
        ANGELUS = 9,

        [Description("Blessing")]
        BLESSING = 10,

        [Description("Increase AGI")]
        INC_AGI = 12,

        [Description("Decrease AGI")]
        DECREASE_AGI = 13,

        [Description("Slow Poison")]
        SLOWPOISON = 14,

        [Description("Impositio Manus")]
        IMPOSITIO = 15,

        [Description("Suffragium")]
        SUFFRAGIUM = 16,

        [Description("Aspersio")]
        ASPERSIO = 17,

        [Description("Benedictio Sanctissimi Sacramenti")]
        BENEDICTIO = 18,

        [Description("Kyrie Eleison")]
        KYRIE = 19,

        [Description("Magnificat")]
        MAGNIFICAT = 20,

        [Description("Gloria")]
        GLORIA = 21,

        [Description("Lex Aeterna")]
        LEXAETERNA = 22,

        [Description("Adrenaline Rush")]
        ADRENALINE = 23,

        [Description("Weapon Perfection")]
        WEAPONPERFECT = 24,

        [Description("Overthrust")]
        OVERTHRUST = 25,

        [Description("Maximize")]
        MAXIMIZE = 26,

        [Description("Peco Riding")]
        PECO_RIDING = 27,

        [Description("Falcon On")]
        FALCON_ON = 28,

        [Description("Crazy Uproar")]
        CRAZY_UPROAR = 30,

        [Description("Energy Coat")]
        ENERGYCOAT = 31,

        [Description("Hallucination")]
        HALLUCINATION = 34,

        [Description("50% Weight")]
        WEIGHT50 = 35,

        [Description("90% Weight")]
        WEIGHT90 = 36,

        [Description("Concentration Potion")]
        CONCENTRATION_POTION = 37,

        [Description("Awakening Potion")]
        AWAKENING_POTION = 38,

        [Description("Berserk Potion")]
        BERSERK_POTION = 39,

        ASPDPOTIONINFINITY = 40,

        [Description("Speed Potion")]
        SPEED_POT = 41,

        [Description("Strip Weapon")]
        STRIPWEAPON = 50,

        [Description("Strip Shield")]
        STRIPSHIELD = 51,

        [Description("Strip Armor")]
        STRIPARMOR = 52,

        [Description("Strip Helm")]
        STRIPHELM = 53,

        [Description("Chemical Protection Weapon")]
        PROTECTWEAPON = 54,

        [Description("Chemical Protection Shield")]
        PROTECTSHIELD = 55,

        [Description("Chemical Protection Armor")]
        PROTECTARMOR = 56,

        [Description("Chemical Protection Helm")]
        PROTECTHELM = 57,

        [Description("Autoguard")]
        AUTOGUARD = 58,

        [Description("Reflect Shield")]
        REFLECTSHIELD = 59,

        [Description("Providence")]
        PROVIDENCE = 61,

        [Description("Defender")]
        DEFENDER = 62,

        WEAPONPROPERTY = 64,

        [Description("Auto Spell")]
        AUTOSPELL = 65,

        [Description("Spear Quicken")]
        SPEARQUICKEN = 68,

        [Description("A Poem of Bragi")]
        POEMBRAGI = 72,

        [Description("The Apple of Idun")]
        APPLEIDUN = 73,

        [Description("Fury")]
        FURY = 86,

        [Description("Elemental Converter (Fire)")]
        PROPERTYFIRE = 90,

        [Description("Elemental Converter (Water)")]
        PROPERTYWATER = 91,

        [Description("Elemental Converter (Wind)")]
        PROPERTYWIND = 92,

        [Description("Elemental Converter (Earth)")]
        PROPERTYGROUND = 93,

        [Description("Undead Property")]
        PROPERTYUNDEAD = 97,

        [Description("Aura Blade")]
        AURABLADE = 103,

        [Description("Parrying")]
        PARRYING = 104,

        [Description("Concentration / Saber Thrust (HR)")]
        LKCONCENTRATION = 105,

        [Description("Tension Relax")]
        TENSIONRELAX = 106,

        [Description("Berserk")]
        BERSERK = 107,

        [Description("Assumptio")]
        ASSUMPTIO = 110,

        [Description("Mystical Amplification")]
        MYST_AMPLIFY = 113,

        [Description("Enchant Deadly Poison")]
        EDP = 114,

        [Description("True Sight")]
        TRUESIGHT = 115,

        [Description("Wind Walk")]
        WINDWALK = 116,

        [Description("Meltdown")]
        MELTDOWN = 117,

        [Description("Cart Boost")]
        CARTBOOST = 118,

        [Description("Reject Sword")]
        SWORDREJECT = 120,

        [Description("Bleeding")]
        BLEEDING = 124,

        [Description("Mind Breaker")]
        MINDBREAKER = 126,

        [Description("Memorize")]
        MEMORIZE = 127,

        [Description("Magnum Break")]
        MAGNUM = 131,

        [Description("Autoberserk")]
        AUTOBERSERK = 132,

        [Description("Dodge On")]
        DODGE_ON = 143,

        [Description("Running")]
        RUN = 145,

        [Description("Elemental Converter (Dark)")]
        PROPERTYDARK = 146,

        [Description("Full Adrenaline Rush")]
        ADRENALINE2 = 147,

        [Description("Elemental Converter (Ghost)")]
        PROPERTYTELEKINESIS = 148,

        [Description("Soul Link")]
        SOULLINK = 149,

        // Resentment Box is also:
        // - Tasty Pink Ration (10 min)
        // - Chewy Ricecake (30 min)
        [Description("Resentment Box / Tasty Pink Ration / Chewy Ricecake")]
        RESENTMENT_BOX = 150,

        // Drowisness Box is also:
        // - Tasty White Ration (10 mins)
        [Description("Drowsiness Box / Tasty White Ration")]
        DROWSINESS_BOX = 151,

        [Description("Kaizel")]
        KAIZEL = 156,

        [Description("Kaahi")]
        KAAHI = 157,

        [Description("Kaupe")]
        KAUPE = 158,

        [Description("One Hand Quicken")]
        ONEHANDQUICKEN = 161,

        [Description("Solar, Lunar, Stellar Heat")]
        WARM = 165,

        [Description("Comfort of the Sun")]
        SUN_COMFORT = 169,

        [Description("Comfort of the Moon")]
        MOON_COMFORT = 170,

        [Description("Comfort of the Stars")]
        STAR_COMFORT = 171,

        [Description("Preserve")]
        PRESERVE = 181,

        [Description("Chase Walk")]
        CHASEWALK = 182,

        SUNLIGHT_BOX = 184,

        [Description("Double Casting")]
        DOUBLECASTING = 186,

        [Description("Maximum Overthrust")]
        OVERTHRUSTMAX = 188,

        [Description("Homunculus Avoid")]
        HOM_AVOID = 192,

        [Description("Shrink")]
        CR_SHRINK = 197,

        [Description("Sight Blaster")]
        SIGHTBLASTER = 198,

        [Description("Madness Canceller")]
        MADNESSCANCEL = 203,

        [Description("Gatling Fever")]
        GATLINGFEVER = 204,

        [Description("Cast-off Cicada Shell / Cicada Skin Shed")]
        CICADA_SKIN_SHED = 206,

        [Description("Illusionary Shadow / Mirror Image")]
        MIRROR_IMAGE = 207,

        [Description("Ninja Aura")]
        NINJA_AURA = 208,

        [Description("Adjustment")]
        ADJUSTMENT = 209,

        [Description("Accuracy")]
        ACCURACY = 210,

        [Description("Food STR+10")]
        FOOD_STR = 241,

        [Description("Food AGI+10")]
        FOOD_AGI = 242,

        [Description("Food VIT+10")]
        FOOD_VIT = 243,

        [Description("Food DEX+10")]
        FOOD_DEX = 244,

        [Description("Food INT+10")]
        FOOD_INT = 245,

        [Description("Food LUK+10")]
        FOOD_LUK = 246,

        // Flee Scroll is also:
        // -- Spray of Flowers (flee +10, 5 mins)
        [Description("Flee Scroll / Spray of Flowers")]
        FLEE_SCROLL = 247,

        [Description("Accuracy Scroll")]
        ACCURACY_SCROLL = 248,

        [Description("Field Manual 100%")]
        FIELD_MANUAL = 250,

        [Description("HE Bubblegum")]
        CASH_RECEIVEITEM = 252,

        FOOD_VIT_CASH = 273,

        [Description("Slow Cast")]
        SLOW_CAST = 282,

        [Description("Critical Wound")]
        CRITICALWOUND = 286,

        [Description("Box of Thunder")]
        BOX_OF_THUNDER = 289,

        REGENERATION_POTION = 292,
        CRITICALPERCENT = 295,
        GLASS_OF_ILLUSION = 296,
        MENTAL_POTION = 298,
        SPELLBREAKER = 300,
        TARGET_BLOOD = 301,

        [Description("Enchant Poison Armor")]
        ENCHANT_POISON_ARMOR = 302,

        CASH_PLUSECLASSXP = 312,

        [Description("Enchant Blade")]
        ENCHANT_BLADE = 316,

        THURISAZ = 319,

        HAGALAZ = 320,

        [Description("Fighting Spirit")]
        FIGHTINGSPIRIT = 322,

        [Description("Lauda Agnus")]
        LAUDA_AGNUS = 331,

        [Description("Lauda Ramus")]
        LAUDA_RAMUS = 332,

        [Description("Hallucination Walk")]
        HALLUCINATIONWALK = 334,

        [Description("Expiatio")]
        EXPIATIO = 335,

        [Description("Freezing")]
        FREEZING = 351,

        [Description("Fear Breeze")]
        FEARBREEZE = 352,

        [Description("Recognized Spell")]
        RECOGNIZEDSPELL = 355,

        ACCELERATION = 361,
        TAO_GUNKA = 368,
        ABELHA = 369,
        ORC_HEROI = 370,
        SR_ORCS = 371,
        OVERHEAT = 373,

        [Description("Vanguard Force")]
        FORCEOFVANGUARD = 391,

        [Description("Shadow Spell")]
        AUTOSHADOWSPELL = 393,

        [Description("Prestige")]
        PRESTIGE = 402,

        [Description("Inspiration")]
        INSPIRATION = 407,

        [Description("Rising Dragon")]
        RAISINGDRAGON = 410,

        ACARAJE = 414,

        [Description("Gentle Touch-Convert")]
        GENTLETOUCH_CHANGE = 426,

        [Description("Gentle Touch-Revitalize")]
        GENTLETOUCH_REVITALIZE = 427,

        [Description("Deep Sleep")]
        DEEP_SLEEP = 435,

        [Description("Venom Splasher")]
        VENOM_SPLASHER = 440,

        [Description("Dances with Wargs")]
        DANCE_WITH_WUG = 441,

        [Description("Windmill Rush")]
        RUSH_WINDMILL = 442,

        [Description("Moonlight Serenade")]
        MOONLIT_SERENADE = 447,

        [Description("Cart Boost")]
        GN_CARTBOOST = 461,

        [Description("Mandragora")]
        MANDRAGORA = 470,

        [Description("HP Increase Potion(Large)")]
        HP_INCREASE_POTION_LARGE = 480,

        SP_INCREASE_POTION_LARGE = 481,

        //ENERGY_DRINK_RESERCH = 481,
        VITATA_POTION = 483,

        ENRICH_CELERMINE_JUICE = 484,
        FULL_SWINGK = 486,
        MANA_PLUS = 487,

        STR_3RD_FOOD = 491,
        INT_3RD_FOOD = 492,
        VIT_3RD_FOOD = 493,
        DEX_3RD_FOOD = 494,
        AGI_3RD_FOOD = 495,
        LUK_3RD_FOOD = 496,
        PAINKILLER = 577,
        RIDDING = 613,
        OVERLAPEXPUP = 618,
        MONSTER_TRANSFORM = 621,

        [Description("Sitting")]
        SIT = 622,

        [Description("16th Night")]
        IZAYOI = 652,

        [Description("Combat Pill")]
        COMBAT_PILL = 662,

        [Description("Arrow Equipped")]
        ARROW_ON = 695,

        [Description("Frigg's Song")]
        FRIGG_SONG = 715,

        [Description("Intense Telekinesis")]
        TELEKINESIS_INTENSE = 717,

        [Description("Unlimited")]
        UNLIMIT = 722,

        [Description("Eternal Chain")]
        E_CHAIN = 753,

        // Main Debuffs

        [Description("Stone Curse (petrified)")]
        STONEWAIT = 875,

        [Description("Frozen")]
        FROZEN = 876,

        [Description("Stun")]
        STUN = 877,

        [Description("Sleep")]
        SLEEP = 878,

        [Description("Stone Curse (initial stage)")]
        STONE = 880,

        [Description("Burning")]
        BURNING = 881,

        [Description("Poison")]
        POISON = 883,

        [Description("Curse")]
        CURSE = 884,

        [Description("Silence")]
        SILENCE = 885,

        [Description("Confusion")]
        CONFUSION = 886,

        [Description("Blind")]
        BLIND = 887,

        [Description("Fear")]
        FEAR = 891,

        [Description("Force Sacrifice")]
        HR_FORCESACRIFICE = 900,

        [Description("Force Haste")]
        HR_FORCEHASTE = 901,

        [Description("Force Persuasion")]
        HR_FORCEPERSUASION = 902,

        [Description("Saber Parry")]
        HR_SABERPARRY = 903,

        [Description("Force Concentration")]
        HR_FORCECONCENTRATE = 905,

        [Description("Jedi Frenzy")]
        HR_JEDIFRENZY = 906,

        [Description("Force Projection")]
        HR_PROJECTION = 907,

        //HR_COLDSKIN = 908, // dupe of 908:RESIST_PROPERTY_WATER
        //HR_SABERTHRUST = 105, // dupe of 105:LKCONCENTRATION

        [Description("Coldproof Potion / Cold Skin (HR)")]
        RESIST_PROPERTY_WATER = 908,

        [Description("Earthproof Potion")]
        RESIST_PROPERTY_GROUND = 909,

        [Description("Fireproof Potion")]
        RESIST_PROPERTY_FIRE = 910,

        [Description("Thunderproof Potion")]
        RESIST_PROPERTY_WIND = 911,

        [Description("Service for You / Gypsy's Kiss")]
        SERVICE_FOR_YOU = 1002,

        [Description("A Poem of Bragi / Magic Strings")]
        POEM_OF_BRAGI = 1005,

        [Description("Apple of Idun / Song of Lutie")]
        APPLE_OF_IDUN = 1006,

        [Description("Mana Shield")]
        MANA_SHIELD = 1007,

        [Description("Refraction")]
        REFRACTION = 1008,

        [Description("Infinity Drink")]
        INFINITY_DRINK = 1065,

        [Description("Basílica")]
        BASILICA = 1122,

        MISTY_FROST = 1141,
        LUX_AMINA = 1154,

        [Description("Powerful Faith")]
        POWERFUL_FAITH = 1160,

        [Description("Firm Faith")]
        FIRM_FAITH = 1162,

        REF_T_POTION = 1169,
        RED_HERB_ACTIVATOR = 1170,
        BLUE_HERB_ACTIVATOR = 1171,

        [Description("Research Report")]
        RESEARCHREPORT = 1248,

        [Description("Shield Spell")]
        SHIELDSPELL = 1316,

        CASH_PLUSEXP = 1400,
        SPIRIT = 1401,

        [Description("Kaite")]
        KAITE = 1402,

        [Description("Box of Storms")]
        BOX_OF_STORMS = 1405,

        [Description("Volcano")]
        VOLCANO = 1412,

        [Description("Deluge")]
        DELUGE = 1413,

        [Description("Violent Gale")]
        VIOLENTGALE = 1414,

        [Description("Land Protector")]
        LANDPROTECTOR = 1415,

        [Description("Hallucination")]
        HALLUCINATION_DEBUFF = 1416,

        [Description("Force Element (Earth)")]
        ELEMENT_EARTH = 1423,

        [Description("Force Element (Wind)")]
        ELEMENT_WIND = 1424,

        [Description("Force Element (Water)")]
        ELEMENT_WATER = 1425,

        [Description("Force Element (Fire)")]
        ELEMENT_FIRE = 1426,

        [Description("Force Element (Ghost)")]
        ELEMENT_GHOST = 1427,

        [Description("Force Element (Shadow)")]
        ELEMENT_SHADOW = 1428,

        [Description("Force Element (Holy)")]
        ELEMENT_HOLY = 1429,

        [Description("Saber Parry")]
        SABERPARRY = 1430,

        [Description("Force Concentration")]
        FORCECONCENTRATE = 1432,

        [Description("Saber Thrust")]
        SABERTHRUST = 1438,

        [Description("Cold Skin")]
        COLDSKIN = 1439,

        [Description("Force Projection")]
        PROJECTION = 1441,

        [Description("Force Persuasion")]
        FORCEPERSUASION = 1431,

        [Description("Jedi Frenzy")]
        JEDIFRENZY = 1433,

        [Description("Force Sacrifice")]
        FORCESACRIFICE = 1434,

        [Description("Force Levitate")]
        FORCELEVITATE = 1435,

        [Description("Jedi Stealth")]
        JEDISTEALTH = 1437,

        [Description("Greed Parry")]
        GREED_PARRY = 1442,

        [Description("Halo Halo")]
        HALOHALO = 2011,

        STR_Biscuit_Stick = 2035,
        VIT_Biscuit_Stick = 2036,
        AGI_Biscuit_Stick = 2037,
        INT_Biscuit_Stick = 2038,
        DEX_Biscuit_Stick = 2039,
        LUK_Biscuit_Stick = 2040,

        [Description("Union of the Sun, Moon and Stars")]
        FUSION = 2063,

        BOVINE = 2068,
        DRAGON = 2069,

        [Description("Solar, Lunar and Stellar Miracle")]
        MIRACLE = 2113,
    }
}