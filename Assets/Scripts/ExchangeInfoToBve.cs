using System.Collections;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using UnityEngine;

namespace MetroDriveEX.Unity
{
    public class ExchangeInfoToBve : MonoBehaviour
    {
        public string SharedMes;
        [SerializeField] string BveExePath;
        internal Process BveProcess;
        NamedPipeServerStream server = new NamedPipeServerStream("metrodrive", PipeDirection.InOut);
        StreamWriter writer; StreamReader reader;
        //MemoryMappedFile MemoryMappedFile;
        async void Start()
        {
            DontDestroyOnLoad(this);
            BveProcess = new Process()
            {
                StartInfo = new ProcessStartInfo(BveExePath)
            };
            writer = new StreamWriter(server); writer.AutoFlush = true;
            reader = new StreamReader(server);
            //MemoryMappedFile = MemoryMappedFile.CreateNew("metrodrive", 1024);//BveではOpenExisting
            await server.WaitForConnectionAsync();
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
        IEnumerator ReadMes()
        {
            SharedMes = reader.ReadLine();
            yield return new WaitForSeconds(1);
        }
        // Update is called once per frame
        void Update()
        {
            switch (SharedMes)
            {
                case "end"://シナリオの終了・FormsのHide();
                    SharedMes = "none";
                    //エンディング再生(Movie？)
                    break;
            }
            SendMesToBve(SharedMes);
        }
        public void OnScenarioStarted(int scenarioNumber)//シナリオを開始させる
        {
            SharedMes = "Scenario:" + scenarioNumber;
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

