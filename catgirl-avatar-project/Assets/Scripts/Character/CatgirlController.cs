// Assets/Scripts/Character/CatgirlController.cs
// 🌸 BambiSleep™ Church CatGirl Avatar Controller 🌸
// Sacred pink frilly catgirl implementation for Unity 6.2 LTS

using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using System.Collections;

[System.Serializable]
public class CatgirlStats
{
    [Header("✨ Frilly Pink Configuration")]
    public float pinkIntensity = 1.0f;
    public float frillinessLevel = 100.0f;
    
    [Header("🐱 Catgirl Properties")]  
    public float purringFrequency = 2.5f;
    public int cuteness = 9999;
    public bool hasSecretCowPowers = true;
    
    [Header("⚡ Cyber Eldritch Terror Stats")]
    public float eldritchEnergy = 666.0f;
    public int factorioProductionMultiplier = 1000;
    public bool powerArmorActive = false;
}

public class CatgirlController : NetworkBehaviour
{
    [Header("🌸 Sacred Configuration")]
    public CatgirlStats stats = new CatgirlStats();
    
    [Header("💎 Movement & Physics")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 12.0f;
    public float purringLevitation = 0.5f;
    
    [Header("🎵 Audio Configuration")]
    public AudioClip purringSound;
    public AudioClip nyanSound;
    public AudioClip cowMooSound;
    
    private CharacterController characterController;
    private Animator animator;
    private AudioSource audioSource;
    // Note: InventorySystem and UniversalBankingSystem will be implemented separately
    
    // Animation Parameters (Mecanim)
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsPurring = Animator.StringToHash("IsPurring");
    private static readonly int CowPowerActive = Animator.StringToHash("CowPowerActive");
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        // Ensure essential components exist
        if (characterController == null)
            characterController = gameObject.AddComponent<CharacterController>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
    
    public override void OnNetworkSpawn()
    {
        // Initialize networked catgirl systems
        if (IsOwner)
        {
            InitializeCatgirlSystems();
        }
    }
    
    private void InitializeCatgirlSystems()
    {
        Debug.Log("🌸 Initializing BambiSleep™ CatGirl Systems... 🌸");
        
        // 🌸 Activate pink frilly aura
        ActivatePinkFrillyAura();
        
        // 🐱 Initialize purring subsystem
        StartCoroutine(PurringCycle());
        
        // 🎯 Unlock secret cow powers if eligible
        if (stats.hasSecretCowPowers)
        {
            UnlockSecretCowPowers();
        }
        
        Debug.Log("💖 CatGirl Systems Online! Cuteness Level: MAXIMUM OVERDRIVE! 💖");
    }
    
    private void ActivatePinkFrillyAura()
    {
        // Implementation for pink frilly visual effects
        var aura = GetComponent<ParticleSystem>();
        if (aura != null)
        {
            var main = aura.main;
            main.startColor = Color.magenta * stats.pinkIntensity;
            Debug.Log($"💎 Pink Frilly Aura activated! Intensity: {stats.pinkIntensity}");
        }
    }
    
    private IEnumerator PurringCycle()
    {
        while (gameObject.activeInHierarchy)
        {
            if (animator != null)
                animator.SetBool(IsPurring, true);
                
            if (audioSource != null && purringSound != null)
                audioSource.PlayOneShot(purringSound);
                
            yield return new WaitForSeconds(1f / stats.purringFrequency);
            
            if (animator != null)
                animator.SetBool(IsPurring, false);
                
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    private void UnlockSecretCowPowers()
    {
        // 🐄 Secret cow powers implementation
        Debug.Log("🐄 MOO! Secret Diablo-level cow powers ACTIVATED! 🐄");
        
        if (animator != null)
            animator.SetBool(CowPowerActive, true);
            
        if (audioSource != null && cowMooSound != null)
            audioSource.PlayOneShot(cowMooSound);
            
        // Increase factorio production capabilities
        stats.factorioProductionMultiplier *= 2;
        Debug.Log($"🔧 Factorio Production Multiplier: {stats.factorioProductionMultiplier}x");
    }
    
    // 🌈 Public API for external systems
    public void PlayNyanSound()
    {
        if (audioSource != null && nyanSound != null)
        {
            audioSource.PlayOneShot(nyanSound);
            Debug.Log("🌈 NYAN NYAN NYAN! 🌈");
        }
    }
    
    public void IncreaseCuteness(int amount)
    {
        stats.cuteness += amount;
        Debug.Log($"💖 Cuteness increased to {stats.cuteness}! 💖");
    }
    
    public void ActivateEldritchMode()
    {
        stats.powerArmorActive = !stats.powerArmorActive;
        Debug.Log($"⚡ Cyber Eldritch Terror Mode: {(stats.powerArmorActive ? "ACTIVATED" : "DEACTIVATED")} ⚡");
    }
}