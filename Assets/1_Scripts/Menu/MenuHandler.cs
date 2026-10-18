using EasyTransition;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [Title("PARAMETERS")]
    [SerializeField] private TransitionSettings _transition;
    [SerializeField] private float _introLoadDelay;

    [Title("REFERENCES")]
    [SerializeField] private string _sceneToLoad;

    private bool _keyPressed = false;

    private void Start()
    {
        ResetToDefault();
        AudioHandler.Instance.Play("Music");
    }

    private void ResetToDefault()
    {
        _keyPressed = false;
        AudioHandler.Instance.Play("Music");
    }

    private void Update()
    {
        if(!_keyPressed && Input.anyKeyDown)
        {
            if (TransitionManager.Instance() != null && _keyPressed == false)
            {
                AudioHandler.Instance.PlayOneShot("SFX_Validation");
                TransitionManager.Instance().Transition(_sceneToLoad, _transition, _introLoadDelay);
            }
            _keyPressed = true;
        }
    }
}
