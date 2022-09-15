using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace NewSuperMarioBrosSaveEditor
{
	class World
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
			World world = new World();

			world.id = (int)j["id"];
			world.nodes = new List<OverworldNode>();
			foreach (JToken n in j["nodes"])
				world.nodes.Add((OverworldNode)n);
			world.paths = new List<OverworldPath>();
			foreach (JToken p in j["paths"])
				world.paths.Add((OverworldPath)p);
			world.cannonDestination = (int)j["cannonDestination"];
			world.normalNextWorld = (int)j["normalNextWorld"];
			world.secretNextWorld = (int)j["secretNextWorld"];

			return world;
		}
	}
}
