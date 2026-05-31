# 🚀 Developer Setup Guide - Demonteks

Welcome to the **Demonteks** development workspace! The game is **100% complete and fully playable (v1.0.0)**. This document guides you through setting up your environment, compiling the C# solution, running the game, and configuring your local editor.

---

## 📋 Prerequisites

Before opening the project, ensure your machine has the following tools installed:

1. **Godot Engine 4.6 (with .NET Support)**:
   - Make sure you download the **.NET version** (Mono-enabled) of Godot 4.6. The standard C++ version will not compile C# scripts.
   - [Download Godot 4.6 .NET](https://godotengine.org/download)

2. **.NET SDK 8.0+**:
   - The game relies on .NET 8.0 features. Ensure you have the SDK installed, not just the Runtime.
   - Run `dotnet --version` in your terminal to verify.
   - [Download .NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)

3. **C# IDE / Code Editor**:
   - **VS Code**: Highly recommended with the `C# Dev Kit` and `Godot Tools` extensions.
   - **Visual Studio 2022** or **JetBrains Rider**: Fully supported out of the box.

---

## 🛠️ Step 1: Open the Project in Godot

1. Launch the **Godot Engine** launcher.
2. Click **Import** on the right.
3. Navigate to your local `Demonteks` directory and select `project.godot`.
4. Click **Import & Edit** to open the project in the editor.

---

## 🏗️ Step 2: Compile the C# Solution

Since all scripts are written in C#, you **must compile the project assemblies** before running it.

1. Once Godot opens, locate the **"Build"** button in the top-right corner of the editor interface (next to the Play buttons).
2. Click **Build**.
3. Alternatively, you can build from your terminal:
   ```bash
   # Build debug assemblies
   godot --build-solutions

   # Or compile via dotnet directly
   dotnet build
   ```

---

## 🎮 Step 3: Run the Game

1. In the Godot editor, press **`F5`** or click the **Play** button in the top-right corner.
2. The game starts on the **Main Menu** (`res://Scenes/UI/MainMenu.tscn`).
3. Click **New Game** to start Level 1 (Jungle), or **Continue** to load your latest checkpoint!

---

## 🔧 Step 4: Configure Your Code Editor

### Visual Studio Code Setup
To get autocomplete and C# debugging running in VS Code, create a `.vscode/launch.json` in the root folder:

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "Play Demonteks (Debug)",
      "type": "coreclr",
      "request": "launch",
      "program": "C:/Path/To/Your/Godot_v4.6-stable_mono_win64/Godot_v4.6-stable_mono_win64.exe",
      "args": ["--path", "${workspaceRoot}"],
      "cwd": "${workspaceRoot}",
      "stopAtEntry": false
    }
  ]
}
```
*(Replace `C:/Path/To/Your/Godot...` with the actual absolute path to your Godot Mono executable).*

---

## 🧪 Step 5: Playtesting & Scene Running

### Testing Individual Levels
If you want to playtest a specific level directly without progressing through the whole game:
1. Open the level scene in the editor:
   - `res://Scenes/Levels/Level1_Jungle.tscn`
   - `res://Scenes/Levels/Level2_Dungeon.tscn`
   - `res://Scenes/Levels/Level3_Castle.tscn`
2. Press **`F6`** (or click the "Play Current Scene" button in the top-right).
3. The game will run just that level, spawning you at the default start location.

### Testing specific components
To test the player or enemies in a sandbox environment:
1. Create a blank 2D scene (`Scene > New Scene > 2D Scene`).
2. Drag and drop `res://Scenes/Characters/Player.tscn` into the viewport.
3. Drag in any enemy variant from `res://Scenes/Characters/Enemies/`.
4. Run the scene with **`F6`** to verify behaviors in isolation.

---

## ⚠️ Troubleshooting Common Setup Issues

### 🔴 Error: "Cannot find type or namespace"
*   **Cause**: The C# assembly has not been built, or the editor cache is stale.
*   **Solution**: Click the **Build** button in the top-right of Godot. If issues persist, close Godot, delete the `.godot/mono/` folder, reopen the project, and click **Build** again.

### 🔴 Error: "Out of Bounds / Infinite Loop in AI"
*   **Cause**: The project's physics snap constraints are out of alignment.
*   **Solution**: Ensure `Default Texture Filter` under Project Settings is set to **Nearest** and `Snap 2D Transforms to Pixel` is enabled.
