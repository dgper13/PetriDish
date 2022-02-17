using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class UI : MonoBehaviour
{
    private Manager manager;
    private bool clicked;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        clicked = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ProcessInput()
    {
        if (!clicked)

        {
            manager.PopulationSize = int.Parse(GameObject.Find("PopulationSize").GetComponent<InputField>().text);
            manager.StartingCases = int.Parse(GameObject.Find("StartingCases").GetComponent<InputField>().text);

            manager.MinHealingTime = int.Parse(GameObject.Find("MinHealingTime").GetComponent<InputField>().text);
            manager.MaxHealingTime = int.Parse(GameObject.Find("MaxHealingTime").GetComponent<InputField>().text);
            
            manager.MinSeverity = int.Parse(GameObject.Find("MinSeverity").GetComponent<InputField>().text);
            manager.MaxSeverity = int.Parse(GameObject.Find("MaxSeverity").GetComponent<InputField>().text);

            manager.TransmissionRate = float.Parse(GameObject.Find("TransmissionRate").GetComponent<InputField>().text);

            manager.Activate();
            clicked = true;
        }
    }
}
