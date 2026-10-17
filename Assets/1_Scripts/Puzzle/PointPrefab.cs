using DG.Tweening;
using EasyTransition;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PointPrefab : MonoBehaviour
{
    [Title("PARAMETERS")]
    [SerializeField] private Sprite _targetSprite;
    [SerializeField] private Sprite _emptySprite;
    [SerializeField] private Sprite _startSprite;
    [SerializeField] private Sprite _endSprite;

    [Title("REFERENCES")]
    [SerializeField] private Image _pointImage;
    [SerializeField] private GameObject _pointPlayerAnim;

    [SerializeField] private CanvasGroup _upLine;
    [SerializeField] private CanvasGroup _downLine;
    [SerializeField] private CanvasGroup _leftLine;
    [SerializeField] private CanvasGroup _rightLine;

    [SerializeField] private PointPrefab _upPoint;
    [SerializeField] private PointPrefab _downPoint;
    [SerializeField] private PointPrefab _leftPoint;
    [SerializeField] private PointPrefab _rightPoint;

    public PointStatus CurrentStatus;
    public bool IsPlayerHere = false;
    public bool IsValid = false;

    public void InitPoint(PointStatus status)
    {
        CurrentStatus = status;
        switch (status)
        {
            case PointStatus.Empty:
                _pointImage.sprite = _emptySprite;
                IsValid = true;
                IsPlayerHere = false;
                TogglePlayerAnim(false);
                break;
            case PointStatus.Target:
                _pointImage.sprite = _targetSprite;
                IsValid = false;
                IsPlayerHere = false;
                TogglePlayerAnim(false);
                break;
            case PointStatus.Start:
                _pointImage.sprite = _startSprite;
                IsValid = true;
                IsPlayerHere = true;
                TogglePlayerAnim(true);
                break;
            case PointStatus.End:
                _pointImage.sprite = _endSprite;
                IsValid = false;
                IsPlayerHere = false;
                TogglePlayerAnim(false);
                break;
            default:
                _pointImage.sprite = _emptySprite;
                IsValid = true;
                IsPlayerHere = false;
                TogglePlayerAnim(false);
                break;
        }
        HideAllLines();
    }

    public void TogglePlayerAnim(bool toggle)
    {
        _pointPlayerAnim.SetActive(toggle);
    }

    public void HideAllLines()
    {
        if(_upLine != null)
        {
            _upLine.alpha = 0;
        }
        if (_downLine != null)
        {
            _downLine.alpha = 0;
        }
        if (_leftLine != null)
        {
            _leftLine.alpha = 0;
        }
        if (_rightLine != null)
        {
            _rightLine.alpha = 0;
        }
    }

    public PointPrefab GetNeighborPoint(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                if (_leftPoint != null)
                {
                    return _leftPoint;
                }
                Debug.LogError("There is no point in this direction!");
                return null;
            case Direction.Right:
                if (_rightPoint != null)
                {
                    return _rightPoint;
                }
                Debug.LogError("There is no point in this direction!");
                return null;
            case Direction.Up:
                if (_upPoint != null)
                {
                    return _upPoint;
                }
                Debug.LogError("There is no point in this direction!");
                return null;
            case Direction.Down:
                if (_downPoint != null)
                {
                    return _downPoint;
                }
                Debug.LogError("There is no point in this direction!");
                return null;
            default:
                return null;
        }
    }

    public bool TryCheckLine(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                if (_leftLine != null && _leftLine.alpha <= 0)
                {
                    _leftLine.alpha = 1;
                    return true;
                }
                else
                {
                    return false;
                }
            case Direction.Right:
                if (_rightLine != null && _rightLine.alpha <= 0)
                {
                    _rightLine.alpha = 1;
                    return true;
                }
                else
                {
                    return false;
                }
            case Direction.Up:
                if (_upLine != null && _upLine.alpha <= 0)
                {
                    _upLine.alpha = 1;
                    return true;
                }
                else
                {
                    return false;
                }
            case Direction.Down:
                if (_downLine != null && _downLine.alpha <= 0)
                {
                    _downLine.alpha = 1;
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                return false;
        }
    }
}

public enum PointStatus
{
    Empty,
    Target,
    Start,
    End,
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}
