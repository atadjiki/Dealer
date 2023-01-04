using System.Collections.Generic;
using Constants;

[System.Serializable]
public class CharacterSpawnData
{
    public Enumerations.CharacterModelID ModelID;
    public Enumerations.Team Team;
    public bool ShowNavDecals = true;
    public bool ShowModelDecal = false;
    public bool ShowCanvas = false;
}
