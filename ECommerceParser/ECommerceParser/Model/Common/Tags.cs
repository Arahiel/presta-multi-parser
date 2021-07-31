using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Common
{
    public class Tags : List<Tag>
    {
        public Tags(IEnumerable<Tag> tags) : base(tags)
        {
        }

        public Tags(string tagsString) : this(tagsString.Split(',').Select(x => new Tag(x)))
        {
        }

        public override string ToString()
        {
            return string.Join(",", this.Select(x => x.Name));
        }
    }
}
