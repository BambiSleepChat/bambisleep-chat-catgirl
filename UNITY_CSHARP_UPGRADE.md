# 🌸 Unity C#/.NET Upgrade Guide

**BambiSleep™ CATHEDRAL Project**  
**Date**: 2025-01-15  
**Source**: GitHub Copilot C#/.NET Development Collection  
**Reference**: https://github.com/github/awesome-copilot/blob/main/collections/csharp-dotnet-development.collection.yml

---

## Executive Summary

This document details the application of GitHub Copilot's C# best practices to the Unity 6.2 avatar system in `bambisleep-chat-catgirl/catgirl-avatar-project`. Key improvements focus on async/await patterns for IPC operations, proper exception handling, and modern C# 13 features.

**Current Status**: Unity scripts use synchronous patterns with `ThreadPool.QueueUserWorkItem()` for background operations.

**Target State**: Adopt Task-based Asynchronous Pattern (TAP) with proper cancellation, error handling, and Unity async integration.

---

## 🎯 Key Improvements from GitHub Copilot Collection

### 1. Async Programming Best Practices

#### Naming Conventions
- **Rule**: Use 'Async' suffix for all async methods
- **Example**: `SendMessage()` → `SendMessageAsync()`
- **Unity Context**: Applies to IPC operations, MCP calls, network requests

#### Return Types
- **Rule**: Return `Task<T>` when returning value, `Task` when void
- **Rule**: Consider `ValueTask<T>` for high-performance scenarios
- **Anti-pattern**: Avoid `async void` except for Unity event handlers (Start, Update)
- **Unity Context**: Unity doesn't support async Main/Awake, use `async void Start()` as entry point

#### Exception Handling
- **Rule**: Use try/catch around await expressions
- **Rule**: Avoid swallowing exceptions
- **Rule**: Use `ConfigureAwait(false)` in library code (not Unity MonoBehaviour)
- **Unity Context**: Unity requires main-thread context, DO NOT use `ConfigureAwait(false)` in MonoBehaviour

#### Performance Patterns
- **Rule**: Use `Task.WhenAll()` for parallel execution
- **Rule**: Use `Task.WhenAny()` for timeouts
- **Rule**: Implement cancellation tokens for long-running operations
- **Unity Context**: Use `CancellationTokenSource` with `OnDestroy()` cleanup

#### Common Pitfalls to Avoid
- ❌ NEVER use `.Wait()`, `.Result`, or `.GetAwaiter().GetResult()`
- ❌ Don't mix blocking and async code
- ❌ Don't create async void methods (except event handlers)
- ❌ Always await Task-returning methods

---

## 📂 Files Requiring Upgrades

### Priority 1: IPC Communication Layer

#### `Assets/Scripts/IPC/IPCBridge.cs` (542 lines)
**Current Issues**:
- Uses `ThreadPool.QueueUserWorkItem()` for stdin reading (line 194)
- Synchronous message processing in `ProcessMessage()` (line 246)
- No cancellation token support
- Blocking I/O with `StreamReader.ReadLine()` (line 207)

**Recommended Changes**:
```csharp
// BEFORE (synchronous ThreadPool)
private void ReadStdin(object state)
{
    while (isRunning)
    {
        string line = stdinReader.ReadLine(); // BLOCKS thread
        lock (messageQueue)
        {
            messageQueue.Enqueue(line);
        }
    }
}

// AFTER (async with cancellation)
private async Task ReadStdinAsync(CancellationToken cancellationToken)
{
    try
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            string line = await stdinReader.ReadLineAsync();
            if (!string.IsNullOrEmpty(line))
            {
                lock (messageQueue)
                {
                    messageQueue.Enqueue(line);
                }
            }
        }
    }
    catch (OperationCanceledException)
    {
        // Expected during shutdown
        Debug.Log("🌸 IPCBridge stdin reader cancelled gracefully");
    }
    catch (Exception e)
    {
        Debug.LogError($"❌ Error reading stdin: {e.Message}");
    }
}
```

**Method Signature Changes**:
```csharp
// Public API changes
public async Task<bool> SendMessageAsync(string type, object data, CancellationToken cancellationToken = default)
public async Task<IPCResponse> SendRequestAsync(string type, object data, int timeoutMs = 5000, CancellationToken cancellationToken = default)

// Event handlers remain async void
private async void Start()
{
    _cancellationTokenSource = new CancellationTokenSource();
    await InitializeStreamsAsync(_cancellationTokenSource.Token);
}

private void OnDestroy()
{
    _cancellationTokenSource?.Cancel();
    _cancellationTokenSource?.Dispose();
}
```

---

