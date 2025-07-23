using Microsoft.EntityFrameworkCore;
using FDA.Database.Model;
using FDA.Database.Context;

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

    Task<bool> AddMember(string name, string? phone, string? email);
}

public sealed class MemberRepository : IMemberRepository
{
    private readonly FDAContext db;

    public MemberRepository(FDAContext database)
    {
        db = database;
    }

    public async Task<bool> AddMember(string name, string? phone, string? email)
    {
        var newMember = new Member()
        {
            Name = name,
            Phone = phone,
            Email = email
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
}