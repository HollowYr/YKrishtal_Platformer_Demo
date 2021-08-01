/***
 * Main component for creating and controlling dialogue system.
 * Dialogue skipping and showing next phrase.
 ***/
using TMPro;
using UnityEngine;

public class UI_Assistant : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] CanvasSwitcher canvasSwitcher;
    [SerializeField] private string filePath;
    [SerializeField] char splitter;

    private string[] messageArray;
    private TextWriter.TextWriterSingle textWriterSingle;
    private AudioSource talkingAudioSource;
    private int messageIndex = 0;

    public void showText()
    {
        if (textWriterSingle != null && textWriterSingle.IsActive())
        {
            // Currently active TextWriter
            textWriterSingle.WriteAllAndDestroy();
        }
        else
        {
            if (messageIndex > messageArray.Length - 1)
            {
                canvasSwitcher.DisableActiveAndActivateUIByName("Game_UI");
                messageIndex = 0;
            }
            else
            {
                string message = messageArray[messageIndex];
                StartTalkingSound();
                textWriterSingle = TextWriter.AddWriter_Static(messageText, message, .02f, true, true, StopTalkingSound);
                messageIndex++;
            }

        }
    }

    private void StartTalkingSound()
    {
        //talkingAudioSource.Play();
    }

    private void StopTalkingSound()
    {
        //talkingAudioSource.Stop();
    }

    private void Awake()
    {
        messageArray = GetComponent<textFromFile>().GetTextFromFile(filePath).Split(splitter);
    }
}
