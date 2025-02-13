using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MetroDriveEX.Unity
{
    public class SelectTrain : MonoBehaviour
    {
        public int DiaglamNumber = 0;
        public int MaxScenarioNumber;
        ExchangeInfoToBve ExchangeInfo;
        SceneController Scenecontroller;
        private void Start()
        {
            Scenecontroller = GameObject.FindWithTag("SceneManager").gameObject.GetComponent<SceneController>();
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                DiaglamNumber--;
                PlaySound("pi");
            }
            if(Input.GetKeyDown(KeyCode.UpArrow)) 
            { 
                DiaglamNumber++;
                PlaySound("pi");
            }
            if (DiaglamNumber < 0) DiaglamNumber = 0;
            if (DiaglamNumber > MaxScenarioNumber) DiaglamNumber = MaxScenarioNumber;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Scenecontroller.ChangeScene("menu",this.gameObject);
            }
            if(Input.GetKeyDown(KeyCode.Return))
            {
                ExchangeInfo.SendMesToBve("Scenario:" + DiaglamNumber);
            }
        }
        void PlaySound(string fileName)
        {

        }
    }
}
