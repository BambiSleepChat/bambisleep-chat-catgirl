/// Law: Commander-Brandynette Agent Authority Coordinator
/// SPDX-License-Identifier: MIT

const EventEmitter = require('events');

//<3 Lore: Hierarchical agent control system inspired by Commander-Brandynette
//<3 Enforces ring layer access and coordinates multi-agent systems

/// Law: Ring layer access levels (0 = highest, 2 = lowest)
const RingLayer = {
  LAYER_0: 0, // Primitives: filesystem, memory
  LAYER_1: 1, // Foundation: git, github, brave-search
  LAYER_2: 2, // Advanced: sequential-thinking, postgres, everything, custom servers
};

/// Law: Agent role definitions
const AgentRole = {
  COMMANDER: 'commander',      // Full authority, can assign ring layers
  SUPERVISOR: 'supervisor',    // Layer 0-1 access
  OPERATOR: 'operator',        // Layer 1-2 access
  OBSERVER: 'observer',        // Read-only, Layer 2 only
};

/// Law: Agent authority structure
class AgentAuthority {
  constructor(data) {
    this.agentId = data.agentId;
    this.role = data.role;
    this.ringLayer = data.ringLayer;
    this.permissions = data.permissions || [];
    this.created = data.created || new Date().toISOString();
    this.lastActive = data.lastActive || new Date().toISOString();
  }

  canAccess(requiredLayer) {
    return this.ringLayer <= requiredLayer;
  }

  hasPermission(permission) {
    return this.permissions.includes(permission) || this.role === AgentRole.COMMANDER;
  }
}

//!? Guardrail: Agent coordination requires explicit authority assignment
//!? All agents must be registered before performing privileged operations

/// Law: Agent coordinator with hierarchical control
class AgentCoordinator extends EventEmitter {
  constructor() {
    super();
    this.agents = new Map();
    this.commanderId = null;
    this.operationLog = [];
  }

  /**
   * Register a new agent in the system
   * @param {string} agentId - Unique agent identifier
   * @param {string} role - Agent role (commander, supervisor, operator, observer)
   * @param {number} ringLayer - Ring layer access (0-2)
   * @returns {AgentAuthority} Registered agent authority
   */
  registerAgent(agentId, role = AgentRole.OBSERVER, ringLayer = RingLayer.LAYER_2) {
    //!? Guardrail: Only one commander allowed
    if (role === AgentRole.COMMANDER && this.commanderId && this.commanderId !== agentId) {
      throw new Error('Commander already exists. Only one commander allowed.');
    }

    const authority = new AgentAuthority({
      agentId,
      role,
      ringLayer,
      permissions: this._getDefaultPermissions(role),
    });

    this.agents.set(agentId, authority);

    if (role === AgentRole.COMMANDER) {
      this.commanderId = agentId;
    }

    this.emit('agentRegistered', authority);
    this._logOperation(agentId, 'register', { role, ringLayer });

    return authority;
  }

  /**
   * Assign ring layer access to an agent (commander only)
   * @param {string} commanderId - Commander agent ID
   * @param {string} targetAgentId - Target agent to modify
   * @param {number} ringLayer - New ring layer (0-2)
   * @returns {AgentAuthority} Updated agent authority
   */
  assignRingLayer(commanderId, targetAgentId, ringLayer) {
    const commander = this.agents.get(commanderId);
    if (!commander || commander.role !== AgentRole.COMMANDER) {
      throw new Error('Only commander can assign ring layers');
    }

    const targetAgent = this.agents.get(targetAgentId);
    if (!targetAgent) {
      throw new Error(`Agent not found: ${targetAgentId}`);
    }

    const previousLayer = targetAgent.ringLayer;
    targetAgent.ringLayer = ringLayer;
    targetAgent.lastActive = new Date().toISOString();

    this.emit('ringLayerAssigned', {
      commanderId,
      targetAgentId,
      previousLayer,
      newLayer: ringLayer,
    });

    this._logOperation(commanderId, 'assignRingLayer', {
      targetAgentId,
      previousLayer,
      newLayer: ringLayer,
    });

    return targetAgent;
  }

