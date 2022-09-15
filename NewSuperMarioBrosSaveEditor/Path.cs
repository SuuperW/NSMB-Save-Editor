using Newtonsoft.Json.Linq;

namespace NewSuperMarioBrosSaveEditor
{
	class OverworldPath
	{
		public int worldId;
		public int idInWorld;
		public int animationId;
		public int cost;
		public bool isUnlockedBySecretGoal;
		public bool isUnlockedBySign;
		public bool exists;
		public bool isInvalid;

		private OverworldPath() { }

		public static explicit operator OverworldPath(JToken j)
		{
			OverworldPath path = new OverworldPath();

			path.worldId = (int)j["worldId"];
			path.idInWorld = (int)j["idInWorld"];
			path.animationId = (int)j["animationId"];
			path.cost = (int)j["cost"];
			path.isUnlockedBySecretGoal = (bool)j["isUnlockedBySecretGoal"];
			path.isUnlockedBySign = (bool)j["isUnlockedBySign"];
			path.exists = (bool)j["exists"];
			path.isInvalid = (bool)j["isInvalid"];

			return path;
		}
	}
}
