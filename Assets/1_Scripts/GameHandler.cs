using DG.Tweening;
using EasyTransition;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    [Title("PARAMETERS")]
    [SerializeField] private TransitionSettings _transition;
    [SerializeField] private float _introLoadDelay;

    [SerializeField] private Color _neutralColor;
    [SerializeField] private Color _anxietyColor;
    [SerializeField] private Color _reassuranceColor;
    [SerializeField] private Color _angerColor;
    [SerializeField] private Color _humorColor;
    [SerializeField] private Color _sadnessColor;
    [SerializeField] private Color _joyColor;

    [SerializeField] private int _knitMaxAmount = 4;
    [SerializeField] private int _knitcurrentAmount = 0;

    [Title("REFERENCES")]
    [SerializeField] private string _sceneToLoad;
    [SerializeField] private CanvasGroup _failEndGroup;
    [SerializeField] private CanvasGroup _successEndGroup;

    [SerializeField] private List<Image> _successScarf;
    [SerializeField] private List<Image> _failureScarf;

    private bool _keyPressed = false;
    private bool _noDialogueLeft;
    private bool _finishedKnitting;

    public Color CurrentEmotion;
    public List<Color> KnitColors = new List<Color>();

    public bool AllowInputs = false;

    public static GameHandler Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        _knitcurrentAmount = 0;
        _noDialogueLeft = false;
        _finishedKnitting = false;
        _failEndGroup.alpha = 0f;
        _successEndGroup.alpha = 0f;
        SwitchCurrentEmotion("neutral");
        PuzzleHandler.Instance.UpdateWholeKnitColor();
    }

    private void Update()
    {
        if (!_keyPressed && Input.GetKeyDown(KeyCode.E) && (_noDialogueLeft || _finishedKnitting))
        {
            if (TransitionManager.Instance() != null && _keyPressed == false)
            {
                TransitionManager.Instance().Transition(_sceneToLoad, _transition, _introLoadDelay);
            }
            _keyPressed = true;
        }
    }

    public void SetNoDialogue(bool toggle)
    {
        _noDialogueLeft = toggle;
        if (_noDialogueLeft == true)
        {
            AllowInputs = false;
            PlayFailEnd();
        }
    }

    private void PlayFailEnd()
    {
        if(KnitColors.Count > 5)
        {
            KnitColors.Reverse();
            for (int i = 0; i < KnitColors.Count; i++)
            {
                _failureScarf[i].color = KnitColors[i];
            }
        }
        else
        {
            for (int i = 0; i < _failureScarf.Count; i++)
            {
                _failureScarf[i].color = _neutralColor;
            }
        }

        _failEndGroup.DOFade(1, 1);
    }
    private void PlaySuccessEnd()
    {
        if (KnitColors.Count > 0)
        {
            KnitColors.Reverse();
            for (int i = 0; i < KnitColors.Count; i++)
            {
                _successScarf[i].color = KnitColors[i];
            }
        }
        _successEndGroup.DOFade(1, 1);
    }

    public void SwitchCurrentEmotion(string emotion)
    {
        switch (emotion)
        {
            case "neutral":
                CurrentEmotion = _neutralColor;
                break;
            case "anxiety":
                CurrentEmotion = _anxietyColor;
                break;
            case "reassurance":
                CurrentEmotion = _reassuranceColor;
                break;
            case "anger":
                CurrentEmotion = _angerColor;
                break;
            case "humor":
                CurrentEmotion = _humorColor;
                break;
            case "sadness":
                CurrentEmotion = _sadnessColor;
                break;
            case "joy":
                CurrentEmotion = _joyColor;
                break;
            default:
                CurrentEmotion = _neutralColor;
                break;
        }
        PuzzleHandler.Instance.UpdateKnitUpColor();
    }

    public void IncrementKnitAmount()
    {
        _knitcurrentAmount++;
        KnitColors.Add(CurrentEmotion);
        CheckForFinishedKnit();
    }

    public void CheckForFinishedKnit()
    {
        if(_knitcurrentAmount >= _knitMaxAmount)
        {
            DialogueHandler.Instance.TerminateDialogueCoroutine();
            AllowInputs = false;
            PlaySuccessEnd();
            _finishedKnitting = true;
            Debug.Log("Finished knitting!");
        }
    }
}
