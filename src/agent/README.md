# Commander-Brandynette Agent Authority System

**Version**: 1.0.0  
**License**: MIT  
**Organization**: BambiSleepChat

## Overview

Hierarchical agent authority system inspired by Commander-Brandynette protocol. Implements ring layer access control for coordinating multiple AI agents with different privilege levels.

## Architecture

### Ring Layer Access Model

```
Layer 0 (Primitives)    - filesystem, memory
Layer 1 (Foundation)    - git, github, brave-search
Layer 2 (Advanced)      - sequential-thinking, postgres, custom MCP servers
```

**Access Rule**: Agents with Ring Layer N can access layers N, N+1, N+2 (higher numbers = lower privilege)

### Agent Roles

1. **Commander** (Ring Layer 0)
   - Full system authority
   - Can assign ring layers to other agents
   - Can grant/revoke permissions
   - Only ONE commander allowed per system

2. **Supervisor** (Ring Layer 0-1)
   - Can read/write/execute
   - Access to Layer 0-1 MCP servers
   - Cannot modify agent authorities

3. **Operator** (Ring Layer 1-2)
   - Standard operational access
   - Can read/write
   - Access to Layer 1-2 MCP servers

4. **Observer** (Ring Layer 2)
   - Read-only access
   - Access to Layer 2 MCP servers only
   - Cannot modify system state

## Usage

### Register Agents

```javascript
const { AgentCoordinator, AgentRole, RingLayer } = require('./src/agent/agent-coordinator');

const coordinator = new AgentCoordinator();

// Register commander (only one allowed)
const commander = coordinator.registerAgent(
  'commander-brandynette',
  AgentRole.COMMANDER,
  RingLayer.LAYER_0
);

// Register supervisor
const supervisor = coordinator.registerAgent(
  'supervisor-bambi',
  AgentRole.SUPERVISOR,
  RingLayer.LAYER_0
);

// Register operator
const operator = coordinator.registerAgent(
  'operator-catgirl',
  AgentRole.OPERATOR,
  RingLayer.LAYER_1
);

// Register observer
const observer = coordinator.registerAgent(
  'observer-analytics',
  AgentRole.OBSERVER,
  RingLayer.LAYER_2
);
```

### Assign Ring Layers (Commander Only)

```javascript
// Commander assigns ring layer to operator
coordinator.assignRingLayer(
  'commander-brandynette',  // Commander ID
  'operator-catgirl',        // Target agent ID
  RingLayer.LAYER_0          // New ring layer
);
```

### Check Authority

```javascript
// Check if agent can access Layer 1
const canAccess = coordinator.checkAuthority(
  'operator-catgirl',
  RingLayer.LAYER_1
);

// Check specific permission
const canWrite = coordinator.checkAuthority(
  'observer-analytics',
  RingLayer.LAYER_2,
  'write'  // Will return false for observers
);
```

### Grant Permissions (Commander Only)

```javascript
// Commander grants special permission
coordinator.grantPermission(
  'commander-brandynette',
  'operator-catgirl',
  'mcp:custom-server-access'
);
```

### Query Agents

```javascript
// Get all agents
const allAgents = coordinator.getAllAgents();

// Get agents by role
const operators = coordinator.getAgentsByRole(AgentRole.OPERATOR);

// Get agents by ring layer
const layer2Agents = coordinator.getAgentsByRingLayer(RingLayer.LAYER_2);
```

### Operation Logging

```javascript
// Get operation log for audit trail
const log = coordinator.getOperationLog(50);  // Last 50 operations

log.forEach(entry => {
  console.log(`${entry.timestamp}: ${entry.agentId} performed ${entry.operation}`);
});
```

## Event System

The coordinator emits events for monitoring:

