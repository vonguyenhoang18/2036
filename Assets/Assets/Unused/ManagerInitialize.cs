using System;
using System.IO;
using System.Threading;
using DG.Tweening;
using UnityEngine;


public class ManagerInitialize : MonoBehaviour
{

    public static bool _waitForInitialize { get; set; }
    private float _lastSaveTime = 0f;
    private const float MIN_SAVE_INTERVAL = 3f; // Minimum time between saves in seconds
    private const float AUTO_SAVE_INTERVAL = 60f; // Auto save every 60 seconds (1 minute)
    private CancellationTokenSource _autoSaveCts;

    async void Start()
    {

        try
        {
            // Apply FPS setting immediately for preview
            Application.targetFrameRate = 60;

            _waitForInitialize = true;

            // Start the auto-save system
            _autoSaveCts = new CancellationTokenSource();
            // AutoSave().Forget();
        }
        catch (Exception e)
        {
            // Ở đây cần show 1 thông báo lỗi và load lại scene
            Debug.LogError($"[ManagerInitialize] {e}\n{e.StackTrace}");
        }

    }



    private void OnDestroy()
    {
        try
        {
            // Cancel auto-save task if it's running
            if (_autoSaveCts != null && !_autoSaveCts.IsCancellationRequested)
            {
                _autoSaveCts.Cancel();
                _autoSaveCts.Dispose();
                _autoSaveCts = null;
            }


            DOTween.Clear(true);
        }
        catch (Exception e)
        {
            Debug.LogError($"[OnDestroy] {e}\n{e.StackTrace}");
        }
    }


    private void OnApplicationPause(bool pauseStatus)
    {
        if (!_waitForInitialize)
            return;

        if (!pauseStatus)
        {
            SaveGame();
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }


    private void Save(Component sender, object param)
    {
        if (!_waitForInitialize)
            return;
        bool.TryParse(param?.ToString(), out var forceSave);
        SaveGame(forceSave);
    }

    private void SaveGame(bool forceSave = false)
    {
        try
        {
            // Check if enough time has passed since last save
            if (!forceSave && Time.realtimeSinceStartup - _lastSaveTime < MIN_SAVE_INTERVAL)
            {
                Debug.Log("Skipping save: too soon since last save");
                return;
            }


            _lastSaveTime = Time.realtimeSinceStartup;

        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveGame] {e}\n{e.StackTrace}");
        }
    }

}
