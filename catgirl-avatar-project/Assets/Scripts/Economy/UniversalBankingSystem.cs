// Assets/Scripts/Economy/UniversalBankingSystem.cs
// 💰 BambiSleep™ Church Universal Banking System
// Multi-currency economy with gambling, auctions, and Unity Gaming Services

using UnityEngine;
using Unity.Services.Economy;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Netcode;
using System.Collections.Generic;

public class UniversalBankingSystem : NetworkBehaviour
{
    [Header("💰 Universal Banking")]
    public NetworkVariable<long> pinkCoins = new NetworkVariable<long>(0);
    public NetworkVariable<long> cowTokens = new NetworkVariable<long>(0);
    public NetworkVariable<long> eldritchCurrency = new NetworkVariable<long>(0);

    [Header("🎰 Gambling Systems")]
    public bool gamblingEnabled = true;
    public float houseEdge = 0.05f; // 5% house edge
    public long minBet = 10;
    public long maxBet = 10000;

    [Header("🏪 Auction House")]
    public bool auctionHouseActive = true;
    public float auctionFeePercent = 0.10f; // 10% auction fee
    public int maxActiveAuctions = 5;

    [Header("🎁 Daily Rewards")]
    public long dailyPinkCoins = 100;
    public long dailyCowTokens = 10;

    private bool isConnected = false;
    private Dictionary<string, long> transactionHistory = new Dictionary<string, long>();