#### `Assets/Scripts/IPC/MCPAgent.cs` (955 lines)
**Current Issues**:
- Synchronous MCP request queue processing
- No async streaming for long-running MCP operations
- Missing cancellation token support for MCP calls

**Recommended Changes**:
```csharp
// BEFORE (synchronous queue)
public void QueueMCPRequest(MCPRequest request)
{
    requestQueue.Enqueue(request);
}

// AFTER (async with Task<T>)
public async Task<MCPResponse> SendMCPRequestAsync(MCPRequest request, CancellationToken cancellationToken = default)
{
    if (!enableMCPAgent)
    {
        return new MCPResponse 
        { 
            success = false, 
            error = "MCP Agent disabled" 
        };
    }

    // Wait for available operation slot
    while (activeOperations >= maxConcurrentOps && !cancellationToken.IsCancellationRequested)
    {
        await Task.Delay(100, cancellationToken);
    }

    activeOperations++;
    try
    {
        // Check cache first
        string cacheKey = $"{request.server}:{request.action}:{request.data}";
        if (cacheResponses && responseCache.TryGetValue(cacheKey, out object cached))
        {
            return (MCPResponse)cached;
        }

        // Send via IPC bridge with timeout
        var ipcRequest = new IPCMessage
        {
            type = "mcp_request",
            data = JsonUtility.ToJson(request)
        };

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(TimeSpan.FromSeconds(30));

        var response = await ipcBridge.SendRequestAsync("mcp_request", request, timeoutCts.Token);
        
        // Cache successful responses
        if (response.success && cacheResponses)
        {
            responseCache[cacheKey] = response;
        }

        return response;
    }
    catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
    {
        return new MCPResponse { success = false, error = "Operation cancelled by user" };
    }
    catch (OperationCanceledException)
    {
        return new MCPResponse { success = false, error = "Request timeout (30s)" };
    }
    catch (Exception e)
    {
        Debug.LogError($"❌ MCP request failed: {e.Message}");
        return new MCPResponse { success = false, error = e.Message };
    }
    finally
    {
        activeOperations--;
    }
}

// NEW: Async streaming for long-running MCP operations
#if UNITY_2021_1_OR_NEWER
public async IAsyncEnumerable<MCPStreamEvent> StreamMCPEventsAsync(
    string server, 
    string action, 
    [EnumeratorCancellation] CancellationToken cancellationToken = default)
{
    var request = new MCPRequest
    {
        server = server,
        action = action + "_stream",
        data = "{}"
    };

    // Subscribe to event stream
    var eventQueue = new Queue<MCPStreamEvent>();
    
    void OnStreamEvent(MCPStreamEvent evt)
    {
        lock (eventQueue)
        {
            eventQueue.Enqueue(evt);
        }
    }

    RegisterStreamHandler(server, action, OnStreamEvent);

    try
    {
        // Start stream
        await SendMCPRequestAsync(request, cancellationToken);

        // Yield events as they arrive
        while (!cancellationToken.IsCancellationRequested)
        {
            MCPStreamEvent evt = null;
            lock (eventQueue)
            {
                if (eventQueue.Count > 0)
                {
                    evt = eventQueue.Dequeue();
                }
            }

            if (evt != null)
            {
                yield return evt;
                if (evt.isComplete) break;
            }
            else
            {
                await Task.Delay(50, cancellationToken);
            }
        }
    }
    finally
    {
        UnregisterStreamHandler(server, action, OnStreamEvent);
    }
}
#endif
```

---

### Priority 2: Unity MonoBehaviour Integration

#### `Assets/Scripts/Character/CatgirlController.cs`
**Current Pattern**: Synchronous movement, animation triggers
**Upgrade**: Async animation sequencing, network calls

```csharp
// BEFORE
void PlayAnimation(string animName)
{
    animator.SetTrigger(animName);
}

// AFTER
public async Task PlayAnimationAsync(string animName, CancellationToken cancellationToken = default)
{
    animator.SetTrigger(animName);
    
    // Wait for animation to complete
    while (animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
    {
        if (cancellationToken.IsCancellationRequested)
            return;
        await Task.Yield(); // Yield to Unity main thread
    }
}

// USAGE: Chaining animations
public async void OnTriggerReceived(string triggerName)
{
    var cts = new CancellationTokenSource();
    try
    {
        await PlayAnimationAsync("idle_to_kneel", cts.Token);
        await PlayAnimationAsync("kneel_loop", cts.Token);
        await Task.Delay(2000, cts.Token);
        await PlayAnimationAsync("kneel_to_idle", cts.Token);
    }
    catch (OperationCanceledException)
    {
        // Animation sequence interrupted
    }
    finally
    {
        cts.Dispose();
    }
}
```

