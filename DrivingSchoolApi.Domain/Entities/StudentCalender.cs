using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public class StudentCalender : Entity<StudentKey>
{
    private List<TimeSlot> _timeSlots = [];
    public IReadOnlyList<TimeSlot> TimeSlots => _timeSlots.AsReadOnly();

    private StudentCalender() {} // EF

    public static StudentCalender Create(StudentKey ownerId)
    {
        return new StudentCalender
        {
            Id = ownerId
        };
    }

    public void AddTimeSlot(TimeSlot newTimeSlot)
    {
        _timeSlots.Add(newTimeSlot);
    }

    public void RemoveTimeSlot(TimeSlot timeSlot)
    {
        _timeSlots.Remove(timeSlot);
    }
}