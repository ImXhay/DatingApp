using System;
using API.Entities;

namespace API.Interfaces;

public interface IMemberRepository
{
    void Update(Members member);
    Task<bool> SaveAllAsync();
    Task<IReadOnlyList<Members>>GetMembersAsyn();
    Task<Members?>GetMembersByIdAsync(string id);
    Task<IReadOnlyList<Photo>>GetPhotosForMembersAsync(string memberId);
}
