using DG.Tweening;
using EasyTransition;
using NUnit.Framework;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PuzzleHandler : MonoBehaviour
{
    [Title("PARAMETERS")]
    [SerializeField] private List<PuzzleSO> _availablePuzzles = new List<PuzzleSO>();

    [Title("REFERENCES")]
    [SerializeField] private List<PointPrefab> _pointsLine1 = new List<PointPrefab>();
    [SerializeField] private List<PointPrefab> _pointsLine2 = new List<PointPrefab>();
    [SerializeField] private List<PointPrefab> _pointsLine3 = new List<PointPrefab>();
    [SerializeField] private List<PointPrefab> _pointsLine4 = new List<PointPrefab>();

    [SerializeField] private CanvasGroup _iconGroup;
    [SerializeField] private Image _iconImg;
    [SerializeField] private Sprite _iconSuccess;
    [SerializeField] private Sprite _iconFailure;

    [SerializeField] private CanvasGroup _puzzleGroup;

    [SerializeField] private Animator _rightHandAnimator;

    [SerializeField] private List<SpriteRenderer> _knitHandsSprites;
    [SerializeField] private SpriteRenderer _knitUpSprite;
    [SerializeField] private SpriteRenderer _knitDownSprite;
    [SerializeField] private List<SpriteRenderer> _knitStripeSprites = new List<SpriteRenderer>();

    [SerializeField] private CanvasGroup _leftGroup;
    [SerializeField] private CanvasGroup _rightGroup;
    [SerializeField] private CanvasGroup _upGroup;
    [SerializeField] private CanvasGroup _downGroup;
    [SerializeField] private CanvasGroup _resetGroup;

    private InputSystem_Actions _inputs;

    private PuzzleSO _currentPuzzle;

    private PointPrefab _playerPoint;

    private int _puzzleIndex;

    public static PuzzleHandler Instance;

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

        _inputs = new InputSystem_Actions();
        _puzzleGroup.alpha = 0f;
        _puzzleIndex = 0;
        _leftGroup.alpha = 0f;
        _rightGroup.alpha = 0f;
        _upGroup.alpha = 0f;
        _downGroup.alpha = 0f;
        _resetGroup.alpha = 0f;
        _iconGroup.alpha = 0f;

    }

    public void StartPuzzles()
    {
        InitNewPuzzle();
        EnablePuzzleInputs();
        _leftGroup.DOFade(1, 1);
        _rightGroup.DOFade(1, 1);
        _upGroup.DOFade(1, 1);
        _downGroup.DOFade(1, 1);
        _resetGroup.DOFade(1, 1);
    }

    private void OnDestroy()
    {
        DisablePuzzleInputs();
    }

    public void InitNewPuzzle()
    {
        if(_puzzleIndex == 1)
        {
            _leftGroup.DOFade(0, 1);
            _rightGroup.DOFade(0, 1);
            _upGroup.DOFade(0, 1);
            _downGroup.DOFade(0, 1);
            DialogueHandler.Instance.StartDialogues();
        }
        if(_puzzleIndex >= _availablePuzzles.Count)
        {
            Debug.LogError("No puzzle found in puzzleSO list!");
            return;
        }
        _currentPuzzle = _availablePuzzles[_puzzleIndex];
        _puzzleIndex++;
        SetUpPuzzle();
    }

    public void SetUpPuzzle()
    {
        if(_currentPuzzle == null)
        {
            Debug.LogError("No current puzzle loaded!");
            return;
        }
        HideAllLines();
        DisablePuzzleInputs();
        _puzzleGroup.DOFade(0, 0.8f).OnComplete(() =>
        {
            for (int i = 0; i < _pointsLine1.Count; i++)
            {
                PointStatus stat = _currentPuzzle.PuzzleLine1[i];
                _pointsLine1[i].InitPoint(stat);
                if (stat == PointStatus.Start)
                {
                    _playerPoint = _pointsLine1[i];
                }
            }
            for (int i = 0; i < _pointsLine2.Count; i++)
            {
                PointStatus stat = _currentPuzzle.PuzzleLine2[i];
                _pointsLine2[i].InitPoint(stat);
                if (stat == PointStatus.Start)
                {
                    _playerPoint = _pointsLine2[i];
                }
            }
            for (int i = 0; i < _pointsLine3.Count; i++)
            {
                PointStatus stat = _currentPuzzle.PuzzleLine3[i];
                _pointsLine3[i].InitPoint(stat);
                if (stat == PointStatus.Start)
                {
                    _playerPoint = _pointsLine3[i];
                }
            }
            for (int i = 0; i < _pointsLine4.Count; i++)
            {
                PointStatus stat = _currentPuzzle.PuzzleLine4[i];
                _pointsLine4[i].InitPoint(stat);
                if (stat == PointStatus.Start)
                {
                    _playerPoint = _pointsLine4[i];
                }
            }
            HideAllLines();
            _puzzleGroup.DOFade(1, 0.8f);
            EnablePuzzleInputs();
            GameHandler.Instance.AllowInputs = true;
        });
    }

    private void HideAllLines()
    {
        for (int i = 0; i < _pointsLine1.Count; i++)
        {
            _pointsLine1[i].HideAllLines();
        }
        for (int i = 0; i < _pointsLine2.Count; i++)
        {
            _pointsLine2[i].HideAllLines();
        }
        for (int i = 0; i < _pointsLine3.Count; i++)
        {
            _pointsLine3[i].HideAllLines();
        }
        for (int i = 0; i < _pointsLine4.Count; i++)
        {
            _pointsLine4[i].HideAllLines();
        }
    }

    public void CheckPuzzleSuccess()
    {
        bool currentlyValid = true;
        DisablePuzzleInputs();
        for (int i = 0; i < _pointsLine1.Count; i++)
        {
            if (_pointsLine1[i].IsValid != true) currentlyValid = false;
        }
        for (int i = 0; i < _pointsLine2.Count; i++)
        {
            if (_pointsLine2[i].IsValid != true) currentlyValid = false;
        }
        for (int i = 0; i < _pointsLine3.Count; i++)
        {
            if (_pointsLine3[i].IsValid != true) currentlyValid = false;
        }
        for (int i = 0; i < _pointsLine4.Count; i++)
        {
            if (_pointsLine4[i].IsValid != true) currentlyValid = false;
        }

        if(currentlyValid == true && _playerPoint.CurrentStatus == PointStatus.End)
        {
            Debug.Log("Finished puzzle!");
            _rightHandAnimator.Play("Right_hand_anim", 0, 0);
            _iconImg.sprite = _iconSuccess;
            StartCoroutine(FlashIcon());
            UpdateStripesColors(GameHandler.Instance.CurrentEmotion);
            AudioHandler.Instance.PlayOneShot("SFX_Validation");
            InitNewPuzzle();
            GameHandler.Instance.IncrementKnitAmount();
        }
        else
        {
            EnablePuzzleInputs();
        }
    }

    private void PlayRandomMoveSFX()
    {
        int rand = UnityEngine.Random.Range(1, 5);
        switch (rand) 
        {
            case 1:
                AudioHandler.Instance.PlayOneShot("SFX_Move1");
                break;
            case 2:
                AudioHandler.Instance.PlayOneShot("SFX_Move2");
                break;
            case 3:
                AudioHandler.Instance.PlayOneShot("SFX_Move3");
                break;
            case 4:
                AudioHandler.Instance.PlayOneShot("SFX_Move4");
                break;
        }

    }

    private void FailPuzzle()
    {
        AudioHandler.Instance.PlayOneShot("SFX_Failure");
        _iconImg.sprite = _iconFailure;
        StartCoroutine(FlashIcon());
        SetUpPuzzle();
    }

    private IEnumerator FlashIcon()
    {
        _iconGroup.DOFade(1, 0.8f);
        yield return new WaitForSeconds(0.5f);
        _iconGroup.DOFade(0, 0.8f);
    }

    public void EnablePuzzleInputs()
    {
        _inputs.Puzzle.Enable();
        _inputs.Puzzle.Up.performed += OnUp;
        _inputs.Puzzle.Down.performed += OnDown;
        _inputs.Puzzle.Left.performed += OnLeft;
        _inputs.Puzzle.Right.performed += OnRight;
        _inputs.Puzzle.Reset.performed += OnReset;
    }

    public void DisablePuzzleInputs()
    {
        _inputs.Puzzle.Up.performed -= OnUp;
        _inputs.Puzzle.Down.performed -= OnDown;
        _inputs.Puzzle.Left.performed -= OnLeft;
        _inputs.Puzzle.Right.performed -= OnRight;
        _inputs.Puzzle.Reset.performed -= OnReset;
        _inputs.Puzzle.Disable();
    }

    private void OnUp(InputAction.CallbackContext context)
    {
        if (GameHandler.Instance.AllowInputs)
        {
            bool success = _playerPoint.TryCheckLine(Direction.Up);
            if (success == false) return;
            _playerPoint.TogglePlayerAnim(false);
            _playerPoint = _playerPoint.GetNeighborPoint(Direction.Up);
            _playerPoint.TogglePlayerAnim(true);
            _playerPoint.IsValid = true;
            PlayRandomMoveSFX();
            CheckPuzzleSuccess();
        }
    }

    private void OnDown(InputAction.CallbackContext context)
    {
        if (GameHandler.Instance.AllowInputs)
        {
            bool success = _playerPoint.TryCheckLine(Direction.Down);
            if (success == false) return;
            _playerPoint.TogglePlayerAnim(false);
            _playerPoint = _playerPoint.GetNeighborPoint(Direction.Down);
            _playerPoint.TogglePlayerAnim(true);
            _playerPoint.IsValid = true;
            PlayRandomMoveSFX();
            CheckPuzzleSuccess();
        }
    }

    private void OnLeft(InputAction.CallbackContext context)
    {
        if (GameHandler.Instance.AllowInputs)
        {
            bool success = _playerPoint.TryCheckLine(Direction.Left);
            if (success == false) return;
            _playerPoint.TogglePlayerAnim(false);
            _playerPoint = _playerPoint.GetNeighborPoint(Direction.Left);
            _playerPoint.TogglePlayerAnim(true);
            _playerPoint.IsValid = true;
            PlayRandomMoveSFX();
            CheckPuzzleSuccess();
        }
    }

    private void OnRight(InputAction.CallbackContext context)
    {
        if (GameHandler.Instance.AllowInputs)
        {
            bool success = _playerPoint.TryCheckLine(Direction.Right);
            if (success == false) return;
            _playerPoint.TogglePlayerAnim(false);
            _playerPoint = _playerPoint.GetNeighborPoint(Direction.Right);
            _playerPoint.TogglePlayerAnim(true);
            _playerPoint.IsValid = true;
            PlayRandomMoveSFX();
            CheckPuzzleSuccess();
        }
    }

    private void OnReset(InputAction.CallbackContext context)
    {
        FailPuzzle();
    }

    public void UpdateKnitUpColor()
    {
        _knitUpSprite.color = GameHandler.Instance.CurrentEmotion;
        foreach (SpriteRenderer item in _knitHandsSprites)
        {
            item.color = GameHandler.Instance.CurrentEmotion;
        }
    }

    public void UpdateWholeKnitColor()
    {
        _knitUpSprite.color = GameHandler.Instance.CurrentEmotion;
        foreach(SpriteRenderer item in _knitHandsSprites)
        {
            item.color = GameHandler.Instance.CurrentEmotion;
        }
        _knitDownSprite.color = GameHandler.Instance.CurrentEmotion;
        for(int i = 0; i < _knitStripeSprites.Count; i++)
        {
            _knitStripeSprites[i].color = GameHandler.Instance.CurrentEmotion;
        }
    }

    private void UpdateKnitStripeColor(Color color, int index)
    {
        if(_knitStripeSprites.Count == 0)
        {
            Debug.LogError("Strip list is empty!");
            return;
        }
        if(index >= _knitStripeSprites.Count || index < 0)
        {
            Debug.LogError("Index isn't within sprie list range");
            return;
        }
        _knitStripeSprites[index].color = color;
    }

    public void UpdateStripesColors(Color color)
    {
        for(int i = _knitStripeSprites.Count - 1; i > 0; i--)
        {
            UpdateKnitStripeColor(_knitStripeSprites[i - 1].color, i);
        }
        _knitStripeSprites[0].color = color;
        _knitDownSprite.color = _knitStripeSprites[_knitStripeSprites.Count - 1].color;
    }
}
