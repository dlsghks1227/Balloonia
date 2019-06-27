using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    public ObjectPooler objectPooler = ObjectPooler.Instance;

    public FadeController fader;

    public delegate void GameManagerEventHandle();
    public event GameManagerEventHandle MenuToggleEvent;
    public event GameManagerEventHandle SkillUIToggleEvent;
    public event GameManagerEventHandle GameOverEvent;

    public bool isMenuOn;
    public bool isSkillUIOn;
    public bool isGameOver;

    private float balloonRespawnTime;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.InGameSoundPlay();

        fader.FadeOut(1.5f, () =>
        {
            fader.fade.gameObject.SetActive(false);
        });

        objectPooler.Initialize();

        BalloonRespawn();

        foreach(var airshipList in objectPooler.GetAllPooledObjects((int)ObjectPooler.OBJECTPOOLER.Airship))
        {
            airshipList.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        balloonRespawnTime += Time.deltaTime;

        if(balloonRespawnTime >= 5.0f)
        {
            BalloonRespawn();
            balloonRespawnTime = 0.0f;
        }
    }

    public void OnExitButton()
    {
        SoundManager.Instance.ButtonClickPlay();
        fader.fade.gameObject.SetActive(true);
        fader.FadeIn(1.5f, () =>
        {
            SoundManager.Instance.SoundStop();
            SceneManager.LoadScene("MainMenu");
        });
    }

    void BalloonRespawn()
    {
        foreach(var balloonList in objectPooler.GetAllPooledObjects((int)ObjectPooler.OBJECTPOOLER.DefaultBalloon))
        {
            if(!balloonList.activeSelf)
            {
                balloonList.gameObject.transform.position = new Vector3(10.0f * Random.Range(-10.0f, 10.0f), 80.0f + Random.Range(-4.0f, 4.0f), 10.0f * Random.Range(-10.0f, 10.0f));
                balloonList.SetActive(true);
            }
        }
    }

    public void CallEventMenuToggle()
    {
        if(MenuToggleEvent != null)
        {
            isGameOver = true;

            MenuToggleEvent();
        }
    }

    public void CallEventSkillUIToggle()
    {
        if (SkillUIToggleEvent != null)
        {

            SkillUIToggleEvent();
        }
    }

    public void CallEventGameOver()
    {
        if (GameOverEvent != null)
        {
            isGameOver = true;
            GameOverEvent();
        }
    }
}
