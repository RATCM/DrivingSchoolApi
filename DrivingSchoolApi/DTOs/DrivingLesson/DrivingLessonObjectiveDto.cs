namespace DrivingSchoolApi.DTOs.DrivingLesson;

public record DrivingLessonObjectiveDto(
    bool RightOfWay,
    bool Highway,
    bool Night,
    bool ThreePointTurn,
    bool ReverseAroundCorner,
    bool ParallelParking
    );