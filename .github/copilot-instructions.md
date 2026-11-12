# GitHub Copilot Instructions for Ultimate Survival (Continued)

## Mod Overview and Purpose

Ultimate Survival (Continued) is an overhaul of the standard RimWorld experience. The mod introduces new storytellers, scenarios, difficulties, and a variety of new elements like raid types, bionic limbs, turrets, and weapons. The overall goal is to provide a challenging and unique survival experience for players.

## Key Features and Systems

- **New Storyteller, Scenario, and Difficulty**: Ultimate Survival introduces a new storyteller, scenario, and difficulty setting to enhance the gaming experience.
- **Enhanced Raid System**: The mod includes new, stronger raid types that scale up quickly in strength. Raid sizes also unlock as time passes.
- **Faster Game Pacing**: Global work speed and learning factor are increased to 3.5x and 5x, respectively, to speed up building, research, and skill development.
- **Orbital Traders**: More frequent orbital traders allow for quick trading of items.
- **New Bionic Limbs, Turrets, and Equipment**: A variety of new bionic limbs, turrets, and craftable items to enhance colonist capabilities.
- **New Faction and Base Defense Mechanisms**: A new faction with five pawns and barbed wire for defensive purposes.

## Coding Patterns and Conventions

- **C# Coding Standards**: Follow standard C# naming conventions, including PascalCase for class names and camelCase for method and variable names.
- **XML Defs Structure**: Use the provided XML def structure for creating new items, factions, and other elements. Ensure proper XML formatting and adherence to RimWorld's def conventions.

## XML Integration

- **Def Files**: All new items, factions, and other game elements are defined in XML files located in the `Defs` directory. Use the existing def files as templates for creating new ones.
- **Def References**: Ensure that all def references in your XML files are correct and that the corresponding defs are defined in the mod.

## Harmony Patching

- **Harmony Library**: Use the Harmony library to create patches for existing RimWorld code. This allows for modification of vanilla game behavior without modifying the core game files.
- **Patch Naming**: Name your patch files and methods clearly to indicate their purpose. Use comments to explain complex patches.

## Suggestions for Copilot

- **C# Snippets**: Use Copilot to generate C# snippets for new buildings, items, and other elements. For example, when creating a new building, use Copilot to generate the basic class structure and methods like `ExposeData`, `Tick`, and `Graphic`.
- **XML Templates**: Use Copilot to generate XML templates for new def files. For example, when creating a new bionic limb, use Copilot to generate the basic structure of the `BodyPartDef` and `BodyDef`.
- **Patch Generation**: Use Copilot to generate Harmony patch snippets. For example, when modifying the vanilla game's raid system, use Copilot to generate the basic structure of the Harmony patch method.

## Example Instructions for Copilot

### C# Example: Creating a New Building

1. **Class Name**: `Building_BarbedWire.cs`
2. **Inheritance**: `Building_BarbedWire : Building_Trap`
3. **Methods**: `Armed`, `DamagePawn`, `ExposeData`, `Graphic`, `Rearm`, `SpringSub`, `Tick`

csharp
using RimWorld;
using Verse;

namespace UltimateSurvival
{
    public class Building_BarbedWire : Building_Trap
    {
        public override bool Armed => base.Armed;

        public override void DamagePawn(Pawn pawn)
        {
            base.DamagePawn(pawn);
        }

        public override void ExposeData()
        {
            base.ExposeData();
        }

        public override Graphic Graphic => base.Graphic;

        public void Rearm()
        {
            // Implement rearming logic
        }

        public void SpringSub()
        {
            // Implement spring sub logic
        }

        public override void Tick()
        {
            base.Tick();
        }
    }
}


### XML Example: Creating a New Body Part Def

1. **Def Name**: `BodyParts_Psyco.xml`
2. **Def Type**: `BodyPartDef`
3. **Attributes**: `label`, `description`, `graphicData`, etc.

xml
<?xml version="1.0" encoding="UTF-8"?>
<Defs>
    <BodyPartDef>
        <defName>PsychoLeg</defName>
        <label>psycho leg</label>
        <description>A powerful bionic leg.</description>
        <graphicData>
            <texPath>Things/Bionic/PsychoLeg</texPath>
        </graphicData>
        <coverType>Leg</coverType>
        <parent>
            <defName>Leg</defName>
        </parent>
    </BodyPartDef>
</Defs>


### Harmony Patch Example: Modifying Raid Frequency

1. **Patch Class**: `RaidFrequencyPatch.cs`
2. **Harmony Method**: `Patch_RaidFrequency`
3. **Target Method**: `RaidFrequency`

csharp
using HarmonyLib;
using Verse;

namespace UltimateSurvival
{
    [HarmonyPatch(typeof(RaidFrequency), "GetNextRaidFrequency")]
    public static class RaidFrequencyPatch
    {
        public static void Postfix(ref float __result)
        {
            __result *= 0.5f; // Reduce raid frequency by 50%
        }
    }
}


By following these guidelines and using Copilot to generate snippets and templates, you can efficiently develop and maintain the Ultimate Survival (Continued) mod.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

