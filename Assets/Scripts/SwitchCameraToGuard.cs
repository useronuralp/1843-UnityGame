using UnityEngine;

public class SwitchCameraToGuard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Get().SwitchCameras(GameManager.Get().GetCamera("Player"), GameManager.Get().GetCamera("Guard1"));
        EventManager.Get().DisableKeyboardInputs();
        EventManager.Get().DisableMouseInputs();
        EventManager.Get().ForceTurnOffCandle();
        Destroy(gameObject);
    }
}
