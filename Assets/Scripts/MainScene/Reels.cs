using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Reels : MonoBehaviour
{
    [HideInInspector]
    public ReelManager reelManager;

    public List<SymbolHolderHandler> symbolsHolders = new List<SymbolHolderHandler>();

    [Header("Spin Animation")]
    [SerializeField] private RectTransform reelRoot;
    [SerializeField] private float spinDistance = 600f;
    [SerializeField] private float spinLoopDuration = 1f;
    [SerializeField] private float stopDuration = 0.25f;

    private Tween spinTween;
    private bool isSpinning;

    public void ReelInit()
    {
        reelRoot.anchoredPosition = Vector2.zero;
    }

    public void SetScreenSymbols(List<int> symbols)
    {
        for (int i = 0; i < symbolsHolders.Count; i++)
        {
            SymbolHandler symbol = reelManager.symbolsManager.GetSymbolByID(symbols[i]);
            symbolsHolders[i].SetSymbol(symbol);
        }
    }

    public void StartSpin()
    {
        /*for (int i = 0; i < symbolsHolders.Count; i++)
        {
            symbolsHolders[i].ClearSymbol();
        }*/

        StartLoopSpin();
    }

    private void StartLoopSpin()
    {
        isSpinning = true;
        reelRoot.anchoredPosition = Vector2.zero;

        spinTween?.Kill();
        reelRoot
        .DOAnchorPosY(-spinDistance, stopDuration)
        .SetEase(Ease.Linear);
    }

    public void StopSpinAndSetResult(List<int> resultSymbols , bool isSpinEnded)
    {
        if (!isSpinning)
            return;

        isSpinning = false;
        spinTween?.Kill();

        float speed = spinDistance / spinLoopDuration;
        float stopDurationCalculated = spinDistance / speed;

        float slowStopDuration = spinLoopDuration * 1.8f;

        reelRoot
            .DOAnchorPosY(-spinDistance, slowStopDuration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                reelRoot.anchoredPosition = Vector2.zero;
                reelRoot.DOPunchAnchorPos(new Vector2(0, -8f), 0.06f);
                SetScreenSymbols(resultSymbols);
                AudioManager.Instance?.PlaySpinStop();
                if (isSpinEnded)
                {
                    GameManager.Instance.OnSpinEnded?.Invoke();
                }
            });
    }

    public void ClearSymbols()
    {
        for (int i = 0; i < symbolsHolders.Count; i++)
        {
            symbolsHolders[i].ClearSymbol();
        }
    }
}
