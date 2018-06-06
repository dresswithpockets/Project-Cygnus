## Project-Cygnus
Project Cygnus _was_ a code base that aimed to recreate the CubeWorld experience. The project's direction has changed drastically, however. Cygnus now aims to be its own game that draws details from games such as CubeWorld, Bloodborne, Diablo, Dark Souls, and even Minecraft.

[We have a design document for the specifications of the project which can be found here.](http://***REMOVED******REMOVED***.com/cygnus/Cygnus.pdf)

The current issues that are listed are not in-line with the current design document.

The game is entirely open source, but the code and content is under Copyright.
If you're interested in using any of the code or content found in this project, please read the "Licensing" section.

#### Dependencies
1. The game is built around Unity3D. The project must be opened in a Unity3D environment of 5.3.4 or later in order to run and compile.
2. At its current stage, the game uses VisionPunk's Ultimate FPS package for Unity, which is licensed software. Certain mechanics are built around UFPS such as playermovement and input management.
 - The dependancy on UFPS will eventually be replaced with our own player movement and input managers. It isn't a major priority at the moment but it will happen eventually!
3. The game's voxel model rendering is done through PicaVoxel - a fully-blown voxel engine, with built in editor. However, we use MagicaVoxel (.vox) models for importing files at runtime. All 3d models you'll find in the game are created using MagicaVoxel. We may add support for Qubicle.

#### Discord
I run a discord for anyone to join and chat. If you need help/support and you're not breaking any of the following rules, ask away. Here is the invite URL: https://discord.gg/86cjgcq

1. Check the FAQ **before** joining and bugging me about the problem.
  * I will probably ignore you if its a question answered on the FAQ.
2. You will be muted or even banned for being annoying.
3. Don't join the discord if you want to report a bug. Read the "New Features and Bugs" section below.
4. Don't join the discord if you want to request or suggest a new feature. Read the "New Features and Bugs" section below.

#### Twitch
On occasion I will stream me working on the game. You can find the stream at http://www.twitch.tv/***REMOVED***

#### New Features and Bugs
Debugging and fixing issues found in the game can be difficult when you're also trying to implement new features in a timely fashion.

* If you're interested in fixing bugs, introducing planned features, or proposing new ones, please read the "Fork Rules" section.

* If you're interested in reporting bugs, send me an e-mail. You MUST have a video attached or linked which shows every step to reproduce the bug at least two times in a row. Make the subject of your e-mail "Cygnus - Bug Report" and make sure to include the following information:
  * Your name
  * The Version/Build Number of the game, which can be found in the log file if you Ctrl+F "game_version", or in the bottom right corner of the game's window.
  * Platform - Windows/Linux/Mac etc
  * Bug description:
    * Reproduceable steps: Clearly define the steps it takes to reproduce the bug.
    * Expected result: How is the game supposed to respond?
    * Actual result: What is the actual response from the game?
  * A link to a video

* If you're interested in requesting or suggesting new features:
  * At the moment, only forked changes and feature suggestions will be considered.
  * If you don't have programming experience or the time to develop your idea, then wait until we have a considerable forum and community in order to manage threads for new ideas and concepts.

#### Fork Rules
1. All code that is not immediately comprehensible, meaning it doesn't read with ease, should be explained in a comment.
2. Timestamp all comments that explain functionality, propose or explain ideas, or in comments that are longer than 2 lines.
  * Timestamp format: "-Initials Date".
  * Example: "-TH 4/20/2016".
3. Follow the coding conventions found in the game's source code.
  * All member and variable names must be lower_case_with_under_scores
  * All type names (structs, classes, enums, delegates) must be UpperCamelCase
  * Tight single-line if statements and loops should usually be capture in a single line without braces, unless the resulting line is very long.
  * All member, variable and type names should be easy to read and understand. Local variable names are less important.
  * The english language only, please. Changes made that aren't in english probably won't be accepted in an official build.

#### Issue Tracking
I use Atlassian Jira to track my personal progress on the project. Every week I will update the issues page to be concurrent with my current issues in Jira. 

## Licensing
Project Cygnus will be referred to as "The Project" and "This Project". Individuals or Entities who intend to use assets or code found in This Project will be referred to as "The Users".

I, ***REMOVED***, hereby grant The Users the right to use the code and content found in The Project, with exceptions for specific scenarios listed below in the "Special Cases" section, for personal, educational or non-commercial use, free of charge, free of royalties. In the case that any assets or code in This Project are redistributed in any format (compiled, packaged, open source, et cetera), there must be an explicit declaration of credit given to the contributors to This Project. This explicit declaration of credit can be found below in the  "Giving Credit" section.

**Special Cases:**
- If you are not one of the developers contributing to This Project then The Single-Seat Copy of PicaVoxel found within This Project may not be used for any other purpose other than to compile this project. Copying the assets or code found within the Single-Seat Copy of PicaVoxel found within This Project is strictly dissallowed by this license.

**Giving Credt:**
The text garnered below in the following Markdown Code block must be included in a text file named "CREDIT.TXT" at the root directory of the redistributed package of assets or code found in This Project.
```
A part or parts of this codebase, assetbase, package or software is redistributed work
originally created by the contributors and developers listed at the following URL:
https://github.com/***REMOVED***/Project-Cygnus/blob/master/HUMANS.md
```

**Commercial Use:**
If The Users are interested in implementing parts of This Project's codebase or assetbase in another solution and intend on commercializing that solution, please contact me at [***REMOVED***@***REMOVED******REMOVED***.com](mailto:***REMOVED***@***REMOVED******REMOVED***.com). I'd be glad to work out a royalty or flat fee depending on the amount of work that is being used from This Project.

