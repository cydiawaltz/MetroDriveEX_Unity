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
        { End, Over, Clear ,other }//�r�����E�A�Q�[���I�[�o�[�A�N���A�A���̑��̏��
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
                    var result = MessageBox.Show("MetroDrive�̋N���Ɏ��s���܂����B�R���|�[�l���g���������C���X�g�[������Ă邩�����m�F���������B", "Error01", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (result == DialogResult.OK)
                    {
                        System.Windows.Forms.Application.Exit();
                        this.gameObject.SetActive(false);
                    }
                }
            }
            writer = new StreamWriter(server); writer.AutoFlush = true;
            reader = new StreamReader(server);
            //MemoryMappedFile = MemoryMappedFile.CreateNew("metrodrive", 1024);//Bve�ł�OpenExisting
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
             * ���b�Z�[�W�^�C�v
             * stay => unity to bve. bve���ł͔F��������"none"�ɏ�������(�ȉ����l)�@�ŏ��ɑ���
             * ready => bve to unity. ��������ʂ��I�����A�N����ʂɂ���
             * Scenario:(Num) => unity to bve. Num�����ɉ����ċN������V�i���I��ύX
             * End => bve to unity. �r�����E�������B
             * Over => bve to uniy. GameOver.
             * Clear => bve to unity GameClear.(����movie��Unity���Őݒ�)
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

