using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJob : MonoBehaviour
{
    public enum PLAYERJOP
    {
        // LV. 1
        Beginner = 0,

        // LV. 2
        Attacker,
        Defender,
        Quicker,
        Specialist,

        // LV. 3
        Rifler,
        Hunter,
        Shielder,
        Mirrorlian,
        Ghost,
        Flash,
        Vulture,
        Magician,

        // LV. 4
        Sniper,
        Cannon_Shooter,
        Knight,
        Reflectist,
        Rogue,
        Thief,
        Bomber,
        Soulmaster,

        // LV. 5
        Razerlist,
        Biochemist,
        King,
        Spiker,
        Assassin,
        Sneaker,
        General,
        Wizard,
    }

    public List<GameObject> jobs;

    private int number;

    [HideInInspector]
    public List<GameObject> jobList;

    // Start is called before the first frame update
    void OnEnable()
    {
        number = 0;
        foreach (var item in jobs)
        {
            GameObject obj = Instantiate(item) as GameObject;
            obj.transform.position = transform.position;
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            jobList.Add(obj);
        }
    }

    public void SetPlayerJob(PLAYERJOP index)
    {
        jobList[number].SetActive(false);
        number = (int)index;
        jobList[number].SetActive(true);
    }
}
