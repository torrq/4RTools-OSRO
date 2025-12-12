using System;
using System.ComponentModel;

namespace _ORTools.Utils
{
    [Flags]
    public enum EffectStatusIDs : uint
    {
        [Description("Provoke")]
        SM_PROVOKE = 0,

        [Description("Endure")]
        SM_ENDURE = 1,

        [Description("Two Hand Quicken")]
        KN_TWOHANDQUICKEN = 2,

        [Description("Concentration")]
        AC_CONCENTRATION = 3,

        [Description("Hide")]
        TF_HIDING = 4,

        [Description("Cloak")]
        AS_CLOAKING = 5,

        [Description("Poison React")]
        AS_POISONREACT = 7,

        [Description("Quagmire")]
        WZ_QUAGMIRE = 8,

        [Description("Angelus")]
        AL_ANGELUS = 9,

        [Description("Blessing")]
        AL_BLESSING = 10,

        [Description("Increase AGI")]
        AL_INCAGI = 12,

        [Description("Decrease AGI")]
        AL_DECAGI = 13,

        [Description("Slow Poison")]
        PR_SLOWPOISON = 14,

        [Description("Impositio Manus")]
        PR_IMPOSITIO = 15,

        [Description("Suffragium")]
        PR_SUFFRAGIUM = 16,

        [Description("Aspersio")]
        PR_ASPERSIO = 17,

        [Description("Benedictio Sanctissimi Sacramenti")]
        PR_BENEDICTIO = 18,

        [Description("Kyrie Eleison")]
        PR_KYRIE = 19,

        [Description("Magnificat")]
        PR_MAGNIFICAT = 20,

        [Description("Gloria")]
        PR_GLORIA = 21,

        [Description("Lex Aeterna")]
        PR_LEXAETERNA = 22,

        [Description("Adrenaline Rush")]
        BS_ADRENALINE = 23,

        [Description("Weapon Perfection")]
        BS_WEAPONPERFECT = 24,

        [Description("Overthrust")]
        BS_OVERTHRUST = 25,

        [Description("Maximize")]
        BS_MAXIMIZE = 26,

        [Description("Peco Riding")]
        KN_RIDING = 27,

        [Description("Falcon On")]
        HT_FALCON = 28,

        [Description("Crazy Uproar")]
        MC_LOUD = 30,

        [Description("Energy Coat")]
        MG_ENERGYCOAT = 31,

        [Description("Hallucination")]
        HALLUCINATION = 34,

        [Description("50% Weight")]
        WEIGHT50 = 35,

        [Description("90% Weight")]
        WEIGHT90 = 36,

        [Description("Concentration Potion")]
        ATTHASTE_POTION1 = 37,

        [Description("Awakening Potion")]
        ATTHASTE_POTION2 = 38,

        [Description("Berserk Potion")]
        ATTHASTE_POTION3 = 39,

        ASPDPOTIONINFINITY = 40,

        [Description("Speed Potion")]
        MOVHASTE_POTION = 41,

        [Description("Strip Weapon")]
        RG_STRIPWEAPON = 50,

        [Description("Strip Shield")]
        RG_STRIPSHIELD = 51,

        [Description("Strip Armor")]
        RG_STRIPARMOR = 52,

        [Description("Strip Helm")]
        RG_STRIPHELM = 53,

        [Description("Chemical Protection Weapon")]
        AM_CP_WEAPON = 54,

        [Description("Chemical Protection Shield")]
        AM_CP_SHIELD = 55,

        [Description("Chemical Protection Armor")]
        AM_CP_ARMOR = 56,

        [Description("Chemical Protection Helm")]
        AM_CP_HELM = 57,

        [Description("Autoguard")]
        CR_AUTOGUARD = 58,

        [Description("Reflect Shield")]
        CR_REFLECTSHIELD = 59,

        [Description("Providence")]
        CR_PROVIDENCE = 61,

        [Description("Defender")]
        CR_DEFENDER = 62,

        WEAPONPROPERTY = 64,

        [Description("Auto Spell")]
        SA_AUTOSPELL = 65,

        [Description("Spear Quicken")]
        CR_SPEARQUICKEN = 68,

        [Description("A Poem of Bragi")]
        BA_POEMBRAGI = 72,

        [Description("The Apple of Idun")]
        BA_APPLEIDUN = 73,

        [Description("Fury")]
        MO_EXPLOSIONSPIRITS = 86,

        [Description("Steel Body")]
        MO_STEELBODY = 87,

        [Description("Endow Weapon: Fire")]
        SA_FLAMELAUNCHER = 90,

        [Description("Endow Weapon: Water")]
        SA_FROSTWEAPON = 91,

        [Description("Endow Weapon: Wind")]
        SA_LIGHTNINGLOADER = 92,

        [Description("Endow Weapon: Earth")]
        SA_SEISMICWEAPON = 93,

        [Description("Endow Weapon: Undead")]
        PROPERTYUNDEAD = 97,

        [Description("Aura Blade")]
        LK_AURABLADE = 103,

        [Description("Parrying")]
        LK_PARRYING = 104,

