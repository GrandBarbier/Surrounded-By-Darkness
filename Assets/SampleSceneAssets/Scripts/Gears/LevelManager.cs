using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager
{
   public static IEnumerator FadeDuration(Color start, Color end, float duration)
   {
      Gears.gears.blackPanel.gameObject.SetActive(true);
      Gears.gears.blackPanel.color = start;
      
      for (float t = 0f; t < duration; t += Time.deltaTime) 
      {
         float normalizedTime = t/duration;
         Gears.gears.blackPanel.color = Color.Lerp(start, end, normalizedTime);
         yield return null;
      }
      
      Gears.gears.blackPanel.color = end;
   }

   public static void LoadScene(int index)
   {
      SceneManager.LoadScene(index);
      Gears.gears.StartCoroutine(FadeDuration(start: new Color(r: 0f, g: 0f, b: 0f, a: 1f), end: new Color(r: 0f, g: 0f, b: 0f, a: 0f), duration: 2f));
   }
}
