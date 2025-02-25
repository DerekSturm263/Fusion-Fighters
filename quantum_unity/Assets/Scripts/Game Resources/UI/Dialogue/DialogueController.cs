using GameResources.UI.Dialogue;

public class DialogueController : SpawnableController<Dialogue>
{
    protected override bool TakeAwayFocus() => true;

    protected override void SetUp(Dialogue t)
    {
        _templateInstance.GetComponent<DialogueBoxInstance>().Setup(t);
    }
}
