using Godot;
using System;

namespace WobblyTower.Helpers
{
	public static class NodeExtractionHelper
	{
		public static T GetChild<T>(Node instance)
		{
			var children = instance.GetChildren();

			foreach (var child in children)
			{
				if (child is T objectToSearch)
				{
					return objectToSearch;
				}
			}

			throw new NullReferenceException();
		}
	}
}