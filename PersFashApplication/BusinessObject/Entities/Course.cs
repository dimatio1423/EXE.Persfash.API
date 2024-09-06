using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Course
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? InstructorId { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<CourseContent> CourseContents { get; set; } = new List<CourseContent>();

    public virtual ICollection<CustomerCourse> CustomerCourses { get; set; } = new List<CustomerCourse>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual FashionInfluencer? Instructor { get; set; }
}