        [Description("Concentration / Saber Thrust (HR)")]
        LK_CONCENTRATION = 105,

        [Description("Tension Relax")]
        LK_TENSIONRELAX = 106,

        [Description("Berserk")]
        LK_BERSERK = 107,

        [Description("Assumptio")]
        HP_ASSUMPTIO = 110,

        [Description("Mystical Amplification")]
        HW_MAGICPOWER = 113,

        [Description("Enchant Deadly Poison")]
        ASC_EDP = 114,

        [Description("True Sight")]
        SN_SIGHT = 115,

        [Description("Wind Walk")]
        SN_WINDWALK = 116,

        [Description("Meltdown")]
        WS_MELTDOWN = 117,

        [Description("Cart Boost")]
        WS_CARTBOOST = 118,

        [Description("Reject Sword")]
        ST_REJECTSWORD = 120,

        [Description("Bleeding")]
        NPC_BLEEDING = 124,

        [Description("Mind Breaker")]
        PF_MINDBREAKER = 126,

        [Description("Memorize")]
        PF_MEMORIZE = 127,

        [Description("Magnum Break")]
        SM_MAGNUM = 131,

        [Description("Autoberserk")]
        SM_AUTOBERSERK = 132,

        [Description("Tornado Stance / Prepare Storm Kick")]
        TK_READYSTORM = 135,

        [Description("Heel Drop Stance / Prepare Axe Kick")]
        TK_READYDOWN = 137,

        [Description("Roundhouse Stance / Prepare Roundhouse Kick")]
        TK_READYTURN = 139,

        [Description("Counter Kick Stance / Prepare Counter Kick")]
        TK_READYCOUNTER = 141,

        [Description("Dodge On")]
        MO_DODGE = 143,

        [Description("Running")]
        TK_RUN = 145,

        [Description("Endow Weapon: Dark")]
        PROPERTYDARK = 146,

        [Description("Advanced/Full Adrenaline Rush")]
        BS_ADRENALINE2 = 147,

        [Description("Endow Weapon: Ghost")]
        PROPERTYTELEKINESIS = 148,

        [Description("Soul Link")]
        SOULLINK = 149,

        // Resentment Box is also:
        // - Tasty Pink Ration (10 min)
        // - Chewy Ricecake (30 min)
        [Description("Resentment Box / Tasty Pink Ration / Chewy Ricecake")]
        PLUSATTACKPOWER = 150,

        // Drowisness Box is also:
        // - Tasty White Ration (10 mins)
        [Description("Drowsiness Box / Tasty White Ration")]
        PLUSMAGICPOWER = 151,

        [Description("Kaizel")]
        SL_KAIZEL = 156,

        [Description("Kaahi")]
        SL_KAAHI = 157,

        [Description("Kaupe")]
        SL_KAUPE = 158,

        [Description("One Hand Quicken")]
        KN_ONEHAND = 161,

        [Description("Solar, Lunar, Stellar Heat")]
        WARM = 165,

        [Description("Comfort of the Sun / Solar Protection")]
        SG_SUN_COMFORT = 169,

        [Description("Comfort of the Moon / Lunar Protection")]
        SG_MOON_COMFORT = 170,

        [Description("Comfort of the Stars / Stellar Protection")]
        SG_STAR_COMFORT = 171,

        [Description("Preserve")]
        ST_PRESERVE = 181,

        [Description("Chase Walk")]
        ST_CHASEWALK = 182,

        [Description("Box of Sunlight")]
        SUNLIGHT_BOX = 184,

        [Description("Double Casting")]
        PF_DOUBLECASTING = 186,

        [Description("Maximum Overthrust/Power Thrust")]
        WS_OVERTHRUSTMAX = 188,

        [Description("Homunculus Avoid")]
        HOM_AVOID = 192,

        [Description("Shrink")]
        CR_SHRINK = 197,

        [Description("Sight Blaster")]
        WZ_SIGHTBLASTER = 198,

        [Description("Madness Canceller")]
        GS_MADNESSCANCEL = 203,

        [Description("Gatling Fever")]
        GS_GATLINGFEVER = 204,

        [Description("Cast-off Cicada Shell / Cicada Skin Shed")]
        NJ_UTSUSEMI = 206,

        [Description("Illusionary Shadow / Mirror Image")]
        NJ_BUNSINJYUTSU = 207,

        [Description("Ninja Aura")]
        NJ_NEN = 208,

        [Description("Adjustment")]
        GS_ADJUSTMENT = 209,

        [Description("Increase Accuracy")]
        GS_INCREASING = 210,

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
        FOOD_BASICAVOIDANCE = 247,

        [Description("Accuracy Scroll")]
        ACCURACY_SCROLL = 248,

        [Description("Field Manual 100%")]
        FIELD_MANUAL = 250,

        [Description("HE Bubblegum")]
        CASH_RECEIVEITEM = 252,

        FOOD_VIT_CASH = 273,

        [Description("Slow Cast")]
        NPC_SLOWCAST = 282,

        [Description("Critical Wound")]
        NPC_CRITICALWOUND = 286,

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
        AS_SPLASHER = 440,

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
        STONECURSE = 875,

