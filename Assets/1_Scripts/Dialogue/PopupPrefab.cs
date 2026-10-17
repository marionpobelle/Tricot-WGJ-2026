using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;

public class PopupPrefab : MonoBehaviour
{
    [Title("PARAMETERS")]
    [SerializeField] private float _fadeTime = 1f;

    [Title("REFERENCES")]
    [SerializeField] private TextMeshProUGUI _popupText;
    [SerializeField] private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup.alpha = 0;
        _popupText.text = "";
    }

    public void InitPopup(string newtext)
    {
        ChangePopText(newtext);
        _canvasGroup.DOFade(1, _fadeTime);
    }

    private void ChangePopText(string newtext)
    {
        _popupText.text = newtext;
    }
}
