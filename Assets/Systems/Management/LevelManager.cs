using Systems.SaveSystem;
using UnityEngine;


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
            _currentLevel = GameSaveData.Instance.levelIndex;
            LevelPrefabs = assingableLevelPrefabs;
            CameraScript = mainCameraObject.GetComponent<Camera_scr>();
            GenerateCurrentLevel();
            
        }

        public static void LevelUp()
        {
            if(_currentLevel < LevelPrefabs.Length - 1)
            {
                _currentLevel++;

            }
            GenerateCurrentLevel();
        }
        public static void GenerateCurrentLevel()
        {
            if(CurrentLevelObject != null) Destroy(CurrentLevelObject);
            
            CurrentLevelObject = Instantiate(LevelPrefabs[_currentLevel] );
            GameSaveData.Instance.levelIndex = _currentLevel;

            Transform newGunTransform = CurrentLevelObject.transform.Find("gun");
            GameObject gun = newGunTransform.gameObject;
            
            CameraScript.AssignGunToFollow(gun);
        }
        public static void ResetToFirstLevel()
        {
            Destroy(CurrentLevelObject);
            
            _currentLevel = 0;
            GameSaveData.Instance.levelIndex = _currentLevel;
            GenerateCurrentLevel();

        }
        
    
    }