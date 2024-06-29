using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YagizEraslan.Duality
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    // Check if an instance of T already exists in the scene
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        // Create a new GameObject and attach the singleton component
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        instance = singletonObject.AddComponent<T>();

                        // Ensure the singleton GameObject persists between scenes
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return instance;
            }
        }

        protected virtual void Awake()
        {
            // Ensure only one instance of the singleton exists
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
