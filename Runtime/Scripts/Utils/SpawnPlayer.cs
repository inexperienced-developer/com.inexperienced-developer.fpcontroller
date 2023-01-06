using InexperiencedDeveloper.Core.FirstPerson;
using UnityEngine;

namespace InexperiencedDeveloper.Utils
{
    public class SpawnPlayer : MonoBehaviour
    {
        public GameObject PlayerPrefab;

        private void Awake()
        {
            Player player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
            player.Init();
        }
    }
}

