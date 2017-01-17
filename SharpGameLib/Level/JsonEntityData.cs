using SharpGameLib.Entities.Interfaces;
using Newtonsoft.Json;
using SharpGameLib.Entities;

namespace SharpGameLib.Level
{
    [JsonObject]
	public class JsonEntityData : EntityDataBase
    {
		public override IEntityData Clone()
        {
            return new JsonEntityData
            {
                Position = this.Position,
                Type = this.Type,
                Properties = this.Properties
            };
        }
    }
}
