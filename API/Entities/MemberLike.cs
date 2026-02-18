using System;

namespace API.Entities;

public class MemberLike
{
    public required string SourceMemberId { get; set; }
    public Members SourceMember { get; set; } = null!;
    public required string TargetMemberId { get; set; }
    public Members TargetMember { get; set; } = null!;
}