---

#### `Assets/Scripts/Networking/CatgirlNetworkManager.cs`
**Current Pattern**: Unity Netcode callbacks
**Upgrade**: Async network initialization, timeout handling

```csharp
// AFTER: Async network connection with timeout
public async Task<bool> ConnectToServerAsync(string serverUrl, int timeoutMs = 10000, CancellationToken cancellationToken = default)
{
    using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
    timeoutCts.CancelAfter(timeoutMs);

    try
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(serverUrl);
        
        bool connected = false;
        NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
                connected = true;
        };

        NetworkManager.Singleton.StartClient();

        // Wait for connection or timeout
        while (!connected && !timeoutCts.Token.IsCancellationRequested)
        {
            await Task.Delay(100, timeoutCts.Token);
        }

        return connected;
    }
    catch (OperationCanceledException) when (timeoutCts.Token.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
    {
        Debug.LogError($"❌ Connection timeout after {timeoutMs}ms");
        return false;
    }
    catch (OperationCanceledException)
    {
        Debug.Log("🌸 Connection cancelled by user");
        return false;
    }
    catch (Exception e)
    {
        Debug.LogError($"❌ Connection failed: {e.Message}");
        return false;
    }
}
```

---

### Priority 3: Audio System Integration

#### `Assets/Scripts/Audio/AudioManager.cs`
**Current Pattern**: Synchronous audio loading from Resources
**Upgrade**: Async audio loading with Addressables, streaming

```csharp
// AFTER: Async audio loading
public async Task<AudioClip> LoadAudioClipAsync(string audioPath, CancellationToken cancellationToken = default)
{
    try
    {
        // Use Unity Addressables for async loading
        var handle = Addressables.LoadAssetAsync<AudioClip>(audioPath);
        
        while (!handle.IsDone && !cancellationToken.IsCancellationRequested)
        {
            await Task.Yield();
        }

        if (cancellationToken.IsCancellationRequested)
        {
            Addressables.Release(handle);
            return null;
        }

        return handle.Result;
    }
    catch (Exception e)
    {
        Debug.LogError($"❌ Failed to load audio '{audioPath}': {e.Message}");
        return null;
    }
}

// Parallel loading of audio playlist
public async Task<List<AudioClip>> LoadPlaylistAsync(List<string> audioPaths, CancellationToken cancellationToken = default)
{
    var tasks = audioPaths.Select(path => LoadAudioClipAsync(path, cancellationToken));
    var clips = await Task.WhenAll(tasks);
    return clips.Where(clip => clip != null).ToList();
}
```

---

## 🛡️ C# Best Practices Applied

### 1. Nullable Reference Types
```csharp
// Enable in .csproj or Directory.Build.props
<Nullable>enable</Nullable>

// Usage in Unity scripts
public class IPCBridge : MonoBehaviour
{
    private StreamReader? stdinReader;
    private StreamWriter? stdoutWriter;
    private CancellationTokenSource? _cancellationTokenSource;

    void Start()
    {
        if (stdinReader is null)
            InitializeStreams();
    }

    void OnDestroy()
    {
        if (_cancellationTokenSource is not null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}
```

### 2. File-Scoped Namespaces (C# 10+)
```csharp
// BEFORE
namespace BambiSleep.CatGirl.IPC
{
    public class IPCBridge : MonoBehaviour
    {
        // ...
    }
}

// AFTER
namespace BambiSleep.CatGirl.IPC;

public class IPCBridge : MonoBehaviour
{
    // ... (one less indentation level)
}
```

### 3. Pattern Matching & Switch Expressions
```csharp
// BEFORE (switch statement)
switch (message.type)
{
    case "initialize":
        HandleInitialize(message.data);
        break;
    case "update":
        HandleUpdate(message.data);
        break;
    default:
        SendError("UNKNOWN_MESSAGE_TYPE", $"Unknown: {message.type}");
        break;
}

// AFTER (switch expression + pattern matching)
var handler = message.type switch
{
    "initialize" => (Action<string>)HandleInitialize,
    "update" => HandleUpdate,
    "render" => HandleRender,
    "camera" => HandleCamera,
    "postprocessing" => HandlePostProcessing,
    "shutdown" => _ => HandleShutdown(),
    _ => _ => SendError("UNKNOWN_MESSAGE_TYPE", $"Unknown: {message.type}")
};
handler(message.data);
```

