using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void TurnOnGameOverPanel()
    {
        SoundManager.Instance.GameOverSoundPlay();
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}
