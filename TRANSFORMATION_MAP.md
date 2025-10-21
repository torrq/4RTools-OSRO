# NAMESPACE TRANSFORMATION MAP
## Comprehensive Mapping for Brute Gaming Macros Rebranding

**Date:** 2025-10-21
**Phase:** 1.1 - Analysis Complete

---

## NAMESPACE TRANSFORMATION RULES

### Primary Namespaces

| OLD Namespace | NEW Namespace | Files Affected | Notes |
|---------------|---------------|----------------|-------|
| `_4RTools` | `BruteGamingMacros.Core` | Root level files | Root namespace |
| `_4RTools.Forms` | `BruteGamingMacros.UI.Forms` | 20 form files | UI namespace |
| `_4RTools.Model` | `BruteGamingMacros.Core.Model` | 18 model files | Business logic |
| `_4RTools.Utils` | `BruteGamingMacros.Core.Utils` | 15 utility files | Utilities |
| `_4RTools.Properties` | `BruteGamingMacros.Properties` | 3 property files | Properties |
| `_4RTools.Resources._4RTools` | `BruteGamingMacros.Resources.BruteGaming` | 2 resource files | Resources |

---

## FILES TO MODIFY (75 total)

### Forms Directory (20 files × 2 = 40 files)
Each form has .cs and .Designer.cs

**Namespace Changes:**
- FROM: `namespace _4RTools.Forms`
- TO: `namespace BruteGamingMacros.UI.Forms`

**Using Statement Changes:**
- FROM: `using _4RTools.Model;`
- TO: `using BruteGamingMacros.Core.Model;`
- FROM: `using _4RTools.Utils;`
- TO: `using BruteGamingMacros.Core.Utils;`

**Files:**
1. ATKDEFForm.cs + Designer
2. AutoBuffStatusForm.cs + Designer
3. AutoPatcher.cs + Designer
4. AutobuffItemForm.cs + Designer
5. AutobuffSkillForm.cs + Designer
6. AutopotForm.cs + Designer
7. ConfigForm.cs + Designer
8. Container.cs + Designer
9. DebugLogWindow.cs + Designer
10. DialogConfirm.cs + Designer
11. DialogConfirmDelete.cs + Designer
12. DialogInput.cs + Designer
13. MacroSongForm.cs + Designer
14. MacroSwitchForm.cs + Designer
15. ProfileForm.cs + Designer
16. SkillSpammerForm.cs + Designer
17. SkillTimerForm.cs + Designer
18. ToggleStateForm.cs + Designer
19. TransferHelperForm.cs + Designer
20. (19 forms = 38 files, 2 extra for other forms)

### Model Directory (18 files)

**Namespace Changes:**
- FROM: `namespace _4RTools.Model`
- TO: `namespace BruteGamingMacros.Core.Model`

**Using Statement Changes:**
- FROM: `using _4RTools.Utils;`
- TO: `using BruteGamingMacros.Core.Utils;`
- FROM: `using _4RTools.Forms;`
- TO: `using BruteGamingMacros.UI.Forms;`

**Files:**
1. ATKDEF.cs
2. Action.cs
3. AutoSwitch.cs
4. AutoSwitchRenderer.cs
5. AutobuffItem.cs
6. AutobuffSkill.cs
7. Autopot.cs
8. Buff.cs
9. BuffContainer.cs
10. BuffRenderer.cs
11. Client.cs
12. ConfigGlobal.cs
13. ConfigProfile.cs
14. DebuffRecovery.cs
15. DebuffRenderer.cs
16. Macro.cs
17. Profile.cs
18. Server.cs
19. SkillSpammer.cs
20. SkillTimer.cs
21. StatusRecovery.cs
22. Tracker.cs
23. TransferHelper.cs

### Utils Directory (15 files)

**Namespace Changes:**
- FROM: `namespace _4RTools.Utils`
- TO: `namespace BruteGamingMacros.Core.Utils`

