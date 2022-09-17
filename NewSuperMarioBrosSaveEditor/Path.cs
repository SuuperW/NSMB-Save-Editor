using Newtonsoft.Json.Linq;

namespace NewSuperMarioBrosSaveEditor
{
	public class OverworldPath
	{
		public int worldId;
		public int idInWorld;
		public int animationId;
		public int cost;
		/// <summary>
		/// Note: This is not reliable. Some paths are unlocked by both, and path from 8-Castle is just wrong.
		/// </summary>
		public bool isUnlockedBySecretGoal;
		public bool isUnlockedBySign;
		public bool exists;
		public bool isInvalid;

		private OverworldPath() { }

		public static explicit operator OverworldPath(JToken j)
		{
			return new OverworldPath
			{
				worldId = (int)j["worldId"],
				idInWorld = (int)j["idInWorld"],
				animationId = (int)j["animationId"],
				cost = (int)j["cost"],
				isUnlockedBySecretGoal = (bool)j["isUnlockedBySecretGoal"],
				isUnlockedBySign = (bool)j["isUnlockedBySign"],
				exists = (bool)j["exists"],
				isInvalid = (bool)j["isInvalid"]
			};
		}
	}
}
