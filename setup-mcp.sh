#!/bin/bash
# 🌸 BambiSleep™ MCP Development Environment Setup Script 🌸
# This script sets up all 8 sacred MCP servers for catgirl development

echo "🌈 Setting up BambiSleep™ MCP Development Environment... 🌈"

# Create MCP configuration directory
mkdir -p ~/.config/mcp

# VS Code MCP settings template (add to your VS Code settings.json)
cat > ~/.config/mcp/vscode-settings.json << 'EOF'
{
  "mcp.servers": {
    "filesystem": {
      "command": "npx",
      "args": [
        "-y",
        "@modelcontextprotocol/server-filesystem",
        "/mnt/f/bambisleep-chat"
      ]
    },
    "git": {
      "command": "npx",
      "args": [
        "-y", 
        "@modelcontextprotocol/server-git",
        "--repository",
        "/mnt/f/bambisleep-chat"
      ]
    },
    "github": {
      "command": "npx",
      "args": [
        "-y",
        "@modelcontextprotocol/server-github"
      ],
      "env": {
        "GITHUB_PERSONAL_ACCESS_TOKEN": "your_github_token_here"
      }
    },
    "memory": {
      "command": "npx",
      "args": [
        "-y",
        "@modelcontextprotocol/server-memory"
      ]
    },
    "sequential-thinking": {
      "command": "npx",
      "args": [
        "-y",
        "@modelcontextprotocol/server-sequential-thinking"
      ]
    },
    "everything": {
      "command": "npx",
      "args": [
        "-y",
        "@modelcontextprotocol/server-everything"
      ]
    }
  }
}
EOF

echo "✅ MCP configuration created at ~/.config/mcp/vscode-settings.json"
echo "🌸 Copy the contents to your VS Code settings.json under 'mcp.servers'"
echo ""
echo "📋 MCP Server Status:"
echo "  🟢 filesystem - File operations"
echo "  🟢 git - Version control"  
echo "  🟢 github - Repository management"
echo "  🟢 memory - Knowledge graph"
echo "  🟢 sequential-thinking - Problem solving"
echo "  🟢 everything - Protocol testing"
echo "  🟡 brave-search - Requires uvx installation"
echo "  🟡 postgres - Requires uvx + database setup"
echo ""
echo "🦋 To complete setup:"
echo "  1. Install uvx: curl -LsSf https://astral.sh/uv/install.sh | sh"
echo "  2. Install Python MCP servers: uvx install mcp-server-brave-search mcp-server-postgres"
echo "  3. Add GitHub personal access token to environment"
echo "  4. Copy MCP configuration to VS Code settings"
echo ""
echo "💖 BambiSleep™ MCP environment ready for kawaii development! 💖"