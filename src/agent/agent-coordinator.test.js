/// Law: Agent Coordinator Tests
/// SPDX-License-Identifier: MIT

const { AgentCoordinator, AgentRole, RingLayer } = require('./agent-coordinator');

describe('AgentCoordinator', () => {
  let coordinator;
  
  beforeEach(() => {
    coordinator = new AgentCoordinator();
  });
  
  describe('registerAgent', () => {
    it('should register a new agent', () => {
      const agent = coordinator.registerAgent('agent-1', AgentRole.OBSERVER, RingLayer.LAYER_2);
      
      expect(agent).toBeDefined();
      expect(agent.agentId).toBe('agent-1');
      expect(agent.role).toBe(AgentRole.OBSERVER);
      expect(agent.ringLayer).toBe(RingLayer.LAYER_2);
    });
    
    it('should register commander', () => {
      const commander = coordinator.registerAgent('commander-1', AgentRole.COMMANDER, RingLayer.LAYER_0);
      
      expect(commander.role).toBe(AgentRole.COMMANDER);
      expect(coordinator.commanderId).toBe('commander-1');
    });
    
    it('should throw error when registering second commander', () => {
      coordinator.registerAgent('commander-1', AgentRole.COMMANDER, RingLayer.LAYER_0);
      
      expect(() => {
        coordinator.registerAgent('commander-2', AgentRole.COMMANDER, RingLayer.LAYER_0);
      }).toThrow('Commander already exists');
    });
    
    it('should emit agentRegistered event', (done) => {
      coordinator.on('agentRegistered', (agent) => {
        expect(agent.agentId).toBe('agent-1');
        done();
      });
      
      coordinator.registerAgent('agent-1', AgentRole.OBSERVER, RingLayer.LAYER_2);
    });
  });
  
  describe('assignRingLayer', () => {
    it('should allow commander to assign ring layer', () => {
      coordinator.registerAgent('commander-1', AgentRole.COMMANDER, RingLayer.LAYER_0);
      coordinator.registerAgent('agent-1', AgentRole.OPERATOR, RingLayer.LAYER_2);
      
      const updated = coordinator.assignRingLayer('commander-1', 'agent-1', RingLayer.LAYER_1);
      
      expect(updated.ringLayer).toBe(RingLayer.LAYER_1);
    });
    
    it('should throw error when non-commander tries to assign', () => {
      coordinator.registerAgent('commander-1', AgentRole.COMMANDER, RingLayer.LAYER_0);
      coordinator.registerAgent('agent-1', AgentRole.OPERATOR, RingLayer.LAYER_2);
      coordinator.registerAgent('agent-2', AgentRole.OPERATOR, RingLayer.LAYER_2);
      
      expect(() => {
        coordinator.assignRingLayer('agent-2', 'agent-1', RingLayer.LAYER_1);
      }).toThrow('Only commander can assign ring layers');
    });
    
    it('should emit ringLayerAssigned event', (done) => {
      coordinator.registerAgent('commander-1', AgentRole.COMMANDER, RingLayer.LAYER_0);
      coordinator.registerAgent('agent-1', AgentRole.OPERATOR, RingLayer.LAYER_2);
      
      coordinator.on('ringLayerAssigned', (event) => {
        expect(event.targetAgentId).toBe('agent-1');
        expect(event.newLayer).toBe(RingLayer.LAYER_1);
        done();
      });
      
      coordinator.assignRingLayer('commander-1', 'agent-1', RingLayer.LAYER_1);
    });
  });
  
  describe('checkAuthority', () => {
    it('should return true when agent has required layer access', () => {
      coordinator.registerAgent('agent-1', AgentRole.SUPERVISOR, RingLayer.LAYER_0);
      
      const hasAccess = coordinator.checkAuthority('agent-1', RingLayer.LAYER_1);
      expect(hasAccess).toBe(true);
    });
    
    it('should return false when agent lacks layer access', () => {
      coordinator.registerAgent('agent-1', AgentRole.OBSERVER, RingLayer.LAYER_2);
      
      const hasAccess = coordinator.checkAuthority('agent-1', RingLayer.LAYER_0);
      expect(hasAccess).toBe(false);
    });
    
    it('should check specific permissions', () => {
      coordinator.registerAgent('agent-1', AgentRole.OPERATOR, RingLayer.LAYER_1);
      
      const hasPermission = coordinator.checkAuthority('agent-1', RingLayer.LAYER_1, 'write');
      expect(hasPermission).toBe(true);
    });
    
    it('should return false for unregistered agent', () => {
      const hasAccess = coordinator.checkAuthority('unknown-agent', RingLayer.LAYER_2);
      expect(hasAccess).toBe(false);
    });
  });
  
  describe('grantPermission', () => {
    it('should allow commander to grant permissions', () => {
      coordinator.registerAgent('commander-1', AgentRole.COMMANDER, RingLayer.LAYER_0);
      coordinator.registerAgent('agent-1', AgentRole.OBSERVER, RingLayer.LAYER_2);
      
      const updated = coordinator.grantPermission('commander-1', 'agent-1', 'special-access');
      
      expect(updated.permissions).toContain('special-access');
    });
    
    it('should throw error when non-commander tries to grant', () => {
      coordinator.registerAgent('commander-1', AgentRole.COMMANDER, RingLayer.LAYER_0);
      coordinator.registerAgent('agent-1', AgentRole.OPERATOR, RingLayer.LAYER_2);
      coordinator.registerAgent('agent-2', AgentRole.OBSERVER, RingLayer.LAYER_2);
      
      expect(() => {
        coordinator.grantPermission('agent-1', 'agent-2', 'special-access');
      }).toThrow('Only commander can grant permissions');
    });
  });
  
  describe('getAgentsByRole', () => {
    it('should return agents filtered by role', () => {
      coordinator.registerAgent('commander-1', AgentRole.COMMANDER, RingLayer.LAYER_0);
      coordinator.registerAgent('operator-1', AgentRole.OPERATOR, RingLayer.LAYER_1);
      coordinator.registerAgent('operator-2', AgentRole.OPERATOR, RingLayer.LAYER_2);
      coordinator.registerAgent('observer-1', AgentRole.OBSERVER, RingLayer.LAYER_2);
      
      const operators = coordinator.getAgentsByRole(AgentRole.OPERATOR);
      
      expect(operators).toHaveLength(2);
      expect(operators.every(a => a.role === AgentRole.OPERATOR)).toBe(true);
    });
  });
  
  describe('getAgentsByRingLayer', () => {
    it('should return agents filtered by ring layer', () => {
      coordinator.registerAgent('agent-1', AgentRole.SUPERVISOR, RingLayer.LAYER_0);
      coordinator.registerAgent('agent-2', AgentRole.OPERATOR, RingLayer.LAYER_1);
      coordinator.registerAgent('agent-3', AgentRole.OBSERVER, RingLayer.LAYER_2);
      coordinator.registerAgent('agent-4', AgentRole.OBSERVER, RingLayer.LAYER_2);
      
      const layer2Agents = coordinator.getAgentsByRingLayer(RingLayer.LAYER_2);
      
      expect(layer2Agents).toHaveLength(2);
      expect(layer2Agents.every(a => a.ringLayer === RingLayer.LAYER_2)).toBe(true);
    });
  });
  
  describe('getOperationLog', () => {
    it('should track all operations', () => {
      coordinator.registerAgent('commander-1', AgentRole.COMMANDER, RingLayer.LAYER_0);
      coordinator.registerAgent('agent-1', AgentRole.OPERATOR, RingLayer.LAYER_2);
      coordinator.assignRingLayer('commander-1', 'agent-1', RingLayer.LAYER_1);
      coordinator.grantPermission('commander-1', 'agent-1', 'special-access');
      
      const log = coordinator.getOperationLog();
      
      expect(log.length).toBeGreaterThanOrEqual(4);
      expect(log.some(l => l.operation === 'register')).toBe(true);
      expect(log.some(l => l.operation === 'assignRingLayer')).toBe(true);
      expect(log.some(l => l.operation === 'grantPermission')).toBe(true);
    });
    
    it('should limit log size', () => {
      coordinator.registerAgent('commander-1', AgentRole.COMMANDER, RingLayer.LAYER_0);
      
      const log = coordinator.getOperationLog(1);
      
      expect(log.length).toBeLessThanOrEqual(1);
    });
  });
});
