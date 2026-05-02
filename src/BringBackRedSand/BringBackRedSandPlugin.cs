using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BringBackRedSand;

[BepInAutoPlugin]
public partial class BringBackRedSandPlugin : BaseUnityPlugin
{
    private static readonly int TopColor2 = Shader.PropertyToID("_TopColor2");
    private static readonly int TopColor = Shader.PropertyToID("_TopColor");
    private static readonly int TopColor1 = Shader.PropertyToID("_TopColor1");
    private static readonly int TopHueShift = Shader.PropertyToID("_TopHueShifts");

    private static readonly Color BloodRedTopColor = new (0.6117647f, 0.05098037f, 0f, 1f);
    private static readonly Color BloodRedTop2Color = new (0.1137255f, 0.007843135f, 0.007843135f, 0.6156863f);
    private static readonly Color BloodRedTop1Color = new(0.2901961f, 0.03869281f, 0f, 0.4196078f);
    private static readonly Vector4 HueShift = new(0.2875f, 0f, 0f, 0f);
    
    private static readonly Color FoliageTint = new(1.14859354f, 0.611051798f, 0.611051798f, 0f);
    private static readonly Color FoliageBaseColor =
        new(0.0392156862745098f, 0.023529411764705882f, 0.01568627450980392f, 0f);
    private static readonly Color FoliageColor1 =
        new(0.3137254901960784f, 0.16470588235294117f, 0.08235294117647059f, 1f);

    private static readonly int Tint = Shader.PropertyToID("_Tint");
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    private static readonly int Color1 = Shader.PropertyToID("_Color1");
    internal static ManualLogSource Log { get; private set; } = null!;

    internal static Material RedRockMaterial { get; private set; } = null!;
    internal static Material RedFoliageMaterial { get; private set; } = null!;

    private void Awake()
    {
        Log = Logger;

        SceneManager.sceneLoaded += CheckMaterials;
        
        Log.LogInfo($"Plugin {Name} is loaded!");
    }

    private void CheckMaterials(Scene arg0, LoadSceneMode arg1)
    {
        if (!RedRockMaterial)
        {
            var mats = Resources.FindObjectsOfTypeAll<Material>();

            Log.LogInfo($"Found {mats.Length} materials...");

            foreach (var material in mats)
            {
                if (material.name is "M_RedRock")
                {
                    RedRockMaterial = material;
                }
                
                if (material.name is "M_Foliage Generic_Red")
                {
                    RedFoliageMaterial = material;
                }
                
                if(RedRockMaterial && RedFoliageMaterial)
                {
                    UpdateMaterialColors();
                    break;
                }
            }
        }
        
    }

    private void UpdateMaterialColors()
    {
        RedRockMaterial.SetColor(TopColor2, BloodRedTop2Color);
        RedRockMaterial.SetColor(TopColor, BloodRedTopColor);
        RedRockMaterial.SetColor(TopColor1, BloodRedTop1Color);
        RedRockMaterial.SetVector(TopHueShift, HueShift);
        
        RedFoliageMaterial.SetColor(Tint, FoliageTint);
        RedFoliageMaterial.SetColor(BaseColor, FoliageBaseColor);
        RedFoliageMaterial.SetColor(Color1, FoliageColor1);
    }
}