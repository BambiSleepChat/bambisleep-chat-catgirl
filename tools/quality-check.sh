#!/bin/bash
# 🌸 Code Quality Check Tool
# Runs linting, formatting, and quality checks across the codebase

set -e

echo "🌸✨ BambiSleep™ Code Quality Check ✨🌸"
echo ""

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

ERRORS=0

# Function to print status
print_status() {
    if [ $1 -eq 0 ]; then
        echo -e "${GREEN}✅ $2${NC}"
    else
        echo -e "${RED}❌ $2${NC}"
        ERRORS=$((ERRORS + 1))
    fi
}

# Check Node.js version
echo "📋 Checking Node.js version..."
NODE_VERSION=$(node --version)
echo "   Node.js: $NODE_VERSION"
echo ""

# Run ESLint
echo "🔍 Running ESLint..."
if npm run lint; then
    print_status 0 "ESLint passed"
else
    print_status 1 "ESLint found issues"
fi
echo ""

# Check formatting
echo "💅 Checking code formatting..."
if npm run format:check; then
    print_status 0 "Code formatting is correct"
else
    print_status 1 "Code formatting needs fixing (run: npm run format)"
fi
echo ""

# Run tests
echo "🧪 Running tests..."
if npm test; then
    print_status 0 "All tests passed"
else
    print_status 1 "Some tests failed"
fi
echo ""

# Check for TODO comments
echo "📝 Checking for TODO comments..."
TODO_COUNT=$(grep -r "TODO" src/ --exclude-dir=node_modules 2>/dev/null | wc -l || echo "0")
echo "   Found $TODO_COUNT TODO comments"
if [ "$TODO_COUNT" -gt 0 ]; then
    grep -r "TODO" src/ --exclude-dir=node_modules 2>/dev/null | head -n 5
fi
echo ""

# Check file permissions
echo "🔐 Checking script permissions..."
find scripts/ -type f -name "*.sh" -exec test -x {} \; -print | while read file; do
    print_status 0 "Executable: $file"
done
echo ""

# Summary
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
if [ $ERRORS -eq 0 ]; then
    echo -e "${GREEN}🎉 All quality checks passed! Nyan nyan nyan! 🎉${NC}"
    exit 0
else
    echo -e "${RED}⚠️  $ERRORS quality check(s) failed${NC}"
    exit 1
fi