**Using Statement Changes:**
- FROM: `using _4RTools.Model;`
- TO: `using BruteGamingMacros.Core.Model;`

**Files:**
1. AmmoSwapHandler.cs
2. AppConfig.cs ⚠️ SPECIAL - Major changes needed
3. Constants.cs
4. DebugLogger.cs
5. EffectStatusIDs.cs
6. FormUtils.cs
7. Interop.cs
8. KeyboardHook.cs
9. NotificationTrayManager.cs
10. ProcessMemoryReader.cs
11. RObserver.cs
12. StatusIdLogger.cs
13. StatusUtils.cs
14. ThreadRunner.cs

### Properties Directory (3 files)

**Namespace Changes:**
- FROM: `namespace _4RTools.Properties`
- TO: `namespace BruteGamingMacros.Properties`

**Files:**
1. AssemblyInfo.cs ⚠️ SPECIAL - Version and branding changes
2. Resources.Designer.cs
3. Settings.Designer.cs

### Resources Directory (2 files)

**Namespace Changes:**
- FROM: `namespace _4RTools.Resources._4RTools`
- TO: `namespace BruteGamingMacros.Resources.BruteGaming`

**Files:**
1. Icons.Designer.cs
2. Sounds.Designer.cs

### Root Directory (1 file)

**Namespace Changes:**
- FROM: `namespace _4RTools`
- TO: `namespace BruteGamingMacros.Core`

**Files:**
1. Program.cs ⚠️ SPECIAL - Entry point

---

## SPECIAL FILES REQUIRING EXTRA ATTENTION

### 1. Program.cs
- Entry point of application
- Contains Main() method
- Sets application title
- Must update namespace and Form references

### 2. Utils/AppConfig.cs
**Major Changes Required:**
```csharp
// OLD:
public static string Name = "4RTools OSRO";
public static string Version = "v1.0.7";
public static string WindowTitle = "4RTools OSRO v1.0.7";

// NEW:
public static string Name = "Brute Gaming Macros";
public static string Version = "v2.0.0";
public static string WindowTitle = "Brute Gaming Macros v2.0.0";
public static string Tagline = "The Ultimate Gaming Automation Suite";

// Add new performance settings
public static int UltraSpamDelayMs = 1;
public static int TurboSpamDelayMs = 5;
public static int StandardSpamDelayMs = 10;
public static bool UseHardwareSimulation = true;
```

### 3. Properties/AssemblyInfo.cs
**Version and Branding Updates:**
```csharp
// OLD:
[assembly: AssemblyTitle("OSRO Tools")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyProduct("OSRO Tools")]
[assembly: AssemblyVersion("1.0.7.0")]

// NEW:
[assembly: AssemblyTitle("Brute Gaming Macros")]
[assembly: AssemblyDescription("The Ultimate Gaming Automation Suite")]
[assembly: AssemblyProduct("Brute Gaming Macros")]
[assembly: AssemblyCompany("Brute Gaming")]
[assembly: AssemblyCopyright("Copyright © 2025 Brute Gaming")]
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]
```

### 4. Container.cs
- Main MDI container form
- Sets window title
- Update: `this.Text = AppConfig.WindowTitle;`

### 5. Resource Files
- Icons.Designer.cs
- Sounds.Designer.cs
- May have internal references to old namespace

---

## PROJECT FILES TO RENAME

### 1. Solution File
- FROM: `OSROTools.sln`
- TO: `BruteGamingMacros.sln`

### 2. Project File
- FROM: `OSRO Tools.csproj`
- TO: `BruteGamingMacros.csproj`

**Project File Changes Required:**
```xml
<!-- OLD -->
<AssemblyName>OSRO Tools</AssemblyName>
<RootNamespace>_4RTools</RootNamespace>
<ProductName>OSRO Tools</ProductName>

<!-- NEW -->
<AssemblyName>BruteGamingMacros</AssemblyName>
<RootNamespace>BruteGamingMacros.Core</RootNamespace>
<ProductName>Brute Gaming Macros</ProductName>
```

