using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class CourseMaterial
{
    public int CourseMaterialId { get; set; }

    public string? MaterialName { get; set; }

    public string? MaterialLink { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CourseContentId { get; set; }

    public virtual CourseContent? CourseContent { get; set; }
}
