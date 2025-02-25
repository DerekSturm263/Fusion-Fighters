using Extensions.Miscellaneous;
using GameResources.Camera;
using Quantum;
using System.Collections;
using UnityEngine;

public class PlayerMessageController : PlayerTracker<RectTransform>
{
    [SerializeField] private GameObject[] _messageBubbles;
    [SerializeField] private Vector2 _offset;

    [SerializeField] private Canvas _canvas;
    [SerializeField] private float _messageTime;

    protected override void Awake()
    {
        base.Awake();

        QuantumEvent.Subscribe<EventOnSendMessage>(listener: this, handler: e =>
        {
            if (e.Message.Length == 0)
                return;

            _messageBubbles[e.Index.Global].SetActive(true);
            _messageBubbles[e.Index.Global].GetComponentInChildren<TMPro.TMP_Text>().SetText(e.Message);

            StopAllCoroutines();
            StartCoroutine(HideBubble(e.Index.Global));
        });
    }

    private IEnumerator HideBubble(int index)
    {
        yield return new WaitForSeconds(_messageTime);
        _messageBubbles[index].SetActive(false);
    }

    protected override void Action(GameObject player, RectTransform t)
    {
        if (CameraController.Instance)
            t.anchoredPosition = (CameraController.Instance.Cam.WorldToScreenPoint(player.transform.position + (Vector3)_offset) - (new Vector3(Screen.width, Screen.height) / 2)) / _canvas.scaleFactor;
    }

    protected override RectTransform GetT(QuantumGame game, PlayerInfoCallbackContext ctx)
    {
        return _messageBubbles[ctx.Index.Global].GetComponent<RectTransform>();
    }

    protected override void CleanUp(RectTransform t)
    {
        t.gameObject.SetActive(false);
    }
}
