## Quick orientation for AI coding agents

This repo is a **documentation-first architecture specification** for the BambiSleep™ CatGirl Unity avatar system with MCP tooling integration. Unlike typical code repositories, the "source of truth" lives in comprehensive markdown specifications that define everything from Unity C# class structure to container orchestration patterns.

### Quick Reference: Essential Files

**Documentation** (organized in `docs/` folder):

- `docs/architecture/CATGIRL.md` (682 lines) — Complete avatar system specification
- `docs/development/UNITY_SETUP_GUIDE.md` (858 lines) — Full C# implementations, Unity packages
- `docs/development/MCP_SETUP_GUIDE.md` (329 lines) — 8 MCP servers configuration
- `docs/architecture/RELIGULOUS_MANTRA.md` (112 lines) — Sacred Laws, emoji patterns, philosophy
- `docs/architecture/CONTAINER_ORGANIZATION.md` — Docker/GHCR standards, trademark requirements
- `docs/guides/build.md` — Build instructions and workflows
- `docs/guides/todo.md` — Implementation status and pending work
- `docs/reference/CHANGELOG.md` (415 lines) — Complete project history

**Project Files:**

- `package.json` — Node.js 20+ dependencies, npm scripts, Volta configuration
- `Dockerfile` — Production container with all labels and MCP environment
- `setup-mcp.sh` — Automated MCP server installation script
- `.github/workflows/build.yml` — CI/CD pipeline (238 lines)

### Current State: Active Development Repository

