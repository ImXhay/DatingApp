using System;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IMemberRepository
{
    void Update(Members member);
    Task<bool> SaveAllAsync();
    Task<PaginatedResult<Members>>GetMembersAsync(MemberParms memberParms);
    Task<Members?>GetMembersByIdAsync(string id);
    Task<IReadOnlyList<Photo>>GetPhotosForMembersAsync(string memberId);
    Task<Members?> GetMemberForUpdate(string id);
}
