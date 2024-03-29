# CombatReadinessCheck

![Image](https://i.imgur.com/buuPQel.png)

Update of MarvinKoshs mod
https://steamcommunity.com/sharedfiles/filedetails/?id=1542424263
with permission.

- Update work by Craze
- Added option to treat pre-industrial armors the same as pre-industrial weapons

![Image](https://i.imgur.com/pufA0kM.png)

	
![Image](https://i.imgur.com/Z4GOv8H.png)

Alters the normal threat points calculation.

Threat points are used by the storyteller to, for example, 'buy' raiders from a list to make up the next group that will raid your colony or ambush one of your caravans.

You can use the in-game Mod Settings menu to adjust some of the settings for how points are generated. Select Combat Readiness Check, and edit the values. They will stay in effect as long as you're using the same saved game folder.

A debug option is available in the mod settings menu. This will output the current armoury points and building points to the log. Colonist points are also logged, but only if you turn on Write Storyteller in development mode.

### Wealth calculation



-  Wealth from weapons and armour counts at market value. Armour is anything which offers better than 29% protection against bullets or arrows.
-  Wealth from pre-industrial weapons counts at 25% of market value.
-  Wealth from other items is not counted (vanilla: all items are counted at market value, converted into points by wealth curve)
-  The combined weapons and armour wealth is converted into points by the Combat Readiness wealth curve.
-  Wealth from buildings is counted at 25% of value and converted into points by the Combat Readiness wealth curve (vanilla: 50% of value, vanilla wealth curve).



### Animals

Animals which have release training count for 9% of their combat strength (vanilla: similar percentage, but all animals that could be trained, even if they aren't trained).

### Colonist calculation



-  Colonists by default count for 45 points (vanilla: starts at 40 points and increases to 110 depending on colony wealth)
-  Colonists who have impairment to their combat abilities (consciousness, manipulation, sight) will count for less points.
-  Colonists with limited movement capability or a limited combined combat ability will count for zero points. 
-  Colonists in cryptosleep will count for zero points.
-  Colonists who can't fight at all will count for zero points.
-  Colonists who are suffering from an infection or disease which needs bed rest will count for zero points if they haven't developed immunity. 
-  Colonists who have enhanced capabilities (from bionics, trauma savant, mechanites or drugs) can count slightly more.



The current adaptation factor affects the final colonist points value (vanilla: adaptation multiplies everything just like difficulty).

### Processing points


The combined points are then processed by the Combat Readiness points curve (vanilla: no points curve).
After that the points are multiplied by the modifier for difficulty level (vanilla: same).
There is a global maximum of 10000 points or 20000 for Merciless difficulty (vanilla: 20000).

### Extra features




- Includes 'Adaptation and Random Threat Point Multipliers Cap to One' which changes the adaptation and random raid factors so that they cap at 1, reducing the effect of those factors.
- Added buttons to change between a few default settings - Fair, This is fine, Feels bad man, Pain train.
- Extended the max range of possible adjustment to 800% for percentage settings.



### Current version: 1.2.0


**Please ask me before re-uploading this mod.**

## Don't comment asking when the mod will update.


Comments regarding mod updates have become a nuisance. I can understand that people get excited and post before they think or read, but I have https://twitter.com/marvinkosh]social media, go and check there to see if I have posted about RimWorld. You will be saving me a lot of time.

### Navigation


https://steamcommunity.com/sharedfiles/filedetails/?id=1518256398]Go to the Beta 19 version.

Go to Ludeon Forums for more information and direct download links: https://ludeon.com/forums/index.php?topic=30508.0

If you like this mod, why not consider looking at some of my http://steamcommunity.com/id/marvinkosh/myworkshopfiles/?appid=294100]other mods?

![Image](https://i.imgur.com/PwoNOj4.png)



-  See if the the error persists if you just have this mod and its requirements active.
-  If not, try adding your other mods until it happens again.
-  Post your error-log using https://steamcommunity.com/workshop/filedetails/?id=818773962]HugsLib or the standalone https://steamcommunity.com/sharedfiles/filedetails/?id=2873415404]Uploader and command Ctrl+F12
-  For best support, please use the Discord-channel for error-reporting.
-  Do not report errors by making a discussion-thread, I get no notification of that.
-  If you have the solution for a problem, please post it to the GitHub repository.
-  Use https://github.com/RimSort/RimSort/releases/latest]RimSort to sort your mods



https://steamcommunity.com/sharedfiles/filedetails/changelog/2314304057]![Image](https://img.shields.io/github/v/release/emipa606/CombatReadinessCheck?label=latest%20version&style=plastic&color=9f1111&labelColor=black)

