# ⚡ Developer Quick Start Cheat-Sheet

A practical reference sheet for common development operations, build commands, testing shortcuts, and cheat commands in the **Demonteks** C# workspace.

---

## 🚀 Speed Commands (Terminal)

Run these commands in the root `Demonteks` directory:

```bash
# Build the C# solution in Debug configuration
dotnet build

# Compile release assemblies for final packaging
dotnet build -c Release

# Clear all C# and Godot caches
Remove-Item -Path ".godot/mono/", "bin/", "obj/" -Recurse -Force -ErrorAction SilentlyContinue
```

---

## 🧪 Quick Test & Cheat Mappings

For rapid playtesting, you can tweak values directly in the Godot Inspector, or use the following codebase shortcuts:

### 1. Maximize Player Health (Invincibility)
To make the player invincible for testing Level 3 castle traps:
- Open `res://Scripts/Player/PlayerStats.cs`
- Locate line 14: `[Export] public int MaxHealth { get; set; } = 100;`
- Temporarily change `MaxHealth` to `99999` in the script, or override the value under the **PlayerStats** node inspector in `Player.tscn`.

### 2. Equip All Weapons Programmatically
To unlock all weapons instantly upon spawning:
- Open `res://Scripts/Player/PlayerCombat.cs`
- In `_Ready()` (line 47), add this testing override right before `EquipWeapon`:
  ```csharp
  // TESTING ONLY: Unlock all weapons
  foreach (var weapon in _weapons.Values) {
      weapon.IsUnlocked = true;
  }
  ```
- Now, pressing `Q` or `Z` in-game will allow you to cycle through all 6 weapons immediately!

### 3. Quick Transition to Specific Levels
To test transitions or specific boss scenarios:
- Set the `Run/Main Scene` in `project.godot` to load that level directly.
- Or, programmatically transition from `GameManager.cs` using:
  ```csharp
  // Transition to Level 3 immediately for boss testing
  GetNode<GameManager>("/root/GameManager").LoadLevel(3);
  ```

---

## 📦 Build & Packaging Mappings

Make sure your target export templates are downloaded in Godot (`Editor > Manage Export Templates`).

### 1. Windows Desktop (x86_64)
```bash
# Clean release build
dotnet build -c Release

# Export production EXE
godot --export-release "Windows Desktop" demonteks.exe
```

### 2. Android (ARM64-v8a / ARMEABI-v7a)
Ensure your release keystore `demonteks.keystore` is placed in the project root.
```bash
# Generate release Keystore if missing
keytool -genkey -v -keystore demonteks.keystore -alias demonteks -keyalg RSA -keysize 2048 -validity 10000

# Export release APK
godot --export-release Android demonteks.apk
```

### 3. iOS (Xcode project export, macOS only)
```bash
# Export the Xcode solution files to a target directory
godot --export-release iOS build_ios/

# Open in Xcode to compile and sign
open build_ios/demonteks.xcodeproj
```

---

## 📊 Code Standards Checklist
- **Naming Conventions**: Use `PascalCase` for public methods/properties, and `_camelCase` with a leading underscore for private fields.
- **Node Referencing**: Use Godot 4 `[Export]` annotations for node paths or node references rather than hardcoding string paths with `GetNode` in ready scripts.
- **Signals**: Always use Godot 4 C# signal naming conventions (`[Signal] public delegate void EventNameEventHandler()`).
