#!/bin/bash
# 🔮 MCP Server Validation Script
# Tests all 8 sacred MCP servers for BambiSleep™ Church development

set -e

echo "🌸✨💎 BambiSleep™ MCP Server Validation 💎✨🌸"
echo "================================================"
echo ""

# Color codes for output
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Test counter
PASSED=0
FAILED=0
TOTAL=8

# Function to test a server
test_server() {
    local name=$1
    local command=$2
    shift 2
    local args=("$@")
    
    echo -n "Testing $name... "
    
    if timeout 5s $command "${args[@]}" &> /dev/null; then
        echo -e "${GREEN}✅ PASS${NC}"
        ((PASSED++))
        return 0
    else
        echo -e "${RED}❌ FAIL${NC}"
        ((FAILED++))
        return 1
    fi
}

echo "🌸 Testing Node.js MCP Servers (via npx)..."
echo "--------------------------------------------"

# Test filesystem server
test_server "filesystem" npx -y @modelcontextprotocol/server-filesystem --help

# Test git server  
test_server "git" npx -y @modelcontextprotocol/server-git --help

# Test GitHub server
if [ -n "$GITHUB_PERSONAL_ACCESS_TOKEN" ]; then
    test_server "github" npx -y @modelcontextprotocol/server-github --version
else
    echo -e "${YELLOW}⚠️  Skipping GitHub server (no GITHUB_PERSONAL_ACCESS_TOKEN)${NC}"
    ((TOTAL--))
fi

# Test memory server
test_server "memory" npx -y @modelcontextprotocol/server-memory --help

# Test sequential-thinking server
test_server "sequential-thinking" npx -y @modelcontextprotocol/server-sequential-thinking --help

# Test everything server
test_server "everything" npx -y @modelcontextprotocol/server-everything --help

echo ""
echo "🐍 Testing Python MCP Servers (via uvx)..."
echo "-------------------------------------------"

# Check if uvx is installed
if ! command -v uvx &> /dev/null; then
    echo -e "${RED}❌ uvx not found! Install with: curl -LsSf https://astral.sh/uv/install.sh | sh${NC}"
    echo -e "${YELLOW}Skipping Python MCP servers...${NC}"
    ((TOTAL-=2))
else
    # Test brave-search server
    test_server "brave-search" uvx mcp-server-brave-search --help || true
    
    # Test postgres server
    test_server "postgres" uvx mcp-server-postgres --help || true
fi

echo ""
echo "================================================"
echo "🌈 VALIDATION RESULTS 🌈"
echo "================================================"
echo -e "Total Servers: $TOTAL"
echo -e "Passed: ${GREEN}$PASSED${NC}"
echo -e "Failed: ${RED}$FAILED${NC}"
echo ""

if [ $FAILED -eq 0 ]; then
    echo -e "${GREEN}✨✨✨ ALL MCP SERVERS OPERATIONAL! ✨✨✨${NC}"
    echo "🌸 BambiSleep™ development environment is READY! 🌸"
    echo ""
    echo "Status: 🎊 $PASSED/$TOTAL operational"
    exit 0
else
    echo -e "${RED}❌ Some MCP servers failed validation${NC}"
    echo "Please check the errors above and ensure all dependencies are installed."
    echo ""
    echo "Status: ⚠️  $PASSED/$TOTAL operational"
    exit 1
fi
