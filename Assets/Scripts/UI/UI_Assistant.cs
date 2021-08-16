/***
 * Main component for creating and controlling dialogue system.
 * Dialogue skipping and showing next phrase.
 ***/
using TMPro;
using UnityEngine;

public class UI_Assistant : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] CanvasSwitcher canvasSwitcher;
    [SerializeField] private string filePath;
    [SerializeField] char splitter;

    private string[] messageArray;
    private TextWriter.TextWriterSingle textWriterSingle;
    private AudioSource talkingAudioSource;
    private int messageIndex = 0;
    private string messageFilePath;

    public void SetMessageArray(string messageFilePath)
    {
        if (messageFilePath == this.messageFilePath) return;

        this.messageFilePath = messageFilePath;
        var textFile = Resources.Load<TextAsset>(messageFilePath);
        messageArray = textFile.text.Split(splitter);
    }

    public string GetMessageFilePath()
    {
        return messageFilePath;
    }

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
                playerScript.joystick.resetJoystickPosition();
                messageIndex = 0;
                GameTime.Instance.isPaused = false;
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
}
