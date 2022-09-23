using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace NewSuperMarioBrosSaveEditor
{
	public class WorldCollection
	{
		private List<World> worlds = new List<World>();
		public World this[int index] => worlds[index];
		public int Count => worlds.Count;

		public static explicit operator WorldCollection(JArray j)
		{
			WorldCollection wc = new WorldCollection();
			foreach (JToken w in j)
				wc.worlds.Add((World)w);

			return wc;
		}

		/// <summary>
		/// A normal exit is a flagpole with a black flag, or normal completion of a tower/castle.
		/// This method will return false if the level does not have a normal exit.
		/// </summary>
		public bool IsNormalExitComplete(SaveFile saveFile, int worldId, int nodeId)
		{
			// We will check if the paths that the level unlocks via a normal exit are unlocked.
			// Or, if it is the last level of the world, check if the world that it unlocks is unlocked.
			OverworldNode node = worlds[worldId].nodes[nodeId];
			if (!node.UnlocksALevel)
			{
				if (!node.isLastLevelInWorld)
					return false;

				if (node.isBowserCastle)
					return saveFile.IsNodeCompleted(worldId, nodeId);
				else
					return (saveFile.GetWorldFlags(worldId) & SaveFile.WorldFlags.CastleCompleted) != 0;
			}

			for (int i = 0; i < node.pathsByNormalExit.Count; i++)
			{
				if (!saveFile.IsPathUnlocked(worldId, node.pathsByNormalExit[i]))
					return false;
			}
			return true;
		}
		/// <summary>
		/// A secret exit is a flagpole with a red flag, or mini-Mario completion of castles that have a mini exit.
		/// This method will return false if the level does not have a secret exit.
		/// </summary>
		public bool IsSecretExitComplete(SaveFile saveFile, int worldId, int nodeId)
		{
			OverworldNode node = worlds[worldId].nodes[nodeId];
			if (!node.HasSecretPaths)
			{
				if (!node.isLastLevelInWorld)
					return false;
				
				return worlds[worldId].secretNextWorld != 0 &&
					(saveFile.GetWorldFlags(worldId) & SaveFile.WorldFlags.CastleCompleted) != 0;
			}

			for (int i = 0; i < node.pathsBySecretExit.Count; i++)
			{
				if (!saveFile.IsPathUnlocked(worldId, node.pathsBySecretExit[i]))
					return false;
			}
			return true;
		}

	}
}
