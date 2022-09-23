﻿using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Linq;

namespace NewSuperMarioBrosSaveEditor
{
	public class World
	{
		public int id;
		public List<OverworldNode> nodes;
		public List<OverworldPath> paths;
		public int cannonDestination;
		public int normalNextWorld;
		public int secretNextWorld;

		private World() { }

		public static explicit operator World(JToken j)
		{
			World world = new World
			{
				id = (int)j["id"],
				nodes = new List<OverworldNode>(),
				paths = new List<OverworldPath>(),
				cannonDestination = (int)j["cannonDestination"],
				normalNextWorld = (int)j["normalNextWorld"],
				secretNextWorld = (int)j["secretNextWorld"]
			};
			foreach (JToken n in j["nodes"])
				world.nodes.Add((OverworldNode)n);
			foreach (JToken p in j["paths"])
				world.paths.Add((OverworldPath)p);

			return world;
		}


		public bool IsUnlocked(SaveFile saveFile)
		{
			return (saveFile.GetWorldFlags(id) & SaveFile.WorldFlags.Unlocked) != 0;
		}

		public bool NodeHasSecretExit(OverworldNode n)
		{
			bool secretPath = !n.pathsByNormalExit.SequenceEqual(n.pathsBySecretExit);
			if (secretPath)
				return true;
			else
				return n.isLastLevelInWorld && secretNextWorld != 0;
		}
	}
}