        [Description("Frozen")]
        FROZEN = 876,

        [Description("Stun")]
        STUN = 877,

        [Description("Sleep")]
        SLEEP = 878,

        [Description("Stone Curse (initial stage)")]
        STONECURSE_ING = 880,

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
        //HR_SABERTHRUST = 105, // dupe of 105:LK_CONCENTRATION

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

        [Description("Light of Sun")]
        LIGHT_OF_SUN = 1036,

        [Description("Light of Star")]
        LIGHT_OF_STAR = 1037,

        [Description("Lunar Stance")]
        LUNAR_STANCE = 1038,

        [Description("Universal Stance")]
        UNIVERSAL_STANCE = 1039,

        [Description("Sun Stance")]
        SUN_STANCE = 1040,

        [Description("New Moon Kick")]
        NEW_MOON_KICK = 1042,

        [Description("Star Stance")]
        STAR_STANCE = 1043,

        [Description("Falling Star")]
        FALLING_STAR = 1048,

        [Description("Soul Collect")]
        SOUL_COLLECT = 1053,

        [Description("Soul Reaper")]
        SOUL_REAPER = 1054,

        [Description("Infinity Drink")]
        INFINITY_DRINK = 1065,

        [Description("Basílica")]
        HP_BASILICA = 1122,

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

        [Description("VIP Bonus")]
        VIP_BONUS = 1401,

        [Description("Kaite")]
        SL_KAITE = 1402,

        [Description("Box of Storms")]
        BOX_OF_STORMS = 1405,

        [Description("Volcano")]
        SA_VOLCANO = 1412,

        [Description("Deluge")]
        SA_DELUGE = 1413,

        [Description("Violent Gale")]
        SA_VIOLENTGALE = 1414,

        [Description("Land Protector")]
        SA_LANDPROTECTOR = 1415,

        [Description("Hallucination")]
        NPC_HALLUCINATION = 1416,

        [Description("Force Element (Earth)")]
        PD_ELEMENT_EARTH = 1423,

        [Description("Force Element (Wind)")]
        PD_ELEMENT_WIND = 1424,

        [Description("Force Element (Water)")]
        PD_ELEMENT_WATER = 1425,

        [Description("Force Element (Fire)")]
        PD_ELEMENT_FIRE = 1426,

        [Description("Force Element (Ghost)")]
        PD_ELEMENT_GHOST = 1427,

        [Description("Force Element (Shadow)")]
        PD_ELEMENT_SHADOW = 1428,

        [Description("Force Element (Holy)")]
        PD_ELEMENT_HOLY = 1429,

        [Description("Saber Parry")]
        JS_SABERPARRY = 1430,

        [Description("Force Persuasion")]
        JS_PERSUADE = 1431,

        [Description("Force Concentration")]
        JS_CONCENTRATE = 1432,

        [Description("Jedi Frenzy")]
        JE_FRENZY = 1433,

        [Description("Force Sacrifice")]
        JE_SACRIFICE = 1434,

        [Description("Force Levitate")]
        JE_LEVITATE = 1435,

        [Description("Jedi Stealth")]
        JE_STEALTH = 1437,

        [Description("Saber Thrust")]
        SI_SABERTHRUST = 1438,

        [Description("Cold Skin")]
        SI_COLDSKIN = 1439,

        [Description("Force Projection")]
        SI_PROJECTION = 1441,

        [Description("Greed Parry")]
        WS_GREEDPARRY = 1442,

        [Description("Energy Drink")]
        ENERGY_DRINK = 1443,

        [Description("Hunter's Potion")]
        HUNTERS_POTION = 1444,

        [Description("Max Potion")]
        MAX_POTION = 1445,

        [Description("Dash Juice")]
        DASH_JUICE = 1446,

        [Description("Demon Extract")]
        DEMON_EXTRACT = 1447,

        [Description("Stoneskin Extract")]
        STONESKIN_EXTRACT = 1449,

        [Description("Psychoserum")]
        PSYCHOSERUM = 1448,

        [Description("STR Tonic")]
        STR_TONIC = 1450,

        [Description("AGI Tonic")]
        AGI_TONIC = 1451,

        [Description("VIT Tonic")]
        VIT_TONIC = 1452,

        [Description("INT Tonic")]
        INT_TONIC = 1453,

        [Description("DEX Tonic")]
        DEX_TONIC = 1454,

        [Description("LUK Tonic")]
        LUK_TONIC = 1455,

        [Description("Halo Halo")]
        HALOHALO = 2011,

        STR_Biscuit_Stick = 2035,
        VIT_Biscuit_Stick = 2036,
        AGI_Biscuit_Stick = 2037,
        INT_Biscuit_Stick = 2038,
        DEX_Biscuit_Stick = 2039,
        LUK_Biscuit_Stick = 2040,

        [Description("Union of the Sun, Moon and Stars")]
        SG_FUSION = 2063,

        BOVINE = 2068,
        DRAGON = 2069,

        [Description("Solar, Lunar and Stellar Miracle")]
        SG_MIRACLE = 2113,
    }
}