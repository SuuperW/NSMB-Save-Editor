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
				Connection c = new Connection();
				c.destinationNodeId = (int)j["destinationNodeId"];
				c.pathIdInWorld = (int)j["pathIdInWorld"];
				c.direction = (string)j["direction"];
				c.isBackwards = (bool)j["isBackwards"];

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
			OverworldNode node = new OverworldNode();
			node.worldId = (int)j["worldId"];
			node.idInWorld = (int)j["idInWorld"];
			node.areaId = (int)j["areaId"];
			node.name = (string)j["name"];
			node.connections = new List<Connection>();
			foreach (JToken c in j["connections"])
				node.connections.Add((Connection)c);
			node.hasStarCoins = (bool)j["hasStarCoins"];
			node.isFirstTower = (bool)j["isFirstTower"];
			node.isCastle = (bool)j["isCastle"];
			node.isVisible = (bool)j["isVisible"];
			node.isLastLevelInWorld = (bool)j["isLastLevelInWorld"];
			node.isSecondTower = (bool)j["isSecondTower"];
			node.isBowserCastle = (bool)j["isBowserCastle"];
			node.is8DashCastle = (bool)j["is8DashCastle"];
			node.pathsByNormalExit = new List<int>();
			foreach (JToken p in j["pathsByNormalExit"])
				node.pathsByNormalExit.Add((int)p);
			node.pathsBySecretExit = new List<int>();
			foreach (JToken p in j["pathsBySecretExit"])
				node.pathsBySecretExit.Add((int)p);
			node.zoomToNormalExit = (int)j["zoomToNormalExit"];
			node.zoomToSecretExit = (int)j["zoomToSecretExit"];
			node.location = new List<int>();
			foreach (JToken l in j["location"])
				node.location.Add((int)l);

			return node;
		}
	}
}
