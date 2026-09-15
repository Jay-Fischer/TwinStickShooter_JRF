using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class RumbleManager : MonoBehaviour
{
    public static RumbleManager Instance;

    [SerializeField] private Gamepad _gamepad;

    private void Awake()
    {
        if (Instance == null)                   
            Instance = this;
        
    }

    public void RumblePulse(float lowFreq, float highFreq, float duration)
    {
        _gamepad = Gamepad.current;
        if(_gamepad != null)
        {
            _gamepad.SetMotorSpeeds(lowFreq, highFreq);
            StartCoroutine(StopRumble(duration, _gamepad));
        }
    }

    IEnumerator StopRumble(float durration, Gamepad _gamepad)
    {

        
        float elapsedTime = 0;
        while(elapsedTime < durration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //yield return new WaitForSeconds(durration);
        _gamepad.SetMotorSpeeds(0, 0);
    }
}
