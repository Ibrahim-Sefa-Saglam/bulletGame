using Systems.Management;
using UnityEngine;
using UnityEngine.Serialization;


public class LevelManager: MonoBehaviour, IDataOps
    {
        public static GameObject[] LevelPrefabs;
        public static GameObject CurrentLevelObject;
        public static Camera_scr CameraScript;
        public GameObject[] assingableLevelPrefabs;
        public GameObject mainCameraObject;
        private static int _currentLevel;
        

        void Start()
        {
              
            LoadData();
            _currentLevel =  PlayerPrefs.GetInt(GameManager.PlayerPrefsNames.Level.ToString(), 0);
            LevelPrefabs = assingableLevelPrefabs;
            CameraScript = mainCameraObject.GetComponent<Camera_scr>();
            GenerateCurrentLevel();
            
        }

        public static void LevelUp()
        {
            if(_currentLevel < LevelPrefabs.Length - 1)
            {
                _currentLevel++;
                
                IDataOps.UpdateLevelData(_currentLevel);
                
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
        public static void ResetToFirstLevel()
        {
            Destroy(CurrentLevelObject);
            
            _currentLevel = 0;
            PlayerPrefs.SetInt(GameManager.PlayerPrefsNames.Level.ToString(), _currentLevel);          
            PlayerPrefs.Save();
            
            GenerateCurrentLevel();

        }


        public void LoadData()
        {
            _currentLevel = IDataOps.CurrentGameData.LevelIndex;
        }
    }