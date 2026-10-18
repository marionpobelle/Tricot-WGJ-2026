using DG.Tweening;
using EasyTransition;
using NUnit.Framework;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleHandler : MonoBehaviour
{
    [Title("PARAMETERS")]
    [SerializeField] private List<PuzzleSO> _availablePuzzles = new List<PuzzleSO>();

    [Title("REFERENCES")]
    [SerializeField] private List<PointPrefab> _pointsLine1 = new List<PointPrefab>();
    [SerializeField] private List<PointPrefab> _pointsLine2 = new List<PointPrefab>();
    [SerializeField] private List<PointPrefab> _pointsLine3 = new List<PointPrefab>();
    [SerializeField] private List<PointPrefab> _pointsLine4 = new List<PointPrefab>();

    [SerializeField] private CanvasGroup _puzzleGroup;

    private InputSystem_Actions _inputs;

    private PuzzleSO _currentPuzzle;

    private PointPrefab _playerPoint;

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
        InitNewPuzzle();
        EnablePuzzleInputs();
    }

    private void OnDestroy()
    {
        DisablePuzzleInputs();
    }

    public void InitNewPuzzle()
    {
        if(_availablePuzzles.Count <= 0)
        {
            Debug.LogError("No puzzle found in puzzleSO list!");
            return;
        }
        _currentPuzzle = _availablePuzzles[UnityEngine.Random.Range(0, _availablePuzzles.Count)];
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
        _puzzleGroup.DOFade(0, 0.5f);
        for (int i = 0; i < _pointsLine1.Count; i++)
        {
            PointStatus stat = _currentPuzzle.PuzzleLine1[i];
            _pointsLine1[i].InitPoint(stat);
            if(stat == PointStatus.Start)
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
                _playerPoint = _pointsLine1[i];
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
        _puzzleGroup.DOFade(1, 0.5f);
        EnablePuzzleInputs();
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
            InitNewPuzzle();
            GameHandler.Instance.IncrementKnitAmount();
            //TO DO: les anims et tout
        }
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
            CheckPuzzleSuccess();
        }
    }

    private void OnReset(InputAction.CallbackContext context)
    {
        SetUpPuzzle();
    }
}
