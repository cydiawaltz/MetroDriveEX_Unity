using System.Collections;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using UnityEngine;
using System.Windows.Forms;

namespace MetroDriveEX.Unity
{
    public class ExchangeInfoToBve : MonoBehaviour
    {
        public string SharedMes;
        [SerializeField] string BveExePath;
        internal Process BveProcess;
        NamedPipeServerStream server = new NamedPipeServerStream("metrodrive", PipeDirection.InOut);
        StreamWriter writer; StreamReader reader;
        public bool IsBveReady = false;
        public GameState State;
        public int DiaglamIndex;
        bool IsWriterRunning = false;
        //MemoryMappedFile MemoryMappedFile;
        public enum GameState
        { End, Over, Clear ,other }//途中離脱、ゲームオーバー、クリア、その他の状態
        async void Start()
        {
            DontDestroyOnLoad(this);
            this.gameObject.tag = "ExchangeInfo";
            try
            {
                BveProcess = new Process()
                {
                    StartInfo = new ProcessStartInfo(BveExePath)
                };
                BveProcess.Start();

            }
            catch
            {
                if(File.Exists(Path.Combine(UnityEngine.Application.streamingAssetsPath,@"savedata\first.txt")))
                {
                    var result = MessageBox.Show("MetroDriveの起動に失敗しました。コンポーネントが正しくインストールされてるかをご確認ください。", "Error01", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (result == DialogResult.OK)
                    {
                        System.Windows.Forms.Application.Exit();
                        this.gameObject.SetActive(false);
                    }
                }
            }
            writer = new StreamWriter(server); writer.AutoFlush = true;
            reader = new StreamReader(server);
            //MemoryMappedFile = MemoryMappedFile.CreateNew("metrodrive", 1024);//BveではOpenExisting
            await server.WaitForConnectionAsync();
            IsWriterRunning = true;
            SendMesToBve("stay");
            StartCoroutine("ReadMes");
        }
        public void SendMesToBve(string sharedMes)
        {
            writer.WriteLine(sharedMes);
            /*using (var accessor = MemoryMappedFile.CreateViewAccessor())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(sharedMes);

            }*/
            /*
             * メッセージタイプ
             * stay => unity to bve. bve側では認識したら"none"に書き換え(以下同様)　最初に送る
             * ready => bve to unity. 準備中画面を終了し、起動画面にする
             * Scenario:(Num) => unity to bve. Num部分に応じて起動するシナリオを変更
             * End => bve to unity. 途中離脱を示す。
             * Over => bve to uniy. GameOver.
             * Clear => bve to unity GameClear.(流すmovieはUnity側で設定)
             */
        }
        IEnumerator ExchangeInfo()
        {
            if(IsWriterRunning)
            {
                SharedMes = reader.ReadLine();
                SendMesToBve(SharedMes);
            }
            yield return new WaitForSeconds(1);
        }
        // Update is called once per frame
        void Update()
        {
            State = GameState.other;
            switch (SharedMes)
            {
                case "ready":
                    IsBveReady = true; State = GameState.other; break;
                case "End":
                    State = GameState.End; break;
                case "Over":
                    State = GameState.Over; break;
                case "Clear":
                    State = GameState.Clear; break;
                default: State = GameState.other; break;
            }
        }
        void OnApplicationQuit()
        {
            server.Close();
            server.Dispose();
            //MemoryMappedFile.Dispose();
            BveProcess.Kill();
            BveProcess.Dispose();
        }
    }
}

