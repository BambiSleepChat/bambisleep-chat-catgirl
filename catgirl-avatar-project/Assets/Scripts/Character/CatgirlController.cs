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

    // Animation Parameters (Mecanim)
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsPurring = Animator.StringToHash("IsPurring");
    private static readonly int CowPowerActive = Animator.StringToHash("CowPowerActive");

    // Movement state
    private Vector3 moveInput;
    private Vector3 velocity;
    private bool isGrounded;
    private bool jumpRequested;
    private const float gravity = -9.81f;

    // Network synchronized stats
    private NetworkVariable<float> networkPinkIntensity = new NetworkVariable<float>(1.0f);
    private NetworkVariable<bool> networkCowPowersActive = new NetworkVariable<bool>(false);

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

            // Synchronize initial stats to network
            UpdateNetworkStatsServerRpc(stats.pinkIntensity, stats.hasSecretCowPowers);
        }

        // Subscribe to network variable changes
        networkPinkIntensity.OnValueChanged += OnPinkIntensityChanged;
        networkCowPowersActive.OnValueChanged += OnCowPowersChanged;
    }

    private void Update()
    {
        if (!IsOwner) return;

        // Check if grounded
        isGrounded = characterController.isGrounded;

        // Handle movement
        HandleMovement();

        // Handle jump
        HandleJump();

        // Apply gravity
        ApplyGravity();

        // Update animations
        UpdateAnimations();
    }

    private void HandleMovement()
    {
        // Get input from new Input System
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.z);

        // Apply purring levitation bonus
        float currentSpeed = moveSpeed;
        if (animator != null && animator.GetBool(IsPurring))
        {
            currentSpeed += purringLevitation;
        }

        // Apply cow power speed boost
        if (stats.hasSecretCowPowers && stats.powerArmorActive)
        {
            currentSpeed *= 1.5f;
        }

        // Move character
        characterController.Move(move * currentSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (jumpRequested && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            jumpRequested = false;

            if (animator != null)
                animator.SetBool(IsJumping, true);
        }
        else if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            if (animator != null)
                animator.SetBool(IsJumping, false);
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        // Update speed parameter
        float speed = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;
        animator.SetFloat(Speed, speed);
    }

    // Input System callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        Vector2 input = context.ReadValue<Vector2>();
        moveInput = new Vector3(input.x, 0, input.y);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        if (context.performed)
        {
            jumpRequested = true;
        }
    }

    public void OnPurr(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        if (context.performed)
        {
            TogglePurringServerRpc();
        }
    }

    // Network synchronization
    [ServerRpc]
    private void UpdateNetworkStatsServerRpc(float pinkIntensity, bool cowPowersActive)
    {
        networkPinkIntensity.Value = pinkIntensity;
        networkCowPowersActive.Value = cowPowersActive;
    }

    [ServerRpc]
    private void TogglePurringServerRpc()
    {
        TogglePurringClientRpc();
    }

    [ClientRpc]
    private void TogglePurringClientRpc()
    {
        if (audioSource != null && purringSound != null)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
            else
                audioSource.PlayOneShot(purringSound);
        }
    }

    private void OnPinkIntensityChanged(float oldValue, float newValue)
    {
        stats.pinkIntensity = newValue;
        ActivatePinkFrillyAura();
    }

    private void OnCowPowersChanged(bool oldValue, bool newValue)
    {
        stats.hasSecretCowPowers = newValue;
        if (newValue)
        {
            UnlockSecretCowPowers();
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
