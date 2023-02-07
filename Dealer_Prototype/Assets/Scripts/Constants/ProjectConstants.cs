using UnityEngine;
using UnityEngine.SceneManagement;

namespace Constants
{
    public class Enumerations
    {
        public enum Team
        {
            None,
            Player,
            Enemy,
            Neutral,
        };

        public enum WeaponID
        {
            None,
            Glock,
        }

        public enum CharacterModelID
        {
            None,
            Model_Male1,
            Model_Male_Player,
            Model_Male_Police,
            Model_Male_Customer,
        }

        public enum CharacterID
        {
            None,
            Player,
            Stranger,
            Customer,
        }

        public enum CharacterClothingType
        {
            None,
            Top,
            Bottom,
            Shoes,
            Hat,
            Accessories,
        }

        public enum CommandType
        {
            None,
            Move,
            Interact,
            Approach,
            Pickup,
        }

        public enum SafehouseStation
        {
            None,
            Save,
            Phone,
            Stash,
            Door,
        }

        public enum MouseContext
        {
            None,
            Move,
            Interact,
            Approach,
            Pickup,
            Save,
            Phone,
            Stash,
            Door,
        }

        public enum MovementState
        {
            None,
            Moving,
            Stopped,
        }

        public enum DistrictName
        {
            None,
            Downtown, 
        };
        public enum Level 
        {
            None,
            Low, 
            Medium, 
            High, 
        };

        public enum InventoryID
        {
            None,
            DRUGS,
            MONEY
        }
    }

    public class NPC
    {
        public enum TaskID
        {
            None,
            GoToRandomLocation,
            PerformIdle
        };
        public enum TaskState
        {
            None,
            ToDo,
            InProgress,
            Complete
        };
    }
}