using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.Entities;

public class DrivingSchoolTests
{
    [Test]
    public void ChangeName_Fails_WhenNameIsIdentical()
    {
        var school = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.Empty),
            DrivingSchoolName.Create("Test name"),
            Address.Create("p", "c", "r", "a"),
            PhoneNumber.Create("12345678"),
            WebAddress.Create("test.web"));

        Assert.Throws<InvalidOperationException>(() => school.ChangeName(DrivingSchoolName.Create("Test name")));
    }
    
    [Test]
    public void ChangeName_Succeeds_WhenNameIsNotIdentical()
    {
        var school = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.Empty),
            DrivingSchoolName.Create("Test name 1"),
            Address.Create("p", "c", "r", "a"),
            PhoneNumber.Create("12345678"),
            WebAddress.Create("test.web"));

        Assert.DoesNotThrow(() => school.ChangeName(DrivingSchoolName.Create("Test name 2")));
    }

    
    [Test]
    public void ChangeAddress_Fails_WhenAddressIsIdentical()
    {
        var school = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.Empty),
            DrivingSchoolName.Create("Test name"),
            Address.Create("p", "c", "r", "a"),
            PhoneNumber.Create("12345678"),
            WebAddress.Create("test.web"));

        Assert.Throws<InvalidOperationException>(() => school.ChangeAddress(Address.Create("p", "c", "r", "a")));
    }
    
    [Test]
    public void ChangeAddress_Succeeds_WhenAddressIsNotIdentical()
    {
        var school = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.Empty),
            DrivingSchoolName.Create("Test name"),
            Address.Create("p", "c", "r", "a1"),
            PhoneNumber.Create("12345678"),
            WebAddress.Create("test.web"));

        Assert.DoesNotThrow(() => school.ChangeAddress(Address.Create("p", "c", "r", "a2")));
    }

    
    [Test]
    public void ChangePhoneNumber_Fails_WhenPhoneNumberIsIdentical()
    {
        var school = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.Empty),
            DrivingSchoolName.Create("Test name"),
            Address.Create("p", "c", "r", "a"),
            PhoneNumber.Create("12345678"),
            WebAddress.Create("test.web"));

        Assert.Throws<InvalidOperationException>(() => school.ChangePhoneNumber(PhoneNumber.Create("12345678")));
    }
    
    [Test]
    public void ChangePhoneNumber_Succeeds_WhenPhoneNumberIsNotIdentical()
    {
        var school = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.Empty),
            DrivingSchoolName.Create("Test name"),
            Address.Create("p", "c", "r", "a"),
            PhoneNumber.Create("12345678"),
            WebAddress.Create("test.web"));

        Assert.DoesNotThrow(() => school.ChangePhoneNumber(PhoneNumber.Create("12345679")));
    }
    
    
    [Test]
    public void ChangeWebAddress_Fails_WhenWebAddressIsIdentical()
    {
        var school = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.Empty),
            DrivingSchoolName.Create("Test name"),
            Address.Create("p", "c", "r", "a"),
            PhoneNumber.Create("12345678"),
            WebAddress.Create("test.web"));

        Assert.Throws<InvalidOperationException>(() => school.ChangeWebAddress(WebAddress.Create("test.web")));
    }
    
    [Test]
    public void ChangeWebAddress_Succeeds_WhenWebAddressIsNotIdentical()
    {
        var school = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.Empty),
            DrivingSchoolName.Create("Test name"),
            Address.Create("p", "c", "r", "a"),
            PhoneNumber.Create("12345678"),
            WebAddress.Create("test1.web"));

        Assert.DoesNotThrow(() => school.ChangeWebAddress(WebAddress.Create("test2.web")));
    }
}