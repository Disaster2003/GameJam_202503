using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    private enum SOUND
    {
        BGM,
        SE,
    }
    [SerializeField] private SOUND sound;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        Slider slider = GetComponent<Slider>();

        switch (sound)
        {
            case SOUND.BGM:
                slider.value = GameManager.GetInstance.GetBGMVolume;
                slider.onValueChanged.AddListener((float value) =>
                {
                    GameManager.GetInstance.SetBGMVolume = value;
                });
                break;
            case SOUND.SE:
                slider.value = GameManager.GetInstance.GetSEVolume;
                slider.onValueChanged.AddListener((float value) =>
                {
                    GameManager.GetInstance.SetSEVolume = value;
                });
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
