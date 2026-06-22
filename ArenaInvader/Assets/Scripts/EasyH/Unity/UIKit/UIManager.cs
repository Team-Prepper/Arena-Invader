using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyH.Unity.UI
{

    public class UIManager : Singleton<UIManager>
    {
        public IGUIFullScreen NowDisplay { get; private set; }

        private IDictionary<string, string> _dic;
        private IQueue<IGUIFullScreen> uiStack;

        private GUIMessageBox _msgBox;

        public void OpenFullScreen(IGUIFullScreen newData)
        {
            if (newData == null) return;

            if (NowDisplay != null)
            {
                uiStack.Enqueue(NowDisplay);
            }

            uiStack.Enqueue(newData);

            IGUIFullScreen tmp = uiStack.Dequeue();

            if (tmp == NowDisplay)
            {
                newData?.SetOff();
                return;
            }

            NowDisplay?.SetOff();
            NowDisplay = newData;
            NowDisplay.SetOn();

        }

        public void CloseFullScreen(IGUIFullScreen closeFullScreen)
        {
            if (closeFullScreen == null) return;

            if (NowDisplay != closeFullScreen)
            {
                uiStack.Remove(closeFullScreen);
                return;
            }

            if (uiStack.Count < 1)
                return;

            NowDisplay = uiStack.Dequeue();
            NowDisplay.SetOn();

        }

        protected override void OnCreate()
        {
            NowDisplay = null;
            uiStack = new StablePriorityQueue<IGUIFullScreen>(
                (a, b) =>
                { 
                    return a.Priority.CompareTo(b.Priority);
                }
            );

            IDictionaryConnector<string, string> connector =
                new JsonDictionaryConnector<string, string>();
            ///new XMLDictionaryReader<string, string>();

            _dic = connector.ReadData("GUIInfor");

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            NowDisplay = null;
            uiStack = new StablePriorityQueue<IGUIFullScreen>();

        }

        public T OpenGUI<T>(string guiName, Action callback = null) where T : Component, IGUI
        {
            if (_dic == null)
            {
                throw new InvalidOperationException(
                    "UIManager is not initialized. Dictionary data is missing.");
            }

            if (!_dic.TryGetValue(guiName, out string path) || string.IsNullOrWhiteSpace(path))
            {
                throw new KeyNotFoundException(
                    $"GUI key '{guiName}' was not found in UI dictionary.");
            }

            IResourceConnector resourceConnector = ResourceManager.Instance?.ResourceConnector;
            if (resourceConnector == null)
            {
                throw new InvalidOperationException(
                    "ResourceConnector is not available.");
            }

            GameObject retGO = resourceConnector.ImportGameObject(path);
            if (retGO == null)
            {
                throw new InvalidOperationException(
                    $"Failed to load GUI prefab at path '{path}'.");
            }

            IGUI gui = retGO.GetComponent<IGUI>();
            if (gui == null)
            {
                throw new InvalidOperationException(
                    $"Loaded GUI prefab '{path}' does not implement IGUI.");
            }

            gui.Open(callback);

            T result = retGO.GetComponent<T>();
            if (result == null)
            {
                throw new InvalidOperationException(
                    $"Loaded GUI prefab '{path}' does not have component {typeof(T).Name}.");
            }

            return result;
        }

        public void DisplayMessage(string messageContent)
        {
            if (_msgBox == null)
            {
                _msgBox = OpenGUI<GUIMessageBox>("MessageBox");
            }

            _msgBox.SetMessage(messageContent);
        }

    }
}
