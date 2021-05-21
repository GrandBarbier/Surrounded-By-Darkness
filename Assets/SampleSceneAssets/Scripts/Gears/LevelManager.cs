using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelManager
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
}
