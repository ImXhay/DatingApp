using System;

namespace API.Helpers;

public class MessageParams : PagingParms
{
    public string? MemberId { get; set; }
    public string Container { get; set; } = "Inbox";

}
