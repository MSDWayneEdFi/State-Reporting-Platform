﻿module App.Models {
    export class EnrollmentFilterModel implements IReportFilterModel {

        DefaultSchoolYear: number;
        SchoolYear?: number;
        SchoolYears?: number[];

        EnglishLanguageLearnerStatuses: string[];
        Ethnicities: string[];
        Grades: string[];
        LunchStatuses: string[];
        SpecialEducationStatuses: string[];


        filteringCount = () => {
            //Starts at one since Grades always shows
            let count = 1;

            if (this.EnglishLanguageLearnerStatuses != null && this.EnglishLanguageLearnerStatuses.length) count++;
            if (this.Ethnicities != null && this.Ethnicities.length) count++;
            if (this.SchoolYears != null && this.SchoolYears.length) count++;
            if (this.LunchStatuses != null && this.LunchStatuses.length) count++;
            if (this.SpecialEducationStatuses != null && this.SpecialEducationStatuses.length) count++;

            return count;
        }

        isFiltering = () => {
            if (this.EnglishLanguageLearnerStatuses != null && this.EnglishLanguageLearnerStatuses.length) return true;
            if (this.Ethnicities != null && this.Ethnicities.length) return true;
            if (this.Grades != null && this.Grades.length) return true;
            if (this.SchoolYears != null && this.SchoolYears.length) return true;
            if (this.LunchStatuses != null && this.LunchStatuses.length) return true;
            if (this.SpecialEducationStatuses != null && this.SpecialEducationStatuses.length) return true;
            return false;
        }

        reset = () => {
            this.Grades = [];
            this.Ethnicities = [];
            this.SchoolYears = [];
            this.EnglishLanguageLearnerStatuses = [];
            this.LunchStatuses = [];
            this.SpecialEducationStatuses = [];
            this.SchoolYear = this.DefaultSchoolYear;
        }

        constructor(schoolYear?: number) {
            if (schoolYear) this.DefaultSchoolYear = schoolYear;
            this.reset();
        }
    }
}