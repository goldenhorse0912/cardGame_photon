using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ShareHandler : MonoBehaviour
{
    // Start is called before the first frame update
	
	public void ShareButton()
    {
		StartCoroutine(TakeScreenshotAndShare());
    }

	private IEnumerator TakeScreenshotAndShare()
	{
		yield return new WaitForEndOfFrame();

		Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
		File.WriteAllBytes(filePath, ss.EncodeToPNG());

		// To avoid memory leaks
		Destroy(ss);

		new NativeShare().AddFile(filePath)
			.SetSubject("Table Top Cribbage").SetText("Let's Play Together Cribbage").SetCallback(
			(result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget)).Share();

	}

}
