using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager
{
   public static IEnumerator FadeDuration(Image image, Color start, Color end, float duration, bool setActiveFalse = true)
   {
      image.gameObject.SetActive(true);
      image.color = start;
      
      for (float t = 0f; t < duration; t += Time.deltaTime) 
      {
         float normalizedTime = t/duration;
         image.color = Color.Lerp(start, end, normalizedTime);
         yield return null;
      }
      
      image.color = end;

      if (setActiveFalse)
      {
         image.gameObject.SetActive(false);
      }
   }

   public static void LoadScene(int index)
   {
      SceneManager.LoadScene(index);
      Gears.gears.StartCoroutine(FadeDuration(Gears.gears.menuManager.blackPanel, start: new Color(r: 0f, g: 0f, b: 0f, a: 1f), end: new Color(r: 0f, g: 0f, b: 0f, a: 0f), duration: 2f));
   }
   
   public static IEnumerator LoadAsyncScene(int sceneIndex)
   {
      // The Application loads the Scene in the background as the current Scene runs.
      // This is particularly good for creating loading screens.
      // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
      // a sceneBuildIndex of 1 as shown in Build Settings.

      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
      
      Gears.gears.StartCoroutine(FadeDuration(Gears.gears.menuManager.blackPanel, start: new Color(r: 0f, g: 0f, b: 0f, a: 1f), end: new Color(r: 0f, g: 0f, b: 0f, a: 0f), duration: 0.5f));
      Gears.gears.menuManager.loadScreen.SetActive(true);

      // Wait until the asynchronous scene fully loads
      while (!asyncLoad.isDone)
      {
         Gears.gears.menuManager.loadBarScaler.transform.localScale = new Vector3(asyncLoad.progress, Gears.gears.menuManager.loadBarScaler.transform.localScale.y, 
            Gears.gears.menuManager.loadBarScaler.transform.localScale.z);
         yield return null;
      }
   }
}
