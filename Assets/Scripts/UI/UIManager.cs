using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private SceneRouteTable sceneRoutes;
    [SerializeField] private string gameplaySceneName = "SampleScene";

    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject tutImg1;
    public GameObject TutImg2;
    public GameObject TutImg3;
    public GameObject title;
    public GameObject backButton;
    public GameObject next;

    private void Start()
    {
        if (sceneRoutes != null && !string.IsNullOrEmpty(sceneRoutes.gameplayScene))
        {
            gameplaySceneName = sceneRoutes.gameplayScene;
        }

        SetActiveIfPresent(button1, true);
        SetActiveIfPresent(button2, true);
        SetActiveIfPresent(button3, true);
        SetActiveIfPresent(title, true);
        SetActiveIfPresent(tutImg1, false);
        SetActiveIfPresent(TutImg2, false);
        SetActiveIfPresent(TutImg3, false);
        SetActiveIfPresent(backButton, false);
        SetActiveIfPresent(next, false);
    }

    private static void SetActiveIfPresent(GameObject target, bool active)
    {
        if (target != null)
        {
            target.SetActive(active);
        }
    }

    public void startGame()
    {
        SceneRouter.Load(gameplaySceneName);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void tutorial()
    {
        SetActiveIfPresent(button1, false);
        SetActiveIfPresent(button2, false);
        SetActiveIfPresent(button3, false);
        SetActiveIfPresent(title, false);
        SetActiveIfPresent(tutImg1, true);
        SetActiveIfPresent(backButton, true);
        SetActiveIfPresent(next, true);
    }

    public void back()
    {
        SetActiveIfPresent(button1, true);
        SetActiveIfPresent(button2, true);
        SetActiveIfPresent(button3, true);
        SetActiveIfPresent(title, true);
        SetActiveIfPresent(tutImg1, false);
        SetActiveIfPresent(TutImg2, false);
        SetActiveIfPresent(TutImg3, false);
        SetActiveIfPresent(backButton, false);
        SetActiveIfPresent(next, false);
    }

    public void nextTut()
    {
        if (tutImg1 != null && tutImg1.activeSelf)
        {
            SetActiveIfPresent(tutImg1, false);
            SetActiveIfPresent(TutImg2, true);
        }
        else if (TutImg2 != null && TutImg2.activeSelf)
        {
            SetActiveIfPresent(TutImg2, false);
            SetActiveIfPresent(TutImg3, true);
        }
        else if (TutImg3 != null && TutImg3.activeSelf)
        {
            SetActiveIfPresent(next, false);
        }
    }
}

public static class SceneRouter
{
    public static void Load(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("SceneRouter.Load called with an empty scene name.");
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"Scene '{sceneName}' is not in the build settings or does not exist.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}
