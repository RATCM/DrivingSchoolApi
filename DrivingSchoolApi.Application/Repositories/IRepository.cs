using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Application.Repositories;

public interface IRepository
{
    Task<Result> Save();
}