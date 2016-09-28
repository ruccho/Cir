using UnityEngine;

public class GameInitial //: MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        
        Screen.SetResolution(Screen.height / 16 * 9, Screen.height, Screen.fullScreen);

    }

    //// Use this for initialization
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}