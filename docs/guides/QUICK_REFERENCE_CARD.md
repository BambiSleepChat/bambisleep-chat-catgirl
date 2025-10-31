# 🎴 Unity Debugging Quick Reference Card

**Print this page and keep it visible while coding!**

---

## 🚀 Launch Sequence

```bash
1. ./setup-unity-debug.sh      # Validate environment
2. ./launch-unity-debug.sh      # Open Unity (or use Unity Hub)
3. Unity → Play mode            # Enter Play mode
4. VS Code → Press F5           # Attach debugger
```

---

## ⌨️ Top 10 Shortcuts

| Key | Action | Use Case |
|-----|--------|----------|
| **F5** | Start Debug | Attach to Unity & start Play |
| **F9** | Toggle Breakpoint | Set/remove breakpoint |
| **F10** | Step Over | Next line (skip functions) |
| **F11** | Step Into | Enter function |
| **Shift+F11** | Step Out | Exit function |
| **Shift+F5** | Stop Debug | Detach & stop Play mode |
| **F12** | Go to Definition | Jump to code |
| **Ctrl+Shift+F** | Find in Files | Search project |
| **Ctrl+P** | Quick Open | Open file by name |
| **Ctrl+.** | Quick Fix | Show code actions |

---

## 🎯 First Breakpoints

**CatgirlController.cs:**
- Line 76 - `Start()` initialization
- Line 112 - `OnNetworkSpawn()` network setup
- Line 150 - `Update()` game loop
- Line 210 - `UpdatePositionServerRpc()` network call

**InventorySystem.cs:**
- Line 89 - `AddItem()` item handling
- Line 150 - `InitializeEconomy()` UGS setup

**UniversalBankingSystem.cs:**
- Line 144 - `AddCurrency()` currency system
- Line 290 - `PlaceBidServerRpc()` auction logic

---

## 👀 Watch Expressions

```csharp
// Network State
IsOwner
OwnerClientId
NetworkObject.IsSpawned
NetworkManager.Singleton.IsServer

// Character State
currentSpeed
transform.position
networkPosition.Value
isGrounded

// Economy State
pinkCoins
cowTokens
AuthenticationService.Instance.IsSignedIn

// Inventory State
items.Count
items[0].displayName
```

---

## 🐛 Common Issues

| Symptom | Fix |
|---------|-----|
| "No Unity process" | Unity must be in Play mode |
| Breakpoint not hitting | Check script attached to GameObject |
| RPC not working | Verify `NetworkObject.IsSpawned == true` |
| UGS fails | Check init order: Services → Auth → Economy |
| Animator stuck | Verify hash IDs: `Animator.StringToHash("Speed")` |

---

## 💡 Pro Tips

**Conditional Breakpoint:**
```
Right-click breakpoint → Edit → Condition:
currentSpeed > 5.0f
```

**Logpoint (no pause):**
```
Right-click line → Add Logpoint → Message:
Speed: {currentSpeed}, Position: {transform.position}
```

**Debug Console Evaluation:**
```
While paused, type in Debug Console:
> currentSpeed * 2
> GameObject.Find("CatGirl").name
```

---

## 📚 Documentation Quick Links

- **Breakpoints**: `BREAKPOINTS_REFERENCE.md`
- **Quick Start**: `docs/guides/UNITY_DEBUG_QUICK_START.md`
- **Shortcuts**: `docs/guides/KEYBOARD_SHORTCUTS_REFERENCE.md`
- **Troubleshooting**: `docs/guides/UNITY_DEBUG_TROUBLESHOOTING.md`
- **Setup**: `docs/guides/DEBUG_SETUP_COMPLETE.md`

---

## 🎓 First Debug Session Checklist

- [ ] Unity Editor installed (6000.2.11f1)
- [ ] Project opened in Unity
- [ ] External editor set to VS Code
- [ ] .csproj files regenerated
- [ ] No compile errors in Unity Console
- [ ] CatgirlController.cs opened in VS Code
- [ ] Breakpoint set on line 112 (F9)
- [ ] Watch expressions added
- [ ] Unity in Play mode
- [ ] VS Code attached (F5)
- [ ] Breakpoint hits! 🎉

---

## 🌸 BambiSleep™ Debug Emoji Guide

- 🦋 NetworkBehaviour events
- 💎 High-value features  
- 🐄 Secret cow powers
- 🔥 Performance-critical code
- ✨ UI polish

---

## 🆘 Emergency Commands

```bash
# Clean Unity project
rm -rf catgirl-avatar-project/{Library,Temp,obj}

# Restart everything
pkill -9 Unity && pkill -9 code
# Then reopen Unity first, VS Code second

# Reinstall vstuc
code --uninstall-extension visualstudiotoolsforunity.vstuc
code --install-extension visualstudiotoolsforunity.vstuc
```

---

## 📞 Quick Help

**Can't attach debugger?**
1. Unity in Play mode? ✓
2. External editor configured? ✓
3. vstuc extension enabled? ✓
4. Restarted both Unity & VS Code? ✓

**Breakpoints not hitting?**
1. Script attached to GameObject? ✓
2. GameObject active in Hierarchy? ✓
3. No compile errors? ✓
4. Breakpoint on executable line? ✓

**Network issues?**
1. NetworkManager started? ✓
2. NetworkObject spawned? ✓
3. Correct ownership? ✓

---

**🌸 Happy Debugging! 🌸**

**Remember:** Press F5 to debug, F9 for breakpoints, F10 to step!

---

_Version: Unity 6.2 LTS (6000.2.11f1) | VS Code with vstuc | BambiSleep™ CatGirl Avatar Project_
