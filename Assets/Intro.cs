using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("House_Scene", LoadSceneMode.Single);
    }
}
