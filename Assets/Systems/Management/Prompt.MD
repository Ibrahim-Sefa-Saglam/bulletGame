in the game I am making I want to implement a data saving an loading logic,
I want there to be a DataManager static class to do the saving into a file and loading from one
the saved and loaded data will be an object with type "GameData"
this is the content of the GameData:

    public BouncerData (int bouncerNumber, Transform bouncerTransform)
    {
            _bouncerNumber = bouncerNumber;
            BouncerCount++;
            _bouncerTransform = bouncerTransform;
    }



    public class GameData
    {
    public List<BouncerData> BouncerData;
    public int PlayerCoinCount;
    public int LevelIndex;
    
}
w

I want DataManager to have a static GameData object and SaveGameData, LoadGameData methods 

these methods can be empty for now. 

I want an interfaced named "IDataOps" that will be given to the files with data to save and load
this interface will play the middle ground between the GameData object of DataManager and the files it is given to

So far I can think of this

    public interface IDataOps
    {

        private static GameData GameData { get; set; } = DataManager.GameData;

        public static void SaveData()
        {
            
        }
        public static void LoadData(){}
    }

I want the SaveData method to take a parameter