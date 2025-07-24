using Script.Enum;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{
    
    public static GameManager Instance;

    public int CurrentPetCount { get; private set; }

    public PlayerAttack Player{ get; private set; }
    private FollowCamera CameraFollow;
    private ShakeCamera CameraShake;

    public Enemy[] Enemies { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        Player = FindAnyObjectByType<PlayerAttack>();
        CameraFollow = FindObjectOfType<FollowCamera>();
        CameraShake = FindAnyObjectByType<ShakeCamera>();
        
        // TODO: AOE which Enemies
        Enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        
        if (CameraFollow) {
            SetCameraFollowTarget(Player.transform);
        }
        
        CurrentPetCount = FindObjectsOfType<Pet>().Length;
    }

    public void ShakeCamera(float duration, float magnitude, float late = 0.0f){
        if (!CameraShake) return;
        CameraShake.Shake(duration, magnitude, late);
    }

    public void SetCameraFollowTarget(Transform target) {
        CameraFollow.SetFollowTarget(target);
    }
    
    public void StartGame() {
        // TODO: Temp Start Scene
        SceneManager.LoadScene((int)SceneType.Level_2);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
