using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Serialize
{

    /// <summary>
    /// テーブルの管理クラス
    /// </summary>
    [System.Serializable]
    public class TableBase<TKey, TValue, Type> where Type : KeyAndValue<TKey, TValue>
    {
        [SerializeField]
        private List<Type> list;
        private Dictionary<TKey, TValue> table;


        public Dictionary<TKey, TValue> GetTable()
        {
            //if (table == null)
            //{
                table = ConvertListToDictionary(list);
            //}
            return table;
        }

        /// <summary>
        /// Editor Only
        /// </summary>
        public List<Type> GetList()
        {
            return list;
        }

        static Dictionary<TKey, TValue> ConvertListToDictionary(List<Type> list)
        {
            Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();
            foreach (KeyAndValue<TKey, TValue> pair in list)
            {
                dic.Add(pair.Lang, pair.Text);
            }
            return dic;
        }

#if UNITY_EDITOR
        /**
         * Inspector拡張クラス*/
         /*
        [CustomEditor(typeof(TableBase<SystemLanguage, string, LocalizePair>))]
        public class CharacterEditor : Editor
        {
            bool folding = false;

            public override void OnInspectorGUI()
            {
                TextLocalizer inspector = target as TextLocalizer;
                GUILayout.TextField("テストラベル");

            }
        }*/
#endif
    }

    /// <summary>
    /// シリアル化できる、KeyValuePair
    /// </summary>
    [System.Serializable]
    public class KeyAndValue<TKey, TValue>
    {
        [SerializeField]
        public TKey Lang;
        [SerializeField, Multiline]
        public TValue Text;

        public KeyAndValue(TKey key, TValue value)
        {
            Lang = key;
            Text = value;
        }
        public KeyAndValue(KeyValuePair<TKey, TValue> pair)
        {
            Lang = pair.Key;
            Text = pair.Value;
        }


    }
}
