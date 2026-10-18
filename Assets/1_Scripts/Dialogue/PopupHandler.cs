using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PopupHandler : MonoBehaviour
{
    [Title("PARAMETERS")]
    [SerializeField] private List<PopText> _availablePopTexts = new List<PopText>();
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

    public void SpawnPopup(string dialogueid)
    {
        if (_availablePopupPositions.Count <= 0 || _availablePopTexts.Count <= 0)
        {
            Debug.LogError("Not enough popup positions or texts to display new popup!");
            return;
        }
        Transform poptransform = _availablePopupPositions[0];
        _availablePopupPositions.RemoveAll(x => x == poptransform);
        PopupPrefab popup = Instantiate(_prefab, poptransform);
        PopText poptext = _availablePopTexts.Find(x => x.DialogueID == dialogueid);
        if (poptext == null)
        {
            Debug.LogError("Could not find popup with given dialogue ID!");
            popup.InitPopup("ERROR");
        }
        else
        {
            _availablePopTexts.RemoveAll(x => x == poptext);
            popup.InitPopup(poptext.Text);
        }
    }
}

[System.Serializable]
public class PopText
{
    public string DialogueID;

    public string Text;
}
