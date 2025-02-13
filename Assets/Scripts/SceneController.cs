using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace MetroDriveEX.Unity
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] float speed;
        /*Scene構造(Scene名)
         *                        ⇓------| ⇓-----------------------------|
         * splash -- opening -- main --  menu -- selecttrain -- wait -- ending
         *                                  |_ museum ---- music
         *                                  |           |_ model
         *                                  |           |_ picture
         *                                  |_ setting
         *                                  |_ about
         *                                  |_(exit) =>これはSceneではない
         */
        private void Start()
        {
            DontDestroyOnLoad(this);
            this.gameObject.tag = "SceneManager";
        }
        public async void ChangeScene(string scenename,GameObject target)
        {
            RectTransform upBar = (RectTransform)target.transform.Find("upbar");//UI要素は上が原点
            RectTransform bottoms = (RectTransform)target.transform.Find("bottoms");
            Color frontImg = target.transform.Find("front").gameObject.GetComponent<Image>().color;//全面の覆い
            RectTransform uIBar = (RectTransform)target.transform.Find("uibar");
            int height = Screen.height;
            while (upBar.position.y < 0 && bottoms.position.y > height)
            {
                upBar.position = new Vector3(upBar.position.x, upBar.position.y + speed, upBar.position.z);
                bottoms.position = new Vector3(bottoms.position.x, bottoms.position.y - speed, bottoms.position.z);
                uIBar.position = new Vector3(uIBar.position.x+speed,uIBar.position.y,uIBar.position.z);
                await Task.Yield();
            }
            while(frontImg.a > 0)
            {
                frontImg = new Color(frontImg.r, frontImg.g, frontImg.b, frontImg.a--);
                target.transform.Find("front").gameObject.GetComponent<Image>().color = frontImg;
                await Task.Yield();
            }
            SceneManager.LoadScene(scenename);
        }
    }
}
