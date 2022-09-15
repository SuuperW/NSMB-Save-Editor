using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace NewSuperMarioBrosSaveEditor
{
	class OverworldNode
	{
		public int worldId;
		public int idInWorld;
		public List<Connection> connections;
		public class Connection
		{
			public int destinationNodeId;
			public int pathIdInWorld;
			public string direction;
			public bool isBackwards;

			public Connection() { }

			public static explicit operator Connection(JToken j)
			{
				Connection c = new Connection
				{
					destinationNodeId = (int)j["destinationNodeId"],
					pathIdInWorld = (int)j["pathIdInWorld"],
					direction = (string)j["direction"],
					isBackwards = (bool)j["isBackwards"]
				};

				return c;
			}
		}
		public int areaId;
		public string name;
		public bool hasStarCoins;
		public bool isFirstTower;
		public bool isCastle;
		public bool isVisible;
		public bool isLastLevelInWorld;
		public bool isSecondTower;
		public bool isBowserCastle;
		public bool is8DashCastle;
		public List<int> pathsByNormalExit;
		public List<int> pathsBySecretExit;
		public int zoomToNormalExit;
		public int zoomToSecretExit;
		public List<int> location;

		private OverworldNode() { }

		public static explicit operator OverworldNode(JToken j)
		{
			OverworldNode node = new OverworldNode
			{
				worldId = (int)j["worldId"],
				idInWorld = (int)j["idInWorld"],
				areaId = (int)j["areaId"],
				name = (string)j["name"],
				connections = new List<Connection>(),
				hasStarCoins = (bool)j["hasStarCoins"],
				isFirstTower = (bool)j["isFirstTower"],
				isCastle = (bool)j["isCastle"],
				isVisible = (bool)j["isVisible"],
				isLastLevelInWorld = (bool)j["isLastLevelInWorld"],
				isSecondTower = (bool)j["isSecondTower"],
				isBowserCastle = (bool)j["isBowserCastle"],
				is8DashCastle = (bool)j["is8DashCastle"],
				pathsByNormalExit = new List<int>(),
				pathsBySecretExit = new List<int>(),
				zoomToNormalExit = (int)j["zoomToNormalExit"],
				zoomToSecretExit = (int)j["zoomToSecretExit"],
				location = new List<int>()
			};
			foreach (JToken c in j["connections"])
				node.connections.Add((Connection)c);
			foreach (JToken p in j["pathsByNormalExit"])
				node.pathsByNormalExit.Add((int)p);
			foreach (JToken p in j["pathsBySecretExit"])
				node.pathsBySecretExit.Add((int)p);
			foreach (JToken l in j["location"])
				node.location.Add((int)l);

			return node;
		}
	}
}
