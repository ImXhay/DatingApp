using System;

namespace API.Helpers;

public class LikeParams : PagingParms
{
    public string MemberId { get; set; } = "";
    public string Predicate { get; set; } = "liked";
}
