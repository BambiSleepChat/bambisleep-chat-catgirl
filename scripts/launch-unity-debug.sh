#!/bin/bash
# Quick launch Unity and attach debugger

echo "🎮 Launching Unity Editor..."

# Try to find Unity installation
UNITY_PATH=""

if [ -f "$HOME/Unity/Hub/Editor/6000.2.11f1/Editor/Unity" ]; then
    UNITY_PATH="$HOME/Unity/Hub/Editor/6000.2.11f1/Editor/Unity"
elif [ -f "/opt/unity/Editor/Unity" ]; then
    UNITY_PATH="/opt/unity/Editor/Unity"
else
    echo "❌ Unity Editor not found!"
    echo "   Install Unity 6000.2.11f1 first"
    exit 1
fi

# Launch Unity with project
PROJECT_PATH="/mnt/f/bambisleep-chat-catgirl/catgirl-avatar-project"

echo "   Unity: $UNITY_PATH"
echo "   Project: $PROJECT_PATH"
echo ""
echo "🚀 Starting Unity Editor..."

"$UNITY_PATH" -projectPath "$PROJECT_PATH" &

echo ""
echo "⏳ Waiting for Unity to initialize (30 seconds)..."
sleep 30

echo ""
echo "🐛 Now you can press F5 in VS Code to attach debugger!"
echo "   Or run: code . && F5"
