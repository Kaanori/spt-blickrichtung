# Blickrichtung (look direction) (SPT 4.1.6)

While the look-around key is held (middle mouse button by default), only the head turns.
On release the head normally snaps back to the front.
This plugin reverses that: **on release the body turns to the look direction**, and the camera stays where it is.

Settings under F12 -> Blickrichtung:
- **Aktiv** (active) – on/off
- **Hoehe uebernehmen** (adopt pitch) – also adopt the look up/down (off = left/right only)

Building: `dotnet build -c Release` – copies the DLL to `<game folder>/BepInEx/plugins/`.

## Requirements

- SPT 4.1.6 (EFT 0.16.9). BepInEx ships with SPT.

## Installation

1. Download `Blickrichtung-1.0.0.zip` from the [Releases](https://github.com/Kaanori/spt-blickrichtung/releases) page.
2. Extract it **into your SPT game folder** (the folder that contains `EscapeFromTarkov.exe`).
   The ZIP already has the right layout; the mod ends up in `BepInEx/plugins/`.
   On Windows, make sure "Extract All" does not add an extra subfolder.
3. Restart the game.

## Uninstall

Delete the mod's files from `BepInEx/plugins/` (settings: `BepInEx/config/` for client mods) and restart the game.

## Building from source

Needs the .NET 10 SDK and an SPT 4.1.6 installation to build against.

```bash
./build.sh             # Linux: build only
./build.sh --install   # build and copy into the game folder
```
```powershell
.\build.ps1            # Windows
.\build.ps1 -Install
```

The game folder is set in `Directory.Build.props` (Windows default `C:/SPT`)
or via the environment variable `SPT_DIR`.

## Support

Bugs and questions: [GitHub issues](https://github.com/Kaanori/spt-blickrichtung/issues).

## License

MIT — see [LICENSE](LICENSE).
