
using System;
using UnityEngine;

// TODO: After Debug -> Change To Static Class ? 
public class GlobalDataManager : MonoBehaviour {

    [field: SerializeField] public float PetStopDistance { get; private set; } = 10.0f;
    [field: SerializeField] public float EnemyBeAttackedCoolDownTime{ get; private set; } = 2.0f;

    public static GlobalDataManager Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}

