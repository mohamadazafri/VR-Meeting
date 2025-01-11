using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WhiteboardSave : MonoBehaviour
{
    public RenderTexture renderTexture; // Assign your RenderTexture here
    public string fileName = "Whiteboard_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Collider Tag: " + other.tag);
    //    if (other.CompareTag("Right Controller") || other.CompareTag("Right Controller"))
    //    {
    //        SaveRenderTexture();
    //    }
    //}
    public void SaveRenderTexture()
    {
        Debug.Log("Pressed" );
        // Ensure the RenderTexture is active
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        // Create a Texture2D to read pixels from the RenderTexture
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        // Restore the previously active RenderTexture
        RenderTexture.active = currentRT;

        // Encode the texture to PNG format
        byte[] imageData = texture.EncodeToPNG();

        // Create a file path to save the image
        //string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string folderPath = Application.dataPath; // This is the "Assets" folder
        string filePath = Path.Combine(folderPath + "/Whiteboard/Saved/", fileName);
        File.WriteAllBytes(filePath, imageData);

        Debug.Log("RenderTexture saved as PNG at: " + filePath);

        // Cleanup
        Destroy(texture);
    }
}
