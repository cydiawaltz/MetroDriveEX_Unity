using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MetroDriveEX.Unity
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] float speed;
        /*Scene構造(Scene名)
         *                        ⇓------| ⇓-----------------------------|
         * splash -- ========== -- main --  menu -- selecttrain -- wait -- ending
         *          openingはsplashに統合    |_ museum ---- music
         *                                  |           |_ model
         *                                  |           |_ picture
         *                                  |_ setting
         * installing<=初回のみ、installdialo|_ about
         * ↑はExchange~から案内する          |_exit => 「アプリを終了させる準備ができました(win95風？)」
         */
        private void Start()
        {
            DontDestroyOnLoad(this); 
            this.gameObject.tag = "SceneManager";
        }
        public async void FadeOut(string scenename,GameObject target)
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
                float a = frontImg.a - speed;
                if (a < 0) a = 0;
                frontImg = new Color(frontImg.r, frontImg.g, frontImg.b, a);
                target.transform.Find("front").gameObject.GetComponent<Image>().color = frontImg;
                await Task.Yield();
            }
            SceneManager.LoadScene(scenename);
        }
    }
}
