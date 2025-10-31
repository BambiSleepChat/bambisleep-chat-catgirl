// Assets/Scripts/Character/CatgirlController.cs
// 🌸 BambiSleep™ Church CatGirl Avatar Controller
// Full NetworkBehaviour implementation with Unity Gaming Services integration

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
    public AudioClip meowSound;
    public AudioClip cowPowerSound;

    private CharacterController characterController;
    private Animator animator;
    private AudioSource audioSource;
    private InventorySystem inventory;
    private UniversalBankingSystem banking;
    private Vector3 velocity;
    private bool isGrounded;

    // Animation Parameters (Mecanim)
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsPurring = Animator.StringToHash("IsPurring");
    private static readonly int CowPowerActive = Animator.StringToHash("CowPowerActive");
    private static readonly int Cuteness = Animator.StringToHash("Cuteness");

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        inventory = GetComponent<InventorySystem>();
        banking = GetComponent<UniversalBankingSystem>();
    }

    public override void OnNetworkSpawn()
    {
        // Initialize networked catgirl systems
        if (IsOwner)
        {
            InitializeCatgirlSystems();
        }

        // Set initial cuteness level for all clients
        if (animator != null)
        {
            animator.SetInteger(Cuteness, stats.cuteness);
        }
    }

    private void Update()
    {
        if (!IsOwner) return;

        HandleMovement();
        HandleJumping();
        UpdateAnimations();
    }

    private void InitializeCatgirlSystems()
    {
        // 🌸 Activate pink frilly aura
        ActivatePinkFrillyAura();

        // 🐱 Initialize purring subsystem
        StartCoroutine(PurringCycle());

        // 💰 Connect to universal banking
        if (banking != null)
        {
            banking.ConnectToUniversalBank();
        }

        // 🎯 Unlock secret cow powers if eligible
        if (stats.hasSecretCowPowers)
        {
            UnlockSecretCowPowers();
        }

        Debug.Log("🌸 BambiSleep™ CatGirl systems initialized! Nyan nyan nyan! 🌸");
    }

    private void ActivatePinkFrillyAura()
    {
        // Implementation for pink frilly visual effects
        var aura = GetComponent<ParticleSystem>();
        if (aura != null)
        {
            var main = aura.main;
            main.startColor = Color.magenta * stats.pinkIntensity;
            main.startSize = stats.frillinessLevel / 100f;
        }
    }

    private IEnumerator PurringCycle()
    {
        while (gameObject.activeInHierarchy)
        {
            if (animator != null)
            {
                animator.SetBool(IsPurring, true);
            }

            if (audioSource != null && purringSound != null)
            {
                audioSource.PlayOneShot(purringSound);
            }

            // Apply purring levitation effect
            if (characterController != null)
            {
                velocity.y += purringLevitation * Time.deltaTime;
            }

            yield return new WaitForSeconds(1f / stats.purringFrequency);

            if (animator != null)
            {
                animator.SetBool(IsPurring, false);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void UnlockSecretCowPowers()
    {
        // 🐄 Secret cow powers implementation
        Debug.Log("🐄 MOO! Secret Diablo-level cow powers ACTIVATED! 🐄");

        if (animator != null)
        {
            animator.SetBool(CowPowerActive, true);
        }

        if (audioSource != null && cowPowerSound != null)
        {
            audioSource.PlayOneShot(cowPowerSound);
        }

        // Apply cow power stat multipliers
        stats.factorioProductionMultiplier *= 10;
        stats.eldritchEnergy += 6666.0f;

        TriggerCowPowerEffectsServerRpc();
    }

    [ServerRpc]
    private void TriggerCowPowerEffectsServerRpc()
    {
        // Broadcast cow power activation to all clients
        TriggerCowPowerEffectsClientRpc();
    }

    [ClientRpc]
    private void TriggerCowPowerEffectsClientRpc()
    {
        // Visual effects for all clients
        Debug.Log("🐄 COW POWER WAVES DETECTED! ALL CATGIRLS RECEIVE +1000% CUTENESS! 🐄");
    }

    private void HandleMovement()
    {
        // Get input (replace with your input system)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Apply gravity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        // Check if grounded
        isGrounded = characterController.isGrounded;
    }

    private void HandleJumping()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);

            if (animator != null)
            {
                animator.SetBool(IsJumping, true);
            }

            if (audioSource != null && meowSound != null)
            {
                audioSource.PlayOneShot(meowSound);
            }
        }
        else if (isGrounded && animator != null)
        {
            animator.SetBool(IsJumping, false);
        }
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        // Calculate movement speed for animation
        Vector3 localVelocity = transform.InverseTransformDirection(characterController.velocity);
        float speed = localVelocity.magnitude;

        animator.SetFloat(Speed, speed);
    }

    // Public methods for inventory/banking integration
    public void ActivatePowerArmor()
    {
        stats.powerArmorActive = true;
        Debug.Log("⚡ CYBER ELDRITCH POWER ARMOR ACTIVATED! ⚡");
    }

    public void IncreaseFrilliness(float amount)
    {
        stats.frillinessLevel = Mathf.Min(10000f, stats.frillinessLevel + amount);
        Debug.Log($"🌸 Frilliness increased to {stats.frillinessLevel}! 🌸");
    }
}
