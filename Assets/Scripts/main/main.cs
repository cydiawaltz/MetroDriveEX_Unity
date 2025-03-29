using DG.Tweening;
using UnityEngine;

namespace MetroDriveEX.Unity
{
    public class main : MonoBehaviour
    {
        [SerializeField] GameObject Logo;
        [SerializeField] float DefaultPos;
        [SerializeField] float TotalFrame;
        [SerializeField] GameObject PushKey;
        [SerializeField] float FlashingCycle;
        // Use this for initialization
        void Start()
        {
            RectTransform logo = Logo.GetComponent<RectTransform>();
            Vector2 endpos = logo.anchoredPosition;
            logo.anchoredPosition = new Vector2(DefaultPos,logo.anchoredPosition.y);
            //DotWeenで移動
            //transform.DOLocalMove(new Vector3(10f, 0, 0), 1f);
            //logo.transform.DOMoveX(-DefaultPos.x,1f).SetEase(Ease.InOutBack).onComplete+=OnLogoInComplete;
            logo.DOAnchorPos(endpos, TotalFrame).SetEase(Ease.InOutQuart).onComplete+=OnLogoInComplete;
        }
        void OnLogoInComplete()
        {
            //Debug.Log("saitama");

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                
            }
            /*if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.DOMove(new Vector3(-5f, 0, 0), 1f).SetEase(Ease.OutQuint);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.DOMove(new Vector3(5f, 0, 0), 1f).SetEase(Ease.OutQuint);
            }*/
        }
        async void FadeOut()
        {

        }
    }
}