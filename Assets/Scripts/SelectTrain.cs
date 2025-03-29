using UnityEngine;

namespace MetroDriveEX.Unity
{
    public class SelectTrain : MonoBehaviour
    {
        public int DiaglamNumber = 0;
        public int MaxScenarioNumber;
        ExchangeInfoToBve ExchangeInfo;
        SceneController Scenecontroller;
        MediaController Sound;
        private void Start()
        {
            Scenecontroller = GameObject.FindWithTag("SceneManager").gameObject.GetComponent<SceneController>();
            Sound = GameObject.FindWithTag("MediaController").gameObject.GetComponent<MediaController>();
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                DiaglamNumber--;
                Sound.PlaySound(0);//pi
            }
            if(Input.GetKeyDown(KeyCode.UpArrow)) 
            { 
                DiaglamNumber++;
                Sound.PlaySound(0);
            }
            if (DiaglamNumber < 0) DiaglamNumber = 0;
            if (DiaglamNumber > MaxScenarioNumber) DiaglamNumber = MaxScenarioNumber;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Sound.PlaySound(1);//escape sound
                Scenecontroller.FadeOut("menu",this.gameObject);
            }
            if(Input.GetKeyDown(KeyCode.Return))
            {
                Sound.PlaySound(2);
                ExchangeInfo.DiaglamIndex = DiaglamNumber;
                ExchangeInfo.SendMesToBve("Scenario:" + DiaglamNumber);
                Scenecontroller.FadeOut("wait", this.gameObject);
            }
        }
    }
}
