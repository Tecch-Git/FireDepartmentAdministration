using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDA.Database.Model;

public class Member
{
    /// <summary>
    /// MEMBER_ID - ID of the member
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// MEMBER_NAME - Name of the member 
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// MEMBER_EMAIL - Email of the member
    /// </summary>
    public string? Email { get; set; }
    /// <summary>
    /// MEMBER_PHONE - Phonenumber of the member
    /// </summary>
    public string? Phone { get; set; }
    /// <summary>
    /// MEMBER_MEMBERSHIP - Membership of the member
    /// </summary>
    public MEMBERSHIP Membership { get; set; } = 0;
}

/// <summary>
/// 0 = None
/// 1 = Active
/// 2 = Reserve
/// 3 = Youth
/// </summary>
public enum MEMBERSHIP
{
    None = 0,
    Active = 1,
    Reserve = 2,
    Youth = 3
}
