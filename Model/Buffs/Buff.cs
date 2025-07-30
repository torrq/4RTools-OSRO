using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace _ORTools.Model
{
    internal class Buff
    {
        public string Name { get; set; }
        public EffectStatusIDs EffectStatusID { get; set; }
        public Bitmap Icon { get; set; }

        public Buff(string name, EffectStatusIDs effectStatus, Bitmap icon)
        {
            this.Name = name;
            this.EffectStatusID = effectStatus;
            this.Icon = icon;
        }

        #region Buff Lists

        private static class SkillBuffDefinitions
        {
            // ARCHER buffs
            public static readonly Dictionary<int, List<Buff>> ArcherServerBuffs = new Dictionary<int, List<Buff>>
            {
                [0] = new List<Buff>
                {
                    CreateBuff("Improve Concentration", "CONCENTRATION", "ac_concentration"),
                    CreateBuff("True Sight", "TRUESIGHT", "sn_sight"),
                    CreateBuff("Wind Walk", "WINDWALK", "sn_windwalk")
                }
            };

            // SWORDMAN buffs
            public static readonly Dictionary<int, List<Buff>> SwordmanServerBuffs = new Dictionary<int, List<Buff>>
            {
                [0] = new List<Buff> // Mid-rate
                {
                    CreateBuff("Endure", "ENDURE", "sm_endure"),
                    CreateBuff("Auto Berserk", "AUTOBERSERK", "sm_autoberserk"),
                    CreateBuff("Auto Guard", "AUTOGUARD", "cr_autoguard"),
                    CreateBuff("Reflect Shield", "REFLECTSHIELD", "cr_reflectshield"),
                    CreateBuff("Spear Quicken", "SPEARQUICKEN", "cr_spearquicken"),
                    CreateBuff("Defender", "DEFENDER", "cr_defender"),
                    CreateBuff("Concentration", "LKCONCENTRATION", "lk_concentration"),
                    CreateBuff("Berserk", "BERSERK", "lk_berserk"),
                    CreateBuff("Two-Hand Quicken", "TWOHANDQUICKEN", "mer_quicken"),
                    CreateBuff("Parry", "PARRYING", "ms_parrying"),
                    CreateBuff("Aura Blade", "AURABLADE", "lk_aurablade"),
                    CreateBuff("Shrink", "CR_SHRINK", "cr_shrink"),
                    CreateBuff("Magnum Break", "MAGNUM", "magnum"),
                    CreateBuff("One-Hand Quicken", "ONEHANDQUICKEN", "onehand"),
                    CreateBuff("Provoke", "PROVOKE", "provoke"),
                    CreateBuff("Providence", "PROVIDENCE", "providence")
                },
                [1] = new List<Buff> // High-rate
                {
                    CreateBuff("Endure", "ENDURE", "sm_endure"),
                    CreateBuff("Auto Berserk", "AUTOBERSERK", "sm_autoberserk"),
                    CreateBuff("Auto Guard", "AUTOGUARD", "cr_autoguard"),
                    CreateBuff("Reflect Shield", "REFLECTSHIELD", "cr_reflectshield"),
                    CreateBuff("Spear Quicken", "SPEARQUICKEN", "cr_spearquicken"),
                    CreateBuff("Defender", "DEFENDER", "cr_defender"),
                    CreateBuff("Concentration", "LKCONCENTRATION", "lk_concentration"),
                    CreateBuff("Berserk", "BERSERK", "lk_berserk"),
                    CreateBuff("Two-Hand Quicken", "TWOHANDQUICKEN", "mer_quicken"),
                    CreateBuff("Parry", "PARRYING", "ms_parrying"),
                    CreateBuff("Aura Blade", "AURABLADE", "lk_aurablade"),
                    CreateBuff("Shrink", "CR_SHRINK", "cr_shrink"),
                    CreateBuff("Magnum Break", "MAGNUM", "magnum"),
                    CreateBuff("One-Hand Quicken", "ONEHANDQUICKEN", "onehand"),
                    CreateBuff("Provoke", "PROVOKE", "provoke"),
                    CreateBuff("Providence", "PROVIDENCE", "providence"),
                    CreateBuff("Mana Shield", "MANA_SHIELD", "manashield")
                }
            };

            // MAGE buffs
            public static readonly Dictionary<int, List<Buff>> MageServerBuffs = new Dictionary<int, List<Buff>>
            {
                [0] = new List<Buff>
                {
                    CreateBuff("Energy Coat", "ENERGYCOAT", "mg_energycoat"),
                    CreateBuff("Sight Blaster", "SIGHTBLASTER", "wz_sightblaster"),
                    CreateBuff("Autospell", "AUTOSPELL", "sa_autospell"),
                    CreateBuff("Double Casting", "DOUBLECASTING", "pf_doublecasting"),
                    CreateBuff("Memorize", "MEMORIZE", "pf_memorize"),
                    CreateBuff("Amplify Magic Power / Mystical Amplification", "MYST_AMPLIFY", "amplify"),
                    CreateBuff("Mind Breaker", "MINDBREAKER", "mindbreaker")
                }
            };

            // MERCHANT buffs
            public static readonly Dictionary<int, List<Buff>> MerchantServerBuffs = new Dictionary<int, List<Buff>>
            {
                [0] = new List<Buff> // Mid-rate
                {
                    CreateBuff("Crazy Uproar", "CRAZY_UPROAR", "mc_loud"),
                    CreateBuff("Overthrust", "OVERTHRUST", "bs_overthrust"),
                    CreateBuff("Adrenaline Rush", "ADRENALINE", "bs_adrenaline"),
                    CreateBuff("Full Adrenaline Rush", "ADRENALINE2", "bs_adrenaline2"),
                    CreateBuff("Weapon Perfection", "WEAPONPERFECT", "bs_weaponperfect"),
                    CreateBuff("Maximize Power", "MAXIMIZE", "bs_maximize"),
                    CreateBuff("Cart Boost", "CARTBOOST", "ws_cartboost"),
                    CreateBuff("Meltdown", "MELTDOWN", "ws_meltdown"),
                    CreateBuff("Maximum Overthrust", "OVERTHRUSTMAX", "ws_overthrustmax"),
                    CreateBuff("Greed Parry", "GREED_PARRY", "ws_greedparry")
                },
                [1] = new List<Buff> // High-rate
                {
                    CreateBuff("Crazy Uproar", "CRAZY_UPROAR", "mc_loud"),
                    CreateBuff("Overthrust", "OVERTHRUST", "bs_overthrust"),
                    CreateBuff("Adrenaline Rush", "ADRENALINE", "bs_adrenaline"),
                    CreateBuff("Full Adrenaline Rush", "ADRENALINE2", "bs_adrenaline2"),
                    CreateBuff("Weapon Perfection", "WEAPONPERFECT", "bs_weaponperfect"),
                    CreateBuff("Maximize Power", "MAXIMIZE", "bs_maximize"),
                    CreateBuff("Cart Boost", "CARTBOOST", "ws_cartboost"),
                    CreateBuff("Meltdown", "MELTDOWN", "ws_meltdown"),
                    CreateBuff("Maximum Overthrust", "OVERTHRUSTMAX", "ws_overthrustmax"),
                    CreateBuff("Greed Parry", "RESIST_PROPERTY_FIRE", "ws_greedparry")
                }
            };

            // THIEF buffs
            public static readonly Dictionary<int, List<Buff>> ThiefServerBuffs = new Dictionary<int, List<Buff>>
            {
                [0] = new List<Buff> // Mid-rate
                {
                    CreateBuff("Poison React", "POISONREACT", "as_poisonreact"),
                    CreateBuff("Reject Sword", "SWORDREJECT", "st_rejectsword"),
                    CreateBuff("Preserve", "PRESERVE", "st_preserve"),
                    CreateBuff("Enchant Deadly Poison", "EDP", "asc_edp"),
                    CreateBuff("Hiding", "HIDING", "hiding"),
                    CreateBuff("Cloaking", "CLOAKING", "cloaking"),
                    CreateBuff("Chase Walk", "CHASEWALK", "chase_walk")
                },
                [1] = new List<Buff> // High-rate
                {
                    CreateBuff("Poison React", "POISONREACT", "as_poisonreact"),
                    CreateBuff("Reject Sword", "SWORDREJECT", "st_rejectsword"),
                    CreateBuff("Preserve", "PRESERVE", "st_preserve"),
                    CreateBuff("Enchant Deadly Poison", "EDP", "asc_edp"),
                    CreateBuff("Hiding", "HIDING", "hiding"),
                    CreateBuff("Cloaking", "CLOAKING", "cloaking"),
                    CreateBuff("Chase Walk", "CHASEWALK", "chase_walk"),
                    CreateBuff("Enchant Poison Armor", "ENCHANT_POISON_ARMOR", "enchantpoisonarmor")
                }
            };

            // ACOLYTE buffs
            public static readonly Dictionary<int, List<Buff>> AcolyteServerBuffs = new Dictionary<int, List<Buff>>
            {
                [0] = new List<Buff> // Mid-rate
                {
                    CreateBuff("Blessing", "BLESSING", "al_blessing1"),
                    CreateBuff("Increase Agility", "INC_AGI", "al_incagi1"),
                    CreateBuff("Gloria", "GLORIA", "pr_gloria"),
                    CreateBuff("Magnificat", "MAGNIFICAT", "pr_magnificat"),
                    CreateBuff("Angelus", "ANGELUS", "al_angelus"),
                    CreateBuff("Fury", "FURY", "fury"),
                    CreateBuff("Impositio Manus", "IMPOSITIO", "impositio_manus"),
                    CreateBuff("Basilica", "BASILICA", "basilica")
                },
                [1] = new List<Buff> // High-rate
                {
                    CreateBuff("Blessing", "BLESSING", "al_blessing1"),
                    CreateBuff("Increase Agility", "INC_AGI", "al_incagi1"),
                    CreateBuff("Gloria", "GLORIA", "pr_gloria"),
                    CreateBuff("Magnificat", "MAGNIFICAT", "pr_magnificat"),
                    CreateBuff("Angelus", "ANGELUS", "al_angelus"),
                    CreateBuff("Fury", "FURY", "fury"),
                    CreateBuff("Impositio Manus", "IMPOSITIO", "impositio_manus"),
                    CreateBuff("Basilica", "BASILICA", "basilica"),
                    CreateBuff("Refraction", "REFRACTION", "refraction"),
                    CreateBuff("Shallow Grave", "KAIZEL", "shallowgrave")
                }
            };

            // NINJA buffs
            public static readonly Dictionary<int, List<Buff>> NinjaServerBuffs = new Dictionary<int, List<Buff>>
            {
                [0] = new List<Buff>
                {
                    CreateBuff("Ninja Aura", "NINJA_AURA", "nj_nen"),
                    CreateBuff("Cast-off Cicada / Cicada SS", "CICADA_SKIN_SHED", "nj_utsusemi"),
                    CreateBuff("Illusion Shadow / Mirror Image", "MIRROR_IMAGE", "bunsinjyutsu")
                }
            };

            // TAEKWON buffs
            public static readonly Dictionary<int, List<Buff>> TaekwonServerBuffs = new Dictionary<int, List<Buff>>
            {
                [0] = new List<Buff>
                {
                    CreateBuff("Mild Wind (Earth)", "PROPERTYGROUND", "tk_mild_earth"),
                    CreateBuff("Mild Wind (Fire)", "PROPERTYFIRE", "tk_mild_fire"),
                    CreateBuff("Mild Wind (Water)", "PROPERTYWATER", "tk_mild_water"),
                    CreateBuff("Mild Wind (Wind)", "PROPERTYWIND", "tk_mild_wind"),
                    CreateBuff("Mild Wind (Ghost)", "PROPERTYTELEKINESIS", "tk_mild_ghost"),
                    CreateBuff("Mild Wind (Holy)", "ASPERSIO", "tk_mild_holy"),
                    CreateBuff("Mild Wind (Shadow)", "PROPERTYDARK", "tk_mild_shadow"),
                    CreateBuff("Tumbling", "DODGE_ON", "tumbling"),
                    CreateBuff("Solar, Lunar, and Stellar Warmth", "WARM", "sun_warm"),
                    CreateBuff("Comfort of the Sun", "SUN_COMFORT", "sun_comfort"),
                    CreateBuff("Comfort of the Moon", "MOON_COMFORT", "moon_comfort"),
                    CreateBuff("Comfort of the Stars", "STAR_COMFORT", "star_comfort"),
                    CreateBuff("Kaupe", "KAUPE", "kaupe"),
                    CreateBuff("Kaite", "KAITE", "kaite"),
                    CreateBuff("Kaizel", "KAIZEL", "kaizel"),
                    CreateBuff("Kaahi", "KAAHI", "kaahi")
                }
            };

            // GUNSLINGER buffs
            public static readonly Dictionary<int, List<Buff>> GunslingerServerBuffs = new Dictionary<int, List<Buff>>
            {
                [0] = new List<Buff>
                {
                    CreateBuff("Gatling Fever", "GATLINGFEVER", "gatling_fever"),
                    CreateBuff("Last Stand", "MADNESSCANCEL", "madnesscancel"),
                    CreateBuff("Adjustment", "ADJUSTMENT", "adjustment"),
                    CreateBuff("Increased Accuracy", "ACCURACY", "increase_accuracy")
                }
            };

            // PADAWAN buffs
            public static readonly Dictionary<int, List<Buff>> PadawanServerBuffs = new Dictionary<int, List<Buff>>
            {
                [1] = new List<Buff> // High-rate
                {
                    CreateBuff("Force Element (Earth)", "PROPERTYGROUND", "forceelement_earth"),
                    CreateBuff("Force Element (Wind)", "PROPERTYWIND", "forceelement_wind"),
                    CreateBuff("Force Element (Water)", "PROPERTYWATER", "forceelement_water"),
                    CreateBuff("Force Element (Fire)", "PROPERTYFIRE", "forceelement_fire"),
                    CreateBuff("Force Element (Ghost)", "PROPERTYTELEKINESIS", "forceelement_ghost"),
                    CreateBuff("Force Element (Shadow)", "PROPERTYDARK", "forceelement_shadow"),
                    CreateBuff("Force Element (Holy)", "ASPERSIO", "forceelement_holy"),
                    CreateBuff("Force Projection", "HR_PROJECTION", "hr_forceprojection"),
                    CreateBuff("Cold Skin", "RESIST_PROPERTY_WATER", "hr_coldskin"),
                    CreateBuff("Saber Parry", "HR_SABERPARRY", "hr_saberparry"),
                    CreateBuff("Force Concentration", "HR_FORCECONCENTRATE", "hr_forceconcentrate"),
                    CreateBuff("Saber Thrust", "LKCONCENTRATION", "hr_saberthrust"),
                    CreateBuff("Force Persuasion", "HR_FORCEPERSUASION", "hr_forcepersuasion"),
                    CreateBuff("Force Haste", "HR_FORCEHASTE", "forcehaste"),
                    CreateBuff("Force Sacrifice", "HR_FORCESACRIFICE", "hr_forcesacrifice"),
                    CreateBuff("Jedi Frenzy", "HR_JEDIFRENZY", "hr_jedifrenzy")
                },
                [0] = new List<Buff> // Mid-rate
                {
                    CreateBuff("Force Element (Earth)", "ELEMENT_EARTH", "forceelement_earth"),
                    CreateBuff("Force Element (Wind)", "ELEMENT_WIND", "forceelement_wind"),
                    CreateBuff("Force Element (Water)", "ELEMENT_WATER", "forceelement_water"),
                    CreateBuff("Force Element (Fire)", "ELEMENT_FIRE", "forceelement_fire"),
                    CreateBuff("Force Element (Ghost)", "ELEMENT_GHOST", "forceelement_ghost"),
                    CreateBuff("Force Element (Shadow)", "ELEMENT_SHADOW", "forceelement_shadow"),
                    CreateBuff("Force Element (Holy)", "ELEMENT_HOLY", "forceelement_holy"),
                    CreateBuff("Force Projection", "PROJECTION", "forceprojection"),
                    CreateBuff("Cold Skin", "COLDSKIN", "coldskin"),
                    CreateBuff("Saber Parry", "SABERPARRY", "saberparry"),
                    CreateBuff("Force Concentration", "FORCECONCENTRATE", "forceconcentrate"),
                    CreateBuff("Saber Thrust", "SABERTHRUST", "saberthrust"),
                    CreateBuff("Force Persuasion", "FORCEPERSUASION", "forcepersuasion"),
                    CreateBuff("Jedi Stealth", "JEDISTEALTH", "jedistealth"),
                    CreateBuff("Force Levitate", "FORCELEVITATE", "forcelevitate"),
                    CreateBuff("Jedi Frenzy", "JEDIFRENZY", "jedifrenzy"),
                    CreateBuff("Force Sacrifice", "FORCESACRIFICE", "forcesacrifice")
                }
            };
        }

        private static class ItemBuffDefinitions
        {
            // POTION buffs
            public static readonly List<Buff> PotionBuffs = new List<Buff>
            {
                CreateBuff("Concentration Potion", "CONCENTRATION_POTION", "concentration_potion"),
                CreateBuff("Awakening Potion", "AWAKENING_POTION", "awakening_potion"),
                CreateBuff("Berserk Potion", "BERSERK_POTION", "berserk_potion")
            };

            // ELEMENT buffs
            public static readonly List<Buff> ElementBuffs = new List<Buff>
            {
                CreateBuff("Fire Elemental Converter", "PROPERTYFIRE", "ele_fire_converter"),
                CreateBuff("Wind Elemental Converter", "PROPERTYWIND", "ele_wind_converter"),
                CreateBuff("Earth Elemental Converter", "PROPERTYGROUND", "ele_earth_converter"),
                CreateBuff("Water Elemental Converter", "PROPERTYWATER", "ele_water_converter"),
                CreateBuff("Box of Storms", "BOX_OF_STORMS", "boxofstorms"),
                CreateBuff("Cursed Water", "PROPERTYDARK", "cursed_water"),
                CreateBuff("Fireproof Potion", "RESIST_PROPERTY_FIRE", "fireproof"),
                CreateBuff("Coldproof Potion", "RESIST_PROPERTY_WATER", "coldproof"),
                CreateBuff("Thunderproof Potion", "RESIST_PROPERTY_WIND", "thunderproof"),
                CreateBuff("Earthproof Potion", "RESIST_PROPERTY_GROUND", "earthproof")
            };

            // FOOD buffs
            public static readonly List<Buff> FoodBuffs = new List<Buff>
            {
                CreateBuff("Steamed Tongue (STR+10)", "FOOD_STR", "strfood"),
                CreateBuff("Steamed Scorpion (AGI+10)", "FOOD_AGI", "agi_food"),
                CreateBuff("Stew of Immortality (VIT+10)", "FOOD_VIT", "vit_food"),
                CreateBuff("Dragon Breath Cocktail (INT+10)", "FOOD_INT", "int_food"),
                CreateBuff("Hwergelmir's Tonic (DEX+10)", "FOOD_DEX", "dex_food"),
                CreateBuff("Cooked Nine Tail's Tails (LUK+10)", "FOOD_LUK", "luk_food"),
                CreateBuff("Halo-Halo", "HALOHALO", "halohalo"),
                CreateBuff("Glass of Illusion", "GLASS_OF_ILLUSION", "Glass_Of_Illusion")
            };

            // BOX buffs
            public static readonly List<Buff> BoxBuffs = new List<Buff>
            {
                CreateBuff("Box of Drowsiness / Tasty W. Ration", "DROWSINESS_BOX", "drowsiness"),
                CreateBuff("Box of Resentment / Tasty P. Ration / Chewy Ricecake", "RESENTMENT_BOX", "resentment"),
                CreateBuff("Box of Sunlight", "SUNLIGHT_BOX", "sunbox"),
                CreateBuff("Box of Gloom", "CONCENTRATION", "gloom"),
                CreateBuff("Box of Thunder", "BOX_OF_THUNDER", "boxofthunder"),
                CreateBuff("Speed Potion", "SPEED_POT", "speedpotion"),
                CreateBuff("Anodyne", "ENDURE", "anodyne"),
                CreateBuff("Aloe Vera", "PROVOKE", "aloevera"),
                CreateBuff("Abrasive", "CRITICALPERCENT", "abrasive"),
                CreateBuff("Combat Pill", "COMBAT_PILL", "combat_pill"),
                CreateBuff("Enriched Celermine Juice", "ENRICH_CELERMINE_JUICE", "celermine"),
                CreateBuff("ASPD Potion Infinity", "ASPDPOTIONINFINITY", "poison")
            };

            // SCROLL buffs
            public static readonly List<Buff> ScrollBuffs = new List<Buff>
            {
                CreateBuff("Increase Agility Scroll", "INC_AGI", "al_incagi1"),
                CreateBuff("Bless Scroll", "BLESSING", "al_blessing1"),
                CreateBuff("Full Chemical Protection ", "PROTECTARMOR", "cr_fullprotection"),
                CreateBuff("Link Scroll", "SOULLINK", "sl_soullinker"),
                CreateBuff("Assumptio", "ASSUMPTIO", "assumptio"),
                CreateBuff("Flee Scroll / Spray of Flowers", "FLEE_SCROLL", "flee_scroll"),
                CreateBuff("Accuracy Scroll", "ACCURACY_SCROLL", "accuracy_scroll")
            };

            // ETC buffs
            public static readonly List<Buff> EtcBuffs = new List<Buff>
            {
                CreateBuff("VIP Ticket", "VIP_BONUS", "vip_ticket"),
                CreateBuff("Field Manual 100% / 300%", "FIELD_MANUAL", "fieldmanual"),
                CreateBuff("Bubble Gum / HE Bubble Gum", "CASH_RECEIVEITEM", "he_bubble_gum")
            };
        }

        private static class DebuffDefinitions
        {
            // Debuffs
            public static readonly List<Buff> Debuffs = new List<Buff>
            {
                CreateBuff("Bleeding", "BLEEDING", "bleeding"),
                CreateBuff("Burning", "BURNING", "burning"),
                CreateBuff("Chaos / Confusion", "CONFUSION", "chaos"),
                CreateBuff("Critical Wound", "CRITICALWOUND", "critical_wound"),
                CreateBuff("Curse", "CURSE", "curse"),
                CreateBuff("Decrease AGI", "DECREASE_AGI", "decrease_agi"),
                CreateBuff("Freezing", "FREEZING", "freezing"),
                CreateBuff("Frozen", "FROZEN", "frozen"),
                CreateBuff("Hallucination", "HALLUCINATION_DEBUFF", "hallucination"),
                CreateBuff("Poison", "POISON", "poison_status"),
                CreateBuff("Silence", "SILENCE", "silence"),
                CreateBuff("Sit", "SIT", "sit"),
                CreateBuff("Deep Sleep", "DEEP_SLEEP", "deep_sleep"),
                CreateBuff("Sleep", "SLEEP", "sleep"),
                CreateBuff("Slow Cast", "SLOW_CAST", "slow_cast"),
                CreateBuff("Stone Curse (initial stage)", "STONE", "stonecurse1"),
                CreateBuff("Stone Curse (petrified)", "STONEWAIT", "stonecurse2"),
                CreateBuff("Stun", "STUN", "stun")
            };
        }

        #endregion

        #region Public Getters
        //--------------------- SKILL BUFFS ------------------------------
        public static List<Buff> GetArcherBuffs() => BuffFactory.CreateServerSpecificBuffList(SkillBuffDefinitions.ArcherServerBuffs);
        public static List<Buff> GetSwordmanBuffs() => BuffFactory.CreateServerSpecificBuffList(SkillBuffDefinitions.SwordmanServerBuffs);
        public static List<Buff> GetMageBuffs() => BuffFactory.CreateServerSpecificBuffList(SkillBuffDefinitions.MageServerBuffs);
        public static List<Buff> GetMerchantBuffs() => BuffFactory.CreateServerSpecificBuffList(SkillBuffDefinitions.MerchantServerBuffs);
        public static List<Buff> GetThiefBuffs() => BuffFactory.CreateServerSpecificBuffList(SkillBuffDefinitions.ThiefServerBuffs);
        public static List<Buff> GetAcolyteBuffs() => BuffFactory.CreateServerSpecificBuffList(SkillBuffDefinitions.AcolyteServerBuffs);
        public static List<Buff> GetNinjaBuffs() => BuffFactory.CreateServerSpecificBuffList(SkillBuffDefinitions.NinjaServerBuffs);
        public static List<Buff> GetTaekwonBuffs() => BuffFactory.CreateServerSpecificBuffList(SkillBuffDefinitions.TaekwonServerBuffs);
        public static List<Buff> GetGunslingerBuffs() => BuffFactory.CreateServerSpecificBuffList(SkillBuffDefinitions.GunslingerServerBuffs);
        public static List<Buff> GetPadawanBuffs() => BuffFactory.CreateServerSpecificBuffList(SkillBuffDefinitions.PadawanServerBuffs);

        //--------------------- ITEM BUFFS ------------------------------
        public static List<Buff> GetPotionsBuffs() => new List<Buff>(ItemBuffDefinitions.PotionBuffs);
        public static List<Buff> GetElementBuffs() => new List<Buff>(ItemBuffDefinitions.ElementBuffs);
        public static List<Buff> GetFoodBuffs() => new List<Buff>(ItemBuffDefinitions.FoodBuffs);
        public static List<Buff> GetBoxBuffs() => new List<Buff>(ItemBuffDefinitions.BoxBuffs);
        public static List<Buff> GetScrollBuffs() => new List<Buff>(ItemBuffDefinitions.ScrollBuffs);
        public static List<Buff> GetEtcBuffs() => new List<Buff>(ItemBuffDefinitions.EtcBuffs);

        //--------------------- DEBUFFS ------------------------------
        public static List<Buff> GetDebuffs() => new List<Buff>(DebuffDefinitions.Debuffs);
        #endregion

        #region Processors

        /// <summary>
        /// A single, centralized factory method to create Buff instances.
        /// It handles parsing the effect status and loading the icon resource.
        /// </summary>
        private static Buff CreateBuff(string name, string effectStatusName, string iconName)
        {
            // Parse the effect status from its string name.
            if (!Enum.TryParse(effectStatusName, out EffectStatusIDs effect))
            {
                // Log an error if the EffectStatusID is invalid.
                DebugLogger.Error($"Invalid EffectStatusID '{effectStatusName}' for buff '{name}'.");
            }

            Bitmap icon = null;
            try
            {
                // Access the resource manager and get the icon by its string name.
                var rm = Resources.Media.Icons.ResourceManager;
                icon = rm.GetObject(iconName) as Bitmap;

                if (icon == null)
                {
                    DebugLogger.Error($"Could not resolve icon '{iconName}' for buff '{name}'.");
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, $"Exception loading icon '{iconName}' for buff '{name}': {ex.Message}");
            }

            return new Buff(name, effect, icon);
        }

        private static class BuffFactory
        {
            public static List<Buff> CreateServerSpecificBuffList(Dictionary<int, List<Buff>> serverBuffs)
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
        #endregion
    }
}