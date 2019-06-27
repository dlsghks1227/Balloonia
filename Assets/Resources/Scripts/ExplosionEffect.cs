using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float time = 2.0f;

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
        StartCoroutine(LifeTime());
    }

    private void OnDisable()
    {
        
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
