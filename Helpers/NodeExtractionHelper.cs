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

		public static T GetParent<T>(Node instance)
		{
			var iterations = 0;

			while (true && iterations < 10)
			{
				instance = instance.GetParent();

				if (instance is T correctParent)
				{
					return correctParent;
				}
				else if (instance == null)
				{
					throw new NullReferenceException("Parent could not be found");
				}

				iterations++;
			}

			throw new NullReferenceException("Parent could not be found");
		}
	}
}
