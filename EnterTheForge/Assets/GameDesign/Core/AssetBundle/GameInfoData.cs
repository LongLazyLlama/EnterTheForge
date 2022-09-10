using UnityEngine;

namespace GameDesign.Core.AssetBundle
{
    [CreateAssetMenu(fileName = "Game Info Data", menuName = "GameDesign/AssetBundle/Game Info", order = 1)]
    public class GameInfoData : ScriptableObject
    {
        [Header("Information for in the Menu")]
        [Tooltip("Name of the game.")]
        public string NameGame = "Game's Name";
        [TextArea(3, 20)]
        public string Info = "High Concept / Pitch";
        public string Genre = "Genre";
        public string Rating = "Pegi 3";
        public string NameDeveloper = "Voornaam Achternaam";

        [Header("Visuals")]
        [Tooltip("Size: 3840px - 1646px, RGB, MipMaps == true, Texture Type: Sprite")]
        public Sprite BackgroundImage;
        public Color ThemeColor;

        //[Space]
        [Header("Project")]
        public string AssemblyDefinition = "GameDesign.Game1";
        [Tooltip("Name of the scene.")]
        public string NameScene = "SampleScene";
        [HideInInspector]
        public string NameAssetBundleGame = "";
        [HideInInspector]
        public TextAsset AssemblyTextAsset;

        [HideInInspector]
        public string LayerInfo;
    }

    [System.Serializable]
    public class LayerCollision
    {
        public bool[] LayerMatrix;
    }
}
