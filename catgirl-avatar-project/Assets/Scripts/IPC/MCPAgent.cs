// 🌸 BambiSleep™ CatGirl MCP Agent
// Integrates Unity with all 10 Model Context Protocol servers for intelligent automation

using UnityEngine;
using Unity.Netcode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BambiSleep.CatGirl.IPC
{
    /// <summary>
    /// 🦋 MCP Agent - Bridge between Unity and 10 MCP servers
    /// Provides intelligent automation via Model Context Protocol integration
    /// </summary>
    public class MCPAgent : MonoBehaviour
    {
        #region Singleton Pattern
        private static MCPAgent _instance;
        public static MCPAgent Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<MCPAgent>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        #region 🌸 MCP Server Configuration
        [Header("🌸 MCP Server Integration")]
        [Tooltip("Enable MCP agent features")]
        public bool enableMCPAgent = true;

        [Tooltip("IPC Bridge reference for Node.js communication")]
        public IPCBridge ipcBridge;

        [Header("💎 MCP Server Features")]
        public bool useFilesystemMCP = true;    // Asset management
        public bool useGitMCP = true;            // Version control
        public bool useGithubMCP = true;         // Issue tracking, PR creation
        public bool useMemoryMCP = true;         // Knowledge graph persistence
        public bool useSequentialThinking = true; // Complex problem solving
        public bool useEverythingMCP = true;     // Diagnostics & testing
        public bool useBraveSearchMCP = true;    // Unity API documentation search
        public bool usePostgresMCP = true;       // Player data persistence
        public bool useStripeMCP = true;         // Economy & payment processing
        public bool useFetchMCP = true;          // External API integration

        [Header("🔥 Performance Optimization")]
        [Tooltip("Batch MCP requests for efficiency")]
        public bool batchRequests = true;
        
        [Tooltip("Cache responses to reduce redundant calls")]
        public bool cacheResponses = true;
        
        [Tooltip("Maximum concurrent MCP operations")]
        [Range(1, 10)]
        public int maxConcurrentOps = 5;

        private Dictionary<string, object> responseCache = new Dictionary<string, object>();
        private Queue<MCPRequest> requestQueue = new Queue<MCPRequest>();
        private int activeOperations = 0;
        #endregion

        #region 🦋 MCP Data Structures
        [Serializable]
        public class MCPRequest
        {
            public string server;        // Which MCP server to use
            public string action;        // Action to perform
            public string data;          // JSON payload
            public Action<MCPResponse> callback; // Response handler
        }

        [Serializable]
        public class MCPResponse
        {
            public bool success;
            public string server;
            public string action;
            public string data;          // JSON response
            public string error;
        }

        [Serializable]
        public class FileSystemOperation
        {
            public string operation;     // read, write, create_directory, move_file, search_files
            public string path;
            public string content;
            public Dictionary<string, object> options;
        }

        [Serializable]
        public class GitOperation
        {
            public string operation;     // commit, push, branch, status, diff
            public string message;
            public List<string> files;
            public Dictionary<string, string> options;
        }

        [Serializable]
        public class GithubOperation
        {
            public string operation;     // create_issue, create_pr, comment, search
            public string title;
            public string body;
            public List<string> labels;
            public Dictionary<string, object> options;
        }

        [Serializable]
        public class MemoryOperation
        {
            public string operation;     // create_entities, add_observations, search_nodes
            public List<string> entities;
            public List<string> observations;
            public string query;
        }

        [Serializable]
        public class SearchOperation
        {
            public string query;
            public int maxResults;
            public string filter;        // For Brave Search filtering
        }

        [Serializable]
        public class StripeOperation
        {
            public string operation;     // create_payment, create_customer, create_subscription
            public long amount;          // In cents
            public string currency;
            public string customerId;
            public Dictionary<string, object> metadata;
        }

        [Serializable]
        public class DatabaseOperation
        {
            public string operation;     // query, insert, update, delete
            public string table;
            public string query;
            public Dictionary<string, object> parameters;
        }
        #endregion

        #region Lifecycle Methods
        private void Start()
        {
            if (!enableMCPAgent)
            {
                Debug.Log("🌸 MCP Agent disabled");
                return;
            }

            if (ipcBridge == null)
            {
                ipcBridge = FindObjectOfType<IPCBridge>();
                if (ipcBridge == null)
                {
                    Debug.LogError("❌ IPCBridge not found! MCP Agent requires IPC Bridge.");
                    enableMCPAgent = false;
                    return;
                }
            }

            InitializeMCPAgent();
            StartCoroutine(ProcessRequestQueue());
        }

        private void InitializeMCPAgent()
        {
            Debug.Log("🌸 Initializing MCP Agent with 10 servers...");

            // Initialize memory graph with project context
            if (useMemoryMCP)
            {
                InitializeMemoryGraph();
            }

            Debug.Log("✅ MCP Agent initialized successfully");
        }

        private void OnDestroy()
        {
            if (useMemoryMCP && enableMCPAgent)
            {
                // Save knowledge graph state before shutdown
                SaveMemoryGraph();
            }
        }
        #endregion

        #region 🌸 Filesystem MCP Integration
        /// <summary>
        /// Read Unity asset file via Filesystem MCP
        /// </summary>
        public void ReadAssetFile(string path, Action<string> callback)
        {
            if (!useFilesystemMCP) return;

            var operation = new FileSystemOperation
            {
                operation = "read",
                path = path
            };

            EnqueueMCPRequest("filesystem", "read_text_file", JsonConvert.SerializeObject(operation), 
                response =>
                {
                    if (response.success)
                    {
                        callback?.Invoke(response.data);
                        Debug.Log($"✅ Read asset: {path}");
                    }
                    else
                    {
                        Debug.LogError($"❌ Failed to read {path}: {response.error}");
                    }
                });
        }

        /// <summary>
        /// Write Unity asset file via Filesystem MCP (for generated content)
        /// </summary>
        public void WriteAssetFile(string path, string content, Action<bool> callback)
        {
            if (!useFilesystemMCP) return;

            var operation = new FileSystemOperation
            {
                operation = "write",
                path = path,
                content = content
            };

            EnqueueMCPRequest("filesystem", "write_file", JsonConvert.SerializeObject(operation),
                response =>
                {
                    callback?.Invoke(response.success);
                    if (response.success)
                        Debug.Log($"✅ Wrote asset: {path}");
                    else
                        Debug.LogError($"❌ Failed to write {path}: {response.error}");
                });
        }

        /// <summary>
        /// Search for Unity assets matching pattern
        /// </summary>
        public void SearchAssets(string pattern, Action<List<string>> callback)
        {
            if (!useFilesystemMCP) return;

            var operation = new FileSystemOperation
            {
                operation = "search_files",
                path = "Assets/",
                options = new Dictionary<string, object> { { "pattern", pattern } }
            };

            EnqueueMCPRequest("filesystem", "search_files", JsonConvert.SerializeObject(operation),
                response =>
                {
                    if (response.success)
                    {
                        var files = JsonConvert.DeserializeObject<List<string>>(response.data);
                        callback?.Invoke(files);
                        Debug.Log($"🔍 Found {files.Count} assets matching '{pattern}'");
                    }
                });
        }
        #endregion

        #region 🐄 Git MCP Integration
        /// <summary>
        /// Commit Unity project changes with emoji convention
        /// </summary>
        public void CommitChanges(string emoji, string message, List<string> files = null, Action<bool> callback = null)
        {
            if (!useGitMCP) return;

            var operation = new GitOperation
            {
                operation = "commit",
                message = $"{emoji} {message}",
                files = files ?? new List<string> { "." }
            };

            EnqueueMCPRequest("git", "commit", JsonConvert.SerializeObject(operation),
                response =>
                {
                    callback?.Invoke(response.success);
                    if (response.success)
                        Debug.Log($"✅ Committed: {emoji} {message}");
                    else
                        Debug.LogError($"❌ Commit failed: {response.error}");
                });
        }

        /// <summary>
        /// Get git status for Unity project
        /// </summary>
        public void GetGitStatus(Action<Dictionary<string, object>> callback)
        {
            if (!useGitMCP) return;

            var operation = new GitOperation { operation = "status" };

            EnqueueMCPRequest("git", "status", JsonConvert.SerializeObject(operation),
                response =>
                {
                    if (response.success)
                    {
                        var status = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.data);
                        callback?.Invoke(status);
                    }
                });
        }

        /// <summary>
        /// Create feature branch with cow powers naming convention
        /// </summary>
        public void CreateFeatureBranch(string featureName, Action<bool> callback)
        {
            if (!useGitMCP) return;

            var branchName = $"feature/{featureName.ToLower().Replace(" ", "-")}";
            var operation = new GitOperation
            {
                operation = "branch",
                options = new Dictionary<string, string> { { "name", branchName } }
            };

            EnqueueMCPRequest("git", "create_branch", JsonConvert.SerializeObject(operation),
                response =>
                {
                    callback?.Invoke(response.success);
                    if (response.success)
                        Debug.Log($"🦋 Created branch: {branchName}");
                });
        }
        #endregion

        #region 👑 Github MCP Integration
        /// <summary>
        /// Create GitHub issue for bug tracking
        /// </summary>
        public void CreateBugReport(string title, string description, string[] labels = null, Action<string> callback = null)
        {
            if (!useGithubMCP) return;

            var operation = new GithubOperation
            {
                operation = "create_issue",
                title = $"🐛 {title}",
                body = $"**Bug Report**\n\n{description}\n\n**Unity Version:** {Application.unityVersion}\n**Platform:** {Application.platform}",
                labels = labels != null ? new List<string>(labels) : new List<string> { "bug", "unity" }
            };

            EnqueueMCPRequest("github", "create_issue", JsonConvert.SerializeObject(operation),
                response =>
                {
                    if (response.success)
                    {
                        var issueUrl = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.data)["url"].ToString();
                        callback?.Invoke(issueUrl);
                        Debug.Log($"✅ Created issue: {issueUrl}");
                    }
                });
        }

        /// <summary>
        /// Create pull request for feature completion
        /// </summary>
        public void CreatePullRequest(string title, string description, string baseBranch = "main", Action<string> callback = null)
        {
            if (!useGithubMCP) return;

            var operation = new GithubOperation
            {
                operation = "create_pr",
                title = title,
                body = description,
                options = new Dictionary<string, object> { { "base", baseBranch } }
            };

            EnqueueMCPRequest("github", "create_pr", JsonConvert.SerializeObject(operation),
                response =>
                {
                    if (response.success)
                    {
                        var prUrl = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.data)["url"].ToString();
                        callback?.Invoke(prUrl);
                        Debug.Log($"✅ Created PR: {prUrl}");
                    }
                });
        }
        #endregion

        #region 💎 Memory MCP Integration
        /// <summary>
        /// Initialize knowledge graph with Unity project context
        /// </summary>
        private void InitializeMemoryGraph()
        {
            var entities = new List<string>
            {
                "Unity 6.2 LTS Project",
                "CatGirl Avatar System",
                "Network Multiplayer",
                "Economy System",
                "Universal Banking",
                "Cow Powers Secret Features",
                "Pink Frilly Aesthetic"
            };

            var operation = new MemoryOperation
            {
                operation = "create_entities",
                entities = entities
            };

            EnqueueMCPRequest("memory", "create_entities", JsonConvert.SerializeObject(operation),
                response =>
                {
                    if (response.success)
                        Debug.Log("✅ Knowledge graph initialized");
                });
        }

        /// <summary>
        /// Add observation to knowledge graph
        /// </summary>
        public void RecordObservation(string entity, string observation, Action<bool> callback = null)
        {
            if (!useMemoryMCP) return;

            var operation = new MemoryOperation
            {
                operation = "add_observations",
                entities = new List<string> { entity },
                observations = new List<string> { observation }
            };

            EnqueueMCPRequest("memory", "add_observations", JsonConvert.SerializeObject(operation),
                response =>
                {
                    callback?.Invoke(response.success);
                    if (response.success)
                        Debug.Log($"📝 Recorded: {entity} → {observation}");
                });
        }

        /// <summary>
        /// Query knowledge graph for insights
        /// </summary>
        public void QueryKnowledge(string query, Action<List<string>> callback)
        {
            if (!useMemoryMCP) return;

            var operation = new MemoryOperation
            {
                operation = "search_nodes",
                query = query
            };

            EnqueueMCPRequest("memory", "search_nodes", JsonConvert.SerializeObject(operation),
                response =>
                {
                    if (response.success)
                    {
                        var results = JsonConvert.DeserializeObject<List<string>>(response.data);
                        callback?.Invoke(results);
                    }
                });
        }

        private void SaveMemoryGraph()
        {
            // Save knowledge graph state on shutdown
            var operation = new MemoryOperation { operation = "persist" };
            EnqueueMCPRequest("memory", "persist", JsonConvert.SerializeObject(operation), null);
        }
        #endregion

        #region 🧠 Sequential Thinking MCP
        /// <summary>
        /// Use sequential thinking for complex problem solving
        /// </summary>
        public void SolveComplexProblem(string problem, Action<string> callback)
        {
            if (!useSequentialThinking) return;

            var request = new Dictionary<string, object>
            {
                { "problem", problem },
                { "context", "Unity 6.2 LTS CatGirl Avatar System with multiplayer networking, economy, and cow powers" }
            };

            EnqueueMCPRequest("sequential-thinking", "think", JsonConvert.SerializeObject(request),
                response =>
                {
                    if (response.success)
                    {
                        var solution = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.data)["solution"].ToString();
                        callback?.Invoke(solution);
                        Debug.Log($"🧠 Sequential thinking solution: {solution}");
                    }
                });
        }
        #endregion

        #region 🔍 Brave Search MCP Integration
        /// <summary>
        /// Search Unity documentation and API references
        /// </summary>
        public void SearchUnityDocs(string query, Action<List<string>> callback)
        {
            if (!useBraveSearchMCP) return;

            var searchOp = new SearchOperation
            {
                query = $"Unity {Application.unityVersion} {query}",
                maxResults = 5,
                filter = "site:docs.unity3d.com OR site:docs-multiplayer.unity3d.com"
            };

            EnqueueMCPRequest("brave-search", "search", JsonConvert.SerializeObject(searchOp),
                response =>
                {
                    if (response.success)
                    {
                        var results = JsonConvert.DeserializeObject<List<string>>(response.data);
                        callback?.Invoke(results);
                        Debug.Log($"🔍 Found {results.Count} Unity docs results");
                    }
                });
        }
        #endregion

        #region 💰 Stripe MCP Integration
        /// <summary>
        /// Create Stripe payment for in-game purchases
        /// </summary>
        public void CreatePayment(long amountCents, string currency, string customerId, Action<string> callback)
        {
            if (!useStripeMCP) return;

            var operation = new StripeOperation
            {
                operation = "create_payment",
                amount = amountCents,
                currency = currency,
                customerId = customerId,
                metadata = new Dictionary<string, object>
                {
                    { "game", "BambiSleep CatGirl Avatar" },
                    { "playerId", customerId }
                }
            };

            EnqueueMCPRequest("stripe", "create_payment_intent", JsonConvert.SerializeObject(operation),
                response =>
                {
                    if (response.success)
                    {
                        var paymentId = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.data)["id"].ToString();
                        callback?.Invoke(paymentId);
                        Debug.Log($"💰 Created payment: {paymentId}");
                    }
                });
        }

        /// <summary>
        /// Create Stripe customer for player
        /// </summary>
        public void CreateStripeCustomer(string playerEmail, Action<string> callback)
        {
            if (!useStripeMCP) return;

            var operation = new StripeOperation
            {
                operation = "create_customer",
                metadata = new Dictionary<string, object>
                {
                    { "email", playerEmail },
                    { "game", "BambiSleep CatGirl Avatar" }
                }
            };

            EnqueueMCPRequest("stripe", "create_customer", JsonConvert.SerializeObject(operation),
                response =>
                {
                    if (response.success)
                    {
                        var customerId = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.data)["id"].ToString();
                        callback?.Invoke(customerId);
                        Debug.Log($"👤 Created Stripe customer: {customerId}");
                    }
                });
        }
        #endregion

        #region 🗄️ Postgres MCP Integration
        /// <summary>
        /// Save player data to PostgreSQL database
        /// </summary>
        public void SavePlayerData(string playerId, Dictionary<string, object> data, Action<bool> callback)
        {
            if (!usePostgresMCP) return;

            var operation = new DatabaseOperation
            {
                operation = "insert",
                table = "player_data",
                parameters = new Dictionary<string, object>
                {
                    { "player_id", playerId },
                    { "data", JsonConvert.SerializeObject(data) },
                    { "updated_at", DateTime.UtcNow.ToString("o") }
                }
            };

            EnqueueMCPRequest("postgres", "execute", JsonConvert.SerializeObject(operation),
                response =>
                {
                    callback?.Invoke(response.success);
                    if (response.success)
                        Debug.Log($"💾 Saved player data: {playerId}");
                });
        }

        /// <summary>
        /// Load player data from PostgreSQL
        /// </summary>
        public void LoadPlayerData(string playerId, Action<Dictionary<string, object>> callback)
        {
            if (!usePostgresMCP) return;

            var operation = new DatabaseOperation
            {
                operation = "query",
                query = "SELECT data FROM player_data WHERE player_id = $1",
                parameters = new Dictionary<string, object> { { "player_id", playerId } }
            };

            EnqueueMCPRequest("postgres", "execute", JsonConvert.SerializeObject(operation),
                response =>
                {
                    if (response.success)
                    {
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.data);
                        callback?.Invoke(data);
                        Debug.Log($"📖 Loaded player data: {playerId}");
                    }
                });
        }
        #endregion

        #region 🌐 Fetch MCP Integration
        /// <summary>
        /// Fetch external API data (e.g., Unity Asset Store, third-party services)
        /// </summary>
        public void FetchExternalAPI(string url, Action<string> callback)
        {
            if (!useFetchMCP) return;

            var request = new Dictionary<string, object>
            {
                { "url", url },
                { "method", "GET" }
            };

            EnqueueMCPRequest("fetch", "fetch", JsonConvert.SerializeObject(request),
                response =>
                {
                    if (response.success)
                    {
                        callback?.Invoke(response.data);
                        Debug.Log($"🌐 Fetched: {url}");
                    }
                });
        }
        #endregion

        #region 🔧 Everything MCP Integration (Diagnostics)
        /// <summary>
        /// Run diagnostic tests via Everything MCP server
        /// </summary>
        public void RunDiagnostics(Action<Dictionary<string, object>> callback)
        {
            if (!useEverythingMCP) return;

            var request = new Dictionary<string, object>
            {
                { "test", "system_health" },
                { "context", new Dictionary<string, object>
                    {
                        { "unity_version", Application.unityVersion },
                        { "platform", Application.platform.ToString() },
                        { "scene", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name }
                    }
                }
            };

            EnqueueMCPRequest("everything", "test", JsonConvert.SerializeObject(request),
                response =>
                {
                    if (response.success)
                    {
                        var diagnostics = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.data);
                        callback?.Invoke(diagnostics);
                        Debug.Log("🔧 Diagnostics complete");
                    }
                });
        }
        #endregion

        #region 🔄 Request Queue Management
        private void EnqueueMCPRequest(string server, string action, string data, Action<MCPResponse> callback)
        {
            var request = new MCPRequest
            {
                server = server,
                action = action,
                data = data,
                callback = callback
            };

            if (batchRequests)
            {
                requestQueue.Enqueue(request);
            }
            else
            {
                StartCoroutine(ExecuteMCPRequest(request));
            }
        }

        private IEnumerator ProcessRequestQueue()
        {
            while (enableMCPAgent)
            {
                if (requestQueue.Count > 0 && activeOperations < maxConcurrentOps)
                {
                    var request = requestQueue.Dequeue();
                    StartCoroutine(ExecuteMCPRequest(request));
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator ExecuteMCPRequest(MCPRequest request)
        {
            activeOperations++;

            // Check cache first
            string cacheKey = $"{request.server}:{request.action}:{request.data}";
            if (cacheResponses && responseCache.ContainsKey(cacheKey))
            {
                Debug.Log($"📦 Using cached response for {request.server}.{request.action}");
                request.callback?.Invoke(responseCache[cacheKey] as MCPResponse);
                activeOperations--;
                yield break;
            }

            // Send request via IPC Bridge
            var ipcMessage = new Dictionary<string, object>
            {
                { "type", "mcp_request" },
                { "server", request.server },
                { "action", request.action },
                { "data", request.data }
            };

            string jsonMessage = JsonConvert.SerializeObject(ipcMessage);
            
            // In a real implementation, this would use IPCBridge to send message
            // For now, simulate the request-response cycle
            Debug.Log($"🔄 MCP Request: {request.server}.{request.action}");

            // Simulate network delay
            yield return new WaitForSeconds(0.5f);

            // Create mock response (in production, this comes from Node.js)
            var response = new MCPResponse
            {
                success = true,
                server = request.server,
                action = request.action,
                data = $"{{\"result\": \"Mock response from {request.server}\"}}",
                error = null
            };

            // Cache response
            if (cacheResponses)
            {
                responseCache[cacheKey] = response;
            }

            // Invoke callback
            request.callback?.Invoke(response);

            activeOperations--;
        }
        #endregion

        #region 🎯 High-Level Automation Examples
        /// <summary>
        /// 🦋 Auto-commit feature with full workflow
        /// </summary>
        [ContextMenu("Auto-Commit Current Feature")]
        public void AutoCommitFeature()
        {
            // 1. Get git status
            GetGitStatus(status =>
            {
                // 2. Record observation in knowledge graph
                RecordObservation("Development Workflow", $"Feature commit at {DateTime.Now}");

                // 3. Commit with emoji convention
                CommitChanges("🦋", "Add butterfly flight feature with networked synchronization", null, success =>
                {
                    if (success)
                    {
                        // 4. Create GitHub issue for testing
                        CreateBugReport("Test butterfly flight feature", "Comprehensive testing needed for new flight mechanics", 
                            new[] { "testing", "feature" });
                    }
                });
            });
        }

        /// <summary>
        /// 🐄 Deploy cow powers secret feature
        /// </summary>
        [ContextMenu("Deploy Cow Powers Feature")]
        public void DeployCowPowersFeature()
        {
            // 1. Use sequential thinking to validate deployment
            SolveComplexProblem("How to deploy cow powers secret feature without breaking multiplayer sync?", solution =>
            {
                Debug.Log($"🧠 Deployment strategy: {solution}");

                // 2. Search Unity multiplayer docs
                SearchUnityDocs("NetworkBehaviour synchronization best practices", docs =>
                {
                    Debug.Log($"📚 Found {docs.Count} relevant docs");

                    // 3. Create feature branch
                    CreateFeatureBranch("cow-powers-deployment", success =>
                    {
                        if (success)
                        {
                            // 4. Record in knowledge graph
                            RecordObservation("Cow Powers Secret Features", "Deployment initiated with multiplayer validation");
                        }
                    });
                });
            });
        }

        /// <summary>
        /// 💰 Setup player economy with Stripe integration
        /// </summary>
        public void SetupPlayerEconomy(string playerEmail, Action<string> callback)
        {
            // 1. Create Stripe customer
            CreateStripeCustomer(playerEmail, customerId =>
            {
                // 2. Save to database
                var playerData = new Dictionary<string, object>
                {
                    { "email", playerEmail },
                    { "stripe_customer_id", customerId },
                    { "pink_coins", 1000 },
                    { "cow_tokens", 0 }
                };

                SavePlayerData(customerId, playerData, success =>
                {
                    if (success)
                    {
                        // 3. Record in knowledge graph
                        RecordObservation("Economy System", $"New player economy setup: {playerEmail}");
                        callback?.Invoke(customerId);
                    }
                });
            });
        }
        #endregion

        #region 📊 Debug & Monitoring
        [ContextMenu("Show MCP Status")]
        public void ShowMCPStatus()
        {
            Debug.Log("🌸 MCP Agent Status:");
            Debug.Log($"  Filesystem: {(useFilesystemMCP ? "✅" : "❌")}");
            Debug.Log($"  Git: {(useGitMCP ? "✅" : "❌")}");
            Debug.Log($"  GitHub: {(useGithubMCP ? "✅" : "❌")}");
            Debug.Log($"  Memory: {(useMemoryMCP ? "✅" : "❌")}");
            Debug.Log($"  Sequential Thinking: {(useSequentialThinking ? "✅" : "❌")}");
            Debug.Log($"  Everything: {(useEverythingMCP ? "✅" : "❌")}");
            Debug.Log($"  Brave Search: {(useBraveSearchMCP ? "✅" : "❌")}");
            Debug.Log($"  Postgres: {(usePostgresMCP ? "✅" : "❌")}");
            Debug.Log($"  Stripe: {(useStripeMCP ? "✅" : "❌")}");
            Debug.Log($"  Fetch: {(useFetchMCP ? "✅" : "❌")}");
            Debug.Log($"  Active Operations: {activeOperations}/{maxConcurrentOps}");
            Debug.Log($"  Queued Requests: {requestQueue.Count}");
            Debug.Log($"  Cached Responses: {responseCache.Count}");
        }

        [ContextMenu("Clear Response Cache")]
        public void ClearCache()
        {
            responseCache.Clear();
            Debug.Log("🗑️ Response cache cleared");
        }

        [ContextMenu("Run Full Diagnostics")]
        public void RunFullDiagnostics()
        {
            RunDiagnostics(results =>
            {
                Debug.Log("🔧 Full Diagnostics Results:");
                foreach (var kvp in results)
                {
                    Debug.Log($"  {kvp.Key}: {kvp.Value}");
                }
            });
        }
        #endregion
    }
}
