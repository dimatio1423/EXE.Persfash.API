using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class CustomerCourse
{
    public int CustomerCourseId { get; set; }

    public int? CustomerId { get; set; }

    public int? CourseId { get; set; }

    public DateTime? EnrollmentDate { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Customer? Customer { get; set; }
}
