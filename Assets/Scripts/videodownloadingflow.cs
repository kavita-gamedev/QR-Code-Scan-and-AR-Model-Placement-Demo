using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class videodownloadingflow : MonoBehaviour
{
    bool progressDone;
    [Header("Panels")]
    public GameObject downloadingPanel;
    [Header("Sliders")]
    public Slider progressBar;
    public void onvaluechanged(int value)
    {
         foreach (Qrdetail obj in RestAPIModule.RestAPIManager.Instance.qr_list.Qrdetail_list)
         {
             if (obj.qr_id == value)
             {
                progressDone = false;
                string url = obj.video_link;
                string filename = null;
                filename = Path.GetFileName(url);
                if (File.Exists(Application.dataPath + "/Downloaded" + string.Format("/{0}.mp4", filename)))
                {
                    Debug.Log("Video Already Downloaded");
                }
               else
               {
                StartCoroutine(testVideoDownload(url, filename));
                downloadingPanel.SetActive(true);
               }
            }
         }
    }
    IEnumerator testVideoDownload(string url, string filename)
    {
        var www = new WWW(url);
        StartCoroutine(ProgressBar(www));
        yield return www;
        print(Application.persistentDataPath);
        progressDone = true;
        File.WriteAllBytes(Application.dataPath + "/Downloaded" + string.Format("/{0}.mp4", filename), www.bytes);
        downloadingPanel.SetActive(false);
    }

    IEnumerator ProgressBar(WWW www)
    {
        while (!progressDone)
        {
            progressBar.value = www.progress;
            yield return null;
        }
        progressDone = false;
    }
}
