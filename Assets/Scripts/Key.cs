using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour
{

    public GameObject OpenParticle;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("DOOR OPEN");
            GameObject OpenParticleObject = (GameObject)Instantiate(OpenParticle, GameObject.FindGameObjectWithTag("Door").transform.position, Quaternion.Euler(0, 0, 0));
            OpenParticleObject.GetComponent<ParticleSystem>().Emit(1);
            OpenParticleObject.transform.parent = GameObject.Find("Stage").transform;
            GameObject.Destroy(GameObject.FindGameObjectWithTag("Door"));
            GameObject.Destroy(gameObject);
        }
    }
}
