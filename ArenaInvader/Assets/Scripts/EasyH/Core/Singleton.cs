using UnityEngine;

namespace EasyH
{

    [ExecuteInEditMode]
    public class Singleton<T> where T : Singleton<T>, new()
    {
        static T _instance;
        static bool _isCreating;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (_isCreating)
                    {
                        throw new System.InvalidOperationException(
                            $"Singleton<{typeof(T).Name}> is already being created.");
                    }

                    _isCreating = true;
                    try
                    {
                        _instance = new T();
                        _instance.OnCreate();
                    }
                    finally
                    {
                        _isCreating = false;
                    }
                }

                return _instance;
            }
        }

        protected virtual void OnCreate()
        {

        }

    }
}
