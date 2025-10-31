#!/bin/bash
# 🌸 Documentation Structure Verification Script 🌸

echo "📚 Verifying BambiSleep™ Documentation Organization..."
echo ""

# Check docs folder exists
if [ -d "docs" ]; then
    echo "✅ docs/ folder exists"
else
    echo "❌ docs/ folder missing"
    exit 1
fi

# Check subdirectories
for dir in architecture development guides reference; do
    if [ -d "docs/$dir" ]; then
        count=$(find "docs/$dir" -name "*.md" | wc -l)
        echo "✅ docs/$dir/ exists ($count files)"
    else
        echo "❌ docs/$dir/ missing"
    fi
done

# Check critical files
critical_files=(
    "docs/README.md"
    "docs/architecture/CATGIRL.md"
    "docs/development/UNITY_SETUP_GUIDE.md"
    "docs/development/MCP_SETUP_GUIDE.md"
    "docs/guides/build.md"
    "docs/guides/todo.md"
    "docs/reference/CHANGELOG.md"
    "README.md"
    ".github/copilot-instructions.md"
)

echo ""
echo "📋 Checking critical files..."
for file in "${critical_files[@]}"; do
    if [ -f "$file" ]; then
        echo "✅ $file"
    else
        echo "❌ $file missing"
    fi
done

# Count total markdown files
total_docs=$(find docs -name "*.md" | wc -l)
echo ""
echo "📊 Total documentation files in docs/: $total_docs"
echo ""
echo "🌸 Documentation structure verified! 🌸"