  /**
   * Check if an agent has authority to perform an operation
   * @param {string} agentId - Agent requesting operation
   * @param {number} requiredLayer - Required ring layer access
   * @param {string} permission - Optional specific permission
   * @returns {boolean} True if authorized
   */
  checkAuthority(agentId, requiredLayer, permission = null) {
    const agent = this.agents.get(agentId);
    if (!agent) {
      return false;
    }

    const layerAccess = agent.canAccess(requiredLayer);
    const permissionAccess = permission ? agent.hasPermission(permission) : true;

    return layerAccess && permissionAccess;
  }

  /**
   * Grant permission to an agent (commander only)
   * @param {string} commanderId - Commander agent ID
   * @param {string} targetAgentId - Target agent
   * @param {string} permission - Permission to grant
   * @returns {AgentAuthority} Updated agent authority
   */
  grantPermission(commanderId, targetAgentId, permission) {
    const commander = this.agents.get(commanderId);
    if (!commander || commander.role !== AgentRole.COMMANDER) {
      throw new Error('Only commander can grant permissions');
    }

    const targetAgent = this.agents.get(targetAgentId);
    if (!targetAgent) {
      throw new Error(`Agent not found: ${targetAgentId}`);
    }

    if (!targetAgent.permissions.includes(permission)) {
      targetAgent.permissions.push(permission);
    }

    this.emit('permissionGranted', {
      commanderId,
      targetAgentId,
      permission,
    });

    this._logOperation(commanderId, 'grantPermission', {
      targetAgentId,
      permission,
    });

    return targetAgent;
  }

  /**
   * Get all agents in the system
   * @returns {Array<AgentAuthority>} Array of agent authorities
   */
  getAllAgents() {
    return Array.from(this.agents.values());
  }

  /**
   * Get agents by role
   * @param {string} role - Agent role to filter by
   * @returns {Array<AgentAuthority>} Filtered agents
   */
  getAgentsByRole(role) {
    return Array.from(this.agents.values()).filter(agent => agent.role === role);
  }

  /**
   * Get agents by ring layer
   * @param {number} ringLayer - Ring layer to filter by
   * @returns {Array<AgentAuthority>} Filtered agents
   */
  getAgentsByRingLayer(ringLayer) {
    return Array.from(this.agents.values()).filter(agent => agent.ringLayer === ringLayer);
  }

  /**
   * Get operation log
   * @param {number} limit - Maximum number of logs to return
   * @returns {Array} Operation log entries
   */
  getOperationLog(limit = 100) {
    return this.operationLog.slice(-limit);
  }

  /**
   * Default permissions based on role
   * @private
   */
  _getDefaultPermissions(role) {
    switch (role) {
      case AgentRole.COMMANDER:
        return ['*']; // All permissions
      case AgentRole.SUPERVISOR:
        return ['read', 'write', 'execute', 'mcp:layer0', 'mcp:layer1'];
      case AgentRole.OPERATOR:
        return ['read', 'write', 'mcp:layer1', 'mcp:layer2'];
      case AgentRole.OBSERVER:
        return ['read', 'mcp:layer2'];
      default:
        return ['read'];
    }
  }

  /**
   * Log operation for audit trail
   * @private
   */
  _logOperation(agentId, operation, details) {
    this.operationLog.push({
      timestamp: new Date().toISOString(),
      agentId,
      operation,
      details,
    });

    // Keep log size manageable
    if (this.operationLog.length > 10000) {
      this.operationLog.shift();
    }
  }
}

//-> Strategy: Export coordinator and constants
module.exports = {
  AgentCoordinator,
  AgentAuthority,
  RingLayer,
  AgentRole,
};
