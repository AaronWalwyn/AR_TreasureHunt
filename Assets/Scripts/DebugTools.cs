using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;

public class DebugTools : MonoBehaviour {

    static Text debugText;
    static Text consoleText;

    static System.Diagnostics.Stopwatch sWatch;

    public const int consoleMax = 40;
    static Queue<string> consoleOutputs = new Queue<string>();

    static Dictionary<string, object> trackedVariables;

    static string logPath;
    static string logName;
    public static StreamWriter logFile;

    public static bool init;
    public bool showCanvas;

    public static Canvas debugCanvas;
    
    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        trackedVariables = new Dictionary<string, object>();
        if (!init) {
            init = Initialise();
        }
        //consoleText = GameObject.Find("DebugConsole").GetComponent<Text>();
    }

    void Start() {
        
    }
    void Update() {
        UpdateLog();
        UpdateText();
    }

    bool Initialise() {
        sWatch = new System.Diagnostics.Stopwatch();

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        try {
            logName = "\\ " + Application.productName + " " + System.DateTime.Now.ToFileTime() + ".txt";
            logPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\..\\Local\\" + Application.companyName + "\\" + Application.productName + logName;
            new FileInfo(logPath).Directory.Create();
            logFile = new StreamWriter(logPath, true);
        } catch (FileNotFoundException e) {
            Log(e.Message);
        }
#elif UNITY_ANDROID
        try {
            logPath = Application.persistentDataPath + "\\ log.txt";
            logFile = new StreamWriter(logPath, false);
        } catch (FileNotFoundException e) {
            Log(e.Message);
        }
#endif

        GameObject DebugCanvas = new GameObject("DebugCanvas");
        DebugCanvas.layer = LayerMask.NameToLayer("UI");
        debugCanvas = DebugCanvas.AddComponent<Canvas>();
        debugCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        debugCanvas.sortingOrder = 9999;

        CanvasGroup cg = DebugCanvas.AddComponent<CanvasGroup>();

        if (showCanvas && Debug.isDebugBuild) {
            cg.interactable = false;
            cg.alpha = 1f;
            cg.blocksRaycasts = false;
        } else {
            cg.interactable = false;
            cg.alpha = 0f;
            cg.blocksRaycasts = false;
        }

        CanvasScaler cs = DebugCanvas.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

        GameObject DebugConsole = new GameObject("DebugConsole");
        RectTransform rt = DebugConsole.AddComponent<RectTransform>();
        rt.parent = DebugCanvas.transform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.zero;
        rt.pivot = Vector2.zero;

        rt.anchoredPosition = new Vector2(10f, 10f);

        consoleText = DebugConsole.AddComponent<Text>();
        consoleText.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
        consoleText.color = Color.green;
        consoleText.alignment = TextAnchor.LowerLeft;

        consoleText.verticalOverflow = VerticalWrapMode.Overflow;
        consoleText.horizontalOverflow = HorizontalWrapMode.Overflow;

        GameObject DebugText = new GameObject("DebugText");
        rt = DebugText.AddComponent<RectTransform>();
        rt.parent = DebugCanvas.transform;
        rt.anchorMin = Vector2.up;
        rt.anchorMax = Vector2.up;
        rt.pivot = Vector2.up;
        rt.anchoredPosition = new Vector2(10, -10f);

        debugText = DebugText.AddComponent<Text>();
        debugText.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
        debugText.color = Color.green;
        debugText.alignment = TextAnchor.UpperLeft;

        debugText.verticalOverflow = VerticalWrapMode.Overflow;
        debugText.horizontalOverflow = HorizontalWrapMode.Overflow;

        debugText.text = "Test String Please Ignore";

        DontDestroyOnLoad(DebugCanvas);

        return true;
    }

    public static void UpdateText() {
        string s = "";
        lock (trackedVariables) {

            try {
                foreach (string k in trackedVariables.Keys) {
                    s += k + ": " + trackedVariables[k].ToString() + "\n";
                }
            } catch (InvalidOperationException e) {
                Debug.Log(e.Message);
            }

        }

        if (debugText != null) {
            debugText.text = s;
        }
    }

    public static void UpdateLog() {
        string[] logs = consoleOutputs.ToArray();
        Array.Reverse(logs);

        string log = "";
        foreach (string l in logs) {
            log = l + "\n" + log;
        }

        if (consoleText != null) {
            consoleText.text = log;
        }
    }

    public static void Log(object s) {
        if (consoleOutputs.Count < consoleMax) {
            consoleOutputs.Enqueue(s.ToString());
        } else {
            consoleOutputs.Dequeue();
            Log(s);
        }
    }

    public static void Log(object s, bool toFile) {
        if (toFile) LogToFile(s.ToString());
        if (consoleOutputs.Count < consoleMax) {
            consoleOutputs.Enqueue(s.ToString());
        } else {
            consoleOutputs.Dequeue();
            Log(s);
        }
    }

    public static void Log(object s, bool toFile, bool toConsole) {
        if (toConsole) Debug.Log(s);
        if (toFile) LogToFile(s.ToString());
        if (consoleOutputs.Count < consoleMax) {
            consoleOutputs.Enqueue(s.ToString());
        } else {
            consoleOutputs.Dequeue();
            Log(s);
        }
    }

    public static void LogToFile(string s) {
        try {
            logFile.WriteLine(s);
            logFile.Flush();
        } catch (NullReferenceException e) {
            Debug.Log(e.Message);
        }
    }

    public static void Track(string key, object value) {
        if (init) {
            lock (trackedVariables) {
                if (trackedVariables.ContainsKey(key)) {
                    trackedVariables[key] = value;
                } else {
                    trackedVariables.Add(key, value);
                    DebugTools.Log("Added Tracked Variable: " + key);
                }
            }
        }
    }


    static long oldTime = 0, newTime, temp;
    public static long GetTimeDiff() {
        if(!sWatch.IsRunning) {
            sWatch.Start();
            return 0;
        } else {
            temp = sWatch.ElapsedMilliseconds;
            newTime = temp - oldTime;
            oldTime = temp;
            return newTime;
        }
    }

    void OnApplicationQuit() {
		if (logFile != null)
        	logFile.Close();
    }
}
