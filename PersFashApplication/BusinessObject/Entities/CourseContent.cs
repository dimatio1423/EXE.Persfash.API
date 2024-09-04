using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class CourseContent
{
    public int CourseContentId { get; set; }

    public string? Content { get; set; }

    public int? Duration { get; set; }

    public int? CourseId { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<CourseMaterial> CourseMaterials { get; set; } = new List<CourseMaterial>();
}
