using System;
using UnityEngine;

namespace ApeCharacter
{
    public class SystemsValidator
    {
        public void ValidateSystems(IApeCharacter apeCharacter, Transform characterTransform)
        {
            // Получаем все компоненты, реализующие IApeSystem
            var systemComponents = characterTransform.GetComponentsInChildren<IApeSystem>();

            foreach (var systemComponent in systemComponents)
            {
                Type systemType = systemComponent.GetType();

                // Проверяем, зарегистрирована ли система в контейнере
                bool isRegistered = apeCharacter.Systems.HasSystem(systemType);

                if (!isRegistered)
                {
                    Debug.LogWarning(
                        $"System of type {systemType.Name} is not registered in the character's systems container and will be destroyed.");
                    UnityEngine.Object.Destroy(systemComponent as MonoBehaviour);
                }
            }
        }
    }
}