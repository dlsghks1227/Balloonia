using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_ToggleMenu : MonoBehaviour
{
    public GameObject menuPanel;

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

    public void CheckForMenuToggleRequest()
    {
        ToggleMenu();
    }

    void ToggleMenu()
    {
        SoundManager.Instance.ButtonClickPlay();
        if(menuPanel != null)
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
            GameManager.Instance.isMenuOn = !GameManager.Instance.isMenuOn;
            GameManager.Instance.CallEventMenuToggle();
        }
    }
}
