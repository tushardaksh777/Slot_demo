using UnityEngine;

public class SymbolHandler : MonoBehaviour
{
    public int symbolID;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject normal;
    public GameObject highlighted;

    public void SetHighlightSymbol()
    {
        normal.SetActive(false);
        highlighted.SetActive(true);
    }

    public void SetNormalSymbol()
    {
        normal.SetActive(true);
        highlighted.SetActive(false);
    }
    public void SetBlurredSymbol()
    {
        // show blurred version (spine blur anim or blur sprite)
        // Example:
        // spineGraphic.AnimationState.SetAnimation(0, "blur", true);
    }
}
