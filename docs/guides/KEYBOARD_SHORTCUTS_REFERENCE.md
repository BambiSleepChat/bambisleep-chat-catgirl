# ⌨️ Unity + VS Code Debugging Keyboard Shortcuts

## 🎮 Essential Debugging Shortcuts

### Start & Control Debugging

| Shortcut | Action | Description |
|----------|--------|-------------|
| **F5** | Start Debugging | Attach to Unity Editor and start Play mode |
| **Ctrl+F5** | Run Without Debugging | Launch without breakpoints |
| **Shift+F5** | Stop Debugging | Detach debugger and stop Play mode |
| **Ctrl+Shift+F5** | Restart Debugging | Restart debug session |
| **F6** | Pause Execution | Pause the running code |

### Code Navigation During Debug

| Shortcut | Action | Description |
|----------|--------|-------------|
| **F10** | Step Over | Execute current line, skip function internals |
| **F11** | Step Into | Step into function/method call |
| **Shift+F11** | Step Out | Step out of current function |
| **Ctrl+Shift+F10** | Run to Cursor | Execute until cursor position |
| **F9** | Toggle Breakpoint | Add/remove breakpoint at current line |
| **Ctrl+Shift+F9** | Remove All Breakpoints | Clear all breakpoints in project |

### Breakpoint Management

| Shortcut | Action | Description |
|----------|--------|-------------|
| **F9** | Toggle Breakpoint | Add or remove breakpoint |
| **Ctrl+K Ctrl+B** | Enable/Disable Breakpoint | Keep breakpoint but disable temporarily |
| **Right-click → Edit** | Conditional Breakpoint | Set condition or hit count |
| **Right-click → Logpoint** | Add Logpoint | Log message without stopping |

## 🔍 VS Code General Shortcuts

### File Navigation

| Shortcut | Action | Description |
|----------|--------|-------------|
| **Ctrl+P** | Quick Open | Open file by name |
| **Ctrl+Shift+F** | Find in Files | Search across entire project |
| **Ctrl+T** | Go to Symbol | Jump to class/method/variable |
| **F12** | Go to Definition | Jump to symbol definition |
| **Alt+F12** | Peek Definition | View definition inline |
| **Shift+F12** | Find All References | See all usages of symbol |
| **Ctrl+G** | Go to Line | Jump to specific line number |

### Editor Control

| Shortcut | Action | Description |
|----------|--------|-------------|
| **Ctrl+B** | Toggle Sidebar | Show/hide file explorer |
| **Ctrl+J** | Toggle Panel | Show/hide terminal/output |
| **Ctrl+Shift+D** | Debug View | Open debug panel |
| **Ctrl+Shift+E** | Explorer | Open file explorer |
| **Ctrl+Shift+G** | Source Control | Open Git panel |
| **Ctrl+\`** | Toggle Terminal | Open integrated terminal |

### Code Editing

| Shortcut | Action | Description |
|----------|--------|-------------|
| **Ctrl+Space** | Trigger Suggestions | Open IntelliSense |
| **Ctrl+.** | Quick Fix | Show code actions |
| **F2** | Rename Symbol | Rename across all files |
| **Alt+Up/Down** | Move Line | Move current line up/down |
| **Ctrl+/** | Toggle Comment | Comment/uncomment line |
| **Shift+Alt+F** | Format Document | Auto-format C# code |

## 🎯 Unity Editor Shortcuts (When Focused)

### Playmode Control

| Shortcut | Action | Description |
|----------|--------|-------------|
| **Ctrl+P** | Play/Pause | Start or pause Play mode |
| **Ctrl+Shift+P** | Step Frame | Advance one frame while paused |
| **Ctrl+R** | Recompile Scripts | Force script recompilation |

### Scene Navigation

| Shortcut | Action | Description |
|----------|--------|-------------|
| **F** | Frame Selected | Focus camera on selected object |
| **Q** | Pan Tool | Move view |
| **W** | Move Tool | Move selected object |
| **E** | Rotate Tool | Rotate selected object |
| **R** | Scale Tool | Scale selected object |
| **T** | Rect Tool | UI rect transform tool |

### Viewport Control

| Shortcut | Action | Description |
|----------|--------|-------------|
| **Right-click + Drag** | Look Around | Rotate camera |
| **Middle-click + Drag** | Pan View | Move camera laterally |
| **Scroll Wheel** | Zoom | Zoom in/out |
| **Alt + Left-click + Drag** | Orbit | Orbit around pivot point |

### Windows & Panels

| Shortcut | Action | Description |
|----------|--------|-------------|
| **Ctrl+1** | Scene View | Focus Scene window |
| **Ctrl+2** | Game View | Focus Game window |
| **Ctrl+3** | Inspector | Focus Inspector |
| **Ctrl+4** | Hierarchy | Focus Hierarchy |
| **Ctrl+5** | Project | Focus Project browser |
| **Ctrl+0** | Console | Open Console window |
| **Ctrl+9** | Asset Store | Open Asset Store |

## 🐛 Debug-Specific VS Code Shortcuts

### Debug Panel

| Shortcut | Action | Description |
|----------|--------|-------------|
| **Ctrl+Shift+Y** | Debug Console | Open debug console |
| **Ctrl+K Ctrl+I** | Show Hover | Show variable value tooltip |

### Watch & Variables

| Action | How To | Description |
|--------|--------|-------------|
| Add to Watch | Right-click variable → "Add to Watch" | Monitor variable value |
| Copy Value | Right-click in Variables → "Copy Value" | Copy variable contents |
| Set Value | Right-click variable → "Set Value" | Change value during debug |

## 🌸 BambiSleep™ Custom Workflows

### Rapid NetworkBehaviour Debugging

```plaintext
1. F12 on NetworkBehaviour class → Go to definition
2. F9 on OnNetworkSpawn() → Set breakpoint
3. F5 → Attach debugger
4. Unity Play mode starts → Breakpoint hits
5. Ctrl+K Ctrl+I on NetworkObject → Check network state
6. F10 → Step through network initialization
7. Shift+F12 on RPC method → Find all RPC calls
```

### Async/Await Debugging Pattern

```plaintext
1. Set breakpoint BEFORE await statement
2. F10 to step over await
3. Breakpoint AFTER await to see result
4. Watch expression: Task.Status
5. Debug Console: Check AuthenticationService.Instance.IsSignedIn
```

### Multi-Player Debugging

```plaintext
1. Open 2 VS Code windows (Ctrl+Shift+N)
2. Window 1: Attach to Unity Editor (Host)
3. Window 2: Attach to Standalone Build (Client)
4. Set breakpoints in both:
   - Host: [ServerRpc] methods
   - Client: [ClientRpc] methods
