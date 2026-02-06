using UnityEngine;
using static AudioManager;

public static class SymbolAudioMapper
{
    public static SymbolAudioType GetAudioType(int symbolId)
    {
        if (symbolId == 2 || symbolId == 1)
            return SymbolAudioType.Wild;

        if (symbolId == 3)
            return SymbolAudioType.HighPay1;

        if (symbolId == 4)
            return SymbolAudioType.HighPay2;

        if (symbolId == 5)
            return SymbolAudioType.HighPay3;

        if (symbolId == 6)
            return SymbolAudioType.HighPay4;

        if (symbolId >= 7 && symbolId <= 11)
            return SymbolAudioType.LowPay;

        return SymbolAudioType.LowPay; // default
    }
}
