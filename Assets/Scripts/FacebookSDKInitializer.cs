using Facebook.Unity;
using UnityEngine;

public class FacebookSDKInitializer : MonoBehaviour
{
    void Awake()
    {
        if (!FB.IsInitialized)
            FB.Init(InitCallback, OnHideUnity);
        else
            FB.ActivateApp();
        
        DontDestroyOnLoad(gameObject);
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
            FB.ActivateApp();
    }

    private void OnHideUnity(bool isGameShown) { }
}