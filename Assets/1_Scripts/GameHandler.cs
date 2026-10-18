using DG.Tweening;
using EasyTransition;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private CanvasGroup _unfocusedEndGroup;

    private bool _keyPressed = false;
    private bool _noDialogueLeft;

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
        _unfocusedEndGroup.alpha = 0f;
        SwitchCurrentEmotion("neutral");
        AllowInputs = true;
    }

    private void Update()
    {
        if (!_keyPressed && Input.anyKeyDown && _noDialogueLeft)
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
            PlayUnfocusedEnd();
        }
    }

    private void PlayUnfocusedEnd()
    {
        _unfocusedEndGroup.DOFade(1, 1);
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
        KnitColors.Add(CurrentEmotion);
    }

    public void IncrementKnitAmount()
    {
        _knitcurrentAmount++;
        CheckForFinishedKnit();
    }

    public void CheckForFinishedKnit()
    {
        if(_knitcurrentAmount >= _knitMaxAmount)
        {
            DialogueHandler.Instance.TerminateDialogueCoroutine();
            AllowInputs = false;
            Debug.Log("Finished knitting!");
        }
    }
}
