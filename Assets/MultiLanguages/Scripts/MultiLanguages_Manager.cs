using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;


namespace UniTech.Language
{

    /// <summary>
    /// Quản lý các ngôn ngữ
    /// Hiển thị ngôn ngữ lên thanh chọn
    /// Tạo sự kiện thay đổi ngôn ngữ
    /// </summary>
    public class MultiLanguages_Manager : MonoBehaviour
    {
        public Languages CurrentLanguage = Languages.Vietnamese;

        public static Action<Languages> OnLanguageChanged;

        [SerializeField] TMP_Dropdown dropdown;

        [SerializeField] static LanguagesData[] data;

        // Start is called before the first frame update
        void Start()
        {
            InitListLanguages();
            ReadData();
            Invoke(nameof(ChangeLanguages), 1);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                ChangeLanguages(CurrentLanguage);
            }
        }

        /// <summary>
        /// Lấy dữ liệu theo key. Như vậy thì key này phải là duy nhất trong bảng
        /// Nếu ko duy nhât thì sẽ lấy cái đầu tiên
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        public static LanguagesData GetDataByKey(string _key)
        {
            LanguagesData d = null;
            try
            {
                if (data.Length > 0)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (data[i].Key == _key)
                        {
                            d = data[i];
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            

                return d;
        }

        /// <summary>
        /// Lấy ra từ theo đúng ngôn ngữ cần 
        /// </summary>
        /// <returns></returns>
        public static string GetText(string _key, Languages _language)
        {
            LanguagesData data = GetDataByKey(_key);
            if (data != null)
            {
                switch (_language)
                {
                    case Languages.Vietnamese:
                        return data.Vietnamese;
                    case Languages.English:
                        return data.English;
                    case Languages.Gemany:
                        return data.Germany;
                    case Languages.Chinese:
                        return data.Chinese;
                    default: return _key;
                }
            }
            //else
            //{
            //    Debug.Log("Data null");
            //}
            return _key;
        }
        /// <summary>
        /// Update các ngôn ngữ lên dropdown list
        /// Đăng ký sự kiện khi thay đổi lựa chọn ngôn ngữ
        /// </summary>
        private void InitListLanguages()
        {
            if (dropdown != null)
            {
                dropdown.options.Clear();
                foreach (var language in Enum.GetValues(typeof(Languages)))
                {
                    dropdown.options.Add(new TMP_Dropdown.OptionData() { text = language.ToString() });
                }

                dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            }
        }

        private void OnDropdownValueChanged(int i)
        {
            if (Enum.TryParse<Languages>(dropdown.options[i].text, out Languages language))
            {
                ChangeLanguages(language);
            }
        }

        private void ReadData()
        {
            try
            {
                string path = Path.Combine(Application.dataPath, "MultiLanguages/Language.csv");
                if (File.Exists(path))
                {
                    string text = File.ReadAllText(path);
                    data = CSVSerializer.Deserialize<LanguagesData>(text);
                    Debug.Log("Data count = " + data.Length);
                }
                else
                {
                    Debug.LogError("File not exist! " + path);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }


        private void ChangeLanguages(Languages languages = Languages.Vietnamese)
        {
            CurrentLanguage = languages;
            OnLanguageChanged?.Invoke(CurrentLanguage);
        }
    }

    public enum Languages
    {
        Vietnamese,
        English,
        Gemany,
        Chinese
    }

    [Serializable]
    public class LanguagesData
    {
        public int ID;
        public string Key;
        public string Vietnamese;
        public string English;
        public string Germany;
        public string Chinese;
    }
}