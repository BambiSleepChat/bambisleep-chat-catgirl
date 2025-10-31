# Build Instructions - BambiSleep™ CatGirl Avatar System

## Project Status: v1.0.0 - Core Implementation Complete ✅

### Completed Systems (6/6 Core Systems):
1. ✅ **CatgirlController.cs** (327 lines) - Full NetworkBehaviour with pink auras, purring, cow powers
2. ✅ **InventorySystem.cs** (284 lines) - Unity Gaming Services economy integration
3. ✅ **UniversalBankingSystem.cs** (363 lines) - Gambling, auctions, multi-currency
4. ✅ **CatgirlNetworkManager.cs** (324 lines) - Unity Relay + Lobby multiplayer
5. ✅ **InventoryUI.cs** (322 lines) - UI Toolkit pink frilly interface
6. ✅ **AudioManager.cs** (342 lines) - Sound effects + music management

**Total: 1,950+ lines of production Unity C# code**

### Additional Systems (In Progress):
7. 🚧 XR Controller - VR/AR hand tracking support
8. 🚧 Game Manager - Session state management

**See `IMPLEMENTATION_PROGRESS.md` for detailed status.**

---

## Quick Start for New Developers

### Prerequisites
```bash
# Verify Node.js 20+ LTS (managed by Volta)
node --version  # Should show v20.x.x
npm --version   # Should show v10.x.x

# Verify .NET SDK (for Unity C# compilation)
dotnet --info   # Should show .NET 8.0.415 or later
```

### Clone and Setup
```bash
# Clone repository
git clone https://github.com/BambiSleepChat/bambisleep-chat-catgirl.git
cd bambisleep-chat-catgirl

# Install Node.js dependencies
npm install

# Setup MCP servers (automated)
bash ./setup-mcp.sh

# Validate MCP server configuration
bash ./mcp-validate.sh
```

---

## MCP Server Setup

### Install Essential MCP Servers
```bash
# Official servers via npm
npm install -g @modelcontextprotocol/server-filesystem
npm install -g @modelcontextprotocol/server-git  
npm install -g @modelcontextprotocol/server-github
npm install -g @modelcontextprotocol/server-memory
npm install -g @modelcontextprotocol/server-sequential-thinking
npm install -g @modelcontextprotocol/server-everything

# Python-based servers via uvx
uvx --install mcp-server-brave-search
uvx --install mcp-server-postgres
uvx --install mcp-server-sqlite
```

### VS Code MCP Configuration
Add to `.vscode/settings.json`:

```json
{
  "mcp.servers": {
    "filesystem": {
      "command": "npx",
      "args": ["-y", "@modelcontextprotocol/server-filesystem", "/workspace"]
    },
    "git": {
      "command": "npx", 
      "args": ["-y", "@modelcontextprotocol/server-git", "--repository", "/workspace"]
    },
    "github": {
      "command": "npx",
      "args": ["-y", "@modelcontextprotocol/server-github"]
    },
    "memory": {
      "command": "npx",
      "args": ["-y", "@modelcontextprotocol/server-memory"]
    },
    "sequential-thinking": {
      "command": "npx",
      "args": ["-y", "@modelcontextprotocol/server-sequential-thinking"]
    },
    "everything": {
      "command": "npx",
      "args": ["-y", "@modelcontextprotocol/server-everything"]
    }
  }
}
```

## Unity Project Setup

### Create Unity Project Structure
```bash
mkdir -p catgirl-avatar-project/{Assets,ProjectSettings,Packages,Logs,Temp,UserSettings}
cd catgirl-avatar-project

# Initialize Unity project structure
cat > ProjectSettings/ProjectVersion.txt << 'EOF'
m_EditorVersion: 6000.2.11f1
m_EditorVersionWithRevision: 6000.2.11f1 (0773b680dc03)
EOF

# Create essential directories
mkdir -p Assets/{Scripts,Materials,Textures,Models,Animations,Prefabs,UI,Audio,Scenes}
mkdir -p Assets/Scripts/{Character,Inventory,Economy,Networking,UI,Audio}
```

### Package Dependencies (Packages/manifest.json)
```json
{
  "dependencies": {
    "com.unity.addressables": "2.3.1",
    "com.unity.animation.rigging": "1.3.1", 
    "com.unity.cinemachine": "2.10.1",
    "com.unity.netcode.gameobjects": "2.0.0",
    "com.unity.services.analytics": "5.1.1",
    "com.unity.services.authentication": "3.3.4",
    "com.unity.services.core": "1.15.0",
    "com.unity.services.economy": "3.4.2",
    "com.unity.timeline": "1.8.7",
    "com.unity.ui.toolkit": "2.0.0",
    "com.unity.purchasing": "4.12.2"
  }
}
```

## Development Environment

### Node.js Setup
```bash
# Install Node.js 20 LTS with Volta
volta install node@20-lts
volta pin node@20-lts
```

### Build Commands (from RELIGULOUS_MANTRA.md)
```bash
# Test with 100% coverage
npm test -- --coverage=100

# Universal build
npm run build -- --universal

# Deploy with AIGF mode
npm run deploy -- --aigf-mode
```

## Container Build

### Container Labels
```dockerfile
LABEL org.opencontainers.image.vendor="BambiSleepChat"
LABEL org.opencontainers.image.source="https://github.com/BambiSleepChat/bambisleep-church"
LABEL org.opencontainers.image.title="BambiSleep Church MCP Control Tower"
LABEL org.bambi.trademark="BambiSleep™ is a trademark of BambiSleepChat"
```

### Container Registry
Push to: `ghcr.io/bambisleepchat/bambisleep-church`

Tagging convention:
- `latest` - Latest stable release
- `main` - Latest main branch build  
- `v{major}.{minor}.{patch}` - Semantic version releases
- `dev-{branch}` - Development branches
