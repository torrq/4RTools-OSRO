using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;

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

        //--------------------- SKILLS ------------------------------

        //Archer Skills
        public static List<Buff> GetArcherBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Improve Concentration", EffectStatusIDs.CONCENTRATION, Resources.Media.Icons.ac_concentration),
                new Buff("True Sight", EffectStatusIDs.TRUESIGHT, Resources.Media.Icons.sn_sight),
                new Buff("Wind Walk", EffectStatusIDs.WINDWALK, Resources.Media.Icons.sn_windwalk),
//                new Buff("Poem of Bragi", EffectStatusIDs.POEMBRAGI, Resources.Media.Icons.poem_of_bragi),
            };

            return skills;
        }

        //Swordsman Skills
        public static List<Buff> GetSwordmanBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Endure", EffectStatusIDs.ENDURE, Resources.Media.Icons.sm_endure),
                new Buff("Auto Berserk", EffectStatusIDs.AUTOBERSERK, Resources.Media.Icons.sm_autoberserk),
                new Buff("Auto Guard", EffectStatusIDs.AUTOGUARD, Resources.Media.Icons.cr_autoguard),
                new Buff("Reflect Shield", EffectStatusIDs.REFLECTSHIELD, Resources.Media.Icons.cr_reflectshield),
                new Buff("Spear Quicken", EffectStatusIDs.SPEARQUICKEN, Resources.Media.Icons.cr_spearquicken),
                new Buff("Defender", EffectStatusIDs.DEFENDER, Resources.Media.Icons.cr_defender),
                new Buff("Concentration", EffectStatusIDs.LKCONCENTRATION, Resources.Media.Icons.lk_concentration),
                new Buff("Berserk", EffectStatusIDs.BERSERK, Resources.Media.Icons.lk_berserk),
                new Buff("Two-Hand Quicken", EffectStatusIDs.TWOHANDQUICKEN, Resources.Media.Icons.mer_quicken),
                new Buff("Parry", EffectStatusIDs.PARRYING, Resources.Media.Icons.ms_parrying),
                new Buff("Aura Blade", EffectStatusIDs.AURABLADE, Resources.Media.Icons.lk_aurablade),
                new Buff("Shrink", EffectStatusIDs.CR_SHRINK, Resources.Media.Icons.cr_shrink),
                new Buff("Magnum Break", EffectStatusIDs.MAGNUM, Resources.Media.Icons.magnum),
                new Buff("One-Hand Quicken", EffectStatusIDs.ONEHANDQUICKEN, Resources.Media.Icons.onehand),
                new Buff("Provoke", EffectStatusIDs.PROVOKE, Resources.Media.Icons.provoke),
                new Buff("Providence", EffectStatusIDs.PROVIDENCE, Resources.Media.Icons.providence),

            };

            // HR server, 3rd Job: Royal Guard
            if (AppConfig.ServerMode == 1)
            {
                skills.Add(new Buff("Mana Shield", EffectStatusIDs.MANA_SHIELD, Resources.Media.Icons.manashield));
            }

            return skills;
        }

        //Mage Skills
        public static List<Buff> GetMageBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Energy Coat", EffectStatusIDs.ENERGYCOAT, Resources.Media.Icons.mg_energycoat),
                new Buff("Sight Blaster", EffectStatusIDs.SIGHTBLASTER, Resources.Media.Icons.wz_sightblaster),
                new Buff("Autospell", EffectStatusIDs.AUTOSPELL, Resources.Media.Icons.sa_autospell),
                new Buff("Double Casting", EffectStatusIDs.DOUBLECASTING, Resources.Media.Icons.pf_doublecasting),
                new Buff("Memorize", EffectStatusIDs.MEMORIZE, Resources.Media.Icons.pf_memorize),
                new Buff("Amplify Magic Power / Mystical Amplification", EffectStatusIDs.MYST_AMPLIFY, Resources.Media.Icons.amplify),
                new Buff("Mind Breaker", EffectStatusIDs.MINDBREAKER, Resources.Media.Icons.mindbreaker),
            };

            return skills;
        }

        //Merchant Skills
        public static List<Buff> GetMerchantBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Crazy Uproar", EffectStatusIDs.CRAZY_UPROAR, Resources.Media.Icons.mc_loud),
                new Buff("Overthrust", EffectStatusIDs.OVERTHRUST, Resources.Media.Icons.bs_overthrust),
                new Buff("Adrenaline Rush", EffectStatusIDs.ADRENALINE, Resources.Media.Icons.bs_adrenaline),
                new Buff("Full Adrenaline Rush", EffectStatusIDs.ADRENALINE2, Resources.Media.Icons.bs_adrenaline2),
                new Buff("Weapon Perfection", EffectStatusIDs.WEAPONPERFECT, Resources.Media.Icons.bs_weaponperfect),
                new Buff("Maximize Power", EffectStatusIDs.MAXIMIZE, Resources.Media.Icons.bs_maximize),
                new Buff("Cart Boost", EffectStatusIDs.CARTBOOST, Resources.Media.Icons.ws_cartboost),
                new Buff("Meltdown", EffectStatusIDs.MELTDOWN, Resources.Media.Icons.ws_meltdown),
                new Buff("Maximum Overthrust", EffectStatusIDs.OVERTHRUSTMAX, Resources.Media.Icons.ws_overthrustmax),
                new Buff("Greed Parry", EffectStatusIDs.GREED_PARRY, Resources.Media.Icons.ws_greedparry),

            };

            return skills;
        }

        //Thief Skills
        public static List<Buff> GetThiefBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Poison React", EffectStatusIDs.POISONREACT, Resources.Media.Icons.as_poisonreact),
                new Buff("Reject Sword", EffectStatusIDs.SWORDREJECT, Resources.Media.Icons.st_rejectsword),
                new Buff("Preserve", EffectStatusIDs.PRESERVE, Resources.Media.Icons.st_preserve),
                new Buff("Enchant Deadly Poison", EffectStatusIDs.EDP, Resources.Media.Icons.asc_edp),
                new Buff("Hiding", EffectStatusIDs.HIDING, Resources.Media.Icons.hiding),
                new Buff("Cloaking", EffectStatusIDs.CLOAKING, Resources.Media.Icons.cloaking),
                new Buff("Chase Walk", EffectStatusIDs.CHASEWALK, Resources.Media.Icons.chase_walk),
            };

            // HR server, 3rd Job: Guillotine Cross
            if (AppConfig.ServerMode == 1)
            {
                skills.Add(new Buff("Enchant Poison Armor", EffectStatusIDs.ENCHANT_POISON_ARMOR, Resources.Media.Icons.enchantpoisonarmor));
            }

            return skills;
        }

        //Acolyte Skills
        public static List<Buff> GetAcolyteBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Blessing", EffectStatusIDs.BLESSING, Resources.Media.Icons.al_blessing1),
                new Buff("Increase Agility", EffectStatusIDs.INC_AGI, Resources.Media.Icons.al_incagi1),
                new Buff("Gloria", EffectStatusIDs.GLORIA, Resources.Media.Icons.pr_gloria),
                new Buff("Magnificat", EffectStatusIDs.MAGNIFICAT, Resources.Media.Icons.pr_magnificat),
                new Buff("Angelus", EffectStatusIDs.ANGELUS, Resources.Media.Icons.al_angelus),
                new Buff("Fury", EffectStatusIDs.FURY, Resources.Media.Icons.fury),
                new Buff("Impositio Manus", EffectStatusIDs.IMPOSITIO, Resources.Media.Icons.impositio_manus),
                new Buff("Basilica", EffectStatusIDs.BASILICA, Resources.Media.Icons.basilica),

            };

            // HR server, 3rd Job: Arch Bishop
            if (AppConfig.ServerMode == 1)
            {
                skills.Add(new Buff("Refraction", EffectStatusIDs.REFRACTION, Resources.Media.Icons.refraction));
                skills.Add(new Buff("Shallow Grave", EffectStatusIDs.KAIZEL, Resources.Media.Icons.shallowgrave));
            }

            return skills;
        }

        //Ninja Skills
        public static List<Buff> GetNinjaBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Ninja Aura", EffectStatusIDs.NINJA_AURA, Resources.Media.Icons.nj_nen),
                new Buff("Cast-off Cicada Shell / Cicada Skin Shed", EffectStatusIDs.CICADA_SKIN_SHED, Resources.Media.Icons.nj_utsusemi),
                new Buff("Illusionary Shadow / Mirror Image", EffectStatusIDs.MIRROR_IMAGE, Resources.Media.Icons.bunsinjyutsu),
                // Possibly Renewal
                //new Buff("Izayoi", EffectStatusIDs.IZAYOI, Resources.Media.Icons.izayoi),
            };

            return skills;
        }

        //Taekwon Skills
        public static List<Buff> GetTaekwonBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Mild Wind (Earth)", EffectStatusIDs.PROPERTYGROUND, Resources.Media.Icons.tk_mild_earth),
                new Buff("Mild Wind (Fire)", EffectStatusIDs.PROPERTYFIRE, Resources.Media.Icons.tk_mild_fire),
                new Buff("Mild Wind (Water)", EffectStatusIDs.PROPERTYWATER, Resources.Media.Icons.tk_mild_water),
                new Buff("Mild Wind (Wind)", EffectStatusIDs.PROPERTYWIND, Resources.Media.Icons.tk_mild_wind),
                new Buff("Mild Wind (Ghost)", EffectStatusIDs.PROPERTYTELEKINESIS, Resources.Media.Icons.tk_mild_ghost),
                new Buff("Mild Wind (Holy)", EffectStatusIDs.ASPERSIO, Resources.Media.Icons.tk_mild_holy),
                new Buff("Mild Wind (Shadow)", EffectStatusIDs.PROPERTYDARK, Resources.Media.Icons.tk_mild_shadow),
                new Buff("Tumbling", EffectStatusIDs.DODGE_ON, Resources.Media.Icons.tumbling),
                //new Buff("Solar, Lunar, and Stellar Miracle", EffectStatusIDs.MIRACLE, Resources.Media.Icons.solar_miracle),
                new Buff("Solar, Lunar, and Stellar Warmth", EffectStatusIDs.WARM, Resources.Media.Icons.sun_warm),
                new Buff("Comfort of the Sun", EffectStatusIDs.SUN_COMFORT, Resources.Media.Icons.sun_comfort),
                new Buff("Comfort of the Moon", EffectStatusIDs.MOON_COMFORT, Resources.Media.Icons.moon_comfort),
                new Buff("Comfort of the Stars", EffectStatusIDs.STAR_COMFORT, Resources.Media.Icons.star_comfort),
                //new Buff("Union of the Sun, Moon, and Stars", EffectStatusIDs.FUSION, Resources.Media.Icons.fusion),
                new Buff("Kaupe", EffectStatusIDs.KAUPE, Resources.Media.Icons.kaupe),
                new Buff("Kaite", EffectStatusIDs.KAITE, Resources.Media.Icons.kaite),
                new Buff("Kaizel", EffectStatusIDs.KAIZEL, Resources.Media.Icons.kaizel),
                new Buff("Kaahi", EffectStatusIDs.KAAHI, Resources.Media.Icons.kaahi),
            };

            return skills;
        }


        public static List<Buff> GetGunsBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Gatling Fever", EffectStatusIDs.GATLINGFEVER, Resources.Media.Icons.gatling_fever),
                new Buff("Last Stand", EffectStatusIDs.MADNESSCANCEL, Resources.Media.Icons.madnesscancel),
                new Buff("Adjustment", EffectStatusIDs.ADJUSTMENT, Resources.Media.Icons.adjustment),
                new Buff("Increased Accuracy", EffectStatusIDs.ACCURACY, Resources.Media.Icons.increase_accuracy),
            };

            return skills;
        }

        // HR‑specific Padawan list (ServerMode == 1)
        private static readonly List<Buff> PadawanBuffsHR = new List<Buff>
        {
            new Buff("Force Element (Earth)",  EffectStatusIDs.PROPERTYGROUND,     Resources.Media.Icons.forceelement_earth),
            new Buff("Force Element (Wind)",   EffectStatusIDs.PROPERTYWIND,       Resources.Media.Icons.forceelement_wind),
            new Buff("Force Element (Water)",  EffectStatusIDs.PROPERTYWATER,      Resources.Media.Icons.forceelement_water),
            new Buff("Force Element (Fire)",   EffectStatusIDs.PROPERTYFIRE,       Resources.Media.Icons.forceelement_fire),
            new Buff("Force Element (Ghost)",  EffectStatusIDs.PROPERTYTELEKINESIS, Resources.Media.Icons.forceelement_ghost),
            new Buff("Force Element (Shadow)", EffectStatusIDs.PROPERTYDARK,       Resources.Media.Icons.forceelement_shadow),
            new Buff("Force Element (Holy)",   EffectStatusIDs.ASPERSIO,           Resources.Media.Icons.forceelement_holy),
            new Buff("Force Projection",       EffectStatusIDs.HR_PROJECTION,      Resources.Media.Icons.hr_forceprojection),
            new Buff("Cold Skin",              EffectStatusIDs.RESIST_PROPERTY_WATER, Resources.Media.Icons.hr_coldskin),
            new Buff("Saber Parry",            EffectStatusIDs.HR_SABERPARRY,      Resources.Media.Icons.hr_saberparry),
            new Buff("Force Concentration",    EffectStatusIDs.HR_FORCECONCENTRATE,Resources.Media.Icons.hr_forceconcentrate),
            new Buff("Saber Thrust",           EffectStatusIDs.LKCONCENTRATION,    Resources.Media.Icons.hr_saberthrust),
            new Buff("Force Persuasion",       EffectStatusIDs.HR_FORCEPERSUASION, Resources.Media.Icons.hr_forcepersuasion),
            new Buff("Force Haste",            EffectStatusIDs.HR_FORCEHASTE,      Resources.Media.Icons.forcehaste),
            new Buff("Force Sacrifice",        EffectStatusIDs.HR_FORCESACRIFICE,  Resources.Media.Icons.hr_forcesacrifice),
            new Buff("Jedi Frenzy",            EffectStatusIDs.HR_JEDIFRENZY,      Resources.Media.Icons.hr_jedifrenzy)
        };

        // MR / LR shared list (ServerMode == 0 or 2)
        private static readonly List<Buff> PadawanBuffsMR = new List<Buff>
        {
            new Buff("Force Element (Earth)",  EffectStatusIDs.ELEMENT_EARTH,   Resources.Media.Icons.forceelement_earth),
            new Buff("Force Element (Wind)",   EffectStatusIDs.ELEMENT_WIND,    Resources.Media.Icons.forceelement_wind),
            new Buff("Force Element (Water)",  EffectStatusIDs.ELEMENT_WATER,   Resources.Media.Icons.forceelement_water),
            new Buff("Force Element (Fire)",   EffectStatusIDs.ELEMENT_FIRE,    Resources.Media.Icons.forceelement_fire),
            new Buff("Force Element (Ghost)",  EffectStatusIDs.ELEMENT_GHOST,   Resources.Media.Icons.forceelement_ghost),
            new Buff("Force Element (Shadow)", EffectStatusIDs.ELEMENT_SHADOW,  Resources.Media.Icons.forceelement_shadow),
            new Buff("Force Element (Holy)",   EffectStatusIDs.ELEMENT_HOLY,    Resources.Media.Icons.forceelement_holy),
            new Buff("Force Projection",       EffectStatusIDs.PROJECTION,      Resources.Media.Icons.forceprojection),
            new Buff("Cold Skin",              EffectStatusIDs.COLDSKIN,        Resources.Media.Icons.coldskin),
            new Buff("Saber Parry",            EffectStatusIDs.SABERPARRY,      Resources.Media.Icons.saberparry),
            new Buff("Force Concentration",    EffectStatusIDs.FORCECONCENTRATE,Resources.Media.Icons.forceconcentrate),
            new Buff("Saber Thrust",           EffectStatusIDs.SABERTHRUST,     Resources.Media.Icons.saberthrust),
            new Buff("Force Persuasion",       EffectStatusIDs.FORCEPERSUASION, Resources.Media.Icons.forcepersuasion),
            new Buff("Jedi Stealth",           EffectStatusIDs.JEDISTEALTH,     Resources.Media.Icons.jedistealth),
            new Buff("Force Levitate",         EffectStatusIDs.FORCELEVITATE,   Resources.Media.Icons.forcelevitate),
            new Buff("Jedi Frenzy",            EffectStatusIDs.JEDIFRENZY,      Resources.Media.Icons.jedifrenzy),
            new Buff("Force Sacrifice",        EffectStatusIDs.FORCESACRIFICE,  Resources.Media.Icons.forcesacrifice)
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
                new Buff("Concentration Potion", EffectStatusIDs.CONCENTRATION_POTION, Resources.Media.Icons.concentration_potion),
                new Buff("Awakening Potion", EffectStatusIDs.AWAKENING_POTION, Resources.Media.Icons.awakening_potion),
                new Buff("Berserk Potion", EffectStatusIDs.BERSERK_POTION, Resources.Media.Icons.berserk_potion),
            };

            return skills;
        }

        public static List<Buff> GetElementalsBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Fire Elemental Converter", EffectStatusIDs.PROPERTYFIRE, Resources.Media.Icons.ele_fire_converter),
                new Buff("Wind Elemental Converter", EffectStatusIDs.PROPERTYWIND, Resources.Media.Icons.ele_wind_converter),
                new Buff("Earth Elemental Converter", EffectStatusIDs.PROPERTYGROUND, Resources.Media.Icons.ele_earth_converter),
                new Buff("Water Elemental Converter", EffectStatusIDs.PROPERTYWATER, Resources.Media.Icons.ele_water_converter),
                new Buff("Box of Storms", EffectStatusIDs.BOX_OF_STORMS, Resources.Media.Icons.boxofstorms),
                //new Buff("Aspersio Scroll", EffectStatusIDs.ASPERSIO, Resources.Media.Icons.ele_holy_converter),
                new Buff("Cursed Water", EffectStatusIDs.PROPERTYDARK, Resources.Media.Icons.cursed_water),
                new Buff("Fireproof Potion", EffectStatusIDs.RESIST_PROPERTY_FIRE, Resources.Media.Icons.fireproof),
                new Buff("Coldproof Potion", EffectStatusIDs.RESIST_PROPERTY_WATER, Resources.Media.Icons.coldproof),
                new Buff("Thunderproof Potion", EffectStatusIDs.RESIST_PROPERTY_WIND, Resources.Media.Icons.thunderproof),
                new Buff("Earthproof Potion", EffectStatusIDs.RESIST_PROPERTY_GROUND, Resources.Media.Icons.earthproof),
            };

            return skills;
        }

        public static List<Buff> GetFoodBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Steamed Tongue (STR+10)", EffectStatusIDs.FOOD_STR, Resources.Media.Icons.strfood),
                new Buff("Steamed Scorpion (AGI+10)", EffectStatusIDs.FOOD_AGI, Resources.Media.Icons.agi_food),
                new Buff("Stew of Immortality (VIT+10)", EffectStatusIDs.FOOD_VIT, Resources.Media.Icons.vit_food),
                new Buff("Dragon Breath Cocktail (INT+10)", EffectStatusIDs.FOOD_INT, Resources.Media.Icons.int_food),
                new Buff("Hwergelmir's Tonic (DEX+10)", EffectStatusIDs.FOOD_DEX, Resources.Media.Icons.dex_food),
                new Buff("Cooked Nine Tail's Tails (LUK+10)", EffectStatusIDs.FOOD_LUK, Resources.Media.Icons.luk_food),
                new Buff("Halo-Halo", EffectStatusIDs.HALOHALO, Resources.Media.Icons.halohalo),
                new Buff("Glass of Illusion", EffectStatusIDs.GLASS_OF_ILLUSION, Resources.Media.Icons.Glass_Of_Illusion),

            };


            return skills;
        }

        public static List<Buff> GetBoxesBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Box of Drowsiness / Tasty White Ration", EffectStatusIDs.DROWSINESS_BOX, Resources.Media.Icons.drowsiness),
                new Buff("Box of Resentment / Tasty Pink Ration / Chewy Ricecake", EffectStatusIDs.RESENTMENT_BOX, Resources.Media.Icons.resentment),
                new Buff("Box of Sunlight", EffectStatusIDs.SUNLIGHT_BOX, Resources.Media.Icons.sunbox),
                new Buff("Box of Gloom", EffectStatusIDs.CONCENTRATION, Resources.Media.Icons.gloom),
                new Buff("Box of Thunder", EffectStatusIDs.BOX_OF_THUNDER, Resources.Media.Icons.boxofthunder),
                new Buff("Speed Potion", EffectStatusIDs.SPEED_POT, Resources.Media.Icons.speedpotion),
                new Buff("Anodyne", EffectStatusIDs.ENDURE, Resources.Media.Icons.anodyne),
                new Buff("Aloe Vera", EffectStatusIDs.PROVOKE, Resources.Media.Icons.aloevera),
                new Buff("Abrasive", EffectStatusIDs.CRITICALPERCENT, Resources.Media.Icons.abrasive),
                new Buff("Combat Pill", EffectStatusIDs.COMBAT_PILL, Resources.Media.Icons.combat_pill),
                new Buff("Enriched Celermine Juice", EffectStatusIDs.ENRICH_CELERMINE_JUICE, Resources.Media.Icons.celermine),
                new Buff("ASPD Potion Infinity", EffectStatusIDs.ASPDPOTIONINFINITY, Resources.Media.Icons.poison)
            };

            return skills;
        }

        public static List<Buff> GetScrollBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Increase Agility Scroll", EffectStatusIDs.INC_AGI, Resources.Media.Icons.al_incagi1),
                new Buff("Bless Scroll", EffectStatusIDs.BLESSING, Resources.Media.Icons.al_blessing1),
                new Buff("Full Chemical Protection (Scroll)", EffectStatusIDs.PROTECTARMOR, Resources.Media.Icons.cr_fullprotection),
                new Buff("Link Scroll", EffectStatusIDs.SOULLINK, Resources.Media.Icons.sl_soullinker),
                new Buff("Assumptio",  EffectStatusIDs.ASSUMPTIO, Resources.Media.Icons.assumptio),
                new Buff("Flee Scroll / Spray of Flowers",  EffectStatusIDs.FLEE_SCROLL, Resources.Media.Icons.flee_scroll),
                new Buff("Accuracy Scroll",  EffectStatusIDs.ACCURACY_SCROLL, Resources.Media.Icons.accuracy_scroll),

            };

            return skills;
        }

        public static List<Buff> GetETCBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("VIP Ticket",  EffectStatusIDs.VIP_BONUS, Resources.Media.Icons.vip_ticket),
                new Buff("Field Manual 100% / 300%", EffectStatusIDs.FIELD_MANUAL, Resources.Media.Icons.fieldmanual),
                new Buff("Bubble Gum / HE Bubble Gum", EffectStatusIDs.CASH_RECEIVEITEM, Resources.Media.Icons.he_bubble_gum),

            };

            return skills;
        }
        //--------------------- DEBUFFS ------------------------------
        public static List<Buff> GetDebuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Bleeding", EffectStatusIDs.BLEEDING, Resources.Media.Icons.bleeding),
                new Buff("Burning", EffectStatusIDs.BURNING, Resources.Media.Icons.burning),
                new Buff("Chaos / Confusion", EffectStatusIDs.CONFUSION, Resources.Media.Icons.chaos),
                new Buff("Critical Wound", EffectStatusIDs.CRITICALWOUND, Resources.Media.Icons.critical_wound),
                new Buff("Curse", EffectStatusIDs.CURSE, Resources.Media.Icons.curse),
                new Buff("Decrease AGI", EffectStatusIDs.DECREASE_AGI, Resources.Media.Icons.decrease_agi),
                new Buff("Freezing", EffectStatusIDs.FREEZING, Resources.Media.Icons.freezing),
                new Buff("Frozen", EffectStatusIDs.FROZEN, Resources.Media.Icons.frozen),
                new Buff("Hallucination", EffectStatusIDs.HALLUCINATION_DEBUFF, Resources.Media.Icons.hallucination),
                new Buff("Poison", EffectStatusIDs.POISON, Resources.Media.Icons.poison_status),
                new Buff("Silence", EffectStatusIDs.SILENCE, Resources.Media.Icons.silence),
                new Buff("Sit", EffectStatusIDs.SIT, Resources.Media.Icons.sit),
                new Buff("Deep Sleep", EffectStatusIDs.DEEP_SLEEP, Resources.Media.Icons.deep_sleep),
                new Buff("Sleep", EffectStatusIDs.SLEEP, Resources.Media.Icons.sleep),
                new Buff("Slow Cast", EffectStatusIDs.SLOW_CAST, Resources.Media.Icons.slow_cast),
                new Buff("Stone Curse (initial stage)", EffectStatusIDs.STONE, Resources.Media.Icons.stonecurse1),
                new Buff("Stone Curse (petrified)", EffectStatusIDs.STONEWAIT, Resources.Media.Icons.stonecurse2),
                new Buff("Stun", EffectStatusIDs.STUN, Resources.Media.Icons.stun),
            };

            return skills;
        }

    }
}
