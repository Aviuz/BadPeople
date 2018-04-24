:: ========= Copy ==========
 
rd "D:\Program Files\Steam\steamapps\common\RimWorld\Mods\BadPeople" /s /q
mkdir "D:\Program Files\Steam\steamapps\common\RimWorld\Mods\BadPeople"

:: About
mkdir "D:\Program Files\Steam\steamapps\common\RimWorld\Mods\BadPeople\About"
xcopy "About\*.*" "D:\Program Files\Steam\steamapps\common\RimWorld\Mods\BadPeople\About" /e

:: Assemblies
mkdir "D:\Program Files\Steam\steamapps\common\RimWorld\Mods\BadPeople\Assemblies"
xcopy "Assemblies\*.*" "D:\Program Files\Steam\steamapps\common\RimWorld\Mods\BadPeople\Assemblies" /e

:: Defs 
mkdir "D:\Program Files\Steam\steamapps\common\RimWorld\Mods\BadPeople\Defs"
xcopy "Defs\*.*" "D:\Program Files\Steam\steamapps\common\RimWorld\Mods\BadPeople\Defs" /e

:: Languages
mkdir "D:\Program Files\Steam\steamapps\common\RimWorld\Mods\BadPeople\Languages"
xcopy "Languages\*.*" "D:\Program Files\Steam\steamapps\common\RimWorld\Mods\BadPeople\Languages" /e

:: Textures
mkdir "D:\Program Files\Steam\steamapps\common\RimWorld\Mods\BadPeople\Textures"
xcopy "Textures\*.*" "D:\Program Files\Steam\steamapps\common\RimWorld\Mods\BadPeople\Textures" /e

:: changelog.txt
copy "changelog.txt" "D:\Program Files\Steam\steamapps\common\RimWorld\Mods\BadPeople\changelog.txt"



:: ========= Run ==========

start steam://rungameid/294100