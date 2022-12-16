using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLeaderboardHUD : MonoBehaviour
{

    [SerializeField] EventSO _eventAround;
    [SerializeField] EventSO _eventFriend;
    [SerializeField] EventSO _eventTop;
    // Start is called before the first frame update

    public void OnClickAround()
    {
        _eventAround.OnLauchEventSceneSO?.Invoke(DataManager.Instance.GetSceneData(SceneManager.GetActiveScene().buildIndex));
    }

    public void OnClickFriend()
    {
        _eventFriend.OnLauchEventSceneSO?.Invoke(DataManager.Instance.GetSceneData(SceneManager.GetActiveScene().buildIndex));
    }

    public void OnClickTop()
    {
        _eventTop.OnLauchEventSceneSO?.Invoke(DataManager.Instance.GetSceneData(SceneManager.GetActiveScene().buildIndex));
    }
}
