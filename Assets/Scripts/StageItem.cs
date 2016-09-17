using UnityEngine;
using System.Collections;

public class StageItem : MonoBehaviour {
    const float initTimeSecond = 0.5f;
	// Use this for initialization
	void Start ()
    {
        transform.localScale = new Vector2(0, 0);
        StartCoroutine(init());
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    IEnumerator init()
    {
        float targetScale = transform.localScale.x;
        int frame = (int)(initTimeSecond * 60);
        float incrementPerFrame = targetScale / frame;
       
        for(int i = 0; i < frame; i++)
        {
            targetScale += incrementPerFrame;
            transform.localScale = new Vector2(targetScale, targetScale);
            yield return null;
        }
        targetScale = transform.localScale.x;
        transform.localScale = new Vector2(targetScale, targetScale);
    }
}
