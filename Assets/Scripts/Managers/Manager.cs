using Unity.VisualScripting;
using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager _instance;
    public static Manager Instance => _instance;

    ResourceManager _resource = new ResourceManager();
    SoundManager _sound = new SoundManager();
    SceneManagerEX _scene = new SceneManagerEX();
    UIManager _ui = new UIManager();
    DataManager _data = new DataManager();

    public static ResourceManager Resource => Instance._resource;
    public static SoundManager Sound => Instance._sound;
    public static SceneManagerEX Scene => Instance._scene;
    public static UIManager UI => Instance._ui;
    public static DataManager Data => Instance._data;

    public int CurrentStage = 0; 

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _data.Init();
        _sound.Init();
    }

    void Start()
    {
        
    }

    void Update()
    {
    }

    void Init()
    {
        _sound.Init();
    }

    void Clear()
    {

    }

    public void SetStageClearCanvas()
    {
        UI.SetStageClearCanvas(FindFirstObjectByType<StageClearUIController>().gameObject);
    }
}