using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public class InstructorCalender : Entity<InstructorKey>
{
    private List<TimeSlot> _timeSlots = [];
    public IReadOnlyList<TimeSlot> TimeSlots => _timeSlots.AsReadOnly();

    private InstructorCalender() {} // EF

    public static InstructorCalender Create(InstructorKey ownerId)
    {
        return new InstructorCalender
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