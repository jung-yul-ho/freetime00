using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.Serialization;

public enum SpeechbubbleType
{
    NORMAL,
    SERIOUS,
    ANGRY,
    THINKING,
}

public class SpeechControler : MonoBehaviour
{
    public enum SpeechbubbleType
    {
        NORMAL,
        SERIOUS,
        ANGRY,
        THINKING,
    }
    [System.Serializable]
    public class SpeechbubblePrefab
    {
        public SpeechbubbleType type;
        public GameObject prefab;
    }

    [Header("Default settings:")]
    [FormerlySerializedAs("defaultColor")]
    [SerializeField]
    public Color _defaultColor = Color.white;

    [FormerlySerializedAs("defaultTimeToLive")]
    [SerializeField]
    public float _defaultTimeToLive = 1;

    [FormerlySerializedAs("is2D")]
    [SerializeField]
    public bool _is2D = true;

    [Tooltip("If you want to change the size of your speechbubbles in a scene without having to change the prefabs then change this value")]
    [FormerlySerializedAs("sizeMultiplier")]
    [SerializeField]
    public float _sizeMultiplier = 1f;

    [Tooltip("If you want to use different managers, for example if you want to have one manager for allies and one for enemies in order to style their speech bubbles differently set this to false. Note that you will need to keep track of a reference some other way in that case.")]
    [SerializeField]
    public bool _isSingleton = true;

    [Header("Prefabs mapping to each type:")]
    [FormerlySerializedAs("prefabs")]
    [SerializeField]
    public List<SpeechbubblePrefab> _prefabs;

    public Dictionary<SpeechbubbleType, GameObject> _prefabsDict = new Dictionary<SpeechbubbleType, GameObject>();
    public Dictionary<SpeechbubbleType, Queue<SpeechBehaviour>> _speechBubbleQueueDict = new Dictionary<SpeechbubbleType, Queue<SpeechBehaviour>>();

    [SerializeField]
    [Tooltip("Will use main camera if left as null")]
    public Camera _cam;

    static SpeechControler instance;
    public static SpeechControler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpeechControler>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<SpeechControler>();
                }
            }
            return instance;
        }
    }

    public Camera Cam
    {
        get
        {
            return _cam;
        }

        set
        {
            _cam = value;
            foreach (var bubbleQueue in _speechBubbleQueueDict.Values)
            {
                foreach (var bubble in bubbleQueue)
                {
                    bubble.Cam = _cam;
                }
            }
        }
    }

    protected void Awake()
    {
        if (_cam == null) _cam = Camera.main;

        if (_isSingleton)
        {
            UnityEngine.Assertions.Assert.IsNull(instance, "_intance was not null. Do you maybe have several Speech Bubble Managers in your scene, all trying to be singletons?");
            //_instance = this.gameObject;
        }
        _prefabsDict.Clear();
        _speechBubbleQueueDict.Clear();
        foreach (var prefab in _prefabs)
        {
            _prefabsDict.Add(prefab.type, prefab.prefab);
            _speechBubbleQueueDict.Add(prefab.type, new Queue<SpeechBehaviour>());
        }
    }

    private IEnumerator DelaySpeechBubble(float delay, Transform objectToFollow, string text, SpeechbubbleType type, float timeToLive, Color color, Vector3 offset)
    {
        yield return new WaitForSeconds(delay);
        if (objectToFollow)
            AddSpeechBubble(objectToFollow, text, type, timeToLive, color, offset);
    }
    
    public SpeechBehaviour AddSpeechBubble(Vector3 position, string text, SpeechbubbleType type = SpeechbubbleType.NORMAL, float timeToLive = 0, Color color = default(Color))
    {
        if (timeToLive == 0) timeToLive = _defaultTimeToLive;
        if (color == default(Color)) color = _defaultColor;
        SpeechBehaviour bubbleBehaviour = GetBubble(type);
        bubbleBehaviour.Setup(position, text, timeToLive, color, Cam);
        _speechBubbleQueueDict[type].Enqueue(bubbleBehaviour);
        return bubbleBehaviour;
    }

    //말풍선 창조 명령
    public SpeechBehaviour AddSpeechBubble(Transform objectToFollow, string text, SpeechbubbleType type = SpeechbubbleType.NORMAL, float timeToLive = 0, Color color = default(Color), Vector3 offset = new Vector3())
    {
        //만일 시간을 정해놓지 않은경우 1초로 고정
        if (timeToLive == 0)
        {
            timeToLive = _defaultTimeToLive;
        }
        //만일 새갈을 지정해 놓지 않은경우 하얀색으로 고정
        if (color == default(Color))
        {
            color = _defaultColor;
        }
        SpeechBehaviour bubbleBehaviour = GetBubble(type);
        bubbleBehaviour.Setup(objectToFollow, offset, text, timeToLive, color, Cam);
        _speechBubbleQueueDict[type].Enqueue(bubbleBehaviour);

        return bubbleBehaviour;
    }
    public void AddDelayedSpeechBubble(float delay, Transform objectToFollow, string text, SpeechbubbleType type = SpeechbubbleType.NORMAL, float timeToLive = 0, Color color = default(Color), Vector3 offset = new Vector3())
    {
        StartCoroutine(DelaySpeechBubble(delay, objectToFollow, text, type, timeToLive, color, offset));
    }
    
    //실질적인 말풍선 창조
    private SpeechBehaviour GetBubble(SpeechbubbleType type = SpeechbubbleType.NORMAL)
    {
        SpeechBehaviour bubbleBehaviour;
        //Check to see if there is a free speechbuble of the right kind to reuse
        if (_speechBubbleQueueDict[type].Count == 0 || _speechBubbleQueueDict[type].Peek().gameObject.activeInHierarchy)
        {
            GameObject newBubble = (GameObject)GameObject.Instantiate(GetPrefab(type));
            newBubble.transform.SetParent(transform);
            newBubble.transform.localScale = _sizeMultiplier * GetPrefab(type).transform.localScale;
            bubbleBehaviour = newBubble.GetComponent<SpeechBehaviour>();
            //If this is not 2D then the speechbubble will need a world space canvas.
            if (!_is2D)
            {
                var canvas = newBubble.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.WorldSpace;
                canvas.overrideSorting = true;
            }
        }
        else
        {
            bubbleBehaviour = _speechBubbleQueueDict[type].Dequeue();
        }
        //Set as last to always place latest on top (in case of screenspace ui that is..)
        bubbleBehaviour.transform.SetAsLastSibling();
        return bubbleBehaviour;
    }

    private GameObject GetPrefab(SpeechbubbleType type)
    {
        return _prefabsDict[type];
    }

    public SpeechbubbleType GetRandomSpeechbubbleType()
    {
        return _prefabs[Random.Range(0, _prefabs.Count)].type;
    }

    /// <summary>
    /// Clears all visible Speech Bubbles from the scene, causing them to be instantly recycled
    /// </summary>
    public void Clear()
    {
        foreach (var bubbleQueue in _speechBubbleQueueDict)
        {
            foreach (var bubble in bubbleQueue.Value)
            {
                bubble.Clear();
            }
        }
    }
}
