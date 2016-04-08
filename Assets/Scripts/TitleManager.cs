using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    
    public GameObject FadeImage;
    public float FadeTimeSecond;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonStart()
    {
        FadeImage.GetComponent<Fade>().FadeIn(0.2f);
        Invoke("GoPlay", FadeTimeSecond);
    }
    
    public void GoPlay()
    {
        SceneManager.LoadScene("Play");
    }

    public void ButtonInfo()
    {

    }
}
