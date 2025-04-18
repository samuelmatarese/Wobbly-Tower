using Godot;
using WobblyTower.Constants;
using WobblyTower.Extensions.Preloader;

public class MainResourcePreloader : ResourcePreloader
{
	public override void _Ready()
	{
		foreach (var preloadItem in Preloads.TowerBlockResources)
		{
			PreloadList(preloadItem);
		}
	}

	public void PreloadList(PreloadItem preloadItem)
	{
		var counter = 1;

		while (true)
		{
			var path = $"{preloadItem.ResourceConstantPath}_{counter}.{preloadItem.ResourceExtension}";

			if (!ResourceLoader.Exists(path))
			{
				break;
			}

			var resource = GD.Load(path);
			AddResource(path, resource);
			counter++;
		}
	}
}
