using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : Menu
{
    [SerializeField] private TextMeshProUGUI flashText;
    public string startingScene = "JanitorClosetRoomScene";
    public float gradientTime = 1;
    public Color left0;
    public Color left1;
    public Color right0;
    public Color right1;

    private Sequence flashSeq;
    private float gradientAlpha = 0;
    private int gradientDir = 1;
    private bool loadStarted = false;


    public override void Close()
    {
        base.Close();
        gameObject.SetActive(false);
    }

    public override void Open()
    {
        gameObject.SetActive(true);
        base.Open();
    }

    private void OnEnable()
    {
        GameManager.Instance.FocusCameraOn(null);
        loadStarted = false;
    }

    private void Update()
    {
        if (!IsOpen)
            return;

        if (Input.anyKeyDown)
            LoadGame();

        if (flashSeq == null || !flashSeq.active)
        {
            flashSeq = DOTween.Sequence()
                .Append(flashText.DOFade(0, 0.4f).SetEase(Ease.OutQuart))
                .Append(flashText.DOFade(1, 0.4f).SetEase(Ease.InQuart))
                .AppendInterval(2.2f);
        }

        flashText.colorGradient = GetNextGradient();
    }

    private VertexGradient GetNextGradient()
    {
        gradientAlpha += Time.deltaTime / gradientTime * gradientDir;

        if (gradientDir == 1 && gradientAlpha >= 1)
            gradientDir = -1;
        else if (gradientDir == -1 && gradientAlpha <= 0)
            gradientDir = 1;

        Color left = Color.Lerp(left0, left1, gradientAlpha);
        Color right = Color.Lerp(right0, right1, gradientAlpha);
        return new VertexGradient(left, right, left, right);
    }

    private async void LoadGame()
    {
        if (loadStarted)
            return;
        loadStarted = true;

        UIManager.Instance.SetTransitionVisible(true, 1.55f);
        await SceneManager.LoadSceneAsync(startingScene, LoadSceneMode.Single);
        await Awaitable.WaitForSecondsAsync(2);
        GameManager.Instance.Player.transform.position = Vector3.zero;
        UIManager.Instance.SetTransitionVisible(false, 1.1f);
        Close();
    }
}
