using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniTech.Language;
using UnityEngine;


namespace UniTech.Language
{

    /// <summary>
    /// Example how to use multi languages
    /// </summary>
    public class MultiLanguages_World : MonoBehaviour
    {
        public Languages UsedLanguage = Languages.Vietnamese;
        public string Key = "DefaultWord"; //chính là từ muốn ghi vào
        [SerializeField] TextMeshProUGUI textMeshProUGUI;

        private void Start()
        {
            MultiLanguages_Manager.OnLanguageChanged += OnLanguageChanged;  
        }

        private void OnLanguageChanged(Languages languages)
        {
            UsedLanguage = languages;
            if(textMeshProUGUI!= null) textMeshProUGUI.text = MultiLanguages_Manager.GetText(Key, languages);
        }
    }
}