```javascript
// Agent registration
coordinator.on('agentRegistered', (agent) => {
  console.log(`Agent registered: ${agent.agentId} (${agent.role})`);
});

// Ring layer assignment
coordinator.on('ringLayerAssigned', (event) => {
  console.log(`${event.commanderId} assigned ring layer ${event.newLayer} to ${event.targetAgentId}`);
});

// Permission granted
coordinator.on('permissionGranted', (event) => {
  console.log(`${event.commanderId} granted ${event.permission} to ${event.targetAgentId}`);
});
```

## Default Permissions by Role

### Commander
- `*` (all permissions)

### Supervisor
- `read`, `write`, `execute`
- `mcp:layer0`, `mcp:layer1`

### Operator
- `read`, `write`
- `mcp:layer1`, `mcp:layer2`

### Observer
- `read`
- `mcp:layer2`

## Integration with MCP Servers

### Example: Restricting MCP Server Access

```javascript
const { AgentCoordinator, RingLayer } = require('./src/agent/agent-coordinator');

function executeMCPServer(agentId, serverName, coordinator) {
  // Determine required layer
  let requiredLayer;
  if (['filesystem', 'memory'].includes(serverName)) {
    requiredLayer = RingLayer.LAYER_0;
  } else if (['git', 'github', 'brave-search'].includes(serverName)) {
    requiredLayer = RingLayer.LAYER_1;
  } else {
    requiredLayer = RingLayer.LAYER_2;
  }
  
  // Check authority
  if (!coordinator.checkAuthority(agentId, requiredLayer)) {
    throw new Error(`Agent ${agentId} lacks authority for ${serverName} (requires Layer ${requiredLayer})`);
  }
  
  // Execute MCP server
  // ... implementation
}
```

## Security Considerations

1. **Single Commander**: Only one commander can exist. Attempting to register a second commander will throw an error.

2. **Audit Trail**: All authority operations are logged with timestamps for security auditing.

3. **Explicit Authority**: Agents must be explicitly registered before performing operations.

4. **Immutable Role Hierarchy**: Roles define baseline permissions that cannot be reduced (only extended).

## Testing

```bash
npm test src/agent/agent-coordinator.test.js
```

Coverage includes:
- Agent registration
- Ring layer assignment
- Authority checking
- Permission management
- Event emission
- Operation logging

## Integration with CATHEDRAL Projects

This system is designed to work with:

- **bambisleep-church-catgirl-control-tower**: MCP orchestration with ring layer enforcement
- **bambisleep-hypnosis-mcp**: Restrict trigger management to authorized agents
- **aigf-personality-mcp**: Control personality switching permissions
- **trigger-system-mcp**: Enforce compliance requirements through authority system
- **chat-analytics-mcp**: Protect sensitive analytics data

## Example: Complete Authority Flow

```javascript
const { AgentCoordinator, AgentRole, RingLayer } = require('./src/agent/agent-coordinator');

// Initialize coordinator
const coordinator = new AgentCoordinator();

// Register commander
const commander = coordinator.registerAgent(
  'commander-brandynette',
  AgentRole.COMMANDER,
  RingLayer.LAYER_0
);

// Register worker agents
coordinator.registerAgent('ai-agent-1', AgentRole.OPERATOR, RingLayer.LAYER_2);
coordinator.registerAgent('ai-agent-2', AgentRole.OBSERVER, RingLayer.LAYER_2);

// Commander promotes ai-agent-1
coordinator.assignRingLayer(
  'commander-brandynette',
  'ai-agent-1',
  RingLayer.LAYER_1
);

// Commander grants special permission
coordinator.grantPermission(
  'commander-brandynette',
  'ai-agent-1',
  'trigger:activate'
);

// Check authority before operation
if (coordinator.checkAuthority('ai-agent-1', RingLayer.LAYER_1, 'trigger:activate')) {
  console.log('ai-agent-1 authorized to activate triggers');
} else {
  console.log('ai-agent-1 NOT authorized');
}

// Review audit log
const log = coordinator.getOperationLog();
console.log('Recent operations:', log);
```

## License

MIT © BambiSleepChat

---

**Part of the CATHEDRAL Project** | [GitHub](https://github.com/BambiSleepChat)
