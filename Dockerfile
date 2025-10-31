# 🌸 BambiSleep™ Church CatGirl Avatar System Container 🌸
# Official Docker container for Unity 6.2 MCP development environment

# Use Node.js 20 LTS as base image (matches our Volta pinning)
FROM node:20-alpine

# Metadata labels as per CONTAINER_ORGANIZATION.md requirements
LABEL org.opencontainers.image.vendor="BambiSleepChat"
LABEL org.opencontainers.image.source="https://github.com/BambiSleepChat/bambisleep-catgirl-church"
LABEL org.opencontainers.image.documentation="https://github.com/BambiSleepChat/bambisleep-catgirl-church#readme"
LABEL org.opencontainers.image.licenses="MIT"
LABEL org.opencontainers.image.title="BambiSleep™ Church CatGirl Avatar System"
LABEL org.opencontainers.image.description="Unity 6.2 MCP Control Tower with Universal Banking & Sacred Development Philosophy"
LABEL org.bambi.trademark="BambiSleep™ is a trademark of BambiSleepChat"
LABEL org.bambi.cuteness="MAXIMUM_OVERDRIVE"
LABEL org.bambi.cow-powers="SECRET_LEVEL_UNLOCKED"

# Install system dependencies
RUN apk add --no-cache \
    git \
    python3 \
    py3-pip \
    bash \
    curl \
    && rm -rf /var/cache/apk/*

# Set working directory
WORKDIR /app

# Copy package configuration first for better layer caching
COPY package*.json ./

# Install Node.js dependencies (use npm install since package-lock.json is not tracked)
RUN npm install --production && npm cache clean --force

# Install UV/UVX for Python MCP servers
RUN curl -LsSf https://astral.sh/uv/install.sh | sh
ENV PATH="/root/.cargo/bin:$PATH"

# Copy project files (after dependencies for better caching)
COPY . .

# Make scripts executable
RUN chmod +x scripts/*.sh

# Create MCP configuration directory
RUN mkdir -p /root/.config/mcp

# Install MCP servers (non-interactive setup)
RUN npm run mcp:setup || echo "⚠️  MCP setup requires interactive configuration - run manually"

# Create Unity project directory
RUN mkdir -p /app/unity-projects

# Expose ports for development
EXPOSE 3000 8080

# Health check - verify Node.js and npm are functional
HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
    CMD node --version && npm --version || exit 1

# Default command
CMD ["npm", "run", "dev"]

# 🌈 Container Usage Instructions:
#
# Build: docker build -t ghcr.io/bambisleepchat/bambisleep-church:latest .
# Run:   docker run -p 3000:3000 -p 8080:8080 -v $(pwd):/app ghcr.io/bambisleepchat/bambisleep-church:latest
#
# 💖 This container includes:
# - Node.js 20 LTS with Volta version management
# - 8 MCP servers for development automation
# - Unity 6.2 project structure templates
# - BambiSleep™ trademark compliance
# - Maximum kawaii development environment
#
# NYAN NYAN NYAN! 🌸💎✨
