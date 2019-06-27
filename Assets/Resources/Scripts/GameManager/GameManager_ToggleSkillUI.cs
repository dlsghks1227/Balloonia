using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_ToggleSkillUI : MonoBehaviour
{
    public GameObject skillUIPanel;

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

    public void CheckForSkillUIToggleRequest()
    {
        ToggleSkillUI();
    }

    void ToggleSkillUI()
    {
        SoundManager.Instance.ButtonClickPlay();
        if (skillUIPanel != null)
        {
            skillUIPanel.SetActive(!skillUIPanel.activeSelf);
            GameManager.Instance.isSkillUIOn = !GameManager.Instance.isSkillUIOn;
            GameManager.Instance.CallEventSkillUIToggle();
        }
    }
}
