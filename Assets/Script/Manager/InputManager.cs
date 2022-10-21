using UnityEngine;

namespace ChessVR
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager instance = null;
        #region  InputAction
        [SerializeField] private VR playerInput = null;

        #endregion
        #region Get
        public static VR Vr
        {
            get
            {
                return instance.playerInput;
            }
        }
        #endregion

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            playerInput = new VR();
            playerInput.Enable();
        }

        private void OnEnable()
        {
            playerInput.Enable();
        }
        private void OnDisable() { }
    }
}