using MetroDriveEX.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Splash : MonoBehaviour
{
    VideoPlayer Player;
    VideoClip[] Clips;//[0]:splash [1]:opening
    ExchangeInfoToBve ExchangeInfoToBve;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.SetResolution(1280, 720, false);
        //Raw =  this.gameObject.GetComponent<RawImage>();
        Player = GetComponent<VideoPlayer>();
        Player.clip = Clips[0];
        Player.isLooping = true;
        Player.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(ExchangeInfoToBve == null)
        {
            ExchangeInfoToBve = GameObject.FindWithTag("ExchangeInfo").GetComponent<ExchangeInfoToBve>();
        }
        if(ExchangeInfoToBve.SharedMes == "ready")
        {
            Screen.fullScreen = true;
            Player.Stop();
            Player.clip = Clips[1];
            Player.isLooping = false;
            Player.loopPointReached += OnPlayEnd;
            Player.Play();
        }
        if(Screen.fullScreen && Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene("main");//オープニングのskip(デバッグ用)
        }
    }

    private void OnPlayEnd(VideoPlayer source)
    {
        SceneManager.LoadScene("main");
    }
}
