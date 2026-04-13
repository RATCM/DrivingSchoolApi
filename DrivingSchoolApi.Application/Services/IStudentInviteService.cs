using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Application.Services;

public interface IStudentInviteService
{
    Task<Result<DrivingSchool>> RedeemStudentInvite(StudentInviteKey id);
}