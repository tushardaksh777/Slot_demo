using UnityEngine;

public class SymbolHolderHandler : MonoBehaviour
{
    public SymbolHandler symbolHandler = null;

    public void SetSymbol(SymbolHandler symbol)
    {
        symbolHandler = symbol;
        symbolHandler.SetNormalSymbol();
        symbolHandler.gameObject.SetActive(true);
        symbolHandler.transform.SetParent(transform);
        symbolHandler.transform.localPosition = Vector3.zero;
    }

    public void ClearSymbol()
    {
            if (symbolHandler != null)
            {
                SymbolsManager.instance.ReturnToPool(symbolHandler);
                symbolHandler = null;
            }
    }

    public void SetBlurredSymbol()
    {
        // show blurred version (spine blur anim or blur sprite)
        // Example:
        // spineGraphic.AnimationState.SetAnimation(0, "blur", true);
    }
}
