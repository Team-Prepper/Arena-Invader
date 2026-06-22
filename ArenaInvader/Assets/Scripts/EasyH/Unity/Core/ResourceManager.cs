using UnityEngine;

namespace EasyH.Unity
{

    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        public IResourceConnector ResourceConnector
            { get; private set; }

        protected override void OnCreate()
        {
            if (ResourceConnector != null)
            {
                return;
            }

            ResourcesResourceConnector connector =
                gameObject.GetComponent<ResourcesResourceConnector>();

            if (connector == null)
            {
                connector = gameObject.AddComponent<ResourcesResourceConnector>();
            }

            ResourceConnector = connector;
        }
    }

}
