using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] string[] allTips;
    [SerializeField] TextMeshProUGUI _textTips;
    [SerializeField] Image ImageBackGround;
    [SerializeField] TextMeshProUGUI _nameLevel;

    [SerializeField] EventSO _eventLaunchLevel;
    [SerializeField] EventSO _eventStartLevel;

    static bool created = false;
    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        _eventLaunchLevel.OnLauchEventInt += StartLoadingScreen;
    }

    private void OnDisable()
    {
        _eventLaunchLevel.OnLauchEventInt -= StartLoadingScreen;
    }

    void ChangeCanvas()
    {
        int rand = Random.Range(0, allTips.Length);
        if (allTips.Length > 0)
            _textTips.text = allTips[rand];
        //ImageBackGround.sprite = obj.BackGroundLoad;
        //_nameLevel.text = obj.MapName;
    }

    public void StartLoadingScreen(int levelIndex)
    {
        StartCoroutine(StartLoad(levelIndex));
        ChangeCanvas();
    }

    IEnumerator StartLoad(int index)
    {
        canvasGroup.alpha = 1;
        loadingScreen.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 3));
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        while (!operation.isDone)
        {
            yield return null;
        }
        yield return StartCoroutine(FadeLoadingScreen(0, 1));
        _eventStartLevel.OnLaunchEvent?.Invoke();
        loadingScreen.SetActive(false);
    }
    IEnumerator FadeLoadingScreen(float targetValue, float duration)
    {
        float startValue = canvasGroup.alpha;
        float time = 0;
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetValue;
    }
}
