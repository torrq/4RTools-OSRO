using BruteGamingMacros.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace BruteGamingMacros.Core.Model
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

        //--------------------- SKILLS ------------------------------

        //Archer Skills
        public static List<Buff> GetArcherBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Improve Concentration", EffectStatusIDs.CONCENTRATION, Resources._4RTools.Icons.ac_concentration),
                new Buff("True Sight", EffectStatusIDs.TRUESIGHT, Resources._4RTools.Icons.sn_sight),
                new Buff("Wind Walk", EffectStatusIDs.WINDWALK, Resources._4RTools.Icons.sn_windwalk),
//                new Buff("Poem of Bragi", EffectStatusIDs.POEMBRAGI, Resources._4RTools.Icons.poem_of_bragi),
            };

            return skills;
        }

        //Swordsman Skills
        public static List<Buff> GetSwordmanBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Endure", EffectStatusIDs.ENDURE, Resources._4RTools.Icons.sm_endure),
                new Buff("Auto Berserk", EffectStatusIDs.AUTOBERSERK, Resources._4RTools.Icons.sm_autoberserk),
                new Buff("Auto Guard", EffectStatusIDs.AUTOGUARD, Resources._4RTools.Icons.cr_autoguard),
                new Buff("Reflect Shield", EffectStatusIDs.REFLECTSHIELD, Resources._4RTools.Icons.cr_reflectshield),
                new Buff("Spear Quicken", EffectStatusIDs.SPEARQUICKEN, Resources._4RTools.Icons.cr_spearquicken),
                new Buff("Defender", EffectStatusIDs.DEFENDER, Resources._4RTools.Icons.cr_defender),
                new Buff("Concentration", EffectStatusIDs.LKCONCENTRATION, Resources._4RTools.Icons.lk_concentration),
                new Buff("Berserk", EffectStatusIDs.BERSERK, Resources._4RTools.Icons.lk_berserk),
                new Buff("Two-Hand Quicken", EffectStatusIDs.TWOHANDQUICKEN, Resources._4RTools.Icons.mer_quicken),
                new Buff("Parry", EffectStatusIDs.PARRYING, Resources._4RTools.Icons.ms_parrying),
                new Buff("Aura Blade", EffectStatusIDs.AURABLADE, Resources._4RTools.Icons.lk_aurablade),
                new Buff("Shrink", EffectStatusIDs.CR_SHRINK, Resources._4RTools.Icons.cr_shrink),
                new Buff("Magnum Break", EffectStatusIDs.MAGNUM, Resources._4RTools.Icons.magnum),
                new Buff("One-Hand Quicken", EffectStatusIDs.ONEHANDQUICKEN, Resources._4RTools.Icons.onehand),
                new Buff("Provoke", EffectStatusIDs.PROVOKE, Resources._4RTools.Icons.provoke),
                new Buff("Providence", EffectStatusIDs.PROVIDENCE, Resources._4RTools.Icons.providence),

            };

            // HR server, 3rd Job: Royal Guard
            if (AppConfig.ServerMode == 1)
            {
                skills.Add(new Buff("Mana Shield", EffectStatusIDs.MANA_SHIELD, Resources._4RTools.Icons.manashield));
            }

            return skills;
        }

        //Mage Skills
        public static List<Buff> GetMageBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Energy Coat", EffectStatusIDs.ENERGYCOAT, Resources._4RTools.Icons.mg_energycoat),
                new Buff("Sight Blaster", EffectStatusIDs.SIGHTBLASTER, Resources._4RTools.Icons.wz_sightblaster),
                new Buff("Autospell", EffectStatusIDs.AUTOSPELL, Resources._4RTools.Icons.sa_autospell),
                new Buff("Double Casting", EffectStatusIDs.DOUBLECASTING, Resources._4RTools.Icons.pf_doublecasting),
                new Buff("Memorize", EffectStatusIDs.MEMORIZE, Resources._4RTools.Icons.pf_memorize),
                new Buff("Amplify Magic Power / Mystical Amplification", EffectStatusIDs.MYST_AMPLIFY, Resources._4RTools.Icons.amplify),
                new Buff("Mind Breaker", EffectStatusIDs.MINDBREAKER, Resources._4RTools.Icons.mindbreaker),
            };

            return skills;
        }

        //Merchant Skills
        public static List<Buff> GetMerchantBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Crazy Uproar", EffectStatusIDs.CRAZY_UPROAR, Resources._4RTools.Icons.mc_loud),
                new Buff("Overthrust", EffectStatusIDs.OVERTHRUST, Resources._4RTools.Icons.bs_overthrust),
                new Buff("Adrenaline Rush", EffectStatusIDs.ADRENALINE, Resources._4RTools.Icons.bs_adrenaline),
                new Buff("Full Adrenaline Rush", EffectStatusIDs.ADRENALINE2, Resources._4RTools.Icons.bs_adrenaline2),
                new Buff("Weapon Perfection", EffectStatusIDs.WEAPONPERFECT, Resources._4RTools.Icons.bs_weaponperfect),
                new Buff("Maximize Power", EffectStatusIDs.MAXIMIZE, Resources._4RTools.Icons.bs_maximize),
                new Buff("Cart Boost", EffectStatusIDs.CARTBOOST, Resources._4RTools.Icons.ws_cartboost),
                new Buff("Meltdown", EffectStatusIDs.MELTDOWN, Resources._4RTools.Icons.ws_meltdown),
                new Buff("Maximum Overthrust", EffectStatusIDs.OVERTHRUSTMAX, Resources._4RTools.Icons.ws_overthrustmax),
                new Buff("Greed Parry", EffectStatusIDs.GREED_PARRY, Resources._4RTools.Icons.ws_greedparry),

            };

            return skills;
        }

        //Thief Skills
        public static List<Buff> GetThiefBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Poison React", EffectStatusIDs.POISONREACT, Resources._4RTools.Icons.as_poisonreact),
                new Buff("Reject Sword", EffectStatusIDs.SWORDREJECT, Resources._4RTools.Icons.st_rejectsword),
                new Buff("Preserve", EffectStatusIDs.PRESERVE, Resources._4RTools.Icons.st_preserve),
                new Buff("Enchant Deadly Poison", EffectStatusIDs.EDP, Resources._4RTools.Icons.asc_edp),
                new Buff("Hiding", EffectStatusIDs.HIDING, Resources._4RTools.Icons.hiding),
                new Buff("Cloaking", EffectStatusIDs.CLOAKING, Resources._4RTools.Icons.cloaking),
                new Buff("Chase Walk", EffectStatusIDs.CHASEWALK, Resources._4RTools.Icons.chase_walk),
            };

            // HR server, 3rd Job: Guillotine Cross
            if (AppConfig.ServerMode == 1)
            {
                skills.Add(new Buff("Enchant Poison Armor", EffectStatusIDs.ENCHANT_POISON_ARMOR, Resources._4RTools.Icons.enchantpoisonarmor));
            }

            return skills;
        }

        //Acolyte Skills
        public static List<Buff> GetAcolyteBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Blessing", EffectStatusIDs.BLESSING, Resources._4RTools.Icons.al_blessing1),
                new Buff("Increase Agility", EffectStatusIDs.INC_AGI, Resources._4RTools.Icons.al_incagi1),
                new Buff("Gloria", EffectStatusIDs.GLORIA, Resources._4RTools.Icons.pr_gloria),
                new Buff("Magnificat", EffectStatusIDs.MAGNIFICAT, Resources._4RTools.Icons.pr_magnificat),
                new Buff("Angelus", EffectStatusIDs.ANGELUS, Resources._4RTools.Icons.al_angelus),
                new Buff("Fury", EffectStatusIDs.FURY, Resources._4RTools.Icons.fury),
                new Buff("Impositio Manus", EffectStatusIDs.IMPOSITIO, Resources._4RTools.Icons.impositio_manus),
                new Buff("Basilica", EffectStatusIDs.BASILICA, Resources._4RTools.Icons.basilica),

            };

            // HR server, 3rd Job: Arch Bishop
            if (AppConfig.ServerMode == 1)
            {
                skills.Add(new Buff("Refraction", EffectStatusIDs.REFRACTION, Resources._4RTools.Icons.refraction));
                skills.Add(new Buff("Shallow Grave", EffectStatusIDs.KAIZEL, Resources._4RTools.Icons.shallowgrave));
            }

            return skills;
        }

        //Ninja Skills
        public static List<Buff> GetNinjaBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Ninja Aura", EffectStatusIDs.NINJA_AURA, Resources._4RTools.Icons.nj_nen),
                new Buff("Cast-off Cicada Shell / Cicada Skin Shed", EffectStatusIDs.CICADA_SKIN_SHED, Resources._4RTools.Icons.nj_utsusemi),
                new Buff("Illusionary Shadow / Mirror Image", EffectStatusIDs.MIRROR_IMAGE, Resources._4RTools.Icons.bunsinjyutsu),
                // Possibly Renewal
                //new Buff("Izayoi", EffectStatusIDs.IZAYOI, Resources._4RTools.Icons.izayoi),
            };

            return skills;
        }

        //Taekwon Skills
        public static List<Buff> GetTaekwonBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Mild Wind (Earth)", EffectStatusIDs.PROPERTYGROUND, Resources._4RTools.Icons.tk_mild_earth),
                new Buff("Mild Wind (Fire)", EffectStatusIDs.PROPERTYFIRE, Resources._4RTools.Icons.tk_mild_fire),
                new Buff("Mild Wind (Water)", EffectStatusIDs.PROPERTYWATER, Resources._4RTools.Icons.tk_mild_water),
                new Buff("Mild Wind (Wind)", EffectStatusIDs.PROPERTYWIND, Resources._4RTools.Icons.tk_mild_wind),
                new Buff("Mild Wind (Ghost)", EffectStatusIDs.PROPERTYTELEKINESIS, Resources._4RTools.Icons.tk_mild_ghost),
                new Buff("Mild Wind (Holy)", EffectStatusIDs.ASPERSIO, Resources._4RTools.Icons.tk_mild_holy),
                new Buff("Mild Wind (Shadow)", EffectStatusIDs.PROPERTYDARK, Resources._4RTools.Icons.tk_mild_shadow),
                new Buff("Tumbling", EffectStatusIDs.DODGE_ON, Resources._4RTools.Icons.tumbling),
                new Buff("Solar, Lunar, and Stellar Miracle", EffectStatusIDs.MIRACLE, Resources._4RTools.Icons.solar_miracle),
                new Buff("Solar, Lunar, and Stellar Warmth", EffectStatusIDs.WARM, Resources._4RTools.Icons.sun_warm),
                new Buff("Comfort of the Sun", EffectStatusIDs.SUN_COMFORT, Resources._4RTools.Icons.sun_comfort),
                new Buff("Comfort of the Moon", EffectStatusIDs.MOON_COMFORT, Resources._4RTools.Icons.moon_comfort),
                new Buff("Comfort of the Stars", EffectStatusIDs.STAR_COMFORT, Resources._4RTools.Icons.star_comfort),
                new Buff("Union of the Sun, Moon, and Stars", EffectStatusIDs.FUSION, Resources._4RTools.Icons.fusion),
                new Buff("Kaupe", EffectStatusIDs.KAUPE, Resources._4RTools.Icons.kaupe),
                new Buff("Kaite", EffectStatusIDs.KAITE, Resources._4RTools.Icons.kaite),
                new Buff("Kaizel", EffectStatusIDs.KAIZEL, Resources._4RTools.Icons.kaizel),
                new Buff("Kaahi", EffectStatusIDs.KAAHI, Resources._4RTools.Icons.kaahi),
            };

            return skills;
        }


        public static List<Buff> GetGunsBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Gatling Fever", EffectStatusIDs.GATLINGFEVER, Resources._4RTools.Icons.gatling_fever),
                new Buff("Last Stand", EffectStatusIDs.MADNESSCANCEL, Resources._4RTools.Icons.madnesscancel),
                new Buff("Adjustment", EffectStatusIDs.ADJUSTMENT, Resources._4RTools.Icons.adjustment),
                new Buff("Increased Accuracy", EffectStatusIDs.ACCURACY, Resources._4RTools.Icons.increase_accuracy),
            };

            return skills;
        }

        // HR‑specific Padawan list (ServerMode == 1)
        private static readonly List<Buff> PadawanBuffsHR = new List<Buff>
        {
            new Buff("Force Element (Earth)",  EffectStatusIDs.PROPERTYGROUND,     Resources._4RTools.Icons.forceelement_earth),
            new Buff("Force Element (Wind)",   EffectStatusIDs.PROPERTYWIND,       Resources._4RTools.Icons.forceelement_wind),
            new Buff("Force Element (Water)",  EffectStatusIDs.PROPERTYWATER,      Resources._4RTools.Icons.forceelement_water),
            new Buff("Force Element (Fire)",   EffectStatusIDs.PROPERTYFIRE,       Resources._4RTools.Icons.forceelement_fire),
            new Buff("Force Element (Ghost)",  EffectStatusIDs.PROPERTYTELEKINESIS, Resources._4RTools.Icons.forceelement_ghost),
            new Buff("Force Element (Shadow)", EffectStatusIDs.PROPERTYDARK,       Resources._4RTools.Icons.forceelement_shadow),
            new Buff("Force Element (Holy)",   EffectStatusIDs.ASPERSIO,           Resources._4RTools.Icons.forceelement_holy),
            new Buff("Force Projection",       EffectStatusIDs.HR_PROJECTION,      Resources._4RTools.Icons.hr_forceprojection),
            new Buff("Cold Skin",              EffectStatusIDs.RESIST_PROPERTY_WATER, Resources._4RTools.Icons.hr_coldskin),
            new Buff("Saber Parry",            EffectStatusIDs.HR_SABERPARRY,      Resources._4RTools.Icons.hr_saberparry),
            new Buff("Force Concentration",    EffectStatusIDs.HR_FORCECONCENTRATE,Resources._4RTools.Icons.hr_forceconcentrate),
            new Buff("Saber Thrust",           EffectStatusIDs.LKCONCENTRATION,    Resources._4RTools.Icons.hr_saberthrust),
            new Buff("Force Persuasion",       EffectStatusIDs.HR_FORCEPERSUASION, Resources._4RTools.Icons.hr_forcepersuasion),
            new Buff("Force Haste",            EffectStatusIDs.HR_FORCEHASTE,      Resources._4RTools.Icons.forcehaste),
            new Buff("Force Sacrifice",        EffectStatusIDs.HR_FORCESACRIFICE,  Resources._4RTools.Icons.hr_forcesacrifice),
            new Buff("Jedi Frenzy",            EffectStatusIDs.HR_JEDIFRENZY,      Resources._4RTools.Icons.hr_jedifrenzy)
        };

        // MR / LR shared list (ServerMode == 0 or 2)
        private static readonly List<Buff> PadawanBuffsMR = new List<Buff>
        {
            new Buff("Force Element (Earth)",  EffectStatusIDs.ELEMENT_EARTH,   Resources._4RTools.Icons.forceelement_earth),
            new Buff("Force Element (Wind)",   EffectStatusIDs.ELEMENT_WIND,    Resources._4RTools.Icons.forceelement_wind),
            new Buff("Force Element (Water)",  EffectStatusIDs.ELEMENT_WATER,   Resources._4RTools.Icons.forceelement_water),
            new Buff("Force Element (Fire)",   EffectStatusIDs.ELEMENT_FIRE,    Resources._4RTools.Icons.forceelement_fire),
            new Buff("Force Element (Ghost)",  EffectStatusIDs.ELEMENT_GHOST,   Resources._4RTools.Icons.forceelement_ghost),
            new Buff("Force Element (Shadow)", EffectStatusIDs.ELEMENT_SHADOW,  Resources._4RTools.Icons.forceelement_shadow),
            new Buff("Force Element (Holy)",   EffectStatusIDs.ELEMENT_HOLY,    Resources._4RTools.Icons.forceelement_holy),
            new Buff("Force Projection",       EffectStatusIDs.PROJECTION,      Resources._4RTools.Icons.forceprojection),
            new Buff("Cold Skin",              EffectStatusIDs.COLDSKIN,        Resources._4RTools.Icons.coldskin),
            new Buff("Saber Parry",            EffectStatusIDs.SABERPARRY,      Resources._4RTools.Icons.saberparry),
            new Buff("Force Concentration",    EffectStatusIDs.FORCECONCENTRATE,Resources._4RTools.Icons.forceconcentrate),
            new Buff("Saber Thrust",           EffectStatusIDs.SABERTHRUST,     Resources._4RTools.Icons.saberthrust),
            new Buff("Force Persuasion",       EffectStatusIDs.FORCEPERSUASION, Resources._4RTools.Icons.forcepersuasion),
            new Buff("Jedi Stealth",           EffectStatusIDs.JEDISTEALTH,     Resources._4RTools.Icons.jedistealth),
            new Buff("Force Levitate",         EffectStatusIDs.FORCELEVITATE,   Resources._4RTools.Icons.forcelevitate),
            new Buff("Jedi Frenzy",            EffectStatusIDs.JEDIFRENZY,      Resources._4RTools.Icons.jedifrenzy),
            new Buff("Force Sacrifice",        EffectStatusIDs.FORCESACRIFICE,  Resources._4RTools.Icons.forcesacrifice)
        };

        public static List<Buff> GetPadawanBuffs()
        {

            List<Buff> skills;

            switch (AppConfig.ServerMode)   // 0 = MR, 1 = HR, 2 = LR
            {
                case 1:  // High‑rate
                    skills = PadawanBuffsHR;
                    break;

                case 0:  // Mid‑rate
                case 2:  // Low‑rate (currently same as MR)
                    skills = PadawanBuffsMR;
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported ServerMode value: {AppConfig.ServerMode}");
            }

            return skills;
        }

        //--------------------- Potions ------------------------------
        public static List<Buff> GetPotionsBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Concentration Potion", EffectStatusIDs.CONCENTRATION_POTION, Resources._4RTools.Icons.concentration_potion),
                new Buff("Awakening Potion", EffectStatusIDs.AWAKENING_POTION, Resources._4RTools.Icons.awakening_potion),
                new Buff("Berserk Potion", EffectStatusIDs.BERSERK_POTION, Resources._4RTools.Icons.berserk_potion),
            };

            return skills;
        }

        public static List<Buff> GetElementalsBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Fire Elemental Converter", EffectStatusIDs.PROPERTYFIRE, Resources._4RTools.Icons.ele_fire_converter),
                new Buff("Wind Elemental Converter", EffectStatusIDs.PROPERTYWIND, Resources._4RTools.Icons.ele_wind_converter),
                new Buff("Earth Elemental Converter", EffectStatusIDs.PROPERTYGROUND, Resources._4RTools.Icons.ele_earth_converter),
                new Buff("Water Elemental Converter", EffectStatusIDs.PROPERTYWATER, Resources._4RTools.Icons.ele_water_converter),
                new Buff("Box of Storms", EffectStatusIDs.BOX_OF_STORMS, Resources._4RTools.Icons.boxofstorms),
                //new Buff("Aspersio Scroll", EffectStatusIDs.ASPERSIO, Resources._4RTools.Icons.ele_holy_converter),
                new Buff("Cursed Water", EffectStatusIDs.PROPERTYDARK, Resources._4RTools.Icons.cursed_water),
                new Buff("Fireproof Potion", EffectStatusIDs.RESIST_PROPERTY_FIRE, Resources._4RTools.Icons.fireproof),
                new Buff("Coldproof Potion", EffectStatusIDs.RESIST_PROPERTY_WATER, Resources._4RTools.Icons.coldproof),
                new Buff("Thunderproof Potion", EffectStatusIDs.RESIST_PROPERTY_WIND, Resources._4RTools.Icons.thunderproof),
                new Buff("Earthproof Potion", EffectStatusIDs.RESIST_PROPERTY_GROUND, Resources._4RTools.Icons.earthproof),
            };

            return skills;
        }

        public static List<Buff> GetFoodBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Steamed Tongue (STR+10)", EffectStatusIDs.FOOD_STR, Resources._4RTools.Icons.strfood),
                new Buff("Steamed Scorpion (AGI+10)", EffectStatusIDs.FOOD_AGI, Resources._4RTools.Icons.agi_food),
                new Buff("Stew of Immortality (VIT+10)", EffectStatusIDs.FOOD_VIT, Resources._4RTools.Icons.vit_food),
                new Buff("Dragon Breath Cocktail (INT+10)", EffectStatusIDs.FOOD_INT, Resources._4RTools.Icons.int_food),
                new Buff("Hwergelmir's Tonic (DEX+10)", EffectStatusIDs.FOOD_DEX, Resources._4RTools.Icons.dex_food),
                new Buff("Cooked Nine Tail's Tails (LUK+10)", EffectStatusIDs.FOOD_LUK, Resources._4RTools.Icons.luk_food),
                new Buff("Halo-Halo", EffectStatusIDs.HALOHALO, Resources._4RTools.Icons.halohalo),
                new Buff("Glass of Illusion", EffectStatusIDs.GLASS_OF_ILLUSION, Resources._4RTools.Icons.Glass_Of_Illusion),

            };


            return skills;
        }

        public static List<Buff> GetBoxesBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Box of Drowsiness / Tasty White Ration", EffectStatusIDs.DROWSINESS_BOX, Resources._4RTools.Icons.drowsiness),
                new Buff("Box of Resentment / Tasty Pink Ration / Chewy Ricecake", EffectStatusIDs.RESENTMENT_BOX, Resources._4RTools.Icons.resentment),
                new Buff("Box of Sunlight", EffectStatusIDs.SUNLIGHT_BOX, Resources._4RTools.Icons.sunbox),
                new Buff("Box of Gloom", EffectStatusIDs.CONCENTRATION, Resources._4RTools.Icons.gloom),
                new Buff("Box of Thunder", EffectStatusIDs.BOX_OF_THUNDER, Resources._4RTools.Icons.boxofthunder),
                new Buff("Speed Potion", EffectStatusIDs.SPEED_POT, Resources._4RTools.Icons.speedpotion),
                new Buff("Anodyne", EffectStatusIDs.ENDURE, Resources._4RTools.Icons.anodyne),
                new Buff("Aloe Vera", EffectStatusIDs.PROVOKE, Resources._4RTools.Icons.aloevera),
                new Buff("Abrasive", EffectStatusIDs.CRITICALPERCENT, Resources._4RTools.Icons.abrasive),
                new Buff("Combat Pill", EffectStatusIDs.COMBAT_PILL, Resources._4RTools.Icons.combat_pill),
                new Buff("Enriched Celermine Juice", EffectStatusIDs.ENRICH_CELERMINE_JUICE, Resources._4RTools.Icons.celermine),
                new Buff("ASPD Potion Infinity", EffectStatusIDs.ASPDPOTIONINFINITY, Resources._4RTools.Icons.poison)
            };

            return skills;
        }

        public static List<Buff> GetScrollBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Increase Agility Scroll", EffectStatusIDs.INC_AGI, Resources._4RTools.Icons.al_incagi1),
                new Buff("Bless Scroll", EffectStatusIDs.BLESSING, Resources._4RTools.Icons.al_blessing1),
                new Buff("Full Chemical Protection (Scroll)", EffectStatusIDs.PROTECTARMOR, Resources._4RTools.Icons.cr_fullprotection),
                new Buff("Burnt Incense",  EffectStatusIDs.SPIRIT, Resources._4RTools.Icons.burnt_incense),
                new Buff("Link Scroll", EffectStatusIDs.SOULLINK, Resources._4RTools.Icons.sl_soullinker),
                new Buff("Assumptio",  EffectStatusIDs.ASSUMPTIO, Resources._4RTools.Icons.assumptio),
                new Buff("Flee Scroll / Spray of Flowers",  EffectStatusIDs.FLEE_SCROLL, Resources._4RTools.Icons.flee_scroll),
                new Buff("Accuracy Scroll",  EffectStatusIDs.ACCURACY_SCROLL, Resources._4RTools.Icons.accuracy_scroll),

            };

            return skills;
        }

        public static List<Buff> GetETCBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Field Manual 100% / 300%", EffectStatusIDs.FIELD_MANUAL, Resources._4RTools.Icons.fieldmanual),
                new Buff("Bubble Gum / HE Bubble Gum", EffectStatusIDs.CASH_RECEIVEITEM, Resources._4RTools.Icons.he_bubble_gum),

            };

            return skills;
        }
        //--------------------- DEBUFFS ------------------------------
        public static List<Buff> GetDebuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Bleeding", EffectStatusIDs.BLEEDING, Resources._4RTools.Icons.bleeding),
                new Buff("Burning", EffectStatusIDs.BURNING, Resources._4RTools.Icons.burning),
                new Buff("Chaos / Confusion", EffectStatusIDs.CONFUSION, Resources._4RTools.Icons.chaos),
                new Buff("Critical Wound", EffectStatusIDs.CRITICALWOUND, Resources._4RTools.Icons.critical_wound),
                new Buff("Curse", EffectStatusIDs.CURSE, Resources._4RTools.Icons.curse),
                new Buff("Decrease AGI", EffectStatusIDs.DECREASE_AGI, Resources._4RTools.Icons.decrease_agi),
                new Buff("Freezing", EffectStatusIDs.FREEZING, Resources._4RTools.Icons.freezing),
                new Buff("Frozen", EffectStatusIDs.FROZEN, Resources._4RTools.Icons.frozen),
                new Buff("Hallucination", EffectStatusIDs.HALLUCINATION_DEBUFF, Resources._4RTools.Icons.hallucination),
                new Buff("Poison", EffectStatusIDs.POISON, Resources._4RTools.Icons.poison_status),
                new Buff("Silence", EffectStatusIDs.SILENCE, Resources._4RTools.Icons.silence),
                new Buff("Sit", EffectStatusIDs.SIT, Resources._4RTools.Icons.sit),
                new Buff("Deep Sleep", EffectStatusIDs.DEEP_SLEEP, Resources._4RTools.Icons.deep_sleep),
                new Buff("Sleep", EffectStatusIDs.SLEEP, Resources._4RTools.Icons.sleep),
                new Buff("Slow Cast", EffectStatusIDs.SLOW_CAST, Resources._4RTools.Icons.slow_cast),
                new Buff("Stone Curse (initial stage)", EffectStatusIDs.STONE, Resources._4RTools.Icons.stonecurse1),
                new Buff("Stone Curse (petrified)", EffectStatusIDs.STONEWAIT, Resources._4RTools.Icons.stonecurse2),
                new Buff("Stun", EffectStatusIDs.STUN, Resources._4RTools.Icons.stun),
            };

            return skills;
        }

        //--------------------- WEIGHT ------------------------------
        public static List<Buff> GetWeightDebuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Overweight (50%)", EffectStatusIDs.WEIGHT50, Resources._4RTools.Icons.weight50),
                new Buff("Overweight (90%)", EffectStatusIDs.WEIGHT90, Resources._4RTools.Icons.weight90)
            };

            return skills;
        }

    }
}
