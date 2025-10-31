# 📁 Project Organization Guide

## 🌸 Directory Structure

```
bambisleep-chat-catgirl/
├── 📚 docs/                       # All documentation
│   ├── architecture/              # System design & specifications
│   ├── development/               # Setup guides & implementation
│   ├── guides/                    # Build instructions & workflows
│   └── reference/                 # Changelog & project history
│
├── 🎮 catgirl-avatar-project/     # Unity 6.2 LTS project
│   ├── Assets/                    # Unity assets & scripts
│   │   ├── Scripts/               # C# source code (1,950+ lines)
│   │   │   ├── Audio/             # AudioManager.cs
│   │   │   ├── Character/         # CatgirlController.cs
│   │   │   ├── Economy/           # InventorySystem, UniversalBankingSystem
│   │   │   ├── IPC/               # Unity ↔ Node.js bridge
│   │   │   ├── Networking/        # CatgirlNetworkManager.cs
│   │   │   └── UI/                # InventoryUI.cs
│   │   ├── Materials/             # Unity materials
│   │   ├── Textures/              # Texture assets
│   │   ├── Models/                # 3D models
│   │   ├── Animations/            # Animation clips
│   │   ├── Prefabs/               # Unity prefabs
│   │   ├── Audio/                 # Sound effects & music
│   │   ├── Scenes/                # Unity scenes
│   │   ├── Resources/             # Runtime-loadable assets
│   │   ├── StreamingAssets/       # Streaming assets
│   │   └── Plugins/               # Native plugins
│   ├── Packages/                  # Unity package manager
│   ├── ProjectSettings/           # Unity project configuration
│   └── README.md                  # Unity project documentation
│
├── 💻 src/                        # Node.js application source
│   ├── unity/                     # Unity integration
│   │   └── unity-bridge.js        # IPC communication bridge
│   ├── server/                    # HTTP/WebSocket server (future)
│   │   └── index.js               # Server module
│   └── cli/                       # Command-line interface (future)
│       └── index.js               # CLI module
│
├── 🔧 scripts/                    # Development & setup scripts
│   ├── setup-mcp.sh               # MCP servers installation
│   ├── mcp-validate.sh            # Validate MCP configuration
│   ├── setup-unity-debug.sh       # Unity debugging environment
│   ├── launch-unity-debug.sh      # Quick Unity launcher
│   └── verify-docs-structure.sh   # Documentation validation
│
├── 🛠️ tools/                      # Build tools & utilities (future)
│
├── 🐙 .github/                    # GitHub configuration
│   ├── workflows/                 # CI/CD pipelines
│   │   └── build.yml              # Build & deploy workflow
│   └── copilot-instructions.md    # AI agent guidance (750+ lines)
│
├── 🎨 .vscode/                    # VS Code configuration
│   ├── settings.json              # Editor settings & MCP config
│   ├── tasks.json                 # Build tasks
│   └── launch.json                # Debug configurations
│
├── 📄 Root Files
│   ├── index.js                   # Main Node.js entry point
│   ├── package.json               # Node.js project configuration
│   ├── Dockerfile                 # Container definition
│   ├── README.md                  # Main project documentation
│   ├── .gitignore                 # Git ignore rules
│   └── .editorconfig              # Editor configuration
│
└── 🔒 Generated/Ignored
    ├── node_modules/              # Node.js dependencies (git ignored)
    ├── catgirl-avatar-project/Library/    # Unity cache (git ignored)
    ├── catgirl-avatar-project/Temp/       # Unity temp files (git ignored)
    └── catgirl-avatar-project/Logs/       # Unity logs (git ignored)
```

## 🎯 Quick Navigation

### Documentation
- **Start Here**: [`docs/README.md`](../docs/README.md)
- **Unity Setup**: [`docs/development/UNITY_SETUP_GUIDE.md`](../docs/development/UNITY_SETUP_GUIDE.md)
- **Architecture**: [`docs/architecture/CATGIRL.md`](../docs/architecture/CATGIRL.md)
- **Build Guide**: [`docs/guides/build.md`](../docs/guides/build.md)
- **Todo List**: [`docs/guides/todo.md`](../docs/guides/todo.md)

### Scripts
- **MCP Setup**: `npm run mcp:setup` or `bash scripts/setup-mcp.sh`
- **Unity Debug**: `npm run unity:debug` or `bash scripts/setup-unity-debug.sh`
- **Unity Launch**: `npm run unity:launch` or `bash scripts/launch-unity-debug.sh`
- **Validate Docs**: `npm run docs:verify` or `bash scripts/verify-docs-structure.sh`

