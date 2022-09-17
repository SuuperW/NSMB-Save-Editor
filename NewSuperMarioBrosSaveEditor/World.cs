using System.Collections.Generic;

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
	}
}
