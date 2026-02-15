using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace API.Data;

public class MemberRepository(AppDbContext context) : IMemberRepository
{
    public async Task<Members?> GetMemberForUpdate(string id)
    {
        return await context.Members
            .Include(x => x.User)
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(x => x.Id == id); 
    }

   
    public async Task<PaginatedResult<Members>> GetMembersAsync(MemberParms memberParms)
    {
        var query = context.Members.AsQueryable();

        query = query.Where(x => x.Id != memberParms.CurrentMemberId);

        if (memberParms.Gender != null)
        {
            query = query.Where(x => x.Gender == memberParms.Gender);
        }
        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-memberParms.MaxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-memberParms.MinAge));

        query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

        query = memberParms.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.Created),
            _=> query.OrderByDescending(x => x.LastActive)
        };  

    return await paginationHelper.CreateAsync(query, memberParms.PageNumber, memberParms.PageSize);
    }

    public async Task<Members?> GetMembersByIdAsync(string id)
    {

        return await context.Members
            .Include(x => x.User)
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<Photo>> GetPhotosForMembersAsync(string memberId)
    {
        return await context.Members
            .Where(x => x.Id == memberId)
            .SelectMany(x => x.Photos)
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Update(Members member)
    {
        context.Entry(member).State = EntityState.Modified; 
    }
}