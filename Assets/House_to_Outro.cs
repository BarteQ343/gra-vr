using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class House_to_Outro : MonoBehaviour
{
    public void ShowOutro()
    {
        SceneManager.LoadScene("Outro", LoadSceneMode.Single);
    }
}
