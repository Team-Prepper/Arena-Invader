using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EHTool.UIKit {

    public class UIManager : Singleton<UIManager> {
        public IGUIFullScreen NowDisplay { get; private set; }

        IDictionary<string, string> _dic;
        IList<IGUIFullScreen> uiStack;

        public void OpenFullScreen(IGUIFullScreen newData)
        {
            if (NowDisplay != null)
            {
                NowDisplay.SetOff();
            }

            uiStack.Add(newData);

            NowDisplay = newData;
            NowDisplay.SetOn();

        }

        public void CloseFullScreen(IGUIFullScreen closeFullScreen)
        {
            if (uiStack.Count < 1)
                return;

            uiStack.Remove(closeFullScreen);

            if (NowDisplay == closeFullScreen)
            {
                NowDisplay = uiStack[uiStack.Count - 1];
                NowDisplay.SetOn();
            }

        }

        protected override void OnCreate()
        {
            NowDisplay = null;
            uiStack = new List<IGUIFullScreen>();

            IDictionaryConnector<string, string> connector =
                //new JsonLangPackReader<string, string>();
                new XMLDictionaryReader<string, string>();

            _dic = connector.ReadData("GUIInfor");

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            NowDisplay = null;
            uiStack = new List<IGUIFullScreen>();

        }

        public T OpenGUI<T>(string guiName, CallbackMethod callback = null) where T : Component, IGUI
        {
            string path = Instance._dic[guiName];

            GameObject retGO = AssetOpener.ImportGameObject(path);
            retGO.GetComponent<IGUI>().Open(callback);

            return retGO.GetComponent<T>();
        }

    }
}
