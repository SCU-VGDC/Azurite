using System.Linq;

public class ScribePageStep : DialogStep
{
    public override bool TransitionAllowed
    {
        get
        {
            LostPages item = GameManager.Instance.Player.Inventory.Items.FirstOrDefault(i => i is LostPages) as LostPages;
            if (item == null)
                return false;

            return item.AllCorrect;
        }
    }
}
