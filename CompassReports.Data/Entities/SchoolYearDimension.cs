﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CompassReports.Data.Entities
{
    [Table("SchoolYearDimension", Schema = "cmp")]
    public class SchoolYearDimension : EntityBase
    {
        [Key]
        public short SchoolYearKey { get; set; }

        [Required]
        [MaxLength(50)]
        public string SchoolYearDescription { get; set; }

        public ICollection<AssessmentFact> AssessmentFacts { get; set; }
        public ICollection<AttendanceFact> AttendanceFacts { get; set; }
        public ICollection<EnrollmentFact> EnrollmentFacts { get; set; }
        public ICollection<GraduationFact> GraduationFacts { get; set; }
    }
}