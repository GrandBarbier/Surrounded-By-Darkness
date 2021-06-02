using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager
{
   public static Action preLoadingScene;
   public static IEnumerator FadeDuration(Image image, Color start, Color end, float duration, bool setActiveFalse = true)
   {
      image.gameObject.SetActive(true);
      image.color = start;
      
      for (float t = 0f; t < duration; t += Time.deltaTime) 
      {
         float normalizedTime = t/duration;

         if (image)
         {
            image.color = Color.Lerp(start, end, normalizedTime);
         }
         else
         {
            break;
         }
         
         yield return null;
      }

      if (image)
      {
         image.color = end;

         if (setActiveFalse)
         {
            image.gameObject.SetActive(false);
         }
      }
   }

   public static void LoadScene(int index)
   {
      preLoadingScene?.Invoke();
      preLoadingScene = null;
      Time.timeScale = 1f;
      SceneManager.LoadScene(index);
      Gears.gears.StartCoroutine(FadeDuration(Gears.gears.menuManager.blackPanel, start: new Color(r: 0f, g: 0f, b: 0f, a: 1f), end: new Color(r: 0f, g: 0f, b: 0f, a: 0f), duration: 2f));
   }
   
   public static IEnumerator LoadAsyncScene(int sceneIndex)
   {
      preLoadingScene?.Invoke();
      preLoadingScene = null;
      
      //Load new scene and display loading screen
      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
      
      Gears.gears.StartCoroutine(FadeDuration(Gears.gears.menuManager.blackPanel, start: new Color(r: 0f, g: 0f, b: 0f, a: 0f), 
         end: new Color(r: 0f, g: 0f, b: 0f, a: 1f), duration: 0.5f));
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
