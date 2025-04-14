using WobblyTower.Extensions.Preloader;

namespace WobblyTower.Constants
{
    public static class Preloads
    {
        public static PreloadItem[] TowerBlockResources = {
            new PreloadItem("Assets/TowerMaterials/block_material", "material"),
            new PreloadItem("Assets/TowerShapes/tower_block", "glb")
        };
    }
}