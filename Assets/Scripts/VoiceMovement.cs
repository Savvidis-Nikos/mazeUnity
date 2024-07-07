using System;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceShooting : MonoBehaviour
{
    private DictationRecognizer m_DictationRecognizer;
    public PlayerMovement playerMovement; // Ensure PlayerMovement is properly assigned in the inspector
    bool isCanceled;

    void Start()
    {
        m_DictationRecognizer = new DictationRecognizer();
        GameObject playerMovement = GameObject.Find("Player"); // Find the PlayerMovement script
        BindResult();

        m_DictationRecognizer.DictationHypothesis += (text) =>
        {
            Debug.LogFormat("Dictation hypothesis: {0}", text);
        };

        m_DictationRecognizer.DictationComplete += (completionCause) =>
        {
            Debug.LogFormat("Dictation completed: {0}.", completionCause);
            if (completionCause != DictationCompletionCause.Complete)
            {
                isCanceled = true;
            }
        };

        m_DictationRecognizer.DictationError += (error, hresult) =>
        {
            Debug.LogFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        };

        m_DictationRecognizer.Start();
        isCanceled = false;
        Debug.Log("Dictation Recognizer started.");
    }

    void Update()
    {
        if (isCanceled)
        {
            BindResult();
            m_DictationRecognizer.Start();
            isCanceled = false;
        }
    }

    private void BindResult()
    {
        m_DictationRecognizer.DictationResult -= this.DictationResult;
        m_DictationRecognizer.DictationResult += this.DictationResult;
        Debug.Log("Bound DictationResult event.");
    }

    private void DictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.LogFormat("Dictation result: {0}", text);
        string keyWord = text.ToLower().Trim();
        if (keyWord == "get")
        {
            playerMovement.Shoot1();
            Debug.Log("shoot");
        }
        else
        {
            Debug.LogFormat("Unrecognized command: {0}", keyWord);
        }
    }

   
}
