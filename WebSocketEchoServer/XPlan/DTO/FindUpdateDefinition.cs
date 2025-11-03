using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.DTO
{
    public class FindUpdateDefinition<TDocument>
    {
        public Func<FilterDefinitionBuilder<TDocument>, FilterDefinition<TDocument>> Filter { get; set; }
        public Func<UpdateDefinitionBuilder<TDocument>, UpdateDefinition<TDocument>> Update { get; set; }
        public bool ReturnAfter { get; set; } = true;
    }

}
