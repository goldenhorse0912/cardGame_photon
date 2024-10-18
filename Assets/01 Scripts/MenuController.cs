using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{

    public GameObject splash, setPlayerNamePanel;
    public InputField nameInput, nameInputOnline;
    public Text messageText, userName;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySplash());
        if (PlayerPrefs.GetString("userName") != string.Empty && SceneManager.GetActiveScene().name == "MainMenu") { nameInput.text = PlayerPrefs.GetString("userName"); }
        if (SceneManager.GetActiveScene().name == "NetworkMenu") { userName.text = PlayerPrefs.GetString("userName"); }
    }

    public void ResetScore()
    {
        PegsScoreManager.ResetScores();
    }
    public void PlayOnlineButton()
    {
        if (PlayerPrefs.GetString("userName") == string.Empty) { setPlayerNamePanel.SetActive(true); }
        else { SceneManager.LoadScene("NetworkMenu"); }
    }
    public void SetUserNameButton()
    {
        if (PlayerPrefs.GetString("userName") != string.Empty) { SceneManager.LoadScene("NetworkMenu"); }
        else { messageText.text = "YOU MUST PROVIDE A VALID USERNAME"; Invoke(nameof(RestoreMessage), 2f); }
    }
    public void BackButtonNamePanel()
    {
        setPlayerNamePanel.SetActive(false);
    }
    public void RestoreMessage()
    {
        messageText.text = "YOU CAN CHANGE YOUR NAME LATER IN SETTINGS SECTION";
    }
    public void ChangeScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    IEnumerator DestroySplash()
    {
        yield return new WaitForSeconds(4f);
        splash.SetActive(false);
        Destroy(splash, 2f);
    }
    public void LoadUserName()
    {
        print(nameInput.text);
        PlayerPrefs.SetString("userName", nameInput.text);
    }
    public void LoadUserNameOnlinePanel()
    {
        PlayerPrefs.SetString("userName", nameInputOnline.text);
    }
}
