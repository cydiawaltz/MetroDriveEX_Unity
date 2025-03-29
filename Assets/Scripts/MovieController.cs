using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace MetroDriveEX.Unity
{
    public class MovieController : MonoBehaviour
    {
        [SerializeField] VideoPlayer Player;
        [SerializeField] RenderTexture render;
        [SerializeField] RawImage image;
        [SerializeField] SceneType Type;
        [SerializeField] int max;
        public string VideoPath;
        public int index;
        enum SceneType { splash,ending,picture,opening,none}

        /* >VideoのIndex
        * 0:splash(「metroDriveを起動中...」)
        * 1:opening(sugamostation => [Hongo MCC(or PineApple computer)])
        * 2~11:Ending01 ~10
        * 12:「運転終了 GameOver」（悲壮感のあるBGMとともに)
        * 13:03Special(以下できれば)
        * 14:20000Special
        * 15:13000Special
        * 16:70000/70090Special
        * >PictureのIndexは17以降、そこはハードコード
        */
        private void Awake()
        {
            image.enabled = false;
        }
        void Start()
        {
            Player = this.gameObject.AddComponent<VideoPlayer>();
            switch (this.gameObject.scene.name)
            {
                case "splash":
                    Type = SceneType.splash;
                    index = 0;
                   break;
                case "opening":
                    Type = SceneType.opening;
                    index = 1; break;
                case "ending":
                    Type = SceneType.ending;
                    ExchangeInfoToBve info = GameObject.FindWithTag("ExchangeInfo").GetComponent<ExchangeInfoToBve>();
                    if (info.State == ExchangeInfoToBve.GameState.Over) index = 12;
                    else if (info.State == ExchangeInfoToBve.GameState.Clear) index = info.DiaglamIndex;
                    else if (info.State == ExchangeInfoToBve.GameState.End) SceneManager.LoadScene("selecttrain");
                    break;
                case "picture":
                    Type = SceneType.picture;
                    index = 13;//後から変更
                    break;
                default:
                    Type = SceneType.none;
                    break;
            }
            LoadVideo();
            Player.prepareCompleted += Player_prepareCompleted;
            Player.Prepare();
        }
        private void Player_prepareCompleted(VideoPlayer source)
        {
            Player.started += Player_started;
            Player.Play();
        }
        private void Player_started(VideoPlayer source)
        {
            image.enabled = true;
        }

        void Update()
        {
            if(Type == SceneType.picture)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow)) index--;
                if (Input.GetKeyDown(KeyCode.UpArrow)) index++;
                if (index < 0) index = 0;
                if (index > max) index = max;
                if (index < 17)
                {
                    image.enabled = false;
                    LoadVideo();
                    Player.Prepare();
                }
                else
                {

                }
            }
        }
        void LoadVideo()
        {
            VideoPath = Path.Combine(Application.streamingAssetsPath, "/video/" + index + ".mp4");
            Player.url = VideoPath;
            Player.renderMode = VideoRenderMode.RenderTexture;
            Player.targetTexture = render;
            image.texture = render;
        }
    }
}