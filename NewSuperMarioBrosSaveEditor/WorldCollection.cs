using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace NewSuperMarioBrosSaveEditor
{
	public class WorldCollection : IEnumerable<World>
	{
		private List<World> worlds = new List<World>();
		public World this[int index] => worlds[index];
		public int Count => worlds.Count;

		public IEnumerator<World> GetEnumerator() => worlds.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => worlds.GetEnumerator();

		public static explicit operator WorldCollection(JArray j)
		{
			WorldCollection wc = new WorldCollection();
			foreach (JToken w in j)
				wc.worlds.Add((World)w);

			return wc;
		}
	}
}
