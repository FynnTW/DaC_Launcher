
<div align="center">
  <h1>Divide and Conquer: V5 Launcher</h1>
  <img src="https://i.imgur.com/SmTqhiE.png" width="1920" alt="DaC Launcher" /></a>
</div>

The following is a repo for the [Divide and Conquer V5](https://www.moddb.com/mods/divide-and-conquer) Launcher. The program is used to configure different aspects of the mod, ensure LAA is applied and to launch the mod.

  <img src="https://i.imgur.com/QijEgXi.png" width="1920" alt="DaC Launcher" /></a>

## Configuration Options
The launcher currently provides the following configuration options

- **Permanent Arrows:** Ensures that arrows embedded in the ground stay for the duration of the battle and do not disappear. Has a small performance impact for low-end PCs.
- **Bypass Launcher:** Next time you go to launch the game via the launcher, this settings menu will not appear and you will load straight into the game (To revert this, open `Dac_Config.json` and set `StartInstantly` to `false`.)

## Download
The launcher is included as part of the DaC install process but if for whatever reason you want to pick up the latest version, you can grab the latest artifact from the latest commit [here](link).

## Building from source

#### [Install .NET 6.0 and verify installation](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

`dotnet --version`

#### Install the dependencies
`dotnet restore`

#### Build the executable

`dotnet publish -c Release --no-restore`

#### Run the executable
`bin\Release\net6.0-windows\win-x86\publish\DaC_Launcher.exe`