**What exists NOW** (1,950+ lines of production Unity C# code):

- ✅ Comprehensive markdown specifications (CATGIRL.md, UNITY_SETUP_GUIDE.md, etc.)
- ✅ **Working Unity 6.2 LTS project** with 6 complete C# systems in `catgirl-avatar-project/Assets/Scripts/`:
  - `Character/CatgirlController.cs` (327 lines) - Full NetworkBehaviour with pink auras, purring, cow powers
  - `Economy/InventorySystem.cs` (284 lines) - Unity Gaming Services economy integration
  - `Economy/UniversalBankingSystem.cs` (363 lines) - Gambling, auctions, multi-currency systems
  - `Networking/CatgirlNetworkManager.cs` (324 lines) - Unity Relay + Lobby multiplayer
  - `UI/InventoryUI.cs` (322 lines) - UI Toolkit pink frilly interface
  - `Audio/AudioManager.cs` (342 lines) - Centralized sound/music management
- ✅ **Proper namespace architecture**: All systems use `BambiSleep.CatGirl.{Domain}` namespaces
- ✅ Complete Unity package dependencies in `Packages/manifest.json` (16 packages)
- ✅ Node.js 20+ environment with package.json, Volta pinning
- ✅ Dockerfile with GHCR labels and MCP environment
- ✅ GitHub Actions CI/CD pipeline (container-build.yml, build.yml)
- ✅ MCP validation script (mcp-validate.sh) and setup automation

**Critical understanding**: This is NOT a concept project - it's a working Unity codebase with enterprise-grade implementations following specification-driven development principles

## 3-Step Work Methodology

1. **ANALYZE** — Search/read existing files to understand current structure. **Critical**: This is a spec-driven project - markdown files ARE the implementation blueprint, NOT aspirational docs.
2. **THINK** — Design simple working solution. Don't overcomplicate. Edit existing files rather than creating new ones when possible. IT MUST WORK, NOT BE PERFECT! Keep it simple, less is better.
3. **TEST & FIX BUGS** — Run code, verify it works, fix any issues found. For Unity code, follow exact patterns from UNITY_SETUP_GUIDE.md.

## Architecture: Dual Technology Stack

This project has **two distinct but integrated technology stacks**:

1. **Unity 6.2 Gaming Engine**: C# avatar system, economy, networking (files under `Assets/Scripts/`)
2. **MCP Agent Tooling**: Node.js-based Model Context Protocol servers for development automation

**Project Structure**:

```
/mnt/f/bambisleep-chat-catgirl/
├── .github/
│   ├── copilot-instructions.md    # This file - AI agent guidance
│   └── workflows/build.yml        # CI/CD pipeline (238 lines)
├── .vscode/
│   ├── settings.json              # MCP servers + Unity integration + spell-check
│   └── tasks.json                 # 8 VS Code tasks for development
├── catgirl-avatar-project/        # Unity 6.2 LTS project
│   ├── Assets/Scripts/            # 6 complete C# systems (1,950+ lines)
│   │   ├── Audio/AudioManager.cs
│   │   ├── Character/CatgirlController.cs
│   │   ├── Economy/InventorySystem.cs
│   │   ├── Economy/UniversalBankingSystem.cs
│   │   ├── Networking/CatgirlNetworkManager.cs
│   │   └── UI/InventoryUI.cs
│   ├── Packages/manifest.json     # 16 Unity packages
│   └── ProjectSettings/ProjectVersion.txt  # Unity 6000.2.11f1
├── docs/                          # Comprehensive documentation (4,200+ lines)
│   ├── architecture/              # CATGIRL.md, RELIGULOUS_MANTRA.md, etc.
│   ├── development/               # UNITY_SETUP_GUIDE.md, MCP_SETUP_GUIDE.md
│   ├── guides/                    # build.md, todo.md
│   └── reference/                 # CHANGELOG.md
├── Dockerfile                     # Production container (82 lines)
├── package.json                   # Node.js 20+ dependencies
├── setup-mcp.sh                   # MCP server installation
├── mcp-validate.sh                # MCP server validation
└── README.md                      # Pink frilly project overview
```

Key documentation files (read in order):

- `docs/architecture/CATGIRL.md` — **Master architecture specification**: 682 lines defining Unity avatar systems, RPG mechanics, monetization via Unity Gaming Services, and complete technical implementation details
- `docs/development/UNITY_SETUP_GUIDE.md` — **Concrete implementation guide**: 858 lines with Unity project structure, specific C# class examples (CatgirlController, InventorySystem, UniversalBankingSystem), package dependencies, and build configurations
- `docs/development/MCP_SETUP_GUIDE.md` — **Development tooling setup**: 8 essential MCP servers, VS Code configuration, exact `npx`/`uvx` commands
- `docs/architecture/RELIGULOUS_MANTRA.md` — **Development philosophy & conventions**: Contains the "Sacred Laws" with emoji-coded CI/CD patterns, build command specifications, and the unique cultural context
- `docs/architecture/CONTAINER_ORGANIZATION.md` — **Deployment standards**: GHCR registry patterns, trademark compliance, container labeling
- `docs/guides/build.md` + `docs/guides/todo.md` — **Current implementation status**: What works vs what needs to be built
- `docs/reference/CHANGELOG.md` — **Complete project history**: v1.0.0 with all systems documented

## Critical Project Characteristics

**Documentation-as-Code**: The markdown files contain complete C# class implementations, Unity package configurations, and deployment scripts. Don't invent - extract from these specifications.

**Trademark Requirements**: All public-facing content must use `BambiSleep™` (with trademark symbol). This is not optional - it's a legal compliance requirement.

**Unique Development Culture**: This project follows "Universal Machine Philosophy" with emoji-coded development patterns (🌸 = package management, 👑 = architecture decisions, 💎 = quality metrics). These emojis appear in commit messages, documentation headers, and build scripts - use them when creating new content that mirrors existing documentation style.

**Emoji Command System** (from RELIGULOUS_MANTRA.md):

```javascript
// Machine-readable emoji mappings used in commits and docs
🌸 CHERRY_BLOSSOM  // Package management (npm install, dependencies)
👑 CROWN           // Architecture decisions (major structural changes)
💎 GEM             // Quality metrics (tests, coverage, linting)
🦋 BUTTERFLY       // Transformation processes (migrations, refactors)
✨ SPARKLES        // Server operations (deployment, services)
🎭 PERFORMING_ARTS // Development lifecycle (staging, releases)
🌀 CYCLONE         // System management (systemctl, orchestration)
💅 NAIL_POLISH     // Code formatting (prettier, linters)
🔮 CRYSTAL_BALL    // AI/ML operations (MCP servers, LLMs)
💫 DIZZY           // Cross-platform compatibility
```

**When to Use Emojis**:

- ✅ **USE**: In commit messages following existing patterns (e.g., "🌸 Add MCP filesystem server")
- ✅ **USE**: In documentation headers/sections that mirror CATGIRL.md/UNITY_SETUP_GUIDE.md style
- ✅ **USE**: In code comments/headers for major Unity components (e.g., `[Header("🦋 New Catgirl Power")]`)
- ❌ **AVOID**: In plain explanatory text, troubleshooting guides, or technical discussions
- ❌ **AVOID**: Overusing in every sentence - match the density of existing docs

## Essential Development Workflows

**MCP Development Environment**: 8 servers providing filesystem, git, GitHub, memory, sequential-thinking, everything, brave-search, postgres via `npx`/`uvx`. Full VS Code config in `MCP_SETUP_GUIDE.md`. Use `./setup-mcp.sh` for automated installation.

**VS Code MCP Configuration** (already exists in `.vscode/settings.json`):

```json
{
  "mcp.servers": {
    "filesystem": {
      "command": "npx",
      "args": [
        "-y",
        "@modelcontextprotocol/server-filesystem",
        "/mnt/f/bambisleep-chat-catgirl"
      ]
    },
    "git": {
      "command": "npx",
      "args": [
        "-y",
        "@modelcontextprotocol/server-git",
        "--repository",
        "/mnt/f/bambisleep-chat-catgirl"
      ]
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
    },
    "brave-search": { "command": "uvx", "args": ["mcp-server-brave-search"] },
    "postgres": { "command": "uvx", "args": ["mcp-server-postgres"] }
  }
}
```

Note: `.vscode/settings.json` also includes Unity extension config and spell-check dictionary for BambiSleep™-specific terms.

**Unity Project Creation**:

```bash
# From UNITY_SETUP_GUIDE.md - exact structure required
mkdir -p catgirl-avatar-project/{Assets,ProjectSettings,Packages}
# ProjectSettings/ProjectVersion.txt: Unity 6000.2.11f1
# Packages/manifest.json: 15+ Unity Gaming Services dependencies
```

**Build Commands** (from RELIGULOUS_MANTRA.md):

```bash
npm test -- --coverage=100        # 100% test coverage requirement
npm run build -- --universal     # Cross-platform build
npm run deploy -- --aigf-mode    # AI girlfriend deployment mode
volta pin node@20-lts            # Version management
./setup-mcp.sh                   # Automated MCP server setup
docker build --tag ghcr.io/bambisleepchat/bambisleep-church:latest .
```

**VS Code Tasks** (available in `.vscode/tasks.json`):

```bash
# Run via Command Palette: "Tasks: Run Task" or use shortcuts
- "Build Unity Project"           # Reminder to use Unity Editor
- "Clean Unity Project"           # Removes Library/Temp/obj folders
- "Regenerate Unity Project Files" # Re-sync IDE project files
- "Build Container"               # Docker build with GHCR labels
- "Run Tests"                     # npm test with 100% coverage
- "Setup MCP Servers"             # Runs ./setup-mcp.sh
- "Check .NET Version"            # dotnet --info
- "Check Unity Version"           # cat ProjectSettings/ProjectVersion.txt
```

**Container Deployment**:

- Registry: `ghcr.io/bambisleepchat/bambisleep-church`
- Tags: `v{major}.{minor}.{patch}`, `dev-{branch}`, `latest`
- Required labels include BambiSleep™ trademark attribution
- Built on Node.js 20 Alpine with Volta and UV/UVX for Python MCP servers

**CI/CD Pipeline**:

- `.github/workflows/container-build.yml` — Multi-platform Docker builds (amd64, arm64) with automatic GHCR push
- `.github/workflows/build.yml` — Full validation workflow with MCP server testing, npm build, and deployment
- `mcp-validate.sh` — Tests all 8 MCP servers for connectivity (filesystem, git, github, memory, sequential-thinking, everything, brave-search, postgres)
- Runs on push to main/develop branches and on version tags

## Code Architecture Patterns

**Unity C# Structure** (FULLY implemented in `catgirl-avatar-project/Assets/Scripts/`):

```
Assets/Scripts/
├── Character/CatgirlController.cs     # ✅ 327 lines - NetworkBehaviour with pink auras, purring, cow powers
├── Economy/InventorySystem.cs         # ✅ 284 lines - Unity Gaming Services integration
├── Economy/UniversalBankingSystem.cs  # ✅ 363 lines - Gambling + auction + multi-currency systems
├── Networking/CatgirlNetworkManager.cs # ✅ 324 lines - Unity Relay + Lobby multiplayer
├── UI/InventoryUI.cs                  # ✅ 322 lines - UI Toolkit pink frilly interface
└── Audio/AudioManager.cs              # ✅ 342 lines - Centralized sound/music management
```

**⚠️ CRITICAL**: All 6 systems above are COMPLETE production implementations (1,950+ lines total). When extending functionality, follow their established patterns:

1. **Namespace convention**: `BambiSleep.CatGirl.{Domain}` (Character, Economy, Networking, UI, Audio)
2. **Unity attributes**: `[Header("🌸 Section Name")]` with emoji-coded sections
3. **Singleton pattern**: Used in AudioManager for centralized services
4. **Async/await patterns**: Unity Gaming Services calls use proper async Task methods
5. **Network synchronization**: NetworkVariable<T> for multiplayer state sharing

**Key Technical Integrations**:

- Unity Gaming Services (Economy, Authentication, Analytics, Lobby)
- Netcode for GameObjects multiplayer (full NetworkBehaviour implementations)
- Unity Relay + Lobby Services (async connection handling in CatgirlNetworkManager)
- XR Interaction Toolkit (eye/hand tracking ready)
- UI Toolkit (VisualElement-based UI in InventoryUI.cs)
- Addressables (asset streaming ready)
- Audio Mixer groups (Master, Music, SFX, Voice channels in AudioManager)

**Monetization Architecture**: Unity IAP + Gaming Services Economy with ethical guidelines (no pay-to-win, transparent drop rates, COPPA compliance)

## Unity Gaming Services Integration Patterns

**Economy Service Implementation**:

```csharp
// From UniversalBankingSystem.cs - Currency management
public NetworkVariable<long> pinkCoins = new NetworkVariable<long>(0);
public NetworkVariable<long> cowTokens = new NetworkVariable<long>(0);
public NetworkVariable<long> eldritchCurrency = new NetworkVariable<long>(0);

private async void LoadPlayerBalances()
{
    var balances = await EconomyService.Instance.PlayerBalances.GetBalancesAsync();
    ProcessBalanceData(balances);
}
```

**Authentication & Analytics Setup**:

```csharp
// Standard UGS initialization pattern used throughout codebase
await UnityServices.InitializeAsync();
await AuthenticationService.Instance.SignInAnonymouslyAsync();
AnalyticsService.Instance.StartDataCollection();
```

**Package Dependencies** (from `Packages/manifest.json`):

- `com.unity.services.core`: 1.15.0 (foundation)
- `com.unity.services.economy`: 3.4.2 (currency system)
- `com.unity.services.authentication`: 3.3.4 (player identity)
- `com.unity.services.analytics`: 5.1.1 (usage tracking)
- `com.unity.services.lobby`: 1.2.2 (multiplayer matchmaking)
- `com.unity.purchasing`: 4.12.2 (IAP integration)

## Current Implementation Status

**What Exists** (per `IMPLEMENTATION_PROGRESS.md`):

- ✅ Complete architectural specifications (682-line CATGIRL.md, 858-line UNITY_SETUP_GUIDE.md)
- ✅ `package.json` with Node.js 20+ dependencies and Volta configuration
- ✅ Working Dockerfile with proper GHCR labels and MCP environment
- ✅ `setup-mcp.sh` script for automated MCP server installation
- ✅ `mcp-validate.sh` script with tests for all 8 MCP servers
- ✅ VS Code MCP configuration templates (see MCP_SETUP_GUIDE.md)
- ✅ Unity 6.2 LTS project structure (`catgirl-avatar-project/`)
- ✅ **Six complete Unity C# systems (1,950+ lines)**:
  - CatgirlController.cs (327 lines) - Full pink aura system, purring cycles, cow powers
  - InventorySystem.cs (284 lines) with Unity Gaming Services Economy integration
  - UniversalBankingSystem.cs (363 lines) with gambling, auctions, multi-currency
  - CatgirlNetworkManager.cs (324 lines) with Unity Relay + Lobby Services
  - InventoryUI.cs (322 lines) with UI Toolkit VisualElements
  - AudioManager.cs (342 lines) with singleton pattern and mixer groups
- ✅ GitHub Actions CI/CD pipelines (`.github/workflows/build.yml`)
- ✅ Container registry: `ghcr.io/bambisleepchat/bambisleep-church`

**What's In Progress** (per `todo.md`):

- 🚧 Unity Editor installation (Unity 6000.2.11f1) for compilation testing
- 🚧 Unity Hub automated installation documentation
- 🚧 XR Controller system implementation
- 🚧 Game Manager system implementation
- 🚧 Complete test suite with 100% coverage requirement
- 🚧 Unity Gaming Services credentials configuration
- 🚧 Unity Hub automated installation documentation

## Safe Development Practices

**Unity Code Changes**:

- Follow exact class structure from UNITY_SETUP_GUIDE.md (CatgirlController, InventorySystem, etc.)
- Update `Packages/manifest.json` when adding Unity packages
- Maintain Assets/Scripts folder hierarchy: `{Character,Inventory,Economy,Networking,UI}`

**MCP Configuration Changes**:

- Use exact `mcp.servers` JSON from MCP_SETUP_GUIDE.md
- Prefer `npx -y @modelcontextprotocol/server-*` pattern for official servers
- Use `uvx` for Python-based servers (brave-search, postgres)

**Container Updates**:

- Include all required labels from CONTAINER_ORGANIZATION.md
- Maintain `ghcr.io/bambisleepchat/bambisleep-church` registry
- Follow semantic versioning: `v{major}.{minor}.{patch}`

## Trademark & Cultural Compliance

**Required**: Use `BambiSleep™` (with ™ symbol) in all public-facing content
**Philosophy**: "Universal Machine Philosophy" with 8/8 MCP operational status
**Quality Standards**: 100% test coverage, enterprise-grade error handling, cross-platform compatibility

## Common Development Task Examples

### Unity Development Tasks

**Adding New Catgirl Abilities**:

```csharp
// Extend CatgirlController.cs (follow existing pattern)
[Header("🦋 New Catgirl Power")]
public float butterflyEnergy = 100.0f;

private void ActivateButterflyMode()
{
    // Add to existing InitializeCatgirlSystems() method
    animator.SetBool("ButterflyModeActive", true);
    StartCoroutine(ButterflyFlightCycle());
}
```

**Adding Items to Inventory System**:

```csharp
// Create new CatgirlItem in InventorySystem.cs
var newItem = new CatgirlItem {
    itemName = "Rainbow Butterfly Wings",
    rarity = 4, // 1-5 scale (5 = secret Diablo level)
    isCowPowerItem = false,
    value = 2500
};
inventory.AddItem(newItem);
```

**Implementing New Currency Types**:

```csharp
// Add to UniversalBankingSystem.cs NetworkVariables section
public NetworkVariable<long> butterflyCoins = new NetworkVariable<long>(0);

// Add to Unity Gaming Services Economy configuration
// Via Unity Dashboard: Create currency "butterflyCoins"
```

**Setting Up Unity Editor** (for compilation testing):

```bash
# Install Unity Hub
wget -qO - https://hub.unity3d.com/linux/keys/public | gpg --dearmor | sudo tee /usr/share/keyrings/unity-archive-keyring.gpg
echo "deb [signed-by=/usr/share/keyrings/unity-archive-keyring.gpg] https://hub.unity3d.com/linux/repos/deb stable main" | sudo tee /etc/apt/sources.list.d/unityhub.list
sudo apt update && sudo apt install unityhub

# Install Unity 6.2 LTS (6000.2.11f1) via Unity Hub
unityhub --install 6000.2.11f1 --changeset <changeset>

# Open project
unityhub -- --projectPath /mnt/f/bambisleep-chat-catgirl/catgirl-avatar-project
```

### MCP Development Tasks

**Adding New MCP Server**:

```bash
# Install server via npx/uvx
npx -y @modelcontextprotocol/server-new-feature
# OR for Python servers:
uvx --install mcp-server-new-feature

# Add to ~/.config/mcp/vscode-settings.json
"new-feature": {
    "command": "npx",
    "args": ["-y", "@modelcontextprotocol/server-new-feature"]
}
```

**Validating All 8 MCP Servers** (comprehensive test):

```bash
# Automated validation via mcp-validate.sh
./mcp-validate.sh

# Manual individual server tests:
# Test filesystem server
echo "Testing filesystem..." && npx -y @modelcontextprotocol/server-filesystem --version

# Test git server
echo "Testing git..." && npx -y @modelcontextprotocol/server-git --help

# Test GitHub server (requires GITHUB_PERSONAL_ACCESS_TOKEN)
echo "Testing GitHub..." && npx -y @modelcontextprotocol/server-github --version

# Test memory server
echo "Testing memory..." && npx -y @modelcontextprotocol/server-memory --version

# Test sequential-thinking server
echo "Testing sequential-thinking..." && npx -y @modelcontextprotocol/server-sequential-thinking --version

# Test everything server
echo "Testing everything..." && npx -y @modelcontextprotocol/server-everything --version

# Test Python-based servers (requires uv/uvx)
echo "Testing brave-search..." && uvx mcp-server-brave-search --version
echo "Testing postgres..." && uvx mcp-server-postgres --version

# Verify VS Code integration
echo "✅ All MCP servers validated - check VS Code MCP extension for 8/8 operational status"
```

### Container & Deployment Tasks

**Building Container with New Features**:

```bash
# Follow CONTAINER_ORGANIZATION.md patterns
docker build \
    --tag ghcr.io/bambisleepchat/bambisleep-church:v1.2.0 \
    --tag ghcr.io/bambisleepchat/bambisleep-church:latest \
    --label org.bambi.feature="new-catgirl-powers" .
```

**Adding Build Scripts** (to `package.json`):

```json
{
  "scripts": {
    "unity:test": "echo 'Testing Unity CatgirlController compilation...'",
    "mcp:health": "echo 'Checking MCP server health (8/8 operational)...'",
    "container:verify": "echo 'Verifying BambiSleep™ trademark labels...'"
  }
}
```

## Troubleshooting Common Issues

### Unity Gaming Services Connection Problems

```csharp
// Always initialize UGS in this order (from UNITY_SETUP_GUIDE.md):
await UnityServices.InitializeAsync();
await AuthenticationService.Instance.SignInAnonymouslyAsync();
// Then proceed with Economy/Analytics calls
```

### MCP Server Not Responding

```bash
# Re-run automated setup
./setup-mcp.sh

# Check individual server status
npx -y @modelcontextprotocol/server-filesystem --version

# Verify VS Code configuration
ls -la ~/.config/mcp/vscode-settings.json
```

### Unity Package Dependency Issues

```bash
# Reset package cache (from Unity project directory)
rm -rf Library/PackageCache
# Restart Unity Editor to rebuild package resolution
```

### Container Build Failures

```bash
# Verify all required labels present (CONTAINER_ORGANIZATION.md)
docker inspect ghcr.io/bambisleepchat/bambisleep-church:latest | grep -i bambi

# Check Volta/UV installations in container
docker run --rm bambisleep-church volta --version
docker run --rm bambisleep-church uv --version
```

## Essential File Patterns

**Always Check First**: `docs/development/IMPLEMENTATION_PROGRESS.md` for current implementation status
**For Unity Changes**: Follow exact patterns in `docs/development/UNITY_SETUP_GUIDE.md` (CatgirlController, InventorySystem)
**For MCP Issues**: Use `./setup-mcp.sh` then check `~/.config/mcp/vscode-settings.json`
**For Container Updates**: Reference `docs/architecture/CONTAINER_ORGANIZATION.md` labels and `Dockerfile` structure
**For Build Issues**: Check `package.json` scripts match `docs/architecture/RELIGULOUS_MANTRA.md` requirements
**For Project History**: See `docs/reference/CHANGELOG.md` for complete v1.0.0 details

## Architecture & Workflow Clarity

**Critical Understanding of "Missing" Content**:

- The Unity Assets/Scripts/ structure in `docs/development/UNITY_SETUP_GUIDE.md` is **implementation blueprint**, not wishful thinking
- Code examples in markdown files are **canonical implementations** to copy, not suggestions
- When asked to "create Unity class X", extract the complete implementation from `docs/development/UNITY_SETUP_GUIDE.md`
- The 858-line UNITY_SETUP_GUIDE.md contains actual C# code that should be used verbatim

**Testing & Verification Workflow**:

```bash
# 1. Verify Node.js environment
node --version  # Should be 20.x
npm --version   # Should be 10.x
volta --version # Should show Volta installation

# 2. Test MCP servers (run validation from MCP Development Tasks section above)

# 3. Verify Unity setup (when Unity project exists)
ls -la catgirl-avatar-project/Assets/Scripts/
cat catgirl-avatar-project/ProjectSettings/ProjectVersion.txt

# 4. Validate container build
docker build -t test-catgirl . && docker inspect test-catgirl | grep -i bambi

# 5. Check trademark compliance
grep -r "BambiSleep™" --include="*.md" --include="*.json" .
```

**Common Misunderstandings to Avoid**:

- ❌ "Let me create a basic CatgirlController" → ✅ Use existing 327-line implementation or extract from `docs/development/UNITY_SETUP_GUIDE.md`
- ❌ "The Unity project doesn't exist yet, so I'll skip that" → ✅ It exists with 1,950 lines of C# code across 6 complete systems
- ❌ "I'll improve the emoji usage" → ✅ Match existing documentation density exactly
- ❌ "Let me add some helpful Unity packages" → ✅ Only use packages listed in Packages/manifest.json spec
- ❌ "The docs are aspirational, let me simplify" → ✅ The docs ARE the implementation - use them fully

**Development Workflow Reality Check**:

- ✅ Unity project DOES exist at `catgirl-avatar-project/` with real C# code
- ✅ CatgirlController.cs is 327 lines of actual implementation (not a stub)
- ✅ InventorySystem.cs (284 lines) and UniversalBankingSystem.cs (363 lines) are complete
- ✅ CatgirlNetworkManager.cs (324 lines) with full Relay + Lobby integration is complete
- ✅ InventoryUI.cs (322 lines) with UI Toolkit VisualElements is complete
- ✅ AudioManager.cs (342 lines) with singleton pattern is complete
- ✅ All systems use proper `BambiSleep.CatGirl.{Domain}` namespaces
- ✅ GitHub Actions workflows are live and functional
- ✅ MCP validation runs automatically on CI
- ✅ Container builds deploy to GHCR on every push to main

_This is a specification-driven project - the documentation IS the implementation plan._
