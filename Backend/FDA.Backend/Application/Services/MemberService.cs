using FDA.Backend.Infrastructure.Data;
using FDA.Database.Model;
using Microsoft.AspNetCore.SignalR;

namespace FDA.Backend.Application.Services
{
    public interface IMemberService
    {
        Task<Member?> GetMemberById(int id);
        Task<IEnumerable<Member>?> GetAllMembers();
        Task<bool> AddMember(string name, string? phone, string? email, int? memberShip);
        Task<Member?> GetMemberByFuzzyName(string name);
        Task<bool> UpdateMember(Member existingMember);
    }
    public class MemberService(IMemberRepository memberRepository) : IMemberService
    {
        private readonly IMemberRepository memberRepository = memberRepository;

        public async Task<bool> AddMember(string name, string? phone, string? email, int? memberShip)
        {
            return await memberRepository.AddMember(name, phone, email, memberShip);
        }

        public async Task<IEnumerable<Member>?> GetAllMembers()
        {
            return await memberRepository.GetAll();
        }

        public async Task<Member?> GetMemberByFuzzyName(string name)
        {
            return await memberRepository.GetMemberByFuzzyName(name);
        }

        public async Task<Member?> GetMemberById(int id)
        {
            return await memberRepository.GetById(id);
        }

        public async Task<bool> UpdateMember(Member existingMember)
        {
            return await memberRepository.UpdateMember(existingMember);
        }
    }
}