### 4. XML Documentation Comments
```csharp
/// <summary>
/// Sends an MCP request to the specified server with timeout and cancellation support.
/// </summary>
/// <param name="request">The MCP request configuration</param>
/// <param name="cancellationToken">Cancellation token for operation cancellation</param>
/// <returns>A task representing the asynchronous operation with MCP response</returns>
/// <exception cref="OperationCanceledException">Thrown when operation is cancelled</exception>
/// <exception cref="TimeoutException">Thrown when request exceeds 30-second timeout</exception>
/// <example>
/// <code>
/// var request = new MCPRequest { server = "filesystem", action = "read_file", data = "{\"path\": \"/test.txt\"}" };
/// var response = await mcpAgent.SendMCPRequestAsync(request, cancellationToken);
/// if (response.success) 
/// {
///     Debug.Log($"File content: {response.data}");
/// }
/// </code>
/// </example>
public async Task<MCPResponse> SendMCPRequestAsync(MCPRequest request, CancellationToken cancellationToken = default)
{
    // Implementation
}
```

---

## 🎮 Unity-Specific Considerations

### Async/Await in Unity

#### ✅ Safe Patterns
```csharp
// 1. async void for Unity event handlers
private async void Start()
{
    await InitializeAsync();
}

private async void OnEnable()
{
    await LoadResourcesAsync();
}

// 2. async Task for methods called from other async methods
private async Task InitializeAsync()
{
    await Task.Delay(100); // Yield to main thread
}

// 3. CancellationToken cleanup in OnDestroy
private CancellationTokenSource _cts;

private void Awake()
{
    _cts = new CancellationTokenSource();
}

private void OnDestroy()
{
    _cts?.Cancel();
    _cts?.Dispose();
}
```

#### ❌ Anti-Patterns
```csharp
// DON'T: Use ConfigureAwait(false) in MonoBehaviour
private async Task BadPattern()
{
    await SomeTaskAsync().ConfigureAwait(false); // ❌ Loses Unity context
    transform.position = Vector3.zero; // ❌ CRASH: Not on main thread
}

// DON'T: Use .Wait() or .Result
private void Update()
{
    var result = SomeTaskAsync().Result; // ❌ DEADLOCK RISK
}

// DON'T: Async Update/FixedUpdate
private async void Update() // ❌ BAD: Called every frame
{
    await SomeTaskAsync(); // ❌ PERFORMANCE: Spawns task every frame
}
```

---

## 📊 Performance Optimization Patterns

### 1. Task.WhenAll for Parallel MCP Calls
```csharp
// Parallel MCP operations
public async Task<(AudioClip audio, PersonalityProfile personality)> LoadGameStateAsync(CancellationToken cancellationToken)
{
    var audioTask = SendMCPRequestAsync(new MCPRequest 
    { 
        server = "hypnosis", 
        action = "get_audio",
        data = "{\"id\": \"sleep1\"}"
    }, cancellationToken);

    var personalityTask = SendMCPRequestAsync(new MCPRequest
    {
        server = "personality",
        action = "get_profile",
        data = "{\"name\": \"default\"}"
    }, cancellationToken);

    // Wait for both in parallel
    await Task.WhenAll(audioTask, personalityTask);

    // Process results
    var audio = await LoadAudioFromResponse(audioTask.Result);
    var personality = ParsePersonality(personalityTask.Result);

    return (audio, personality);
}
```

### 2. Task.WhenAny for Timeout Implementation
```csharp
public async Task<MCPResponse> SendMCPRequestWithTimeoutAsync(MCPRequest request, int timeoutMs = 30000)
{
    var requestTask = SendMCPRequestAsync(request);
    var timeoutTask = Task.Delay(timeoutMs);

    var completedTask = await Task.WhenAny(requestTask, timeoutTask);

    if (completedTask == timeoutTask)
    {
        throw new TimeoutException($"MCP request to {request.server} timed out after {timeoutMs}ms");
    }

    return await requestTask;
}
```

### 3. ValueTask<T> for High-Performance Scenarios
```csharp
// Use ValueTask for frequently-called cached operations
public ValueTask<string> GetCachedPlayerNameAsync(string playerId)
{
    if (_playerNameCache.TryGetValue(playerId, out string? name))
    {
        return new ValueTask<string>(name); // Completes synchronously, no heap allocation
    }

    return new ValueTask<string>(LoadPlayerNameFromDatabaseAsync(playerId));
}

private async Task<string> LoadPlayerNameFromDatabaseAsync(string playerId)
{
    var response = await SendMCPRequestAsync(new MCPRequest
    {
        server = "postgres",
        action = "query",
        data = $"{{\"sql\": \"SELECT name FROM players WHERE id = '{playerId}'\"}}"
    });

    string name = ParsePlayerName(response);
    _playerNameCache[playerId] = name;
    return name;
}
```

