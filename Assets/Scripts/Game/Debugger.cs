using UnityEngine;

namespace Game
{
    public class Debugger : MonoBehaviour
    {
        [SerializeField] private bool _enabled = true;
        // [Inject] private ILevelManager _levelManager;

        private void Start()
        {
            // _levelManager.LoadCurrent();
        }

        private void Update()
        {
            if (!_enabled)
                return;
            if (Input.GetKeyDown(KeyCode.R))
            {

            }

            if (Input.GetKeyDown(KeyCode.W))
            {
     
            }
        
            if (Input.GetKeyDown(KeyCode.L))
            {
             
            }
        }
    }
}
