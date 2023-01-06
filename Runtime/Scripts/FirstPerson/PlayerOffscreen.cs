using UnityEngine;

namespace InexperiencedDeveloper.Core.FirstPerson
{
    [RequireComponent(typeof(Player))]
    public class PlayerOffscreen : MonoBehaviour
    {
        [SerializeField] private Camera m_Camera;
        private Player m_Player;
        private Player m_LocalPlayer;
        public Player OffscreenPlayer => m_LocalPlayer;
        public Camera Camera => m_Camera;

        public void Init(Player player)
        {
            m_Player = player;
            m_LocalPlayer = GetComponent<Player>();
            m_LocalPlayer.Init(m_Player);
        }
    }
}

