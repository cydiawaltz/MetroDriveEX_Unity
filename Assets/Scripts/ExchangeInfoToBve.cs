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
            //MemoryMappedFile = MemoryMappedFile.CreateNew("metrodrive", 1024);//Bve�ł�OpenExisting
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
             * ���b�Z�[�W�^�C�v
             * stay => unity to bve. bve���ł͔F��������"none"�ɏ�������(�ȉ����l)�@�ŏ��ɑ���
             * ready => bve to unity. ��������ʂ��I�����A�N����ʂɂ���
             * Scenario:(Num) => unity to bve. Num�����ɉ����ċN������V�i���I��ύX
             * End => bve to unity. �r�����E�������B
             * Over => bve to uniy. GameOver.
             * Clear => bve to unity GameClear.(����movie��Unity���Őݒ�)
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
                case "end"://�V�i���I�̏I���EForms��Hide();
                    SharedMes = "none";
                    //�G���f�B���O�Đ�(Movie�H)
                    break;
            }
            SendMesToBve(SharedMes);
        }
        public void OnScenarioStarted(int scenarioNumber)//�V�i���I���J�n������
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

