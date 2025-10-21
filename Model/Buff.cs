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
                new Buff("Improve Concentration", EffectStatusIDs.CONCENTRATION, BruteGamingMacros.Resources.BruteGaming.Icons.ac_concentration),
                new Buff("True Sight", EffectStatusIDs.TRUESIGHT, BruteGamingMacros.Resources.BruteGaming.Icons.sn_sight),
                new Buff("Wind Walk", EffectStatusIDs.WINDWALK, BruteGamingMacros.Resources.BruteGaming.Icons.sn_windwalk),
//                new Buff("Poem of Bragi", EffectStatusIDs.POEMBRAGI, BruteGamingMacros.Resources.BruteGaming.Icons.poem_of_bragi),
            };

            return skills;
        }

        //Swordsman Skills
        public static List<Buff> GetSwordmanBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Endure", EffectStatusIDs.ENDURE, BruteGamingMacros.Resources.BruteGaming.Icons.sm_endure),
                new Buff("Auto Berserk", EffectStatusIDs.AUTOBERSERK, BruteGamingMacros.Resources.BruteGaming.Icons.sm_autoberserk),
                new Buff("Auto Guard", EffectStatusIDs.AUTOGUARD, BruteGamingMacros.Resources.BruteGaming.Icons.cr_autoguard),
                new Buff("Reflect Shield", EffectStatusIDs.REFLECTSHIELD, BruteGamingMacros.Resources.BruteGaming.Icons.cr_reflectshield),
                new Buff("Spear Quicken", EffectStatusIDs.SPEARQUICKEN, BruteGamingMacros.Resources.BruteGaming.Icons.cr_spearquicken),
                new Buff("Defender", EffectStatusIDs.DEFENDER, BruteGamingMacros.Resources.BruteGaming.Icons.cr_defender),
                new Buff("Concentration", EffectStatusIDs.LKCONCENTRATION, BruteGamingMacros.Resources.BruteGaming.Icons.lk_concentration),
                new Buff("Berserk", EffectStatusIDs.BERSERK, BruteGamingMacros.Resources.BruteGaming.Icons.lk_berserk),
                new Buff("Two-Hand Quicken", EffectStatusIDs.TWOHANDQUICKEN, BruteGamingMacros.Resources.BruteGaming.Icons.mer_quicken),
                new Buff("Parry", EffectStatusIDs.PARRYING, BruteGamingMacros.Resources.BruteGaming.Icons.ms_parrying),
                new Buff("Aura Blade", EffectStatusIDs.AURABLADE, BruteGamingMacros.Resources.BruteGaming.Icons.lk_aurablade),
                new Buff("Shrink", EffectStatusIDs.CR_SHRINK, BruteGamingMacros.Resources.BruteGaming.Icons.cr_shrink),
                new Buff("Magnum Break", EffectStatusIDs.MAGNUM, BruteGamingMacros.Resources.BruteGaming.Icons.magnum),
                new Buff("One-Hand Quicken", EffectStatusIDs.ONEHANDQUICKEN, BruteGamingMacros.Resources.BruteGaming.Icons.onehand),
                new Buff("Provoke", EffectStatusIDs.PROVOKE, BruteGamingMacros.Resources.BruteGaming.Icons.provoke),
                new Buff("Providence", EffectStatusIDs.PROVIDENCE, BruteGamingMacros.Resources.BruteGaming.Icons.providence),

            };

            // HR server, 3rd Job: Royal Guard
            if (AppConfig.ServerMode == 1)
            {
                skills.Add(new Buff("Mana Shield", EffectStatusIDs.MANA_SHIELD, BruteGamingMacros.Resources.BruteGaming.Icons.manashield));
            }

            return skills;
        }

        //Mage Skills
        public static List<Buff> GetMageBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Energy Coat", EffectStatusIDs.ENERGYCOAT, BruteGamingMacros.Resources.BruteGaming.Icons.mg_energycoat),
                new Buff("Sight Blaster", EffectStatusIDs.SIGHTBLASTER, BruteGamingMacros.Resources.BruteGaming.Icons.wz_sightblaster),
                new Buff("Autospell", EffectStatusIDs.AUTOSPELL, BruteGamingMacros.Resources.BruteGaming.Icons.sa_autospell),
                new Buff("Double Casting", EffectStatusIDs.DOUBLECASTING, BruteGamingMacros.Resources.BruteGaming.Icons.pf_doublecasting),
                new Buff("Memorize", EffectStatusIDs.MEMORIZE, BruteGamingMacros.Resources.BruteGaming.Icons.pf_memorize),
                new Buff("Amplify Magic Power / Mystical Amplification", EffectStatusIDs.MYST_AMPLIFY, BruteGamingMacros.Resources.BruteGaming.Icons.amplify),
                new Buff("Mind Breaker", EffectStatusIDs.MINDBREAKER, BruteGamingMacros.Resources.BruteGaming.Icons.mindbreaker),
            };

            return skills;
        }

        //Merchant Skills
        public static List<Buff> GetMerchantBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Crazy Uproar", EffectStatusIDs.CRAZY_UPROAR, BruteGamingMacros.Resources.BruteGaming.Icons.mc_loud),
                new Buff("Overthrust", EffectStatusIDs.OVERTHRUST, BruteGamingMacros.Resources.BruteGaming.Icons.bs_overthrust),
                new Buff("Adrenaline Rush", EffectStatusIDs.ADRENALINE, BruteGamingMacros.Resources.BruteGaming.Icons.bs_adrenaline),
                new Buff("Full Adrenaline Rush", EffectStatusIDs.ADRENALINE2, BruteGamingMacros.Resources.BruteGaming.Icons.bs_adrenaline2),
                new Buff("Weapon Perfection", EffectStatusIDs.WEAPONPERFECT, BruteGamingMacros.Resources.BruteGaming.Icons.bs_weaponperfect),
                new Buff("Maximize Power", EffectStatusIDs.MAXIMIZE, BruteGamingMacros.Resources.BruteGaming.Icons.bs_maximize),
                new Buff("Cart Boost", EffectStatusIDs.CARTBOOST, BruteGamingMacros.Resources.BruteGaming.Icons.ws_cartboost),
                new Buff("Meltdown", EffectStatusIDs.MELTDOWN, BruteGamingMacros.Resources.BruteGaming.Icons.ws_meltdown),
                new Buff("Maximum Overthrust", EffectStatusIDs.OVERTHRUSTMAX, BruteGamingMacros.Resources.BruteGaming.Icons.ws_overthrustmax),
                new Buff("Greed Parry", EffectStatusIDs.GREED_PARRY, BruteGamingMacros.Resources.BruteGaming.Icons.ws_greedparry),

            };

            return skills;
        }

        //Thief Skills
        public static List<Buff> GetThiefBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Poison React", EffectStatusIDs.POISONREACT, BruteGamingMacros.Resources.BruteGaming.Icons.as_poisonreact),
                new Buff("Reject Sword", EffectStatusIDs.SWORDREJECT, BruteGamingMacros.Resources.BruteGaming.Icons.st_rejectsword),
                new Buff("Preserve", EffectStatusIDs.PRESERVE, BruteGamingMacros.Resources.BruteGaming.Icons.st_preserve),
                new Buff("Enchant Deadly Poison", EffectStatusIDs.EDP, BruteGamingMacros.Resources.BruteGaming.Icons.asc_edp),
                new Buff("Hiding", EffectStatusIDs.HIDING, BruteGamingMacros.Resources.BruteGaming.Icons.hiding),
                new Buff("Cloaking", EffectStatusIDs.CLOAKING, BruteGamingMacros.Resources.BruteGaming.Icons.cloaking),
                new Buff("Chase Walk", EffectStatusIDs.CHASEWALK, BruteGamingMacros.Resources.BruteGaming.Icons.chase_walk),
            };

            // HR server, 3rd Job: Guillotine Cross
            if (AppConfig.ServerMode == 1)
            {
                skills.Add(new Buff("Enchant Poison Armor", EffectStatusIDs.ENCHANT_POISON_ARMOR, BruteGamingMacros.Resources.BruteGaming.Icons.enchantpoisonarmor));
            }

            return skills;
        }

        //Acolyte Skills
        public static List<Buff> GetAcolyteBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Blessing", EffectStatusIDs.BLESSING, BruteGamingMacros.Resources.BruteGaming.Icons.al_blessing1),
                new Buff("Increase Agility", EffectStatusIDs.INC_AGI, BruteGamingMacros.Resources.BruteGaming.Icons.al_incagi1),
                new Buff("Gloria", EffectStatusIDs.GLORIA, BruteGamingMacros.Resources.BruteGaming.Icons.pr_gloria),
                new Buff("Magnificat", EffectStatusIDs.MAGNIFICAT, BruteGamingMacros.Resources.BruteGaming.Icons.pr_magnificat),
                new Buff("Angelus", EffectStatusIDs.ANGELUS, BruteGamingMacros.Resources.BruteGaming.Icons.al_angelus),
                new Buff("Fury", EffectStatusIDs.FURY, BruteGamingMacros.Resources.BruteGaming.Icons.fury),
                new Buff("Impositio Manus", EffectStatusIDs.IMPOSITIO, BruteGamingMacros.Resources.BruteGaming.Icons.impositio_manus),
                new Buff("Basilica", EffectStatusIDs.BASILICA, BruteGamingMacros.Resources.BruteGaming.Icons.basilica),

            };

            // HR server, 3rd Job: Arch Bishop
            if (AppConfig.ServerMode == 1)
            {
                skills.Add(new Buff("Refraction", EffectStatusIDs.REFRACTION, BruteGamingMacros.Resources.BruteGaming.Icons.refraction));
                skills.Add(new Buff("Shallow Grave", EffectStatusIDs.KAIZEL, BruteGamingMacros.Resources.BruteGaming.Icons.shallowgrave));
            }

            return skills;
        }

        //Ninja Skills
        public static List<Buff> GetNinjaBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Ninja Aura", EffectStatusIDs.NINJA_AURA, BruteGamingMacros.Resources.BruteGaming.Icons.nj_nen),
                new Buff("Cast-off Cicada Shell / Cicada Skin Shed", EffectStatusIDs.CICADA_SKIN_SHED, BruteGamingMacros.Resources.BruteGaming.Icons.nj_utsusemi),
                new Buff("Illusionary Shadow / Mirror Image", EffectStatusIDs.MIRROR_IMAGE, BruteGamingMacros.Resources.BruteGaming.Icons.bunsinjyutsu),
                // Possibly Renewal
                //new Buff("Izayoi", EffectStatusIDs.IZAYOI, BruteGamingMacros.Resources.BruteGaming.Icons.izayoi),
            };

            return skills;
        }

        //Taekwon Skills
        public static List<Buff> GetTaekwonBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Mild Wind (Earth)", EffectStatusIDs.PROPERTYGROUND, BruteGamingMacros.Resources.BruteGaming.Icons.tk_mild_earth),
                new Buff("Mild Wind (Fire)", EffectStatusIDs.PROPERTYFIRE, BruteGamingMacros.Resources.BruteGaming.Icons.tk_mild_fire),
                new Buff("Mild Wind (Water)", EffectStatusIDs.PROPERTYWATER, BruteGamingMacros.Resources.BruteGaming.Icons.tk_mild_water),
                new Buff("Mild Wind (Wind)", EffectStatusIDs.PROPERTYWIND, BruteGamingMacros.Resources.BruteGaming.Icons.tk_mild_wind),
                new Buff("Mild Wind (Ghost)", EffectStatusIDs.PROPERTYTELEKINESIS, BruteGamingMacros.Resources.BruteGaming.Icons.tk_mild_ghost),
                new Buff("Mild Wind (Holy)", EffectStatusIDs.ASPERSIO, BruteGamingMacros.Resources.BruteGaming.Icons.tk_mild_holy),
                new Buff("Mild Wind (Shadow)", EffectStatusIDs.PROPERTYDARK, BruteGamingMacros.Resources.BruteGaming.Icons.tk_mild_shadow),
                new Buff("Tumbling", EffectStatusIDs.DODGE_ON, BruteGamingMacros.Resources.BruteGaming.Icons.tumbling),
                new Buff("Solar, Lunar, and Stellar Miracle", EffectStatusIDs.MIRACLE, BruteGamingMacros.Resources.BruteGaming.Icons.solar_miracle),
                new Buff("Solar, Lunar, and Stellar Warmth", EffectStatusIDs.WARM, BruteGamingMacros.Resources.BruteGaming.Icons.sun_warm),
                new Buff("Comfort of the Sun", EffectStatusIDs.SUN_COMFORT, BruteGamingMacros.Resources.BruteGaming.Icons.sun_comfort),
                new Buff("Comfort of the Moon", EffectStatusIDs.MOON_COMFORT, BruteGamingMacros.Resources.BruteGaming.Icons.moon_comfort),
                new Buff("Comfort of the Stars", EffectStatusIDs.STAR_COMFORT, BruteGamingMacros.Resources.BruteGaming.Icons.star_comfort),
                new Buff("Union of the Sun, Moon, and Stars", EffectStatusIDs.FUSION, BruteGamingMacros.Resources.BruteGaming.Icons.fusion),
                new Buff("Kaupe", EffectStatusIDs.KAUPE, BruteGamingMacros.Resources.BruteGaming.Icons.kaupe),
                new Buff("Kaite", EffectStatusIDs.KAITE, BruteGamingMacros.Resources.BruteGaming.Icons.kaite),
                new Buff("Kaizel", EffectStatusIDs.KAIZEL, BruteGamingMacros.Resources.BruteGaming.Icons.kaizel),
                new Buff("Kaahi", EffectStatusIDs.KAAHI, BruteGamingMacros.Resources.BruteGaming.Icons.kaahi),
            };

            return skills;
        }


        public static List<Buff> GetGunsBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Gatling Fever", EffectStatusIDs.GATLINGFEVER, BruteGamingMacros.Resources.BruteGaming.Icons.gatling_fever),
                new Buff("Last Stand", EffectStatusIDs.MADNESSCANCEL, BruteGamingMacros.Resources.BruteGaming.Icons.madnesscancel),
                new Buff("Adjustment", EffectStatusIDs.ADJUSTMENT, BruteGamingMacros.Resources.BruteGaming.Icons.adjustment),
                new Buff("Increased Accuracy", EffectStatusIDs.ACCURACY, BruteGamingMacros.Resources.BruteGaming.Icons.increase_accuracy),
            };

            return skills;
        }

        // HR‑specific Padawan list (ServerMode == 1)
        private static readonly List<Buff> PadawanBuffsHR = new List<Buff>
        {
            new Buff("Force Element (Earth)",  EffectStatusIDs.PROPERTYGROUND,     BruteGamingMacros.Resources.BruteGaming.Icons.forceelement_earth),
            new Buff("Force Element (Wind)",   EffectStatusIDs.PROPERTYWIND,       BruteGamingMacros.Resources.BruteGaming.Icons.forceelement_wind),
            new Buff("Force Element (Water)",  EffectStatusIDs.PROPERTYWATER,      BruteGamingMacros.Resources.BruteGaming.Icons.forceelement_water),
            new Buff("Force Element (Fire)",   EffectStatusIDs.PROPERTYFIRE,       BruteGamingMacros.Resources.BruteGaming.Icons.forceelement_fire),
            new Buff("Force Element (Ghost)",  EffectStatusIDs.PROPERTYTELEKINESIS, BruteGamingMacros.Resources.BruteGaming.Icons.forceelement_ghost),
            new Buff("Force Element (Shadow)", EffectStatusIDs.PROPERTYDARK,       BruteGamingMacros.Resources.BruteGaming.Icons.forceelement_shadow),
            new Buff("Force Element (Holy)",   EffectStatusIDs.ASPERSIO,           BruteGamingMacros.Resources.BruteGaming.Icons.forceelement_holy),
            new Buff("Force Projection",       EffectStatusIDs.HR_PROJECTION,      BruteGamingMacros.Resources.BruteGaming.Icons.hr_forceprojection),
            new Buff("Cold Skin",              EffectStatusIDs.RESIST_PROPERTY_WATER, BruteGamingMacros.Resources.BruteGaming.Icons.hr_coldskin),
            new Buff("Saber Parry",            EffectStatusIDs.HR_SABERPARRY,      BruteGamingMacros.Resources.BruteGaming.Icons.hr_saberparry),
            new Buff("Force Concentration",    EffectStatusIDs.HR_FORCECONCENTRATE,BruteGamingMacros.Resources.BruteGaming.Icons.hr_forceconcentrate),
            new Buff("Saber Thrust",           EffectStatusIDs.LKCONCENTRATION,    BruteGamingMacros.Resources.BruteGaming.Icons.hr_saberthrust),
            new Buff("Force Persuasion",       EffectStatusIDs.HR_FORCEPERSUASION, BruteGamingMacros.Resources.BruteGaming.Icons.hr_forcepersuasion),
            new Buff("Force Haste",            EffectStatusIDs.HR_FORCEHASTE,      BruteGamingMacros.Resources.BruteGaming.Icons.forcehaste),
            new Buff("Force Sacrifice",        EffectStatusIDs.HR_FORCESACRIFICE,  BruteGamingMacros.Resources.BruteGaming.Icons.hr_forcesacrifice),
            new Buff("Jedi Frenzy",            EffectStatusIDs.HR_JEDIFRENZY,      BruteGamingMacros.Resources.BruteGaming.Icons.hr_jedifrenzy)
        };

        // MR / LR shared list (ServerMode == 0 or 2)
        private static readonly List<Buff> PadawanBuffsMR = new List<Buff>
        {
            new Buff("Force Element (Earth)",  EffectStatusIDs.ELEMENT_EARTH,   BruteGamingMacros.Resources.BruteGaming.Icons.forceelement_earth),
            new Buff("Force Element (Wind)",   EffectStatusIDs.ELEMENT_WIND,    BruteGamingMacros.Resources.BruteGaming.Icons.forceelement_wind),
            new Buff("Force Element (Water)",  EffectStatusIDs.ELEMENT_WATER,   BruteGamingMacros.Resources.BruteGaming.Icons.forceelement_water),
            new Buff("Force Element (Fire)",   EffectStatusIDs.ELEMENT_FIRE,    BruteGamingMacros.Resources.BruteGaming.Icons.forceelement_fire),
            new Buff("Force Element (Ghost)",  EffectStatusIDs.ELEMENT_GHOST,   BruteGamingMacros.Resources.BruteGaming.Icons.forceelement_ghost),
            new Buff("Force Element (Shadow)", EffectStatusIDs.ELEMENT_SHADOW,  BruteGamingMacros.Resources.BruteGaming.Icons.forceelement_shadow),
            new Buff("Force Element (Holy)",   EffectStatusIDs.ELEMENT_HOLY,    BruteGamingMacros.Resources.BruteGaming.Icons.forceelement_holy),
            new Buff("Force Projection",       EffectStatusIDs.PROJECTION,      BruteGamingMacros.Resources.BruteGaming.Icons.forceprojection),
            new Buff("Cold Skin",              EffectStatusIDs.COLDSKIN,        BruteGamingMacros.Resources.BruteGaming.Icons.coldskin),
            new Buff("Saber Parry",            EffectStatusIDs.SABERPARRY,      BruteGamingMacros.Resources.BruteGaming.Icons.saberparry),
            new Buff("Force Concentration",    EffectStatusIDs.FORCECONCENTRATE,BruteGamingMacros.Resources.BruteGaming.Icons.forceconcentrate),
            new Buff("Saber Thrust",           EffectStatusIDs.SABERTHRUST,     BruteGamingMacros.Resources.BruteGaming.Icons.saberthrust),
            new Buff("Force Persuasion",       EffectStatusIDs.FORCEPERSUASION, BruteGamingMacros.Resources.BruteGaming.Icons.forcepersuasion),
            new Buff("Jedi Stealth",           EffectStatusIDs.JEDISTEALTH,     BruteGamingMacros.Resources.BruteGaming.Icons.jedistealth),
            new Buff("Force Levitate",         EffectStatusIDs.FORCELEVITATE,   BruteGamingMacros.Resources.BruteGaming.Icons.forcelevitate),
            new Buff("Jedi Frenzy",            EffectStatusIDs.JEDIFRENZY,      BruteGamingMacros.Resources.BruteGaming.Icons.jedifrenzy),
            new Buff("Force Sacrifice",        EffectStatusIDs.FORCESACRIFICE,  BruteGamingMacros.Resources.BruteGaming.Icons.forcesacrifice)
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
                new Buff("Concentration Potion", EffectStatusIDs.CONCENTRATION_POTION, BruteGamingMacros.Resources.BruteGaming.Icons.concentration_potion),
                new Buff("Awakening Potion", EffectStatusIDs.AWAKENING_POTION, BruteGamingMacros.Resources.BruteGaming.Icons.awakening_potion),
                new Buff("Berserk Potion", EffectStatusIDs.BERSERK_POTION, BruteGamingMacros.Resources.BruteGaming.Icons.berserk_potion),
            };

            return skills;
        }

        public static List<Buff> GetElementalsBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Fire Elemental Converter", EffectStatusIDs.PROPERTYFIRE, BruteGamingMacros.Resources.BruteGaming.Icons.ele_fire_converter),
                new Buff("Wind Elemental Converter", EffectStatusIDs.PROPERTYWIND, BruteGamingMacros.Resources.BruteGaming.Icons.ele_wind_converter),
                new Buff("Earth Elemental Converter", EffectStatusIDs.PROPERTYGROUND, BruteGamingMacros.Resources.BruteGaming.Icons.ele_earth_converter),
                new Buff("Water Elemental Converter", EffectStatusIDs.PROPERTYWATER, BruteGamingMacros.Resources.BruteGaming.Icons.ele_water_converter),
                new Buff("Box of Storms", EffectStatusIDs.BOX_OF_STORMS, BruteGamingMacros.Resources.BruteGaming.Icons.boxofstorms),
                //new Buff("Aspersio Scroll", EffectStatusIDs.ASPERSIO, BruteGamingMacros.Resources.BruteGaming.Icons.ele_holy_converter),
                new Buff("Cursed Water", EffectStatusIDs.PROPERTYDARK, BruteGamingMacros.Resources.BruteGaming.Icons.cursed_water),
                new Buff("Fireproof Potion", EffectStatusIDs.RESIST_PROPERTY_FIRE, BruteGamingMacros.Resources.BruteGaming.Icons.fireproof),
                new Buff("Coldproof Potion", EffectStatusIDs.RESIST_PROPERTY_WATER, BruteGamingMacros.Resources.BruteGaming.Icons.coldproof),
                new Buff("Thunderproof Potion", EffectStatusIDs.RESIST_PROPERTY_WIND, BruteGamingMacros.Resources.BruteGaming.Icons.thunderproof),
                new Buff("Earthproof Potion", EffectStatusIDs.RESIST_PROPERTY_GROUND, BruteGamingMacros.Resources.BruteGaming.Icons.earthproof),
            };

            return skills;
        }

        public static List<Buff> GetFoodBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Steamed Tongue (STR+10)", EffectStatusIDs.FOOD_STR, BruteGamingMacros.Resources.BruteGaming.Icons.strfood),
                new Buff("Steamed Scorpion (AGI+10)", EffectStatusIDs.FOOD_AGI, BruteGamingMacros.Resources.BruteGaming.Icons.agi_food),
                new Buff("Stew of Immortality (VIT+10)", EffectStatusIDs.FOOD_VIT, BruteGamingMacros.Resources.BruteGaming.Icons.vit_food),
                new Buff("Dragon Breath Cocktail (INT+10)", EffectStatusIDs.FOOD_INT, BruteGamingMacros.Resources.BruteGaming.Icons.int_food),
                new Buff("Hwergelmir's Tonic (DEX+10)", EffectStatusIDs.FOOD_DEX, BruteGamingMacros.Resources.BruteGaming.Icons.dex_food),
                new Buff("Cooked Nine Tail's Tails (LUK+10)", EffectStatusIDs.FOOD_LUK, BruteGamingMacros.Resources.BruteGaming.Icons.luk_food),
                new Buff("Halo-Halo", EffectStatusIDs.HALOHALO, BruteGamingMacros.Resources.BruteGaming.Icons.halohalo),
                new Buff("Glass of Illusion", EffectStatusIDs.GLASS_OF_ILLUSION, BruteGamingMacros.Resources.BruteGaming.Icons.Glass_Of_Illusion),

            };


            return skills;
        }

        public static List<Buff> GetBoxesBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Box of Drowsiness / Tasty White Ration", EffectStatusIDs.DROWSINESS_BOX, BruteGamingMacros.Resources.BruteGaming.Icons.drowsiness),
                new Buff("Box of Resentment / Tasty Pink Ration / Chewy Ricecake", EffectStatusIDs.RESENTMENT_BOX, BruteGamingMacros.Resources.BruteGaming.Icons.resentment),
                new Buff("Box of Sunlight", EffectStatusIDs.SUNLIGHT_BOX, BruteGamingMacros.Resources.BruteGaming.Icons.sunbox),
                new Buff("Box of Gloom", EffectStatusIDs.CONCENTRATION, BruteGamingMacros.Resources.BruteGaming.Icons.gloom),
                new Buff("Box of Thunder", EffectStatusIDs.BOX_OF_THUNDER, BruteGamingMacros.Resources.BruteGaming.Icons.boxofthunder),
                new Buff("Speed Potion", EffectStatusIDs.SPEED_POT, BruteGamingMacros.Resources.BruteGaming.Icons.speedpotion),
                new Buff("Anodyne", EffectStatusIDs.ENDURE, BruteGamingMacros.Resources.BruteGaming.Icons.anodyne),
                new Buff("Aloe Vera", EffectStatusIDs.PROVOKE, BruteGamingMacros.Resources.BruteGaming.Icons.aloevera),
                new Buff("Abrasive", EffectStatusIDs.CRITICALPERCENT, BruteGamingMacros.Resources.BruteGaming.Icons.abrasive),
                new Buff("Combat Pill", EffectStatusIDs.COMBAT_PILL, BruteGamingMacros.Resources.BruteGaming.Icons.combat_pill),
                new Buff("Enriched Celermine Juice", EffectStatusIDs.ENRICH_CELERMINE_JUICE, BruteGamingMacros.Resources.BruteGaming.Icons.celermine),
                new Buff("ASPD Potion Infinity", EffectStatusIDs.ASPDPOTIONINFINITY, BruteGamingMacros.Resources.BruteGaming.Icons.poison)
            };

            return skills;
        }

        public static List<Buff> GetScrollBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Increase Agility Scroll", EffectStatusIDs.INC_AGI, BruteGamingMacros.Resources.BruteGaming.Icons.al_incagi1),
                new Buff("Bless Scroll", EffectStatusIDs.BLESSING, BruteGamingMacros.Resources.BruteGaming.Icons.al_blessing1),
                new Buff("Full Chemical Protection (Scroll)", EffectStatusIDs.PROTECTARMOR, BruteGamingMacros.Resources.BruteGaming.Icons.cr_fullprotection),
                new Buff("Burnt Incense",  EffectStatusIDs.SPIRIT, BruteGamingMacros.Resources.BruteGaming.Icons.burnt_incense),
                new Buff("Link Scroll", EffectStatusIDs.SOULLINK, BruteGamingMacros.Resources.BruteGaming.Icons.sl_soullinker),
                new Buff("Assumptio",  EffectStatusIDs.ASSUMPTIO, BruteGamingMacros.Resources.BruteGaming.Icons.assumptio),
                new Buff("Flee Scroll / Spray of Flowers",  EffectStatusIDs.FLEE_SCROLL, BruteGamingMacros.Resources.BruteGaming.Icons.flee_scroll),
                new Buff("Accuracy Scroll",  EffectStatusIDs.ACCURACY_SCROLL, BruteGamingMacros.Resources.BruteGaming.Icons.accuracy_scroll),

            };

            return skills;
        }

        public static List<Buff> GetETCBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Field Manual 100% / 300%", EffectStatusIDs.FIELD_MANUAL, BruteGamingMacros.Resources.BruteGaming.Icons.fieldmanual),
                new Buff("Bubble Gum / HE Bubble Gum", EffectStatusIDs.CASH_RECEIVEITEM, BruteGamingMacros.Resources.BruteGaming.Icons.he_bubble_gum),

            };

            return skills;
        }
        //--------------------- DEBUFFS ------------------------------
        public static List<Buff> GetDebuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Bleeding", EffectStatusIDs.BLEEDING, BruteGamingMacros.Resources.BruteGaming.Icons.bleeding),
                new Buff("Burning", EffectStatusIDs.BURNING, BruteGamingMacros.Resources.BruteGaming.Icons.burning),
                new Buff("Chaos / Confusion", EffectStatusIDs.CONFUSION, BruteGamingMacros.Resources.BruteGaming.Icons.chaos),
                new Buff("Critical Wound", EffectStatusIDs.CRITICALWOUND, BruteGamingMacros.Resources.BruteGaming.Icons.critical_wound),
                new Buff("Curse", EffectStatusIDs.CURSE, BruteGamingMacros.Resources.BruteGaming.Icons.curse),
                new Buff("Decrease AGI", EffectStatusIDs.DECREASE_AGI, BruteGamingMacros.Resources.BruteGaming.Icons.decrease_agi),
                new Buff("Freezing", EffectStatusIDs.FREEZING, BruteGamingMacros.Resources.BruteGaming.Icons.freezing),
                new Buff("Frozen", EffectStatusIDs.FROZEN, BruteGamingMacros.Resources.BruteGaming.Icons.frozen),
                new Buff("Hallucination", EffectStatusIDs.HALLUCINATION_DEBUFF, BruteGamingMacros.Resources.BruteGaming.Icons.hallucination),
                new Buff("Poison", EffectStatusIDs.POISON, BruteGamingMacros.Resources.BruteGaming.Icons.poison_status),
                new Buff("Silence", EffectStatusIDs.SILENCE, BruteGamingMacros.Resources.BruteGaming.Icons.silence),
                new Buff("Sit", EffectStatusIDs.SIT, BruteGamingMacros.Resources.BruteGaming.Icons.sit),
                new Buff("Deep Sleep", EffectStatusIDs.DEEP_SLEEP, BruteGamingMacros.Resources.BruteGaming.Icons.deep_sleep),
                new Buff("Sleep", EffectStatusIDs.SLEEP, BruteGamingMacros.Resources.BruteGaming.Icons.sleep),
                new Buff("Slow Cast", EffectStatusIDs.SLOW_CAST, BruteGamingMacros.Resources.BruteGaming.Icons.slow_cast),
                new Buff("Stone Curse (initial stage)", EffectStatusIDs.STONE, BruteGamingMacros.Resources.BruteGaming.Icons.stonecurse1),
                new Buff("Stone Curse (petrified)", EffectStatusIDs.STONEWAIT, BruteGamingMacros.Resources.BruteGaming.Icons.stonecurse2),
                new Buff("Stun", EffectStatusIDs.STUN, BruteGamingMacros.Resources.BruteGaming.Icons.stun),
            };

            return skills;
        }

        //--------------------- WEIGHT ------------------------------
        public static List<Buff> GetWeightDebuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Overweight (50%)", EffectStatusIDs.WEIGHT50, BruteGamingMacros.Resources.BruteGaming.Icons.weight50),
                new Buff("Overweight (90%)", EffectStatusIDs.WEIGHT90, BruteGamingMacros.Resources.BruteGaming.Icons.weight90)
            };

            return skills;
        }

    }
}
