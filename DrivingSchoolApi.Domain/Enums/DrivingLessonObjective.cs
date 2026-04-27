namespace DrivingSchoolApi.Domain.Enums;

[Flags]
public enum DrivingLessonObjective
{
    RightOfWay = 1,
    Highway = 2,
    Night = 4,
    ThreePointTurn = 8,
    ReverseAroundCorner = 16,
    ParallelParking = 32
}