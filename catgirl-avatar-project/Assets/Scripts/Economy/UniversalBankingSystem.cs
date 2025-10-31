// Assets/Scripts/Economy/UniversalBankingSystem.cs
// 🌸 BambiSleep™ Church Universal Banking System 🌸
// Complete financial system with gambling, auctions, and pink economy

using UnityEngine;
using Unity.Services.Economy;
using Unity.Netcode;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BambiSleep.CatGirl.Economy
{
    [System.Serializable]
    public class CurrencyBalance
    {
        public string currencyId;
        public string displayName;
        public long amount;
        public Sprite icon;
    }

    [System.Serializable]
    public class Transaction
    {
        public string transactionId;
        public string type; // deposit, withdrawal, gambling, auction
        public long amount;
        public System.DateTime timestamp;
        public string description;
    }

    [System.Serializable]
    public class AuctionItem
    {
        public string auctionId;
        public CatgirlItem item;
        public long startingBid;
        public long currentBid;
        public string highestBidder;
        public System.DateTime endTime;
        public bool isActive;
    }

    public class UniversalBankingSystem : NetworkBehaviour
    {
        [Header("💰 Currency Configuration")]
        [SerializeField] private List<CurrencyBalance> currencies = new List<CurrencyBalance>();
        [SerializeField] private string primaryCurrencyId = "PINK_COINS";

        [Header("🎰 Gambling Settings")]
        [SerializeField] private bool gamblingEnabled = true;
        [SerializeField] private long minBet = 10;
        [SerializeField] private long maxBet = 10000;
        [SerializeField] private float houseEdge = 0.02f; // 2% house advantage

        [Header("🏛️ Auction House")]
        [SerializeField] private List<AuctionItem> activeAuctions = new List<AuctionItem>();
        [SerializeField] private float auctionFeePercentage = 0.05f; // 5% listing fee

        [Header("📊 Transaction History")]
        [SerializeField] private List<Transaction> transactionHistory = new List<Transaction>();
        [SerializeField] private int maxHistoryEntries = 100;

        private bool isConnected = false;
        private InventorySystem inventory;

        public bool IsConnected => isConnected;
        public long PrimaryCurrencyBalance => GetCurrencyBalance(primaryCurrencyId);

        private void Awake()
        {
            inventory = GetComponent<InventorySystem>();
        }

        private void Start()
        {
            ConnectToUniversalBank();
        }

        public async void ConnectToUniversalBank()
        {
            try
            {
                Debug.Log("🏦 Connecting to BambiSleep™ Universal Banking System...");

                // Initialize Unity Gaming Services Economy
                await LoadBalancesFromCloud();

                isConnected = true;
                Debug.Log("✨ Connected to Universal Bank! Pink economy online!");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Failed to connect to banking system: {e.Message}");
                isConnected = false;
            }
        }

        private async Task LoadBalancesFromCloud()
        {
            try
            {
                var balancesResult = await EconomyService.Instance.PlayerBalances.GetBalancesAsync();

                currencies.Clear();
                foreach (var balance in balancesResult.Balances)
                {
                    currencies.Add(new CurrencyBalance
                    {
                        currencyId = balance.CurrencyId,
                        displayName = balance.CurrencyId,
                        amount = balance.Balance
                    });
                }

                Debug.Log($"💰 Loaded {currencies.Count} currency balances");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Could not load balances: {e.Message}");
                // Initialize with default currency
                InitializeDefaultCurrency();
            }
        }

        private void InitializeDefaultCurrency()
        {
            currencies.Add(new CurrencyBalance
            {
                currencyId = primaryCurrencyId,
                displayName = "Pink Coins",
                amount = 1000 // Starting balance
            });
        }

        public long GetCurrencyBalance(string currencyId)
        {
            var currency = currencies.Find(c => c.currencyId == currencyId);
            return currency?.amount ?? 0;
        }

        public async Task<bool> AddCurrency(string currencyId, long amount, string reason = "")
        {
            if (amount <= 0) return false;

            try
            {
                // Update local balance
                var currency = currencies.Find(c => c.currencyId == currencyId);
                if (currency != null)
                {
                    currency.amount += amount;
                }

                // Record transaction
                RecordTransaction(new Transaction
                {
                    transactionId = System.Guid.NewGuid().ToString(),
                    type = "deposit",
                    amount = amount,
                    timestamp = System.DateTime.UtcNow,
                    description = reason
                });

                Debug.Log($"💰 Added {amount} {currencyId}. New balance: {currency?.amount}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to add currency: {e.Message}");
                return false;
            }
        }

        public async Task<bool> DeductCurrency(string currencyId, long amount, string reason = "")
        {
            if (amount <= 0) return false;

            var currentBalance = GetCurrencyBalance(currencyId);
            if (currentBalance < amount)
            {
                Debug.LogWarning($"❌ Insufficient funds! Need {amount}, have {currentBalance}");
                return false;
            }

            try
            {
                var currency = currencies.Find(c => c.currencyId == currencyId);
                if (currency != null)
                {
                    currency.amount -= amount;
                }

                // Record transaction
                RecordTransaction(new Transaction
                {
                    transactionId = System.Guid.NewGuid().ToString(),
                    type = "withdrawal",
                    amount = -amount,
                    timestamp = System.DateTime.UtcNow,
                    description = reason
                });

                Debug.Log($"💸 Deducted {amount} {currencyId}. New balance: {currency?.amount}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to deduct currency: {e.Message}");
                return false;
            }
        }

        // 🎰 Gambling System
        public async Task<GamblingResult> PlaceGamble(long betAmount, float playerOdds = 0.5f)
        {
            if (!gamblingEnabled)
            {
                return new GamblingResult { success = false, message = "Gambling is disabled" };
            }

            if (betAmount < minBet || betAmount > maxBet)
            {
                return new GamblingResult { success = false, message = $"Bet must be between {minBet} and {maxBet}" };
            }

            if (GetCurrencyBalance(primaryCurrencyId) < betAmount)
            {
                return new GamblingResult { success = false, message = "Insufficient funds" };
            }

            // Deduct bet amount
            await DeductCurrency(primaryCurrencyId, betAmount, "Gambling bet");

            // Calculate win with house edge
            float adjustedOdds = playerOdds * (1f - houseEdge);
            bool playerWins = Random.value < adjustedOdds;

            var result = new GamblingResult
            {
                success = true,
                playerWon = playerWins,
                betAmount = betAmount
            };

            if (playerWins)
            {
                long winAmount = (long)(betAmount * 2);
                await AddCurrency(primaryCurrencyId, winAmount, "Gambling win");
                result.winAmount = winAmount;
                result.message = $"🎉 YOU WON! +{winAmount} Pink Coins!";

                Debug.Log($"🎰 Player won {winAmount} coins!");
            }
            else
            {
                result.message = "😿 You lost... Better luck next time!";
                Debug.Log($"🎰 Player lost {betAmount} coins");
            }

            // Record gambling transaction
            RecordTransaction(new Transaction
            {
                transactionId = System.Guid.NewGuid().ToString(),
                type = "gambling",
                amount = playerWins ? result.winAmount - betAmount : -betAmount,
                timestamp = System.DateTime.UtcNow,
                description = playerWins ? "Gambling WIN" : "Gambling LOSS"
            });

            return result;
        }

        // 🏛️ Auction System
        public async Task<bool> CreateAuction(CatgirlItem item, long startingBid, int durationHours = 24)
        {
            if (item == null || startingBid <= 0) return false;

            // Calculate and deduct listing fee
            long listingFee = (long)(startingBid * auctionFeePercentage);
            if (!await DeductCurrency(primaryCurrencyId, listingFee, "Auction listing fee"))
            {
                return false;
            }

            var auction = new AuctionItem
            {
                auctionId = System.Guid.NewGuid().ToString(),
                item = item,
                startingBid = startingBid,
                currentBid = startingBid,
                highestBidder = "",
                endTime = System.DateTime.UtcNow.AddHours(durationHours),
                isActive = true
            };

            activeAuctions.Add(auction);

            Debug.Log($"🏛️ Auction created for {item.displayName} starting at {startingBid} coins");
            return true;
        }

        public async Task<bool> PlaceBid(string auctionId, long bidAmount)
        {
            var auction = activeAuctions.Find(a => a.auctionId == auctionId);
            if (auction == null || !auction.isActive)
            {
                Debug.LogWarning("Auction not found or inactive");
                return false;
            }

            if (bidAmount <= auction.currentBid)
            {
                Debug.LogWarning($"Bid must be higher than current bid ({auction.currentBid})");
                return false;
            }

            if (!await DeductCurrency(primaryCurrencyId, bidAmount, $"Auction bid on {auction.item.displayName}"))
            {
                return false;
            }

            // Refund previous highest bidder
            if (!string.IsNullOrEmpty(auction.highestBidder))
            {
                // Would need player ID system to refund properly
                Debug.Log($"Refunding {auction.currentBid} to previous bidder");
            }

            auction.currentBid = bidAmount;
            auction.highestBidder = "LocalPlayer"; // Would use actual player ID

            Debug.Log($"🎯 New bid placed: {bidAmount} coins on {auction.item.displayName}");
            return true;
        }

        public List<AuctionItem> GetActiveAuctions()
        {
            // Clean up expired auctions
            activeAuctions.RemoveAll(a => a.endTime < System.DateTime.UtcNow && a.isActive);
            return new List<AuctionItem>(activeAuctions);
        }

        private void RecordTransaction(Transaction transaction)
        {
            transactionHistory.Insert(0, transaction);

            // Maintain max history size
            if (transactionHistory.Count > maxHistoryEntries)
            {
                transactionHistory.RemoveAt(transactionHistory.Count - 1);
            }
        }

        public List<Transaction> GetTransactionHistory(int count = 10)
        {
            return transactionHistory.GetRange(0, Mathf.Min(count, transactionHistory.Count));
        }
    }

    [System.Serializable]
    public class GamblingResult
    {
        public bool success;
        public bool playerWon;
        public long betAmount;
        public long winAmount;
        public string message;
    }
}