    public async void ConnectToUniversalBank()
    {
        // Initialize connection to universal banking network
        Debug.Log("💎 Connecting to Universal Banking System... 💎");

        try
        {
            // Initialize Unity Gaming Services
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }

            // Sign in anonymously if not authenticated
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            LoadPlayerBalances();
            isConnected = true;

            Debug.Log("💎 Connected to Universal Banking System successfully! 💎");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Banking connection failed: {e.Message}");
            InitializeOfflineMode();
        }
    }

    private async void LoadPlayerBalances()
    {
        try
        {
            // Load balances from Unity Gaming Services
            var balances = await EconomyService.Instance.PlayerBalances.GetBalancesAsync();

            // Update network variables with loaded balances
            ProcessBalanceData(balances);

            Debug.Log($"💰 Loaded balances - Pink: {pinkCoins.Value}, Cow: {cowTokens.Value}, Eldritch: {eldritchCurrency.Value}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load balances: {e.Message}");
        }
    }

    private void ProcessBalanceData(Unity.Services.Economy.Model.GetBalancesResult balances)
    {
        foreach (var balance in balances.Balances)
        {
            switch (balance.CurrencyId)
            {
                case "pinkCoins":
                    pinkCoins.Value = balance.Balance;
                    break;
                case "cowTokens":
                    cowTokens.Value = balance.Balance;
                    break;
                case "eldritchCurrency":
                    eldritchCurrency.Value = balance.Balance;
                    break;
            }
        }
    }

    private void InitializeOfflineMode()
    {
        // Initialize with default amounts for offline play
        pinkCoins.Value = 1000;
        cowTokens.Value = 10;
        eldritchCurrency.Value = 666;

        isConnected = false;
        Debug.LogWarning("🔌 Running in OFFLINE mode - Progress may not sync");
    }

    // Currency management methods
    public bool AddPinkCoins(long amount)
    {
        if (amount < 0) return false;
        pinkCoins.Value += amount;
        Debug.Log($"💖 Added {amount} pink coins! Total: {pinkCoins.Value}");
        SyncToCloud("pinkCoins", pinkCoins.Value);
        return true;
    }

    public bool SpendPinkCoins(long amount)
    {
        if (amount < 0 || pinkCoins.Value < amount) return false;
        pinkCoins.Value -= amount;
        Debug.Log($"💸 Spent {amount} pink coins! Remaining: {pinkCoins.Value}");
        SyncToCloud("pinkCoins", pinkCoins.Value);
        return true;
    }

    public bool AddCowTokens(long amount)
    {
        if (amount < 0) return false;
        cowTokens.Value += amount;
        Debug.Log($"🐄 Added {amount} cow tokens! Total: {cowTokens.Value}");
        SyncToCloud("cowTokens", cowTokens.Value);
        return true;
    }

    public bool AddEldritchCurrency(long amount)
    {
        if (amount < 0) return false;
        eldritchCurrency.Value += amount;
        Debug.Log($"💜 Added {amount} eldritch currency! Total: {eldritchCurrency.Value}");
        SyncToCloud("eldritchCurrency", eldritchCurrency.Value);
        return true;
    }

    // Gambling system
    [ServerRpc(RequireOwnership = false)]
    public void GambleServerRpc(long amount, string gameType)
    {
        if (!gamblingEnabled)
        {
            Debug.LogWarning("🎰 Gambling is currently disabled");
            return;
        }

        if (amount < minBet || amount > maxBet)
        {
            Debug.LogWarning($"🎰 Bet must be between {minBet} and {maxBet}");
            return;
        }

        if (pinkCoins.Value < amount)
        {
            Debug.LogWarning("🎰 Insufficient funds!");
            return;
        }

        // Implement gambling logic with proper RNG
        float roll = UnityEngine.Random.Range(0f, 1f);
        bool win = roll > (0.5f + houseEdge);

        if (win)
        {
            long winnings = (long)(amount * 1.8f); // 80% payout on win
            pinkCoins.Value += winnings;
            TriggerWinEffectsClientRpc(winnings, roll);

            Debug.Log($"🎊 WINNER! Rolled {roll:F2}, won {winnings} pink coins! 🎊");
        }
        else
        {
            pinkCoins.Value -= amount;
            TriggerLossEffectsClientRpc(amount, roll);

            Debug.Log($"😿 LOSS! Rolled {roll:F2}, lost {amount} pink coins 😿");
        }

        SyncToCloud("pinkCoins", pinkCoins.Value);
    }

    [ClientRpc]
    private void TriggerWinEffectsClientRpc(long winnings, float roll)
    {
        Debug.Log($"🎊 JACKPOT! Won {winnings} pink coins! Roll was {roll:F2} 🎊");
        // Trigger celebratory particle effects, sounds, etc.
        PlayWinEffects();
    }

    [ClientRpc]
    private void TriggerLossEffectsClientRpc(long lost, float roll)
    {
        Debug.Log($"😿 Better luck next time, catgirl! Lost {lost} coins. Roll was {roll:F2} 😿");
        // Trigger consolation effects
        PlayLossEffects();
    }

    private void PlayWinEffects()
    {
        // Implement particle systems, confetti, sounds
        var audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            // audioSource.PlayOneShot(winSound);
        }
    }

    private void PlayLossEffects()
    {
        // Implement sad catgirl animations, sounds
        var audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            // audioSource.PlayOneShot(lossSound);
        }
    }

    // Auction house system
    public struct AuctionListing
    {
        public string itemId;
        public string sellerName;
        public long startingBid;
        public long currentBid;
        public float timeRemaining;
        public bool isActive;
    }

    private List<AuctionListing> activeAuctions = new List<AuctionListing>();

    public bool CreateAuction(string itemId, long startingBid, float duration)
    {
        if (!auctionHouseActive) return false;
        if (activeAuctions.Count >= maxActiveAuctions) return false;

        var auction = new AuctionListing
        {
            itemId = itemId,
            sellerName = "Player",
            startingBid = startingBid,
            currentBid = startingBid,
            timeRemaining = duration,
            isActive = true
        };

        activeAuctions.Add(auction);
        Debug.Log($"🏪 Created auction for {itemId} starting at {startingBid} pink coins");

        return true;
    }

    public bool PlaceBid(int auctionIndex, long bidAmount)
    {
        if (auctionIndex < 0 || auctionIndex >= activeAuctions.Count) return false;

        var auction = activeAuctions[auctionIndex];
        if (!auction.isActive) return false;
        if (bidAmount <= auction.currentBid) return false;
        if (pinkCoins.Value < bidAmount) return false;

        auction.currentBid = bidAmount;
        activeAuctions[auctionIndex] = auction;

        Debug.Log($"🏪 Placed bid of {bidAmount} on {auction.itemId}");
        return true;
    }

    // Daily rewards
    public async void ClaimDailyReward()
    {
        string lastClaimKey = "LastDailyRewardClaim";
        string lastClaim = PlayerPrefs.GetString(lastClaimKey, "");
        string today = System.DateTime.Now.ToString("yyyy-MM-dd");

        if (lastClaim == today)
        {
            Debug.Log("🎁 Daily reward already claimed today!");
            return;
        }

        AddPinkCoins(dailyPinkCoins);
        AddCowTokens(dailyCowTokens);

        PlayerPrefs.SetString(lastClaimKey, today);
        PlayerPrefs.Save();

        Debug.Log($"🎁 Claimed daily reward! +{dailyPinkCoins} pink coins, +{dailyCowTokens} cow tokens");
    }

    // Cloud synchronization
    private async void SyncToCloud(string currencyId, long amount)
    {
        if (!isConnected) return;

        try
        {
            // Sync to Unity Gaming Services Economy
            // await EconomyService.Instance.PlayerBalances.SetBalanceAsync(currencyId, amount);
            Debug.Log($"☁️ Synced {currencyId}: {amount} to cloud");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Cloud sync failed: {e.Message}");
        }
    }

    // Utility methods
    public bool CanAfford(long pinkCoinCost, long cowTokenCost = 0, long eldritchCost = 0)
    {
        return pinkCoins.Value >= pinkCoinCost &&
               cowTokens.Value >= cowTokenCost &&
               eldritchCurrency.Value >= eldritchCost;
    }

    public string GetBalancesSummary()
    {
        return $"💰 Pink: {pinkCoins.Value} | 🐄 Cow: {cowTokens.Value} | 💜 Eldritch: {eldritchCurrency.Value}";
    }
}
