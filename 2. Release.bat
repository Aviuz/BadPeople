:: ========= Variables =========

SET mod_name=BadPeople
SET steam_id=1369675292
SET github_release=https://github.com/Aviuz/BadPeople/releases
SET ludeon_thread=https://ludeon.com/forums/index.php?topic=41303.0
SET steam_changelog=https://steamcommunity.com/sharedfiles/filedetails/changelog/%steam_id%
SET steam_description=https://steamcommunity.com/sharedfiles/itemedittext/?id=%steam_id%

SET target_directory=D:\Program Files\Steam\steamapps\common\RimWorld\Mods\%mod_name%
SET zip_directory=C:\Users\avius\Desktop\%mod_name%.zip


:: ========= Zip archive ==========
 
"D:\Program Files\WinRAR\Rar.exe" a -ep1 -r "%zip_directory%" "%target_directory%\*"


:: ========= Run ==========

start %github_release%
start %steam_changelog%
start %steam_description%
start %ludeon_thread%
start steam://rungameid/294100