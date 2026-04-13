using System.Data;
using System.Diagnostics.CodeAnalysis;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public sealed class DrivingSchool : Entity<DrivingSchoolKey>
{
    private List<Package> _packages = [];
    private List<StudentInvite> _studentInvites = [];
    
    public DrivingSchoolName DrivingSchoolName { get; private set; } = null!;
    public StreetAddress StreetAddress { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public WebAddress WebAddress { get; private set; } = null!;
    public IReadOnlyList<Package> Packages => _packages.AsReadOnly();
    public IReadOnlyList<StudentInvite> StudentInvites => _studentInvites.AsReadOnly();
    
    private DrivingSchool() {} // EF
    
    public static DrivingSchool Create(
        DrivingSchoolKey id,
        DrivingSchoolName drivingSchoolName,
        StreetAddress schoolStreetAddress,
        PhoneNumber phoneNumber,
        WebAddress webAddress,
        Package[] packages)
    {
        return new DrivingSchool
        {
            Id = id,
            DrivingSchoolName = drivingSchoolName,
            StreetAddress = schoolStreetAddress,
            PhoneNumber = phoneNumber,
            WebAddress = webAddress,
            _packages = packages.ToList()
        };
    }

    public void ChangeName(DrivingSchoolName newName)
    {
        if (newName == this.DrivingSchoolName)
        {
            throw new InvalidOperationException("Can't change name to the same name");
        }
        this.DrivingSchoolName = newName;
    }

    public void ChangeAddress(StreetAddress newAddress)
    {
        if (StreetAddress.Equals(newAddress))
            throw new InvalidOperationException("Can't change address to the same address");
        StreetAddress = newAddress;
    }
    
    public void ChangePhoneNumber(PhoneNumber newPhoneNumber)
    {
        if (PhoneNumber.Equals(newPhoneNumber))
            throw new InvalidOperationException("Can't change the same phone number to the same phone number");
        PhoneNumber = newPhoneNumber;
    }

    public void ChangeWebAddress(WebAddress newWebAddress)
    {
        if (WebAddress.Equals(newWebAddress))
            throw new InvalidOperationException("Can't change the same web address to the same web address");
        WebAddress = newWebAddress;
    }

    public void AddStudentInvite(StudentInvite invite)
    {
        if (_studentInvites.Select(x => x.Id).Contains(invite.Id))
            throw new InvalidOperationException("Can't add an invite that already exists"); 
        _studentInvites.Add(invite);
    }

    public void RemoveStudentInvite(StudentInvite invite)
    {
        if (!_studentInvites.Select(x => x.Id).Contains(invite.Id))
            throw new InvalidOperationException("Can't remove an invite that doesn't exist");
        _studentInvites.Remove(invite);
    }

    public void AddPackage(Package package)
    {
        _packages.Add(package);
    }

    public void RemovePackage(Package package)
    {
        _packages.Remove(package);
    }
}