using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class PopupHandler : MonoBehaviour
{
    [Title("PARAMETERS")]
    [SerializeField] private List<string> _availablePopTexts = new List<string>();
    [SerializeField] private List<Transform> _availablePopupPositions = new List<Transform>();

    [Title("REFERENCES")]
    [SerializeField] private PopupPrefab _prefab;
    [SerializeField] private Transform _transformHolder;

    public static PopupHandler Instance;

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

    public void SpawnPopup()
    {
        if (_availablePopupPositions.Count <= 0 || _availablePopTexts.Count <= 0)
        {
            Debug.LogError("Not enough popup positions or texts to display new popup!");
            return;
        }
        Transform poptransform = _availablePopupPositions[UnityEngine.Random.Range(0, _availablePopupPositions.Count)];
        _availablePopupPositions.RemoveAll(x => x == poptransform);
        PopupPrefab popup = Instantiate(_prefab, poptransform);
        string poptext = _availablePopTexts[UnityEngine.Random.Range(0, _availablePopTexts.Count)];
        _availablePopTexts.RemoveAll(x => x == poptext);
        popup.InitPopup(poptext);
    }
}
