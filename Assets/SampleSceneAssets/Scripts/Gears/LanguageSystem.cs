using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LanguageSystem
{
      public static TextAsset textAssetData;
  
      public static string[] data;
      
      public enum Languages {English, French, Spanish }

      public static string[] outputData;

      public static Action setLanguageEvent;
  
      public static void Init()
      {
          textAssetData = Resources.Load<TextAsset>("LanguageBoard");
          
          data = textAssetData.text.Split(new [] {";", "\n"}, StringSplitOptions.None);

          SetLanguage((Languages)Enum.Parse(typeof(Languages), PlayerPrefs.GetString("LanguagePref", "English")));
      }
  
      public static void SetLanguage(Languages language)
      {
          int languageId = 2;
          
          switch (language)
          {
              case Languages.French :
                  languageId = 1;
                  break;
              case Languages.English :
                  languageId = 2;
                  break;
              case Languages.Spanish :
                  languageId = 3;
                  break;
          }

          int nbrOfWord = 4;
          
          outputData = new string[nbrOfWord];
          
          for (int i = 0; i < nbrOfWord; i++)
          {
              //Debug.Log(data[languageId + 4 * i + 4]); //1 = french 2 = english, 4 * i (4 = length of the board, add 4 to get to the value below), + 4 = start at the scnd column
              outputData[i] = data[languageId + 4 * i + 4];
          }
          
          PlayerPrefs.SetString("LanguagePref", language.ToString());
          
          setLanguageEvent?.Invoke();
      }
}
