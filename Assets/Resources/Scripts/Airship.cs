using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airship : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    private void OnEnable()
    {
        float angle = Random.Range(0.0f, 360.0f);
        gameObject.transform.position = new Vector3(10.0f * Random.Range(-10.0f, 10.0f), 80.0f + Random.Range(-10.0f, 10.0f), 10.0f * Random.Range(-10.0f, 10.0f));
        gameObject.transform.rotation = Quaternion.LookRotation(new Vector3(Mathf.Sin(angle), 0.0f, Mathf.Cos(angle)));
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(new Vector3(0.0f, 0.0f, 1.0f) * speed * Time.deltaTime);
    }

    private void OnDestroy()
    {

    }

    void PlayEffect(Vector3 pos)
    {
        GameObject obj = GameManager.Instance.objectPooler.GetPooledObject((int)ObjectPooler.OBJECTPOOLER.ExplosionEffect);
        if (obj == null)
            return;
        obj.transform.position = pos;
        obj.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Bullet"))
        {
            PlayEffect(other.transform.position);
            other.gameObject.SetActive(false);
        }
    }
}
