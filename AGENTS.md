# AGENTS.md (LLM + contributor notes)

This repo is a Windows/.NET Framework WinForms app (“OSRO Tools”) that reads RO client memory and drives macros. It’s performance-sensitive (ms-level loops) and UI responsiveness matters.

## Build / run

- This is a **.NET Framework 4.8.1** project (`OSRO Tools.csproj`).
- Prefer **Visual Studio** or **MSBuild (VS Build Tools)** to compile. `dotnet build` may fail or behave oddly for this WinForms/.NET Framework solution depending on installed workloads/SDK behavior.
- When reproducing issues, run in **DebugMode** only when you actually need logs; debug output can be expensive.

## Repo conventions

- Keep changes **small and localized**; this code drives in-game automation and is easy to regress.
- Avoid one-letter variable names unless matching existing local style.
- Keep UI code in `Forms/**` and logic in `Model/**` / `Utils/**`.
- Be careful editing `*.Designer.cs`: prefer WinForms Designer when possible. If you must hand-edit:
  - Keep control names consistent with code-behind.
  - Ensure controls are declared, instantiated, added to `Controls`, and `ResumeLayout/PerformLayout` calls remain correct.

## Performance / responsiveness (important)

### Memory reading hot paths

- `Utils/Memory/ProcessMemoryReader.cs` is on hot paths (HP/SP, statuses, etc.).
- Do **not** add heavy allocations or logging on the success path.
- If handling failure/retry loops, prefer:
  - early-exit when process is gone,
  - log throttling,
  - backoff rather than tight loops.

### Debug log UI

- Logging can easily freeze WinForms if every message forces an `Invoke` + RichTextBox formatting.
- The debug UI should **buffer and flush on a timer** (batch updates). Avoid per-message synchronous `Invoke`.
- `Utils/DebugLogger.cs` should avoid holding its lock while executing UI callbacks; call event handlers **outside** the logger lock.

### Why `requireAdministrator` is required

- Reading game process memory (`ReadProcessMemory` / `OpenProcess` with `PROCESS_VM_READ`) requires the tool to run at a higher integrity level than a normal user process. Do **not** remove or downgrade the `requestedExecutionLevel` in `app.manifest`. The DPI awareness block (`dpiAware` / `dpiAwareness`) in the same manifest is the mitigation for the DWM compositing overhead that elevated processes pay — keep both blocks present and uncommented.

## Server mode / feature flags

- `Utils/Lists/AppConfig.cs` contains server-mode settings:
  - `ServerMode == 0` → MR, `ServerMode == 1` → HR.
- Prefer central “feature flags” in `AppConfig` (e.g. `SupportsFishing`) instead of sprinkling `ServerMode` checks throughout the UI.
  - Example: in HR, Fishing UI should be hidden entirely, not merely left empty.

## Config / preferences

- Global settings come from `Config/config.json` (see `Model/Settings/ConfigGlobal.cs`).
- Profile/user preferences live in `Profiles/*` and `Model/Settings/ConfigProfile.cs`.
- When adding a preference:
  - add it to `ConfigProfile`,
  - load/apply it in UI forms on `PROFILE_CHANGED`,
  - persist by calling `ProfileSingleton.SetConfiguration(...)`.

## Common pitfalls

- **BOM / encoding:** Some files may include a UTF-8 BOM. When applying patches, match existing encoding/line endings to avoid patch failures or noisy diffs.
- **DebugMode gating:** If adding debug-only functionality, gate it behind the *actual* configured debug mode (global config), not a standalone static flag that can drift.
- **UI layout overlap:** Hiding a control is not the same as removing it from layout; explicitly set `Visible/Enabled/TabStop` as needed.

## Quick “where is X?” map

- Main window: `Forms/Container.cs`
- Auto-Off tab: `Forms/Tabs/AutoOffForm.cs` (+ `.Designer.cs`)
- Autobuff Item tab: `Forms/Tabs/AutobuffItemForm.cs` (+ `.Designer.cs`)
- Status logging: `Utils/Memory/StatusEffectLogger.cs`
- Memory reads: `Utils/Memory/ProcessMemoryReader.cs`
- Thread loops: `Utils/ThreadRunner.cs`

