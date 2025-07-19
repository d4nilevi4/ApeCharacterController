using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ApeCharacter
{
    [CreateAssetMenu(menuName = "ApeCharacter/CharacterStaticData", fileName = "CharacterStaticData")]
    public class ApeCharacterSOProvider : ScriptableObject, ICharacterStaticDataProvider
    {
        [SerializeField] public List<ScriptableObject> Configs;
        
        public T GetConfig<T>()
        {
            return Configs.OfType<T>().FirstOrDefault();
        }
    }
}