# NPlusPlusAssistant

N++ Assistant helps with backups and installing community mod stuff for the videogame [N++](https://store.steampowered.com/app/230270/N_NPLUSPLUS/). Only for the Steam Windows version.

Needs Visual Studio Community 2019 with C# .Net stuff installed to compile from source.

## Stuff it does:

* autodetect main game asset folders and link their explorer folder paths

* backup profile, editor levels, replays, sounds, palettes and game level map packs

* detect if npp is running (avoid install / uninstall while running)

* list all profiles on backup and select one to replace current one or delete

* list metanet palettes (auto download from discord link) and select which to install

* list community palettes (from google spreadsheet) and select which to install

* list all palettes on backup and select one to install or delete

* display number of custom palettes installed (and warn of maximum when reached)

* list all soundpacks on backup and select one to install or delete

* list community soundpacks (from google spreadsheet) and select which to install

* list metanet maps and select one to put on your map levels editor folder (for practice / remixes)

* list all maps in editor and select which ones to backup or delete

* list all maps in backup and select one to install to your map levels editor folder

* button to launch n++ from within N++ Assistant

* list community map packs (from google spreadsheet) and select which to install

* list all map packs in backup and select one to install or delete (with optional same timestamp profile)

## Stuff it'll eventually do:

* update status bar while downloading / creating first launch stuff

* preload all spreadsheet data instead of getting it on tab change

* option to refresh lists (in case people are renaming things on folder)

* option to rename backups from within N++ Assistant

* edit nppconf (useful for troubleshooting some launch problems), careful to make sure it's always valid

* better icon

* display nprofile road to 100% completion details / corruption checks

* button to relaunch steam in offline/online mode (useful for speedrunning)

* way to inject maps into map packs to facilitate composing map packs

* option to change save/backup directory

* maps, search by map name

* maps, rename solo to solo n++ (likewise co-op/race)

* links to spreadsheets being used to retrieve the community data (either on readme or on app or both)

## Known issues:

* rename buttons not working

* some palettes installation don't work because they're not packed cleanly (need to repack, code smarter extractor or create a new column on spreadsheet)

* missing some confirmations on critical stuff that can't be reverted

* missing some checks if npp is running on critical stuff that requires npp to be closed to take effect

* incoherent status message style

## Credits:

* ps - main development

* daniel - steam detection code

* cloudead - beta-testing

* schmole - beta-testing
