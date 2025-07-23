using FDA.Backend.Infrastructure.Data;
using FDA.Database.Model;
using Microsoft.AspNetCore.SignalR;

namespace FDA.Backend.Application.Services
{
    public interface IMemberService
    {
        Task<Member?> GetMemberById(int id);
        Task<IEnumerable<Member>?> GetAllMembers();
        Task<bool> AddMember(string name, string? phone, string? email);
    }
    public class MemberService(IMemberRepository memberRepository) : IMemberService
    {
        private readonly IMemberRepository memberRepository = memberRepository;

        public async Task<bool> AddMember(string name, string? phone, string? email)
        {
            return await memberRepository.AddMember(name, phone, email);
        }

        public async Task<IEnumerable<Member>?> GetAllMembers()
        {
            return await memberRepository.GetAll();
        }

        public async Task<Member?> GetMemberById(int id)
        {
            return new Member();
        }
    }
}