### Source Code
- **Unity C#**: `catgirl-avatar-project/Assets/Scripts/`
- **Node.js**: `src/`
- **Entry Point**: `index.js`

## 📦 Organization Principles

### 1. **Separation of Concerns**
- **Unity Project** → Self-contained in `catgirl-avatar-project/`
- **Node.js Code** → Organized in `src/` by domain
- **Scripts** → Utility scripts in `scripts/`
- **Documentation** → Centralized in `docs/`

### 2. **Standard Unity Layout**
- Follows Unity's recommended Assets folder structure
- All standard directories present (Materials, Prefabs, Scenes, etc.)
- Scripts organized by domain (Audio, Character, Economy, etc.)

### 3. **Node.js Best Practices**
- Source code in `src/` directory
- Modules organized by purpose (unity, server, cli)
- Entry point at root (`index.js`)
- Dependencies in `package.json`

### 4. **Tool Organization**
- Development scripts in `scripts/` (setup, validation)
- Build tools in `tools/` (future)
- All scripts maintain executable permissions

### 5. **Documentation Structure**
- Hierarchical organization by category
- Cross-referenced with hyperlinks
- AI agent instructions in `.github/`

## 🚀 Getting Started

### Initial Setup
```bash
# 1. Install dependencies
npm install

# 2. Display system info
npm start

# 3. Setup MCP servers
npm run mcp:setup

# 4. Validate MCP configuration
npm run mcp:validate

# 5. Setup Unity debugging
npm run unity:debug

# 6. Verify documentation
npm run docs:verify
```

### Development Workflow
```bash
# Start Unity project
npm run unity:launch

# Or manually open Unity Hub and load:
# /path/to/bambisleep-chat-catgirl/catgirl-avatar-project

# In VS Code, press F5 to attach debugger
```

## 📋 File Naming Conventions

### Unity Assets
- **Scripts**: PascalCase (e.g., `CatgirlController.cs`)
- **Scenes**: PascalCase (e.g., `MainScene.unity`)
- **Prefabs**: PascalCase (e.g., `CatgirlAvatar.prefab`)
- **Materials**: PascalCase (e.g., `PinkFrilly.mat`)

### Node.js Code
- **Modules**: kebab-case (e.g., `unity-bridge.js`)
- **Classes**: PascalCase (e.g., `class UnityBridge`)
- **Constants**: UPPER_SNAKE_CASE (e.g., `MAX_CONNECTIONS`)

### Scripts
- **Shell Scripts**: kebab-case with .sh (e.g., `setup-mcp.sh`)
- **Executable**: chmod +x required

### Documentation
- **Markdown**: UPPER_SNAKE_CASE.md (e.g., `UNITY_SETUP_GUIDE.md`)
- **README**: Always capitalized (e.g., `README.md`)

## 🎨 BambiSleep™ Conventions

### Emoji Usage in Code
- 🌸 Cherry Blossom - Core features, packages
- 🦋 Butterfly - State changes, NetworkBehaviour
- 💎 Gem - High-value features
- 👑 Crown - Authority, enterprise patterns
- 🐄 Cow - Secret features (easter eggs)
- 🔥 Fire - Performance-critical code
- ✨ Sparkles - UI polish, effects

### Commit Message Format
```bash
git commit -m "🦋 Add feature description"
# Emoji prefix required!
```

### Trademark
- Always use `BambiSleep™` (with ™) in public-facing content
- User-facing strings must include trademark symbol

## 🔄 Migration Notes

### October 31, 2025 Reorganization
This structure was established to improve codebase organization:

**Changes Made:**
- ✅ Created `scripts/` directory for all utility scripts
- ✅ Created `src/` directory for Node.js application code
- ✅ Moved debug reference docs to `docs/guides/`
- ✅ Created standard Unity Assets subdirectories
- ✅ Created `index.js` main entry point
- ✅ Updated `package.json` scripts to use new paths
- ✅ Updated `.vscode/tasks.json` script references
- ✅ Updated `Dockerfile` to reference new structure
- ✅ Enhanced `.gitignore` with organization patterns

**Backward Compatibility:**
- All npm scripts still work (now point to new locations)
- VS Code tasks updated automatically
- Documentation paths remain unchanged
- Git history preserved

## 📞 Support

### Issues?
- Check `docs/guides/UNITY_DEBUG_TROUBLESHOOTING.md` for common issues
- Review `.github/copilot-instructions.md` for AI agent guidance
- Consult `docs/development/UNITY_SETUP_GUIDE.md` for setup help

### Contributing
- Follow directory structure conventions
- Maintain emoji commit message format
- Update documentation for new features
- Run validation scripts before committing

---

🌸 **Organization is sacred! Keep the codebase clean and kawaii!** 🌸
