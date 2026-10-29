# GitHub Copilot Instructions for Combat Readiness Check (Continued) RimWorld Mod

## Mod Overview and Purpose

The Combat Readiness Check (Continued) mod is an update of MarvinKoshs mod, which alters the normal threat points calculation in RimWorld. The mod provides additional options for adjusting settings and includes a debug option to output current armoury points and building points to the log.

## Key Features and Systems

- **Threat Points Calculation**: The mod modifies the threat points calculation used by the storyteller to determine raiders and ambushes.
- **Wealth Calculation**: The mod adjusts wealth calculation for weapons, armor, and buildings.
- **Colonist Calculation**: The mod modifies the way colonists are counted in threat points, taking into account their combat abilities and conditions.
- **Adaptation and Random Threat Point Multipliers Cap**: An additional feature to cap adaptation and random raid factors at 1.
- **Default Settings Buttons**: Buttons to change between default settings.
- **Extended Adjustment Range**: Extended the max range of possible adjustment to 800% for percentage settings.

## Coding Patterns and Conventions

- **C# Files**: The mod consists of 5 C# files, each with its own purpose:
  - `ArmouryUtility.cs`: Contains methods for getting armoury wealth, battle score, colonist armoury points, and storyteller armoury points.
  - `MarvsCaravanWealthForStoryTeller.cs`: Contains a prefix method for caravan wealth calculation.
  - `MarvsStoryTellerUtility.cs`: Contains methods for combat readiness points and prefix methods.
  - `CRC_Loader.cs`: Contains methods for loading the mod, settings window, and patching methods.
  - `MarvsMapWealthForStoryTeller.cs`: Contains a prefix method for map wealth calculation.
- **XML Files**: The mod uses XML for Harmony patching, specifically in `Storyteller_Patched.xml`.

## XML Integration

The mod uses XML files for Harmony patching. The `Storyteller_Patched.xml` file contains the necessary patches to modify the threat points calculation.

## Harmony Patching

The mod uses Harmony for patching RimWorld's code. Key patches include:
- `MarvsCaravanWealthForStoryTeller.cs`: Prefix method for caravan wealth calculation.
- `MarvsMapWealthForStoryTeller.cs`: Prefix method for map wealth calculation.
- `CRC_Loader.cs`: Methods for patching the default threat points now, patching the map, and patching the caravan.

## Suggestions for Copilot

- **Coding Assistance**: Use Copilot to assist with coding tasks such as creating new methods, modifying existing methods, and implementing new features.
- **XML Patching**: Use Copilot to assist with creating and modifying XML patch files for Harmony.
- **Debugging**: Use Copilot to assist with debugging by suggesting potential issues and solutions.
- **Testing**: Use Copilot to assist with testing by suggesting test cases and scenarios.

## Additional Notes

- **Dependencies**: The mod depends on `brrainz.harmony`.
- **Version**: The current version of the mod is 1.2.0.
- **Contact**: If you have any questions or issues, please use the Discord-channel for error-reporting or the GitHub repository for solutions.

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

