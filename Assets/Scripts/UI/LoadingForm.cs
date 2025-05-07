using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingForm : Form
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private RectTransform _progressbar;

    public override void Init()
    {
    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public void SetProgress(float percent)
    {
        _progressbar.localScale = new Vector3(percent, 1, 1);
    }

    public void SetText(string text)
    {
        _titleText.text = text;
    }
}
