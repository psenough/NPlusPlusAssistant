# NPlusPlusAssistant

N++ Assistant helps with backups and installing community mod stuff for the videogame [N++](https://store.steampowered.com/app/230270/N_NPLUSPLUS/). Only for the Steam Windows version.

Needs Visual Studio Community 2019 with C# .Net stuff installed to compile from source.

## Stuff it already does:

* autodetect main game asset folders (list them with explorer path link)

* backup profile, editor levels, replays, sounds, palettes and game levels

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

## Stuff it'll eventually do:

* progress bar while downloading / installing soundpacks

* option to rename backups

* icon to refresh lists (in case people are renaming on folder)

* install/revert community map packs

* edit nppconf (useful for troubleshooting some launch problems)

* have an n++ icon (winning stickman?)

* display nprofile road to 100% completion details / corruption checks

## Known issues:

* some palettes/soundpacks installation don't work because they're not packed cleanly (need to repack, code smarter extractor or create a new column on spreadsheet)

* missing some confirmation checkboxes on critical stuff that can't be reverted

* missing some checks if npp is running on critical stuff that requires npp to be closed to take effect

## Credits:

* ps main development

* daniel steam detection code

* cloudead beta-testing
