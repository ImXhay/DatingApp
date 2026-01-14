using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
     [Authorize]
    public class MembersController(IMemberRepository memberRepository) : BaseApiController
    {
        [HttpGet]

        public async Task <ActionResult<IReadOnlyList<Members>>> GetMembers()
        {
            return Ok(await memberRepository.GetMembersAsyn());
        }
        
       
        [HttpGet("{id}")] //localhost:5001/api/members/bob-id
        public async Task<ActionResult<Members>> GetMember(string id)
        {
            var member = await memberRepository.GetMembersByIdAsync(id);
            
            if (member==null) return NotFound();

            return member;

        }
        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string id)
        {
            return Ok(await memberRepository.GetPhotosForMembersAsync(id));
        }
    }
}