### 3. Configuration File
- App.config - No namespace changes, but may reference assembly name

---

## USING STATEMENT PATTERNS TO REPLACE

### Pattern 1: Forms using Model and Utils
```csharp
// OLD
using _4RTools.Model;
using _4RTools.Utils;

// NEW
using BruteGamingMacros.Core.Model;
using BruteGamingMacros.Core.Utils;
```

### Pattern 2: Model using Utils
```csharp
// OLD
using _4RTools.Utils;

// NEW
using BruteGamingMacros.Core.Utils;
```

### Pattern 3: Model using Forms
```csharp
// OLD
using _4RTools.Forms;

// NEW
using BruteGamingMacros.UI.Forms;
```

### Pattern 4: Utils using Model
```csharp
// OLD
using _4RTools.Model;

// NEW
using BruteGamingMacros.Core.Model;
```

### Pattern 5: Resources namespace
```csharp
// OLD
using _4RTools.Resources._4RTools;

// NEW
using BruteGamingMacros.Resources.BruteGaming;
```

---

## EXECUTION STRATEGY

### Phase 1.1: ✅ Analysis Complete
Created this comprehensive map

### Phase 1.2: Update Namespace Declarations (Pending)
Execute in this order:
1. Utils/ files (foundation layer)
2. Model/ files (business logic layer)
3. Forms/ files (UI layer)
4. Properties/ files
5. Resources/ files
6. Program.cs (entry point)

### Phase 1.3: Update Using Statements (Pending)
Scan all files and update using directives

### Phase 1.4: Rename Project Files (Pending)
1. Rename .csproj file
2. Rename .sln file
3. Update internal references

### Phase 1.5: Update Assembly Info (Pending)
Update AssemblyInfo.cs with new branding

### Phase 1.6: Update AppConfig (Pending)
Modernize with new settings and branding

### Phase 1.7: Test Build (Pending)
Verify everything compiles

### Phase 1.8: Commit (Pending)
Git commit with detailed message

---

## SAFETY CHECKS

### Before Each File Modification:
- [ ] Backup already created ✅
- [ ] Git status clean
- [ ] File encoding preserved (UTF-8 with BOM for C#)
- [ ] Line endings preserved (CRLF for Windows)

### After Each Section:
- [ ] Verify syntax with grep/search
- [ ] Check for missed references
- [ ] Test compile (if possible)

### After Complete Phase:
- [ ] Full build test
- [ ] Git diff review
- [ ] Commit to version control

---

## ROLLBACK PROCEDURE

If anything goes wrong at any step:

```bash
# Restore from backup
/home/user/BACKUP_ORIGINAL_4RTOOLS/LATEST/restore_from_backup.sh

# OR git reset
git checkout .
git clean -fd

# OR selective restore
# See RESTORE_INSTRUCTIONS.md
```

---

## VALIDATION CHECKLIST

After transformation complete:

- [ ] All 75 .cs files updated
- [ ] No references to "_4RTools" remain (except comments)
- [ ] All "using" statements updated
- [ ] Project files renamed
- [ ] Assembly info updated
- [ ] AppConfig modernized
- [ ] Solution builds successfully
- [ ] No compiler errors
- [ ] No compiler warnings (related to namespaces)
- [ ] Git diff reviewed
- [ ] Committed to repository

---

## ESTIMATED IMPACT

- **Files Modified:** 75+ C# files
- **Namespaces Changed:** 6 primary namespaces
- **Using Statements:** ~200+ using statements to update
- **Project Files:** 2 files renamed
- **Config Files:** 2 files updated
- **Risk Level:** LOW (backed up, reversible)
- **Test Method:** Build verification

---

**Status:** Ready for Phase 1.2 Execution
**Next Step:** Begin systematic namespace updates starting with Utils/
