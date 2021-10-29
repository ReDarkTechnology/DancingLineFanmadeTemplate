using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public GameManager manager;
    public UnityEngine.Events.UnityEvent OnPlayerTap;
    public UnityEngine.Events.UnityEvent OnGameFinished;
    public UnityEngine.Events.UnityEvent OnGameStarted;
    public UnityEngine.Events.UnityEvent OnPlayerDies;
    public UnityEngine.Events.UnityEvent OnPlayerDiesInCheckpoint;
    public UnityEngine.Events.UnityEvent OnCheckpointReset;
    public UnityEngine.Events.UnityEvent OnCheckpointObtained;
    // Start is called before the first frame update
    void Start()
    {
        if (manager == null)
        {
            if (FindObjectOfType<GameManager>() != null)
            {
                manager = FindObjectOfType<GameManager>();
            }
            else
            {
                Debug.LogError("There's no GameManager found in the scene.");
            }
        }
        manager.OnPlayerTap += CallPlayerTap;
        manager.OnGameFinished += CallGameFinished;
        manager.OnGameStarted += CallGameStarted;
        manager.OnPlayerDiesInCheckpoint += CallPlayerDiesInCheckpoint;
        manager.OnPlayerDies += CallPlayerDies;
        manager.OnCheckpointReset += CallCheckpointReset;
        manager.OnCheckpointObtained += CallCheckpointObtained;
    }
    public void CallPlayerTap(int a, Vector3 e)
    {
        OnPlayerTap.Invoke();
    }
    public void CallGameFinished()
    {
        OnGameFinished.Invoke();
    }
    public void CallGameStarted()
    {
        OnGameStarted.Invoke();
    }
    public void CallPlayerDies()
    {
        OnPlayerDies.Invoke();
    }
    public void CallPlayerDiesInCheckpoint(int checkpointNumber)
    {
        OnPlayerDiesInCheckpoint.Invoke();
    }
    public void CallCheckpointReset(int checkpointNumber)
    {
        OnCheckpointReset.Invoke();
    }
    public void CallCheckpointObtained(int checkpointNumber){
    	OnCheckpointObtained.Invoke();
    }
}