---

## 🧪 Testing Recommendations

### Unit Testing Async Methods
```csharp
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.TestTools;

public class MCPAgentTests
{
    [UnityTest]
    public IEnumerator SendMCPRequestAsync_Success_ReturnsValidResponse()
    {
        // Arrange
        var agent = new GameObject().AddComponent<MCPAgent>();
        var request = new MCPRequest
        {
            server = "filesystem",
            action = "read_file",
            data = "{\"path\": \"/test.txt\"}"
        };

        // Act
        Task<MCPResponse> task = agent.SendMCPRequestAsync(request);
        yield return new WaitUntil(() => task.IsCompleted);

        // Assert
        Assert.IsTrue(task.Result.success);
        Assert.IsNotEmpty(task.Result.data);
    }

    [UnityTest]
    public IEnumerator SendMCPRequestAsync_Cancelled_ThrowsOperationCanceledException()
    {
        // Arrange
        var agent = new GameObject().AddComponent<MCPAgent>();
        var cts = new CancellationTokenSource();
        var request = new MCPRequest { server = "memory", action = "search_nodes", data = "{}" };

        // Act
        Task<MCPResponse> task = agent.SendMCPRequestAsync(request, cts.Token);
        cts.Cancel(); // Cancel immediately

        yield return new WaitUntil(() => task.IsCompleted);

        // Assert
        Assert.IsFalse(task.Result.success);
        Assert.AreEqual("Operation cancelled by user", task.Result.error);
    }
}
```

---

## 🚀 Migration Strategy

### Phase 1: Non-Breaking Changes (Week 1)
1. ✅ Add XML documentation to all public methods
2. ✅ Enable nullable reference types in new files
3. ✅ Adopt file-scoped namespaces for new scripts
4. ✅ Add CancellationTokenSource to MonoBehaviour lifecycle

### Phase 2: IPC Layer (Week 2)
1. Convert `IPCBridge.ReadStdin()` to async
2. Add `SendMessageAsync()` and `SendRequestAsync()` methods
3. Implement timeout handling with `Task.WhenAny()`
4. Update `MCPAgent` to use async MCP calls

### Phase 3: Unity Integration (Week 3)
1. Add async animation sequencing to `CatgirlController`
2. Convert network calls in `CatgirlNetworkManager` to async
3. Implement async audio loading in `AudioManager`
4. Add IAsyncEnumerable streaming for MCP events (Unity 2021.1+)

### Phase 4: Testing & Validation (Week 4)
1. Write UnityTest async test cases
2. Performance benchmarking (compare sync vs async)
3. Memory profiling (Task allocations)
4. Integration testing with bambisleep-church Node.js backend

---

## 📋 Checklist: Before Merging

- [ ] All async methods have 'Async' suffix
- [ ] All Task-returning methods are awaited (no fire-and-forget)
- [ ] CancellationToken support added to long-running operations
- [ ] CancellationTokenSource disposed in OnDestroy()
- [ ] No `.Wait()`, `.Result`, or `.GetAwaiter().GetResult()` calls
- [ ] No `ConfigureAwait(false)` in MonoBehaviour classes
- [ ] XML documentation added to public async APIs
- [ ] Unit tests updated for async patterns
- [ ] Performance benchmarks show acceptable overhead
- [ ] Integration tests pass with Node.js IPC bridge

---

## 🔗 References

1. **GitHub Copilot Collections**:
   - C# Instructions: https://github.com/github/awesome-copilot/blob/main/instructions/csharp.instructions.md
   - C# Async Patterns: https://github.com/github/awesome-copilot/blob/main/prompts/csharp-async.prompt.md
   - .NET Architecture: https://github.com/github/awesome-copilot/blob/main/instructions/dotnet-architecture-good-practices.instructions.md

2. **Unity Documentation**:
   - Unity and async/await: https://docs.unity3d.com/Manual/overview-of-dot-net-in-unity.html
   - UniTask (Unity async library): https://github.com/Cysharp/UniTask

3. **Microsoft Documentation**:
   - Task-based Asynchronous Pattern: https://learn.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap
   - Async best practices: https://learn.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming

4. **BambiSleep™ CATHEDRAL Documentation**:
   - IPC Protocol: `docs/architecture/UNITY_IPC_PROTOCOL.md`
   - MCP Integration: `PRODUCTION_UPGRADE.md`
   - Workspace Instructions: `.github/copilot-instructions.md`

---

**Last Updated**: 2025-01-15  
**Maintained By**: BambiSleepChat Organization  
**License**: MIT
