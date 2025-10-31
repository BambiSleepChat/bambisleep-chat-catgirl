#!/bin/bash
# 🌸 Development Environment Setup
# Quick setup script for new developers

echo "🌸✨ BambiSleep™ Development Environment Setup ✨🌸"
echo ""

# Check prerequisites
echo "📋 Checking prerequisites..."

# Check Node.js
if ! command -v node &> /dev/null; then
    echo "❌ Node.js not found. Please install Node.js 20 LTS first."
    exit 1
fi
echo "✅ Node.js $(node --version)"

# Check npm
if ! command -v npm &> /dev/null; then
    echo "❌ npm not found. Please install npm first."
    exit 1
fi
echo "✅ npm $(npm --version)"

# Check git
if ! command -v git &> /dev/null; then
    echo "❌ git not found. Please install git first."
    exit 1
fi
echo "✅ git $(git --version | head -n 1)"

echo ""

# Install dependencies
echo "📦 Installing Node.js dependencies..."
npm install

if [ $? -ne 0 ]; then
    echo "❌ Failed to install dependencies"
    exit 1
fi
echo "✅ Dependencies installed"
echo ""

# Create .env from example
if [ ! -f ".env" ]; then
    echo "📝 Creating .env file from .env.example..."
    cp .env.example .env
    echo "✅ .env file created - please configure it"
else
    echo "ℹ️  .env file already exists"
fi
echo ""

# Create logs directory
if [ ! -d "logs" ]; then
    echo "📁 Creating logs directory..."
    mkdir -p logs
    echo "✅ Logs directory created"
else
    echo "ℹ️  Logs directory already exists"
fi
echo ""

# Setup MCP servers (optional)
echo "🔮 MCP Server Setup (optional):"
read -p "Do you want to set up MCP servers now? (y/n) " -n 1 -r
echo
if [[ $REPLY =~ ^[Yy]$ ]]; then
    bash scripts/setup-mcp.sh
else
    echo "ℹ️  You can run 'npm run mcp:setup' later to configure MCP servers"
fi
echo ""

# Run quality check
echo "🔍 Running quality check..."
bash tools/quality-check.sh

if [ $? -eq 0 ]; then
    echo ""
    echo "🌸✨━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━✨🌸"
    echo "   Development environment ready!"
    echo "🌸✨━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━✨🌸"
    echo ""
    echo "💎 Quick Start Commands:"
    echo "   npm start          - Display system info"
    echo "   npm run dev        - Start development server"
    echo "   npm test           - Run tests"
    echo "   npm run lint       - Check code quality"
    echo "   npm run format     - Format code"
    echo ""
    echo "📚 Documentation:"
    echo "   docs/PROJECT_ORGANIZATION.md - Project structure guide"
    echo "   docs/README.md - Documentation index"
    echo ""
    echo "Nyan nyan nyan! 💖"
else
    echo ""
    echo "⚠️  Setup completed with some warnings"
    echo "Please review the issues above and fix them"
fi
