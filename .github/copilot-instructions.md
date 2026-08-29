# GitHub Copilot Instructions for Combat Readiness Check (Continued)

## Mod Overview and Purpose
The Combat Readiness Check (Continued) mod is an update of MarvinKosh's original mod, extended and maintained by Craze. It aims to alter the normal threat points calculation in RimWorld. Threat points are used by the storyteller to determine the composition of raider groups that will raid your colony or ambush your caravans. This mod provides more granular control over how these points are generated, allowing players to adjust settings to personalize their gameplay experience.

## Key Features and Systems
- **Revised Threat Points Calculation:** The mod alters how threat points are derived from colony wealth, colonist abilities, and trained animals.
- **In-game Mod Settings Menu:** Customize the generation of threat points via an in-game settings menu. Changes persist across saves.
- **Debugging Options:** Enable a debug mode to log the current threat points breakdown, including detailed logs for colonists, armoury, and buildings.
- **Wealth Calculation Adjustments:**
  - Weapons and armor are evaluated at market value.
  - Pre-industrial weapons count as 25% of their market value.
  - Building wealth contributes only 25% to threat points.
- **Colonist Point Adjustments:**
  - Baseline colonist points set at 45 with adjustments based on impairments or enhancements.
  - Non-combatants and incapacitated colonists do not contribute to threat points.
- **Adaptation Factor Adjustment:** Caps adaptation and random multipliers to reduce their impact. 
- **Customizable Settings Presets:** Switch between predefined global settings such as "Fair," "This is fine," "Feels bad man," and "Pain train."
- **Extended Adjustment Range:** Set percentage adjustments up to 800%.

## Coding Patterns and Conventions
- **Naming Conventions:** Classes use CamelCase. Methods use camelCase and are descriptive.
- **Extension Methods:** Extensive use of static utility methods to process wealth and threat points.
- **Modular Design:** Code is compartmentalized, with distinct files for handling separate aspects of the game (e.g., MarvsMapWealthForStoryTeller.cs).

## XML Integration
- The mod includes a single XML file (`Storyteller_Patched.xml`) which likely contains the necessary configuration settings to interact with RimWorld's storyteller mechanisms. This file is crucial for linking C# logic with the RimWorld environment.

## Harmony Patching
- The mod uses Harmony for patching, an essential tool for modifying RimWorld's game logic without directly altering the source code.
- **Key Harmony Files:**
  - `MarvsMapWealthForStoryTeller.cs` and `MarvsCaravanWealthForStoryTeller.cs` leverage Harmony's prefixing capabilities to intercept and alter the behavior of relevant game functions.
  - Ensure all patches are enclosed within Harmony annotations such as `[HarmonyPatch]`, and verify their compatibility with other mods.

## Suggestions for Copilot
- **Generate Code Templates:** Copilot can help by generating initial code templates for common tasks, such as setting up Harmony patches or creating new mod settings.
- **Handle Edge Cases:** Suggest improvements to handle exceptions and edge cases in point calculations or settings adjustments.
- **Improve Readability:** Propose refactoring suggestions that enhance code readability without altering functionality.
- **Code Documentation:** Assist in generating XML comments or in-line comments to document complex logic within methods.
- **Unit Testing Support:** Provide basic scaffolding for unit tests to ensure the reliability of threat point calculations and other critical functions.

By leveraging the above instructions, contributors can effectively understand and develop the Combat Readiness Check (Continued) mod with GitHub Copilot's assistance.

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

