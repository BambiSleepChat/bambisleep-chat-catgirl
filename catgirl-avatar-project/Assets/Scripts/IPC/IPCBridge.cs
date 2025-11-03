/**
 * 🌸 Unity IPC Bridge - C# Implementation
 *
 * Handles bidirectional JSON-based communication between Unity and Node.js
 * via stdin/stdout pipes according to UNITY_IPC_PROTOCOL.md specification.
 *
 * @namespace BambiSleep.CatGirl.IPC
 * @see docs/architecture/UNITY_IPC_PROTOCOL.md
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BambiSleep.CatGirl.IPC
{
    /// <summary>
    /// Message structure for IPC communication
    /// </summary>
    [Serializable]
    public class IPCMessage
    {
        public string type;
        public string timestamp;
        public string data; // JSON string for nested data
    }

    /// <summary>
    /// Initialize message data
    /// </summary>
    [Serializable]
    public class InitializeData
    {
        public string sceneName;
        public float cathedralWidth;
        public float cathedralLength;
        public float cathedralHeight;
        public int archCount;
        public float neonIntensity;
    }

    /// <summary>
    /// Update message data
    /// </summary>
    [Serializable]
    public class UpdateData
    {
        public float cathedralWidth;
        public float cathedralLength;
        public float neonIntensity;
        public float bloomIntensity;
    }

    /// <summary>
    /// Render message data
    /// </summary>
    [Serializable]
    public class RenderData
    {
        public string outputPath;
        public int width;
        public int height;
        public string format;
    }

    /// <summary>
    /// Camera control data
    /// </summary>
    [Serializable]
    public class CameraData
    {
        public Vector3Data position;
        public Vector3Data rotation;
        public float fieldOfView;
    }

    /// <summary>
    /// Vector3 serialization helper
    /// </summary>
    [Serializable]
    public class Vector3Data
    {
        public float x;
        public float y;
        public float z;

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        public static Vector3Data FromVector3(Vector3 v)
        {
            return new Vector3Data { x = v.x, y = v.y, z = v.z };
        }
    }

    /// <summary>
    /// Post-processing control data
    /// </summary>
    [Serializable]
    public class PostProcessingData
    {
        public float bloom;
        public float chromaticAberration;
        public float vignette;
        public bool enabled;
    }

    /// <summary>
    /// IPC Bridge MonoBehaviour
    /// Handles message routing and Unity integration
    /// </summary>
    public class IPCBridge : MonoBehaviour
    {
        [Header("🌸 IPC Configuration")]
        [Tooltip("Enable debug logging")]
        public bool debugMode = true;

        [Tooltip("Message processing frequency (messages per second)")]
        public int messageProcessingRate = 60;

        // Stdin/stdout streams
        private StreamReader stdinReader;
        private StreamWriter stdoutWriter;
        private Queue<string> messageQueue = new Queue<string>();
        private bool isRunning = true;

        // Heartbeat
        private float heartbeatInterval = 1.0f;
        private float lastHeartbeat = 0f;

        /// <summary>
        /// Static entry point for Unity command-line execution
        /// </summary>
        public static void StartIPC()
        {
            // Create GameObject with IPCBridge component
            GameObject ipcObject = new GameObject("IPCBridge");
            ipcObject.AddComponent<IPCBridge>();
            DontDestroyOnLoad(ipcObject);
        }

        void Start()
        {
            InitializeStreams();
            SendMessage("ready", new { unity_version = Application.unityVersion });

            if (debugMode)
            {
                Debug.Log("🌸 IPCBridge initialized and ready");
            }
        }

        void Update()
        {
            // Process incoming messages
            ProcessMessages();

            // Send heartbeat
            if (Time.time - lastHeartbeat >= heartbeatInterval)
            {
                SendHeartbeat();
                lastHeartbeat = Time.time;
            }
        }

        /// <summary>
        /// Initialize stdin/stdout streams
        /// </summary>
        private void InitializeStreams()
        {
            try
            {
                // Get stdin/stdout streams
                var stdin = Console.OpenStandardInput();
                var stdout = Console.OpenStandardOutput();

                stdinReader = new StreamReader(stdin, Encoding.UTF8);
                stdoutWriter = new StreamWriter(stdout, Encoding.UTF8)
                {
                    AutoFlush = true
                };

                // Start reading stdin in background thread
                System.Threading.ThreadPool.QueueUserWorkItem(ReadStdin);
            }
            catch (Exception e)
            {
                Debug.LogError($"❌ Failed to initialize IPC streams: {e.Message}");
            }
        }

        /// <summary>
        /// Background thread for reading stdin
        /// </summary>
        private void ReadStdin(object state)
        {
            while (isRunning)
            {
                try
                {
                    string line = stdinReader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        lock (messageQueue)
                        {
                            messageQueue.Enqueue(line);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"❌ Error reading stdin: {e.Message}");
                    break;
                }
            }
        }

        /// <summary>
        /// Process queued messages on main thread
        /// </summary>
        private void ProcessMessages()
        {
            int processed = 0;
            int maxPerFrame = Mathf.Max(1, messageProcessingRate / 60);

            lock (messageQueue)
            {
                while (messageQueue.Count > 0 && processed < maxPerFrame)
                {
                    string json = messageQueue.Dequeue();
                    ProcessMessage(json);
                    processed++;
                }
            }
        }

        /// <summary>
        /// Process a single IPC message
        /// </summary>
        private void ProcessMessage(string json)
        {
            try
            {
                IPCMessage message = JsonUtility.FromJson<IPCMessage>(json);

                if (debugMode)
                {
                    Debug.Log($"🦋 Received: {message.type}");
                }

                switch (message.type)
                {
                    case "initialize":
                        HandleInitialize(message.data);
                        break;
                    case "update":
                        HandleUpdate(message.data);
                        break;
                    case "render":
                        HandleRender(message.data);
                        break;
                    case "camera":
                        HandleCamera(message.data);
                        break;
                    case "postprocessing":
                        HandlePostProcessing(message.data);
                        break;
                    case "shutdown":
                        HandleShutdown();
                        break;
                    default:
                        SendError("UNKNOWN_MESSAGE_TYPE", $"Unknown message type: {message.type}");
                        break;
                }
            }
            catch (Exception e)
            {
                SendError("PARSE_ERROR", $"Failed to parse message: {e.Message}");
            }
        }

        /// <summary>
        /// Handle initialize message
        /// </summary>
        private void HandleInitialize(string dataJson)
        {
            try
            {
                InitializeData data = JsonUtility.FromJson<InitializeData>(dataJson);

                // Load scene if specified
                if (!string.IsNullOrEmpty(data.sceneName))
                {
                    SceneManager.LoadScene(data.sceneName);
                }

                // TODO: Apply cathedral parameters
                // This would configure procedural generation systems

                SendMessage("scene-loaded", new
                {
                    sceneName = data.sceneName,
                    objectCount = FindObjectsOfType<GameObject>().Length,
                    renderTime = Time.realtimeSinceStartup
                });

                SendMessage("update-ack", new { success = true });
            }
            catch (Exception e)
            {
                SendError("INIT_FAILED", e.Message);
            }
        }

        /// <summary>
        /// Handle update message
        /// </summary>
        private void HandleUpdate(string dataJson)
        {
            try
            {
                UpdateData data = JsonUtility.FromJson<UpdateData>(dataJson);

                // TODO: Update cathedral parameters in real-time
                // This would modify procedural generation or material properties

                SendMessage("update-ack", new { success = true });
            }
            catch (Exception e)
            {
                SendError("UPDATE_FAILED", e.Message);
            }
        }

        /// <summary>
        /// Handle render message
        /// </summary>
        private void HandleRender(string dataJson)
        {
            try
            {
                RenderData data = JsonUtility.FromJson<RenderData>(dataJson);
                StartCoroutine(CaptureScreenshot(data));
            }
            catch (Exception e)
            {
                SendError("RENDER_FAILED", e.Message);
            }
        }

        /// <summary>
        /// Capture screenshot coroutine
        /// </summary>
        private IEnumerator CaptureScreenshot(RenderData data)
        {
            // Wait for end of frame to ensure rendering is complete
            yield return new WaitForEndOfFrame();

            try
            {
                // Capture screenshot
                ScreenCapture.CaptureScreenshot(data.outputPath);

                // Wait a frame for file write
                yield return null;

                SendMessage("render-complete", new
                {
                    outputPath = data.outputPath,
                    width = data.width,
                    height = data.height,
                    renderTime = Time.realtimeSinceStartup
                });
            }
            catch (Exception e)
            {
                SendError("RENDER_FAILED", e.Message);
            }
        }

        /// <summary>
        /// Handle camera control message
        /// </summary>
        private void HandleCamera(string dataJson)
        {
            try
            {
                CameraData data = JsonUtility.FromJson<CameraData>(dataJson);
                Camera mainCamera = Camera.main;

                if (mainCamera != null)
                {
                    if (data.position != null)
                    {
                        mainCamera.transform.position = data.position.ToVector3();
                    }

                    if (data.rotation != null)
                    {
                        mainCamera.transform.eulerAngles = data.rotation.ToVector3();
                    }

                    if (data.fieldOfView > 0)
                    {
                        mainCamera.fieldOfView = data.fieldOfView;
                    }

                    SendMessage("camera-updated", new { success = true });
                }
                else
                {
                    SendError("CAMERA_NOT_FOUND", "No main camera in scene");
                }
            }
            catch (Exception e)
            {
                SendError("CAMERA_FAILED", e.Message);
            }
        }

        /// <summary>
        /// Handle post-processing message
        /// </summary>
        private void HandlePostProcessing(string dataJson)
        {
            try
            {
                PostProcessingData data = JsonUtility.FromJson<PostProcessingData>(dataJson);

                // TODO: Apply post-processing settings
                // This would configure Unity Post Processing Stack

                SendMessage("postprocessing-updated", new { success = true });
            }
            catch (Exception e)
            {
                SendError("POSTPROCESSING_FAILED", e.Message);
            }
        }

        /// <summary>
        /// Handle shutdown message
        /// </summary>
        private void HandleShutdown()
        {
            isRunning = false;
            SendMessage("shutdown-ack", new { success = true });

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// Send message to Node.js
        /// </summary>
        private void SendMessage(string type, object data)
        {
            try
            {
                var message = new
                {
                    type = type,
                    timestamp = DateTime.UtcNow.ToString("o"),
                    data = data
                };

                string json = JsonUtility.ToJson(message);
                stdoutWriter.WriteLine(json);

                if (debugMode)
                {
                    Debug.Log($"✨ Sent: {type}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"❌ Failed to send message: {e.Message}");
            }
        }

        /// <summary>
        /// Send error message
        /// </summary>
        private void SendError(string errorCode, string message)
        {
            SendMessage("error", new
            {
                errorCode = errorCode,
                message = message,
                stackTrace = ""
            });

            if (debugMode)
            {
                Debug.LogError($"❌ IPC Error [{errorCode}]: {message}");
            }
        }

        /// <summary>
        /// Send heartbeat message
        /// </summary>
        private void SendHeartbeat()
        {
            SendMessage("heartbeat", new
            {
                fps = (int)(1.0f / Time.deltaTime),
                frameCount = Time.frameCount,
                uptime = Time.realtimeSinceStartup
            });
        }

        void OnDestroy()
        {
            isRunning = false;

            if (stdoutWriter != null)
            {
                stdoutWriter.Close();
            }

            if (stdinReader != null)
            {
                stdinReader.Close();
            }
        }

        void OnApplicationQuit()
        {
            isRunning = false;
        }
    }
}
