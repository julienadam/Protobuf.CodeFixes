using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace CodeFixes.Samples
{
    [ProtoInclude(1, typeof(DuplicateProtoIncludeTagsFoo))]
    [ProtoInclude(1, typeof(DuplicateProtoIncludeTagsBar))]
    class DuplicateProtoIncludeTags
    {
    }

    class DuplicateProtoIncludeTagsFoo : DuplicateProtoIncludeTags { }
    class DuplicateProtoIncludeTagsBar : DuplicateProtoIncludeTags { }
}
