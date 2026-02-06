using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AssetsLoader : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image progressFill;
    [SerializeField] private TMP_Text progressText;

    [Header("Config")]
    [SerializeField] private string nextSceneName = "";

    [Header("Preload Addresses")]
    [SerializeField] private List<string> preloadAddresses;

    async void Start()
    {
        int loadedCount = 0;
        int assetCount = preloadAddresses.Count;

        foreach (string address in preloadAddresses)
        {
            AsyncOperationHandle handle = Addressables.LoadResourceLocationsAsync(address);
            await handle.Task;

            loadedCount++;
            UpdateUI((float)loadedCount / assetCount);
        }
        Debug.Log("All assets loaded. Transitioning to next scene...");
        SceneManager.LoadScene(nextSceneName);
    }

    private void UpdateUI(float progress)
    {
        progressFill.fillAmount = progress;
        progressText.text = $"{Mathf.RoundToInt(progress * 100f)}%";
    }
}
