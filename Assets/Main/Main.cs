using UI;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Start()
    {
        Router.CloseAndOpen("LoginView");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
