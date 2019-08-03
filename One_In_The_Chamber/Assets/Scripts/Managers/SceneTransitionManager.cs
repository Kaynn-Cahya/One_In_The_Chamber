using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager> {
    public void TransitionToSceneBySceneName(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void TransitionToSceneBySceneIndex(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }
}
