using UnityEngine;
using UnityEngine.SceneManagement;


public class SceaneLoder : MonoBehaviour
{
    [SerializeField] SceneName _sceneName;
    public enum SceneName
    {
        MainGame,
        MiniGame
    }
    public void SceaneLoader()
    {
        Debug.Log(_sceneName.ToString());
        SceneManager.LoadScene(_sceneName.ToString());
    }
}
