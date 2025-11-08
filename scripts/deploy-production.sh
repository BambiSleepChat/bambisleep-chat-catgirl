#!/usr/bin/env bash

/// Law: Production Deployment Script for BambiSleep™ Chat Catgirl
/// SPDX-License-Identifier: MIT

set -e

echo "🌸 BambiSleep™ Chat Catgirl Production Deployment 🌸"
echo "======================================================"

# Check prerequisites
echo "📋 Checking prerequisites..."
command -v docker >/dev/null 2>&1 || { echo "❌ Docker is required but not installed. Aborting."; exit 1; }
command -v docker-compose >/dev/null 2>&1 || { echo "❌ Docker Compose is required but not installed. Aborting."; exit 1; }

# Check for .env.production
if [ ! -f .env.production ]; then
    echo "⚠️  .env.production not found!"
    echo "📝 Creating from template..."
    cp .env.production.template .env.production
    echo "✏️  Please edit .env.production with your production values before continuing."
    exit 1
fi

# Validate critical environment variables
echo "🔍 Validating environment configuration..."
source .env.production

REQUIRED_VARS=(
    "POSTGRES_PASSWORD"
    "SESSION_SECRET"
    "JWT_SECRET"
)

for var in "${REQUIRED_VARS[@]}"; do
    if [ -z "${!var}" ] || [ "${!var}" == "CHANGE_ME"* ]; then
        echo "❌ Required environment variable $var is not set or using default value!"
        echo "   Please update .env.production before deploying."
        exit 1
    fi
done

# Install MCP server dependencies
echo "📦 Installing custom MCP server dependencies..."
for mcp_dir in mcp-servers/*/; do
    if [ -f "${mcp_dir}package.json" ]; then
        echo "   Installing $(basename $mcp_dir)..."
        (cd "$mcp_dir" && npm ci --production)
    fi
done

# Run tests before deployment
echo "🧪 Running production readiness tests..."
npm run test:ci

# Build Docker images
echo "🏗️  Building Docker images..."
docker-compose -f docker-compose.prod.yml build

# Stop existing containers
echo "🛑 Stopping existing containers..."
docker-compose -f docker-compose.prod.yml down

# Start database services first
echo "🗄️  Starting database services..."
docker-compose -f docker-compose.prod.yml up -d postgres redis
sleep 10

# Wait for databases to be healthy
echo "⏳ Waiting for databases to be ready..."
until docker-compose -f docker-compose.prod.yml exec -T postgres pg_isready -U bambisleep; do
    echo "   Waiting for PostgreSQL..."
    sleep 2
done

until docker-compose -f docker-compose.prod.yml exec -T redis redis-cli ping | grep -q PONG; do
    echo "   Waiting for Redis..."
    sleep 2
done

# Run database migrations (if applicable)
if [ -f "scripts/migrate-db.sh" ]; then
    echo "🔄 Running database migrations..."
    bash scripts/migrate-db.sh
fi

# Start application services
echo "🚀 Starting application services..."
docker-compose -f docker-compose.prod.yml up -d

# Wait for application to be healthy
echo "⏳ Waiting for application to be ready..."
sleep 15

# Health check
MAX_RETRIES=30
RETRY_COUNT=0

while [ $RETRY_COUNT -lt $MAX_RETRIES ]; do
    if curl -f http://localhost:3000/health >/dev/null 2>&1; then
        echo "✅ Application is healthy!"
        break
    fi

    RETRY_COUNT=$((RETRY_COUNT + 1))
    echo "   Retry $RETRY_COUNT/$MAX_RETRIES..."
    sleep 2
done

if [ $RETRY_COUNT -eq $MAX_RETRIES ]; then
    echo "❌ Application failed to become healthy!"
    echo "📋 Container logs:"
    docker-compose -f docker-compose.prod.yml logs --tail=50 catgirl-app
    exit 1
fi

# Validate MCP servers
echo "🔌 Validating MCP server connections..."
if [ -f "scripts/mcp-validate.sh" ]; then
    bash scripts/mcp-validate.sh
fi

# Display deployment summary
echo ""
echo "🎉 Deployment Complete! 🎉"
echo "=========================="
echo ""
echo "🌐 Application URL: http://localhost"
echo "📊 Metrics: http://localhost:3000/metrics"
echo "🔍 Health: http://localhost:3000/health"
echo ""
echo "📋 Service Status:"
docker-compose -f docker-compose.prod.yml ps
echo ""
echo "📝 View logs: docker-compose -f docker-compose.prod.yml logs -f"
echo "🛑 Stop services: docker-compose -f docker-compose.prod.yml down"
echo ""
echo "🌸 BambiSleep™ Chat Catgirl is now running! Nyan nyan nyan! 🌸"
