#!/bin/bash
# 🌸 Project Statistics Tool
# Generates comprehensive statistics about the codebase

echo "🌸✨ BambiSleep™ CatGirl Avatar System Statistics ✨🌸"
echo ""

# Code Statistics
echo "📊 Code Statistics:"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

# JavaScript files
JS_FILES=$(find src/ -name "*.js" 2>/dev/null | wc -l)
JS_LINES=$(find src/ -name "*.js" -exec wc -l {} \; 2>/dev/null | awk '{sum+=$1} END {print sum}')
echo "   JavaScript Files: $JS_FILES"
echo "   JavaScript Lines: $JS_LINES"

# C# files
CS_FILES=$(find catgirl-avatar-project/Assets/Scripts/ -name "*.cs" 2>/dev/null | wc -l)
CS_LINES=$(find catgirl-avatar-project/Assets/Scripts/ -name "*.cs" -exec wc -l {} \; 2>/dev/null | awk '{sum+=$1} END {print sum}')
echo "   C# Files: $CS_FILES"
echo "   C# Lines: $CS_LINES"

# Test files
TEST_FILES=$(find __tests__/ -name "*.test.js" 2>/dev/null | wc -l)
TEST_LINES=$(find __tests__/ -name "*.test.js" -exec wc -l {} \; 2>/dev/null | awk '{sum+=$1} END {print sum}')
echo "   Test Files: $TEST_FILES"
echo "   Test Lines: $TEST_LINES"

echo ""

# Documentation Statistics
echo "📚 Documentation Statistics:"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

DOC_FILES=$(find docs/ -name "*.md" 2>/dev/null | wc -l)
DOC_LINES=$(find docs/ -name "*.md" -exec wc -l {} \; 2>/dev/null | awk '{sum+=$1} END {print sum}')
echo "   Documentation Files: $DOC_FILES"
echo "   Documentation Lines: $DOC_LINES"

echo ""

# Dependencies
echo "📦 Dependencies:"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

if [ -f "package.json" ]; then
    DEPS=$(node -e "console.log(Object.keys(require('./package.json').dependencies || {}).length)")
    DEV_DEPS=$(node -e "console.log(Object.keys(require('./package.json').devDependencies || {}).length)")
    echo "   Production Dependencies: $DEPS"
    echo "   Development Dependencies: $DEV_DEPS"
fi

echo ""

# Git Statistics
echo "📈 Git Statistics:"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

if [ -d ".git" ]; then
    COMMITS=$(git rev-list --count HEAD 2>/dev/null || echo "0")
    CONTRIBUTORS=$(git log --format='%aN' | sort -u | wc -l 2>/dev/null || echo "0")
    LAST_COMMIT=$(git log -1 --format="%h - %s (%ar)" 2>/dev/null || echo "N/A")

    echo "   Total Commits: $COMMITS"
    echo "   Contributors: $CONTRIBUTORS"
    echo "   Last Commit: $LAST_COMMIT"
fi

echo ""

# File Structure
echo "📁 Project Structure:"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

echo "   Root Files: $(ls -1 | wc -l)"
echo "   Source Modules: $(find src/ -type d -mindepth 1 -maxdepth 1 2>/dev/null | wc -l)"
echo "   Scripts: $(find scripts/ -name "*.sh" 2>/dev/null | wc -l)"
echo "   Tools: $(find tools/ -name "*.sh" 2>/dev/null | wc -l)"
echo "   Unity Scripts: $(find catgirl-avatar-project/Assets/Scripts/ -name "*.cs" 2>/dev/null | wc -l)"

echo ""

# BambiSleep™ Metrics
echo "💎 BambiSleep™ Metrics:"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

EMOJI_COUNT=$(grep -r "🌸\|💎\|🦋\|✨\|🐄\|👑" . --exclude-dir={node_modules,coverage,.git} 2>/dev/null | wc -l || echo "0")
COW_POWER_COUNT=$(grep -r "cow.*power" . --exclude-dir={node_modules,coverage,.git} -i 2>/dev/null | wc -l || echo "0")
PINK_COUNT=$(grep -r "pink\|frilly" . --exclude-dir={node_modules,coverage,.git} -i 2>/dev/null | wc -l || echo "0")

echo "   Emoji Usage: $EMOJI_COUNT"
echo "   Cow Power References: $COW_POWER_COUNT"
echo "   Pink/Frilly Mentions: $PINK_COUNT"
echo "   Cuteness Level: MAXIMUM_OVERDRIVE 💖"

echo ""
echo "🌸✨━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━✨🌸"
echo "           Nyan nyan nyan! 💖"
echo "🌸✨━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━✨🌸"
echo ""
