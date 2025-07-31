using Microsoft.EntityFrameworkCore;
using FDA.Database.Model;
using FDA.Database.Context;
using FDA.Backend.Application.Services;

namespace FDA.Backend.Infrastructure.Data;

/// <summary>
/// Repository for handling the member data in the database
/// </summary>
public interface IMemberRepository
{
    /// <summary>
    /// Get all members
    /// </summary>
    /// <param name="quantity">Amount of entries</param>
    /// <returns>List of Members</returns>
    Task<List<Member>?> GetAll();
    /// <summary>
    /// Get member by id
    /// </summary>
    /// <returns>Member</returns>
    Task<Member?> GetById(int id);

    Task<bool> AddMember(string name, string? phone, string? email, int? memberShip);
    Task<Member?> GetMemberByFuzzyName(string name);
    Task<bool> UpdateMember(Member existingMember);
}

public sealed class MemberRepository : IMemberRepository
{
    private readonly FDAContext db;

    public MemberRepository(FDAContext database)
    {
        db = database;
    }

    public async Task<bool> AddMember(string name, string? phone, string? email, int? memberShip)
    {
        var memberShipEnum = MEMBERSHIP.None;
        if (memberShip != null && Enum.IsDefined(typeof(MEMBERSHIP), memberShip))
            memberShipEnum = (MEMBERSHIP)memberShip;

        var newMember = new Member()
        {
            Name = name,
            Phone = phone,
            Email = email,
            Membership = memberShipEnum
        };
        await db.Members.AddAsync(newMember);
        await db.SaveChangesAsync();
        return true;
    }

    /// <inheritdoc/>
    public async Task<List<Member>?> GetAll()
    {
        return await db.Members.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Member?> GetById(int id)
    {
        return await db.Members.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Member?> GetMemberByFuzzyName(string name)
    {
        var nameSplits = name.Split(' ');

        // Try matching name ignore case
        var match = await db.Members.Where(m => m.Name.ToLower() == name.ToLower()).ToListAsync();
        // Try matching with reorderd name
        if (match == null || match.Count < 1)
            match = await db.Members.Where(m => m.Name.ToLower() == string.Join(' ', nameSplits.Reverse()).ToLower()).ToListAsync();

        if (match.Count < 1)
        {
            return null;
        }
        else if (match.Count > 1)
        {
            Console.WriteLine($"Error! Found mutliple matches with name '{name}'! Results:");
            foreach (var member in match)
                Console.WriteLine($"Name: {member.Name}");
            return null;
        }
        return match.FirstOrDefault();
    }

    public async Task<bool> UpdateMember(Member existingMember)
    {
        try
        {
            db.Members.Update(existingMember);
            await db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{nameof(UpdateMember)}:: Unexcpected Error.");
            Console.WriteLine(ex.Message);
        }
        return false;
    }
}