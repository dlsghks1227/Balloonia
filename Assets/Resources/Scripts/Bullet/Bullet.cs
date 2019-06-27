using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 movement = new Vector3(0.0f, 0.0f, 1.0f);

    public float time;
    public float damage;
    public float speed;

    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(LifeTime());
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(movement * speed * Time.deltaTime);
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        gameObject.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        gameObject.name = "StrayBullet";

    }
}
