using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerImage : MonoBehaviour
{
    public List<Sprite> sprites;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePlayerImage(PlayerJob.PLAYERJOP index)
    {
        gameObject.GetComponent<Image>().sprite = sprites[(int)index];
    }
}
