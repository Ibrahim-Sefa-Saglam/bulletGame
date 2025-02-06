using UnityEngine;
using UnityEngine.Serialization;


public class LevelManager: MonoBehaviour
    {
        public static GameObject[] LevelPrefabs;
        public static GameObject CurrentLevelObject;
        public static Camera_scr CameraScript;
        public GameObject[] assingableLevelPrefabs;
        public GameObject mainCameraObject;
        private static int _currentLevel;

        void Start()
        {
            _currentLevel =  PlayerPrefs.GetInt(GameManager.PlayerPrefsNames.Level.ToString(), 0);
            LevelPrefabs = assingableLevelPrefabs;  
            CameraScript = mainCameraObject.GetComponent<Camera_scr>();
            GenerateCurrentLevel();
            
        }
        
        public static void GenerateNextLevel()
        {
            
            Destroy(CurrentLevelObject);
            
            if(_currentLevel < LevelPrefabs.Length - 1)
            {
                _currentLevel++;
                PlayerPrefs.SetInt(GameManager.PlayerPrefsNames.Level.ToString(), _currentLevel);          
                PlayerPrefs.Save();
            }
            
            GenerateCurrentLevel();
            
        }

        public static void GenerateCurrentLevel()
        {
            if(CurrentLevelObject != null) Destroy(CurrentLevelObject);
            CurrentLevelObject = Instantiate(LevelPrefabs[_currentLevel] );
            Transform newGunTransform = CurrentLevelObject.transform.Find("gun");
            GameObject gun = newGunTransform.gameObject;
            CameraScript.AssingGunToFollow(gun);
        }

    }
