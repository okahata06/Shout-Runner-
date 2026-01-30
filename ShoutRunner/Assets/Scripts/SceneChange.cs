using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GamemainButton : MonoBehaviour
{
    [SerializeField] GameObject IrisPanel;
    [SerializeField] RectTransform unmask;

    readonly Vector2 IRIS_MID_SCALE1 = new Vector2(1.0f, 1.5f);
    readonly Vector2 IRIS_MID_SCALE2 = new Vector2(2.0f, 3.0f);

    private AudioSource audiosourse;

    public AudioClip buttonclick;
    public AudioClip backbutton;
    public AudioClip pause;

    public static bool _playermove = true;

    private bool _clickonce;

    private void Awake()
    {
        audiosourse = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        IrisPanel.SetActive(false);

        _clickonce = true;
        _playermove = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("joystick button 7") && !Panel.activeSelf)
        //{
        //    Panel.SetActive(true);

        //    audiosourse.PlayOneShot(pause);

        //    Time.timeScale = 0;

        //    _clickonce = true;
        //    _playermove = false;
        //}
    }

    public void Yes()
    {
        if (_clickonce)
        {
            StartCoroutine(Title());

            _clickonce = false;
        }
    }

    public void No()
    {
        if (_clickonce)
        {
            StartCoroutine(Back());

            _clickonce = false;
        }
    }

    public void IrisOut()
    {
        unmask.DOScale(IRIS_MID_SCALE1, 0.2f).SetEase(Ease.InCubic);
        unmask.DOScale(IRIS_MID_SCALE2, 0.2f).SetDelay(0.2f).SetEase(Ease.OutCubic);
        unmask.DOScale(new Vector2(0, 0), 0.4f).SetDelay(0.4f).SetEase(Ease.InCubic);
    }

    private IEnumerator Title()
    {
        audiosourse.PlayOneShot(buttonclick);
        Time.timeScale = 1;
        IrisPanel.SetActive(true);
        IrisOut();
        yield return new WaitForSeconds(1f);
        _playermove = true;
        SceneManager.LoadScene("Title");
    }

    private IEnumerator Back()
    {
        audiosourse.PlayOneShot(backbutton);
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.1f);
        _playermove = true;
    }
}
