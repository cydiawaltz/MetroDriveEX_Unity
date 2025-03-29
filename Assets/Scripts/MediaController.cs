using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

namespace MetroDriveEX.Unity
{
    internal class MediaController : MonoBehaviour
    {
        //AudioファイルはResources\musicに配置
        [SerializeField] List<AudioSource> Audio = new List<AudioSource>(2);
        [SerializeField] List<AudioClip> AudioChannel = new List<AudioClip>(2);
        [SerializeField] AudioMixer mixer;

        /*
         * >SEのIndex
         * 0:pi（上下移動の時の音）
         * 1:won (ps2とかでよくありがちな戻る音)
         * 2:den (決定の音)
         * 3:whoon (ゲーム開始の音（ウ"ォ"ｦ"ン゙みたいな音）)
         * 
         * >BGMのIndex
         * 4:MainBGM(menuで流すやつ、「モニターマンの逆襲」?)
         * 5:SelectBGM(TrainSelectで流す、「電GO Final menu」?)
         * 6~15:隠しBGM?(Distance,ComeBackToMe Remix,TRIPOD BABY,Tasete Your Stuff などEDのBGM)
         * 
         * >放送(museum用)
         * 16~24:日比谷線発車時/到着時（Q）
         * 25~34:同（新）
         * 35~39:TH用放送(発車/到着)
         * 40~51:発車メロディ(A線)　秋葉原は更新前、後両方
         * 
         * 
         * >VideoのIndex
         * 0:splash(「metroDriveを起動中...」)
         * 1:opening (sugamostation => [Hongo MCC(or PineApple computer)])
         * 2~11:Ending01~10
         * 12:「運転終了 GameOver」（悲壮感のあるBGMとともに)
         * 13:03Special(以下できれば)
         * 14:20000Special
         * 15:13000Special
         * 16:70000/70090Special
         * >PictureのIndexは17以降、そこはハードコード
         */
        private void Start()
        {
            DontDestroyOnLoad(this);
            this.gameObject.tag = "MediaController";
            SceneManager.sceneLoaded += OnSceneLoaded;
            Audio[0].loop = false; Audio[1].loop = true;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (Audio[1].isPlaying) Audio[1].Stop();
            switch(arg0.name)//動画はScene側で再生しろ！
            {
                case "menu":
                    LoadAudio(1, "/sound/4.mp3");
                    Audio[1].Play();
                    break;
                case "selecttrain":
                    LoadAudio(1, "/sound/5.mp3");
                    Audio[1].Play();
                    break;
            }
        }

        public void PlaySound(int musicIndex)//SoundEffectとMuseumでの
        {
            LoadAudio(0, "/sound/" + musicIndex + ".mp3");
            Audio[0].PlayOneShot(AudioChannel[0]);
        }
        public float GetdBValue()
        {
            float volume;
            mixer.GetFloat("Volume", out volume);
            return volume;
        }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        IEnumerator LoadAudio(int type,string filePath)
        {
            string path = Path.Combine(Application.streamingAssetsPath, filePath);
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Audio Load Error: " + www.error);
                }
                else
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                    Audio[type].clip = clip;
                }
            }
        }
    }
}