5. Step through simultaneously
```

## 💡 Pro Tips

### Keyboard Maestro Combinations

```plaintext
# Quick breakpoint in NetworkBehaviour lifecycle
Ctrl+P → Type "CatgirlController.cs" → Enter
Ctrl+G → Type "112" → Enter (OnNetworkSpawn line)
F9 → Set breakpoint
F5 → Start debugging

# Fast symbol search across project
Ctrl+T → Type "AddCurrency" → Enter
F12 → Go to implementation
F9 → Breakpoint on currency logic

# Rapid error investigation
Ctrl+Shift+M → Open Problems panel
Click error → Jump to code
F9 → Breakpoint on error line
F5 → Debug

# Quick RPC debugging
Ctrl+Shift+F → Search "[ServerRpc]"
Navigate results with F4/Shift+F4
F9 on each RPC method → Set breakpoints
F5 → Debug network calls
```

### Custom Keybinding Examples (settings.json)

```json
{
  "key": "ctrl+shift+u",
  "command": "workbench.action.tasks.runTask",
  "args": "Check Unity Version"
},
{
  "key": "ctrl+shift+b",
  "command": "workbench.action.debug.selectandstart",
  "args": "Attach to Unity Editor and Play"
}
```

## 📚 Shortcut Cheat Sheet (Print-Friendly)

### Must-Know Top 10

1. **F5** - Start Debug (most important!)
2. **F9** - Toggle Breakpoint
3. **F10** - Step Over
4. **F11** - Step Into
5. **Shift+F5** - Stop Debug
6. **Ctrl+Shift+D** - Debug Panel
7. **F12** - Go to Definition
8. **Ctrl+P** - Quick Open File
9. **Ctrl+Shift+F** - Find in Files
10. **Ctrl+.** - Quick Fix/Actions

### Unity-Specific Top 5

1. **Ctrl+P** (Unity) - Play/Pause Mode
2. **Ctrl+R** (Unity) - Recompile Scripts
3. **Ctrl+0** (Unity) - Console Window
4. **F** (Unity) - Frame Selected Object
5. **Ctrl+Shift+P** (Unity) - Step Frame

---

## 🎓 Learning Path

### Beginner (Week 1)
- Master: F5, F9, F10, Shift+F5
- Learn: Setting breakpoints and stepping through code
- Practice: Debug simple Start() and Update() methods

### Intermediate (Week 2)
- Master: F11, Shift+F11, Ctrl+Shift+F10
- Learn: Conditional breakpoints and watch expressions
- Practice: Debug NetworkBehaviour RPC calls

### Advanced (Week 3+)
- Master: Custom keybindings, multi-window debugging
- Learn: Logpoints, data breakpoints, debug console evaluation
- Practice: Debug async/await UGS calls, complex multiplayer scenarios

---

🌸 **Print this guide and keep it next to your keyboard!** 🌸
