using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public FadeController fader;

    public Text playerNameText;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.MenuSoundPlay();
        fader.FadeOut(1.5f, () =>
        {
            fader.fade.gameObject.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButton()
    {
        SoundManager.Instance.ButtonClickPlay();
        PlayerInfo.Instance.playername = playerNameText.text;
        fader.fade.gameObject.SetActive(true);
        fader.FadeIn(1.5f, () =>
        {
            SoundManager.Instance.SoundStop();
            SceneManager.LoadScene("MainGame");
        });
    }
}
