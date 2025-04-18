namespace WobblyTower.Extensions.Preloader
{
    public class PreloadItem
    {
        public string ResourceConstantPath { get; set; }
        public string ResourceExtension { get; set; }

        public PreloadItem(string resourceConstantPath, string resourceExtension)
        {
            ResourceConstantPath = resourceConstantPath;
            ResourceExtension = resourceExtension;
        }
    }
}